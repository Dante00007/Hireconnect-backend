using System.Net.Http.Json;

using HireConnect.Analytic.External.DTO;
using HireConnect.Analytic.External.Interfaces;

namespace HireConnect.Analytic.External.Services;

public class InterviewApiClient
    : IInterviewApiClient
{
    private readonly HttpClient _http;

    public InterviewApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<InterviewSummaryDTO>>
    GetRecruiterInterviewsAsync(int recruiterId)
    {
        var response =
            await _http.GetAsync(
                $"api/interviews/internal/recruiter/{recruiterId}"
                
            );

        if(!response.IsSuccessStatusCode)
        {
            return new();
        }

        return await response.Content
            .ReadFromJsonAsync<
                List<InterviewSummaryDTO>
            >()
            ?? new();
    }
}