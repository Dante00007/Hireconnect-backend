
using HireConnect.Job.DTO;
using HireConnect.Job.Exceptions;
using HireConnect.Job.External.Interfaces;
using HireConnect.Job.Models;
using HireConnect.Job.Repository.Interface;
using HireConnect.Job.Service.Interface;

namespace HireConnect.Job.Service.ServiceImplement;


public class JobServiceImpl : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly IProfileApiClient _profileApiClient;
    public JobServiceImpl(IJobRepository jobRepository, IProfileApiClient profileApiClient)
    {
        _jobRepository = jobRepository;
        _profileApiClient = profileApiClient;
    }

    public async Task AddJobAsync(int postBy, JobDTO jobDto)
    {
        if (jobDto.SalaryMin > jobDto.SalaryMax)
            throw new InvalidJobDataException("Minimum salary cannot be greater than maximum salary.");
        if (jobDto.Skills == null || !jobDto.Skills.Any())
            throw new InvalidJobDataException("At least one skill is required to post a job.");
        
        var recruiter = await _profileApiClient.GetRecruiterProfileAsync(postBy);
        if (recruiter == null)
            throw new JobException($"Recruiter profile {postBy} not found");


        var job = new Jobs
        {
            Title = jobDto.Title,
            Category = jobDto.Category,
            Type = jobDto.Type,
            Location = jobDto.Location,
            SalaryMin = jobDto.SalaryMin,
            SalaryMax = jobDto.SalaryMax,
            Description = jobDto.Description,
            Skills = jobDto.Skills,
            ExperienceRequired = jobDto.ExperienceRequired,
            PostedBy = postBy,
            PostedByName = recruiter.FullName,
            Status = "Active",
            PostedAt = DateTime.UtcNow
        };
        await _jobRepository.AddAsync(job);
        await _jobRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<JobResponseDTO>> GetAllJobsAsync(){  
        var jobs = await _jobRepository.GetAllAsync();
        return jobs.Select(MapToJobResponseDTO);
    }

    public async Task<JobResponseDTO?> GetJobByIdAsync(int id)
    {
        Jobs? job = await _jobRepository.FindByIdAsync(id) ?? throw new JobNotFoundException(id);
        var jobResponse = MapToJobResponseDTO(job);
        return jobResponse;
    }

    public async Task<JobResponseDTO?> UpdateJobAsync(JobDTO jobDTO, int id, int recruiterIdFromToken)
    {
        var job = await _jobRepository.FindByIdAsync(id) ?? throw new JobNotFoundException(id);

        if(jobDTO.SalaryMin > jobDTO.SalaryMax)
            throw new InvalidJobDataException("Minimum salary cannot be greater than maximum salary.");

        if (job.PostedBy != recruiterIdFromToken)
            throw new UnauthorizedJobAccessException(recruiterIdFromToken, id);
        
        if (job.Status == "Closed")
            throw new JobAlreadyClosedException(job.JobId);

        job.Title = jobDTO.Title;
        job.Category = jobDTO.Category;
        job.Type = jobDTO.Type;
        job.Location = jobDTO.Location;
        job.SalaryMin = jobDTO.SalaryMin;
        job.SalaryMax = jobDTO.SalaryMax;
        job.Description = jobDTO.Description;
        job.Skills = jobDTO.Skills;
        job.ExperienceRequired = jobDTO.ExperienceRequired;
        job.PostedAt = DateTime.UtcNow;


        await _jobRepository.UpdateAsync(job);
        await _jobRepository.SaveChangesAsync();

        var jobResponse = MapToJobResponseDTO(job);

        return jobResponse;
    }
    public async Task DeleteJobAsync(int id, int recruiterIdFromToken)
    {
        var job = await _jobRepository.FindByIdAsync(id);

        if (job == null)
            throw new JobNotFoundException(id);

        if (job.PostedBy != recruiterIdFromToken)
            throw new UnauthorizedJobAccessException(recruiterIdFromToken, id);

        await _jobRepository.DeleteAsync(id);
        await _jobRepository.SaveChangesAsync();
    }



    public async Task<IEnumerable<JobResponseDTO>> SearchJobsAsync(string title, string location, double minSalary, double maxSalary)
    {
        if (minSalary > maxSalary)
            throw new InvalidJobDataException("Search criteria invalid: minSalary exceeds maxSalary.");

        var jobs = await _jobRepository.GetAllAsync();



        return jobs
            .Where(j =>
                (string.IsNullOrEmpty(title) || j.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(location) || j.Location.Contains(location, StringComparison.OrdinalIgnoreCase)) &&
                j.SalaryMin >= minSalary &&
                j.SalaryMax <= maxSalary
            )
            .Select(MapToJobResponseDTO);
    }
    public async Task<IEnumerable<JobResponseDTO>> GetJobsByCategoryAsync(string category){
        var jobs = await _jobRepository.FindByCategoryAsync(category);
        return jobs.Select(MapToJobResponseDTO);
    }

    public async Task<IEnumerable<JobResponseDTO>> GetJobsByLocationAsync(string location){
        var jobs = await _jobRepository.FindByLocationAsync(location);
        return jobs.Select(MapToJobResponseDTO);
        
    }

    public async Task<IEnumerable<JobResponseDTO>> GetJobsByPostedByAsync(int recruiterId){

        var jobs = await _jobRepository.FindByPostedByAsync(recruiterId);
        return jobs.Select(MapToJobResponseDTO);
    }

    private JobResponseDTO MapToJobResponseDTO(Jobs job)
    {
        return new JobResponseDTO
        {
            JobId = job.JobId,
            Title = job.Title,
            Category = job.Category,
            Type = job.Type,
            Location = job.Location,
            SalaryMin = job.SalaryMin,
            SalaryMax = job.SalaryMax,
            Description = job.Description,
            Skills = job.Skills,
            ExperienceRequired = job.ExperienceRequired,
            PostedBy = job.PostedBy,
            PostedByName = job.PostedByName,
            Status = job.Status,
            CreatedAt = job.PostedAt
        };
    }
}