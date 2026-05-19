using System.Net.Http.Json;

using HireConnect.Application.External.DTOs;

using HireConnect.Application.External.Interfaces;

namespace HireConnect.Application.External.Services;

public class JobApiClient
    : IJobApiClient
{
    private readonly HttpClient _http;

    public JobApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<JobDetailsDTO?>
    GetJobByIdAsync(int jobId)
    {
        var response =
            await _http.GetAsync(
                $"api/job/internal/{jobId}"
            );

        if(!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response
            .Content
            .ReadFromJsonAsync<JobDetailsDTO>();
    }
}