using HireConnect.Application.DTO;
using HireConnect.Application.Models;

namespace HireConnect.Application.Service.Interface
{

    public interface IApplicationService
    {

        Task<ApplicationResponseDTO> SubmitApplicationAsync(ApplicationSubmitDTO applicationdto, int candidateId);
        Task<ApplicationResponseDTO> UpdateStatusAsync(int applicationId, string newStatus, int recruiterId);

        Task WithdrawApplicationAsync(int applicationId, int candidateId);
        Task<IEnumerable<ApplicationResponseDTO>> GetByCandidateIdAsync(int candidateId);
        Task<IEnumerable<ApplicationResponseDTO>> GetByJobIdAsync(int jobId);
        Task<ApplicationResponseDTO?> GetByIdAsync(int applicationId);
        Task<int> CountByJobIdAsync(int jobId);
    }
}