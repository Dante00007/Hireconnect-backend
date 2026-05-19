using HireConnect.Application.DTO;
using HireConnect.Application.Exceptions;
using HireConnect.Application.External.Interfaces;
using HireConnect.Application.Models;
using HireConnect.Application.Repository.Interface;
using HireConnect.Application.Service.Interface;
using HireConnect.Contracts.Events;

using MassTransit;
using MassTransit.Initializers;

namespace HireConnect.Application.Service.ServiceImplement
{

    public class ApplicationServiceImpl : IApplicationService
    {
        private readonly IApplicationRepository _repository;

        private readonly IJobApiClient _jobApiClient;

        private readonly IProfileApiClient _profileApiClient;

        private readonly IPublishEndpoint _publishEndpoint;
        public ApplicationServiceImpl(IApplicationRepository repository
            , IJobApiClient jobApiClient, IProfileApiClient profileApiClient, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _jobApiClient = jobApiClient;
            _profileApiClient = profileApiClient;
        }

        public async Task<ApplicationResponseDTO> SubmitApplicationAsync(ApplicationSubmitDTO dto, int candidateId)
        {
            // Check if application already exists for this job/candidate pair
            var existing = await _repository.FindFirstByJobIdAndCandidateIdAsync(dto.JobId, candidateId);
            if (existing != null)
            {
                throw new DuplicateException("Candidate has already applied for this job.");
            }

            var jobDetails = await _jobApiClient.GetJobByIdAsync(dto.JobId);
            if (jobDetails == null)
            {
                throw new JobNotExistException("Job not found.");
            }

            var candidateProfile = await _profileApiClient.GetCandidateProfileAsync(candidateId);
            if (candidateProfile == null)
            {
                throw new ProfileNotFoundException("Candidate profile not found.Please complete your profile first");
            }
            var application = new ApplicationIdentity
            {
                JobId = dto.JobId,
                CandidateId = candidateId,
                CandidateName = candidateProfile.FullName,
                JobTitle = jobDetails.Title,
                RecruiterName = jobDetails.RecruiterName,
                CoverLetter = dto.CoverLetter,
                ResumeUrl = candidateProfile.ResumeUrl,
                Status = ApplicationStatus.Applied.ToString(),
                AppliedAt = DateTime.UtcNow

            };


            await _repository.AddAsync(application);
            await _repository.SaveChangesAsync();
            await _publishEndpoint.Publish(
            new ApplicationSubmittedEvent
                {
                    RecruiterId =
                        jobDetails.PostedBy,

                    CandidateName =
                        candidateProfile.FullName,

                    JobTitle =
                        jobDetails.Title
                }
            );
            return MapToResponseDTO(application);
        }

        public async Task<ApplicationResponseDTO> UpdateStatusAsync(int applicationId, string newStatus, int recruiterId)
        {
            var application = await _repository.GetByIdAsync(applicationId);
            if (application == null) throw new ApplicationNotFoundException("Application not found.");
    

            application.Status = newStatus;

            await _repository.UpdateAsync(application);
            await _repository.SaveChangesAsync();

            if (string.Equals(newStatus, ApplicationStatus.Accepted.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                await _publishEndpoint.Publish(
                    new ApplicationAcceptedEvent
                    {
                        CandidateId = application.CandidateId,
                        JobTitle = application.JobTitle,
                    }
                );
            }
            else if (string.Equals(newStatus, ApplicationStatus.Rejected.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                await _publishEndpoint.Publish(
                    new ApplicationRejectedEvent
                    {
                        CandidateId = application.CandidateId,
                        JobTitle = application.JobTitle,
                    }
                );
            }
            else if (string.Equals(newStatus, ApplicationStatus.Interview.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                await _publishEndpoint.Publish(
                    new ApplicationInterviewEvent
                    {
                        CandidateId = application.CandidateId,
                        JobTitle = application.JobTitle,
                    }
                );
            }
            
            return MapToResponseDTO(application); // Return the updated application;
        }

        public async Task<IEnumerable<ApplicationResponseDTO>> GetByCandidateIdAsync(int candidateId)
        {
            var applications = await _repository.FindByCandidateIdAsync(candidateId);
            return applications.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<ApplicationResponseDTO>> GetByJobIdAsync(int jobId)
        {
            var applications = await _repository.FindByJobIdAsync(jobId);
            return applications.Select(MapToResponseDTO);
        }


        public async Task WithdrawApplicationAsync(int applicationId, int candidateId)
        {
            var application = await _repository.GetByIdAsync(applicationId);
            if (application == null) throw new KeyNotFoundException("Application not found.");

            if (application.CandidateId != candidateId)
            {
                throw new UnauthorizedAccessException("Candidate can only withdraw their own applications.");
            }

            await _repository.DeleteAsync(applicationId);
            await _repository.SaveChangesAsync();

        }

        public async Task<ApplicationResponseDTO?> GetByIdAsync(int applicationId)
        {
            var application = await _repository.GetByIdAsync(applicationId);
            if (application == null) return null;

            return MapToResponseDTO(application);
        }

        public async Task<int> CountByJobIdAsync(int jobId)
        {
            return await _repository.CountByJobIdAsync(jobId);
        }

        private ApplicationResponseDTO MapToResponseDTO(ApplicationIdentity application)
        {
            return new ApplicationResponseDTO
            {
                ApplicationId = application.ApplicationId,
                JobId = application.JobId,
                CandidateName = application.CandidateName,
                CandidateId = application.CandidateId,
                JobTitle = application.JobTitle,
                RecruiterName = application.RecruiterName,
                AppliedAt = application.AppliedAt,
                Status = application.Status,
                CoverLetter = application.CoverLetter,
                ResumeUrl = application.ResumeUrl
            };
        }
    }
}