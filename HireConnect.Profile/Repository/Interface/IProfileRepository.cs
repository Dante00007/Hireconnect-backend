using HireConnect.Profile.Models;

namespace HireConnect.Profile.Repository.Interface;

public interface IProfileRepository
{
    // Candidate Operations
    Task<CandidateProfile?> FindCandidateByEmailAsync(string email);
    Task<CandidateProfile?> FindCandidateByUserIdAsync(int userId);
    Task AddCandidateAsync(CandidateProfile profile);

    // Recruiter Data Access [cite: 213, 225]
    Task<RecruiterProfile?> FindRecruiterByEmailAsync(string email);
    Task<RecruiterProfile?> FindRecruiterByUserIdAsync(int userId);
    Task AddRecruiterAsync(RecruiterProfile profile);

    // Common Operations [cite: 218, 225]
    Task<Address?> FindAddressByIdAsync(int addressId);
    Task UpdateCandidateAsync(CandidateProfile profile);
    Task UpdateRecruiterAsync(RecruiterProfile profile);
    Task DeleteCandidateByUserIdAsync(int userId);
    Task DeleteRecruiterByUserIdAsync(int userId);

    Task<bool> SaveChangesAsync();
}
