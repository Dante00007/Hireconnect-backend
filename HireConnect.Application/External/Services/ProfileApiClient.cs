using System.Net.Http.Json;

using HireConnect.Application.External.DTOs;

using HireConnect.Application.External.Interfaces;

namespace HireConnect.Application.External.Services;

public class ProfileApiClient
    : IProfileApiClient
{
    private readonly HttpClient _http;

    public ProfileApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<CandidateProfileDTO?>
    GetCandidateProfileAsync(int userId)
    {
        var response =
            await _http.GetAsync(
                $"api/profile/internal/candidate/{userId}"
            );

        if(!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response
            .Content
            .ReadFromJsonAsync<CandidateProfileDTO>();
    }
}