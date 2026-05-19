using System.Net.Http.Json;
using HireConnect.Analytic.External.DTO;
using HireConnect.Analytic.External.Interfaces;

namespace HireConnect.Analytic.External.Services;

public class ApplicationApiClient
    : IApplicationApiClient
{
    private readonly HttpClient _http;

    public ApplicationApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ApplicationSummaryDTO>>
    GetApplicationsByJobAsync(int jobId)
    {
        var response =
            await _http.GetAsync(
                $"api/applications/internal/job/{jobId}"
            );

        if(!response.IsSuccessStatusCode)
        {
            return new();
        }

        return await response.Content
            .ReadFromJsonAsync<
                List<ApplicationSummaryDTO>
            >()
            ?? new();
    }
}