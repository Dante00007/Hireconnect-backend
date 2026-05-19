using System.Net.Http.Json;

using HireConnect.Analytic.External.DTO;
using HireConnect.Analytic.External.Interfaces;

namespace HireConnect.Analytic.External.Services;

public class JobApiClient
    : IJobApiClient
{
    private readonly HttpClient _http;

    public JobApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<JobSummaryDTO>>
    GetRecruiterJobsAsync(int recruiterId)
    {
        var response =
            await _http.GetAsync(
                $"api/job/internal/myJobs/{recruiterId}"
            );

        if (!response.IsSuccessStatusCode)
        {
            return new();
        }

        return await response.Content
            .ReadFromJsonAsync<
                List<JobSummaryDTO>
            >()
            ?? new();
    }
}