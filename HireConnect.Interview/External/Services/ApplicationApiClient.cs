using HireConnect.Interview.External.DTO;
using HireConnect.Interview.External.Interfaces;

namespace HireConnect.Interview.External.Services
{
    public class ApplicationApiClient : IApplicationApiClient
    {
        private readonly HttpClient _httpClient;

        public ApplicationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApplicationInternalResponseDTO?> GetApplicationDetailsAsync(int applicationId)
        {
            var response =
            await _httpClient.GetAsync(
                $"api/applications/internal/{applicationId}"
            );

        if(!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response
            .Content
            .ReadFromJsonAsync<ApplicationInternalResponseDTO>();
        }
    }
}