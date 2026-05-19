using HireConnect.Job.DTO;
using HireConnect.Job.Models;

namespace HireConnect.Job.Service.Interface;

public interface IJobService
{
    Task AddJobAsync(int postBy, JobDTO job);
    Task<JobResponseDTO?> UpdateJobAsync(JobDTO jobDTO, int id, int recruiterIdFromToken);
    Task DeleteJobAsync(int id, int recruiterIdFromToken);

    Task<IEnumerable<JobResponseDTO>> GetAllJobsAsync();
    Task<JobResponseDTO?> GetJobByIdAsync(int id);
    Task<IEnumerable<JobResponseDTO>> SearchJobsAsync(string title, string location, double minSalary, double maxSalary);
    Task<IEnumerable<JobResponseDTO>> GetJobsByPostedByAsync(int recruiterId);
    Task<IEnumerable<JobResponseDTO>> GetJobsByCategoryAsync(string category);
    Task<IEnumerable<JobResponseDTO>> GetJobsByLocationAsync(string location);
    
}
