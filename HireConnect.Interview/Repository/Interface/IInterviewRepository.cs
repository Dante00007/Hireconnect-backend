using HireConnect.Interview.Models;

namespace HireConnect.Interview.Repository.Interface
{
    public interface IInterviewRepository
    {
        Task<List<InterviewIdentity>>GetByCandidateIdAsync(int candidateId);
        Task<List<InterviewIdentity>>GetByRecruiterIdAsync(int recruiterId);
        Task<InterviewIdentity?> GetByIdAsync(int interviewId);
        Task<InterviewIdentity?> GetActiveInterviewByApplicationIdAsync(int applicationId);

        Task<List<InterviewIdentity>> FindByApplicationIdAsync(int applicationId);
        Task<List<InterviewIdentity>> FindByStatusAsync(string status);
        Task<List<InterviewIdentity>> FindByScheduledAtBetweenAsync(DateTime start, DateTime end);
        
        Task AddAsync(InterviewIdentity interview);
        Task UpdateAsync(InterviewIdentity interview);
        Task DeleteAsync(int interviewId);
        Task<bool> SaveChangesAsync();
    }
}