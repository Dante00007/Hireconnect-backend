using HireConnect.Interview.Data;
using HireConnect.Interview.Models;
using HireConnect.Interview.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HireConnect.Interview.Repository.RepoImplement
{
    public class InterviewRepositoryImpl : IInterviewRepository
    {
        private readonly InterviewDbContext _context;

        public InterviewRepositoryImpl(InterviewDbContext context)
        {
            _context = context;
        }

        public async Task<List<InterviewIdentity>> GetByCandidateIdAsync(int candidateId)
        {
            return await _context.Interviews
                .Where(i =>
                    i.CandidateId == candidateId
                )
                .OrderByDescending(i =>
                    i.ScheduledAt
                )
                .ToListAsync();
        }
        public async Task<List<InterviewIdentity>> GetByRecruiterIdAsync(int recruiterId)
        {
            return await _context.Interviews
                .Where(i =>
                    i.RecruiterId == recruiterId
                )
                .OrderByDescending(i =>
                    i.ScheduledAt
                )
                .ToListAsync();
        }
        public async Task<InterviewIdentity?> GetByIdAsync(int interviewId)
        {
            return await _context.Interviews.FindAsync(interviewId);
        }
        public async Task<InterviewIdentity?>GetActiveInterviewByApplicationIdAsync(int applicationId)
        {
            return await _context.Interviews
                .FirstOrDefaultAsync(i =>

                    i.ApplicationId == applicationId

                    &&

                    i.Status !=
                        InterviewStatus.Cancelled
                            .ToString()
                    &&
                    i.Status !=
                        InterviewStatus.Completed
                            .ToString()
                );
        }
        public async Task<List<InterviewIdentity>> FindByApplicationIdAsync(int applicationId)
        {
            return await _context.Interviews
                .Where(i => i.ApplicationId == applicationId)
                .ToListAsync();
        }

        public async Task<List<InterviewIdentity>> FindByStatusAsync(string status)
        {
            return await _context.Interviews
                .Where(i => i.Status == status)
                .ToListAsync();
        }

        public async Task<List<InterviewIdentity>> FindByScheduledAtBetweenAsync(DateTime start, DateTime end)
        {
            return await _context.Interviews
                .Where(i => i.ScheduledAt >= start && i.ScheduledAt <= end)
                .ToListAsync();
        }



        public async Task AddAsync(InterviewIdentity interview)
        {
            await _context.Interviews.AddAsync(interview);

        }

        public async Task UpdateAsync(InterviewIdentity interview)
        {
            _context.Interviews.Update(interview);

        }

        public async Task DeleteAsync(int interviewId)
        {
            var interview = await _context.Interviews.FindAsync(interviewId);
            if (interview != null)
            {
                _context.Interviews.Remove(interview);

            }
        }

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}