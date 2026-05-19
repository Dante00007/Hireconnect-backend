using HireConnect.Job.Database;
using HireConnect.Job.Models;
using HireConnect.Job.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace HireConnect.Job.Repository.RepoImplement
{
    public class JobRepository : IJobRepository
    {
        private readonly JobDbContext _context;

        public JobRepository(JobDbContext context)
        {
            _context = context;
        }

        public async Task<Jobs?> FindByIdAsync(int id) => 
            await _context.Jobs.FindAsync(id);

        public async Task<IEnumerable<Jobs>> GetAllAsync() => 
            await _context.Jobs.ToListAsync();

        public async Task<Jobs?> FindByTitleAsync(string title) => 
            await _context.Jobs.FirstOrDefaultAsync(j => j.Title == title); 

        public async Task<IEnumerable<Jobs>> FindByTitleContainingAsync(string keyword) => 
            await _context.Jobs.Where(j => j.Title.Contains(keyword)).ToListAsync(); 

        public async Task<IEnumerable<Jobs>> FindByCategoryAsync(string category) => 
            await _context.Jobs.Where(j => j.Category.ToLower() == category.ToLower()).ToListAsync(); 

        public async Task<IEnumerable<Jobs>> FindByLocationAsync(string location) => 
            await _context.Jobs.Where(j => j.Location.ToLower() == location.ToLower()).ToListAsync();

        public async Task<IEnumerable<Jobs>> FindByStatusAsync(string status) => 
            await _context.Jobs.Where(j => j.Status.ToLower() == status.ToLower()).ToListAsync();

        public async Task<IEnumerable<Jobs>> FindByPostedByAsync(int recruiterId) => 
            await _context.Jobs.Where(j => j.PostedBy == recruiterId).ToListAsync(); 

        public async Task AddAsync(Jobs job) => 
            await _context.Jobs.AddAsync(job);

        public async Task UpdateAsync(Jobs job) => 
            _context.Jobs.Update(job); 

        public async Task DeleteAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null) _context.Jobs.Remove(job);
        }

        public async Task<bool> SaveChangesAsync() => 
            await _context.SaveChangesAsync() > 0;
    }
}