using HireConnect.Profile.DTO;

namespace HireConnect.Profile.Service.Interface
{
    public interface IProfileService
    {
        
        Task AddCandidateProfileAsync(CandidateProfileDTO dto, int userId); 
        Task UpdateCandidateProfileAsync( CandidateProfileDTO dto,int userId); 


        Task AddRecruiterProfileAsync(RecruiterProfileDTO dto, int userId); 
        Task UpdateRecruiterProfileAsync(RecruiterProfileDTO dto,int userId);

        Task<UserProfileDTO?> GetProfileByUserIdAsync(int userId); 
        Task<UserProfileDTO?> GetByEmailAsync(string email); 
        Task<List<UserProfileDTO>> GetAllProfilesAsync(); 
        
        
        Task DeleteProfileAsync(int userId);
    }
}