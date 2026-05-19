using HireConnect.Application.Models;

namespace HireConnect.Application.Repository.Interface
{
    
    public interface IApplicationRepository
    {
      
        Task<ApplicationIdentity?> GetByIdAsync(int applicationId);

     
        Task<IEnumerable<ApplicationIdentity>> FindByCandidateIdAsync(int candidateId);

    
        Task<IEnumerable<ApplicationIdentity>> FindByJobIdAsync(int jobId);

      
        Task<IEnumerable<ApplicationIdentity>> FindByStatusAsync(string status);

        
        Task<ApplicationIdentity?> FindFirstByJobIdAndCandidateIdAsync(int jobId, int candidateId);

        Task<int> CountByJobIdAsync(int jobId);

        Task AddAsync(ApplicationIdentity application);
        Task UpdateAsync(ApplicationIdentity application);
        Task DeleteAsync(int applicationId);
        Task SaveChangesAsync();
    }
}