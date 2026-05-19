using HireConnect.Analytic.External.DTO;

namespace HireConnect.Analytic.External.Interfaces;
public interface IApplicationApiClient
{
    Task<List<ApplicationSummaryDTO>>
    GetApplicationsByJobAsync(int jobId);
}