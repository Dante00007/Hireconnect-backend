using System.Net.Http.Json;

using HireConnect.Job.External.Interfaces;
using HireConnect.Job.External.DTO;

namespace HireConnect.Job.External.Services;

public class ProfileApiClient
    : IProfileApiClient
{
    private readonly HttpClient _http;

    public ProfileApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<RecruiterProfileInternalDTO?>
    GetRecruiterProfileAsync(int userId)
    {
        var response =
            await _http.GetAsync(
                $"api/profile/internal/recruiter/{userId}"
            );

        if(!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response
            .Content
            .ReadFromJsonAsync<RecruiterProfileInternalDTO>();
    }
}