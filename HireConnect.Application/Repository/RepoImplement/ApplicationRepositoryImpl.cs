using HireConnect.Application.Database;
using HireConnect.Application.Models;
using HireConnect.Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HireConnect.Application.Repository.RepoImplement
{

    public class ApplicationRepositoryImpl : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepositoryImpl(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationIdentity?> GetByIdAsync(int applicationId)
        {
            return await _context.Applications.FindAsync(applicationId);
        }

        public async Task<IEnumerable<ApplicationIdentity>> FindByCandidateIdAsync(int candidateId)
        {
            return await _context.Applications
                .Where(a => a.CandidateId == candidateId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationIdentity>> FindByJobIdAsync(int jobId)
        {
            return await _context.Applications
                .Where(a => a.JobId == jobId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationIdentity>> FindByStatusAsync(string status)
        {
            return await _context.Applications
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<ApplicationIdentity?> FindFirstByJobIdAndCandidateIdAsync(int jobId, int candidateId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.JobId == jobId && a.CandidateId == candidateId); 
        }
        public async Task<int> CountByJobIdAsync(int jobId)
        {
            return await _context.Applications.CountAsync(a => a.JobId == jobId);
        }

        public async Task AddAsync(ApplicationIdentity application)
        {
            await _context.Applications.AddAsync(application);
        }

        public async Task UpdateAsync(ApplicationIdentity application)
        {
            _context.Applications.Update(application);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int applicationId)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application != null)
            {
                _context.Applications.Remove(application);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync(); 
        }
    }
}