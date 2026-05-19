
using HireConnect.Profile.Database;
using HireConnect.Profile.Models;
using HireConnect.Profile.Repository.Interface;

using Microsoft.EntityFrameworkCore;

namespace HireConnect.Profile.Repository.RepoImplement;

public class ProfileRepositoryImpl : IProfileRepository
{
    private readonly ProfileDbContext _context;

    public ProfileRepositoryImpl(ProfileDbContext context)
    {
        _context = context;
    }

  
    public async Task<CandidateProfile?> FindCandidateByEmailAsync(string email) =>
        await _context.Candidates.Include(c => c.Address).FirstOrDefaultAsync(c => c.Email == email);

    public async Task<CandidateProfile?> FindCandidateByUserIdAsync(int userId) =>
        await _context.Candidates.Include(c => c.Address).FirstOrDefaultAsync(c => c.UserId == userId);

    public async Task AddCandidateAsync(CandidateProfile profile) =>
        await _context.Candidates.AddAsync(profile);

    // Recruiter Data Access
    public async Task<RecruiterProfile?> FindRecruiterByEmailAsync(string email) =>
        await _context.Recruiters.Include(r => r.Address).FirstOrDefaultAsync(r => r.Email == email);

    public async Task<RecruiterProfile?> FindRecruiterByUserIdAsync(int userId) =>
        await _context.Recruiters.Include(r => r.Address).FirstOrDefaultAsync(r => r.UserId == userId);

    public async Task AddRecruiterAsync(RecruiterProfile profile) =>
        await _context.Recruiters.AddAsync(profile);

    // Common Operations
    public async Task<Address?> FindAddressByIdAsync(int addressId) =>
        await _context.Addresses.FindAsync(addressId);

    public async Task UpdateCandidateAsync(CandidateProfile profile)
    {
        _context.Candidates.Update(profile);
        await Task.CompletedTask;
    }

    public async Task UpdateRecruiterAsync(RecruiterProfile profile)
    {
        _context.Recruiters.Update(profile);
        await Task.CompletedTask;
    }

    public async Task DeleteCandidateByUserIdAsync(int userId)
    {
        var profile = await FindCandidateByUserIdAsync(userId);
        if (profile != null) _context.Candidates.Remove(profile);
    }

    public async Task DeleteRecruiterByUserIdAsync(int userId)
    {
        var profile = await FindRecruiterByUserIdAsync(userId);
        if (profile != null) _context.Recruiters.Remove(profile);
    }

    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() > 0;
}