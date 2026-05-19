using HireConnect.Contracts.Events;
using HireConnect.Interview.DTO;
using HireConnect.Interview.Exceptions;
using HireConnect.Interview.External.Interfaces;
using HireConnect.Interview.Models;
using HireConnect.Interview.Repository.Interface;
using HireConnect.Interview.Service.Interface;
using MassTransit;

namespace HireConnect.Interview.Services
{
    public class InterviewServiceImpl : IInterviewService
    {
        private readonly IInterviewRepository _repository;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public InterviewServiceImpl(IInterviewRepository repository, IApplicationApiClient applicationApiClient, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _applicationApiClient = applicationApiClient;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<InterviewResponseDTO> ScheduleInterviewAsync(InterviewDto interviewdto, int userId)
        {
            var application = await _applicationApiClient.GetApplicationDetailsAsync(interviewdto.ApplicationId);


            if (application == null)
            {
                throw new ApplicationNotFoundException(
                    "Application not found"
                );
            }
            var existing = await _repository.GetActiveInterviewByApplicationIdAsync(interviewdto.ApplicationId);

            if (existing != null)
            {
                throw new DuplicateInterviewException(
                    "Active interview already exists"
                );
            }
            if (interviewdto.InterviewMode == "Online" && string.IsNullOrWhiteSpace(interviewdto.MeetLink))
            {
                throw new MeetingException(
                    "Meet link required"
                );
            }
            if (interviewdto.InterviewMode == "In-Person" && string.IsNullOrWhiteSpace(interviewdto.Location))
            {
                throw new MeetingException(
                    "Location required"
                );
            }
            var interview = new InterviewIdentity
            {
                ApplicationId = interviewdto.ApplicationId,

                CandidateId = application.CandidateId,

                RecruiterId = userId,

                CandidateName = application.CandidateName,

                JobTitle = application.JobTitle,

                ScheduledAt = interviewdto.ScheduledAt.ToUniversalTime(),

                InterviewMode = interviewdto.InterviewMode,

                MeetLink = interviewdto.MeetLink,

                Location = interviewdto.Location,

                Notes = interviewdto.Notes,

                DurationMinutes = interviewdto.DurationMinutes,

                Status = InterviewStatus.Scheduled.ToString()
            };

            await _repository.AddAsync(interview);
            await _repository.SaveChangesAsync();
            await _publishEndpoint.Publish(
                new InterviewScheduledEvent
                {
                    CandidateId = interview.CandidateId,

                    JobTitle = interview.JobTitle,

                    ScheduledAt = interview.ScheduledAt
                }
            );

            return MapToResponseDTO(interview);
        }

        public async Task<InterviewResponseDTO> ConfirmInterviewAsync(int interviewId, int userId)
        {
            var interview = await _repository.GetByIdAsync(interviewId);
            if (interview == null) throw new InterviewNotFoundException("Interview not found");

            if (interview.CandidateId != userId)
            {
                throw new UnauthorizedAccessException();
            }
            if (interview.Status == InterviewStatus.Cancelled.ToString())
            {
                throw new InterviewStatusException(
                    "Cancelled interview cannot be confirmed"
                );
            }
            if (interview.Status == InterviewStatus.Confirmed.ToString())
            {
                throw new InterviewStatusException(
                    "Interview already confirmed"
                );
            }

            interview.Status = InterviewStatus.Confirmed.ToString();
            await _repository.UpdateAsync(interview);
            await _repository.SaveChangesAsync();

            await _publishEndpoint.Publish(
                new InterviewConfirmedEvent
                {
                    RecruiterId = interview.RecruiterId,

                    CandidateName = interview.CandidateName,

                    JobTitle = interview.JobTitle
                }
            );


            return MapToResponseDTO(interview);
        }

        public async Task<InterviewResponseDTO> RescheduleInterviewAsync(int interviewId, int userId, RescheduleDto rescheduleDto)
        {
            var interview = await _repository.GetByIdAsync(interviewId);
            if (interview == null) throw new InterviewNotFoundException("Interview not found");
            if (interview.RecruiterId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            var oldDateTime = interview.ScheduledAt;
            interview.MeetLink = rescheduleDto.MeetLink;

            interview.Location = rescheduleDto.Location;

            interview.Notes = rescheduleDto.Notes;
            await _repository.UpdateAsync(interview);
            await _repository.SaveChangesAsync();

            await _publishEndpoint.Publish(
                new InterviewRescheduledEvent
                {
                    CandidateId =
                        interview.CandidateId,

                    JobTitle =
                        interview.JobTitle,

                    NewDateTime =
                        interview.ScheduledAt.ToUniversalTime(),
                }
            );

            return MapToResponseDTO(interview);
        }

        public async Task<InterviewResponseDTO> CancelInterviewAsync(int interviewId, int userId)
        {
            var interview = await _repository.GetByIdAsync(interviewId);
            if (interview == null) throw new InterviewNotFoundException("Interview not found");
            if (interview.RecruiterId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            interview.Status = InterviewStatus.Cancelled.ToString();
            await _repository.UpdateAsync(interview);
            await _repository.SaveChangesAsync();
            await _publishEndpoint.Publish(
                new InterviewCancelledEvent
                {

                    CandidateId = interview.CandidateId,

                    JobTitle = interview.JobTitle
                }
            );
            return MapToResponseDTO(interview);

        }

        public async Task<IEnumerable<InterviewResponseDTO>> GetRecruiterInterviewsAsync(int recruiterId)
        {
            var interviews = await _repository.GetByRecruiterIdAsync(recruiterId);
            return interviews.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<InterviewResponseDTO>> GetCandidateInterviewsAsync(int candidateId)
        {
            var interviews = await _repository.GetByCandidateIdAsync(candidateId);
            return interviews.Select(MapToResponseDTO);
        }
        public async Task<InterviewResponseDTO?> GetByIdAsync(int interviewId)
        {
            var interview = await _repository.GetByIdAsync(interviewId);
            return interview != null ? MapToResponseDTO(interview) : null;
        }


        public async Task<IEnumerable<InterviewResponseDTO>> GetByApplicationAsync(int applicationId)
        {

            var interviews = await _repository.FindByApplicationIdAsync(applicationId);
            return interviews.Select(MapToResponseDTO);
        }


        private InterviewResponseDTO MapToResponseDTO(InterviewIdentity interview)
        {
            return new InterviewResponseDTO
            {
                InterviewId = interview.InterviewId,
                ApplicationId = interview.ApplicationId,
                CandidateId = interview.CandidateId,
                RecruiterId = interview.RecruiterId,
                CandidateName = interview.CandidateName,
                JobTitle = interview.JobTitle,
                ScheduledAt = interview.ScheduledAt,
                InterviewMode = interview.InterviewMode,
                MeetLink = interview.MeetLink,
                Location = interview.Location,
                Notes = interview.Notes,
                Status = interview.Status,
                DurationMinutes = interview.DurationMinutes
            };
        }
    }
}