using HireConnect.Analytic.External.DTO;

namespace HireConnect.Analytic.External.Interfaces;

public interface IJobApiClient
{
    Task<List<JobSummaryDTO>> GetRecruiterJobsAsync(int recruiterId);
}