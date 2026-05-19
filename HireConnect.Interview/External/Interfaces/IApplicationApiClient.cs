using HireConnect.Interview.External.DTO;

namespace HireConnect.Interview.External.Interfaces
{
    public interface IApplicationApiClient
    {
        Task<ApplicationInternalResponseDTO?> GetApplicationDetailsAsync(int applicationId);
    }
}