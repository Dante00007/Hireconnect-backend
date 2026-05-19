using HireConnect.Application.External.DTOs;

namespace HireConnect.Application.External.Interfaces;

public interface IJobApiClient
{
    Task<JobDetailsDTO?>
    GetJobByIdAsync(int jobId);
}