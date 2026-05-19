using HireConnect.Analytic.External.DTO;

namespace HireConnect.Analytic.External.Interfaces;

public interface IInterviewApiClient
{
    Task<List<InterviewSummaryDTO>>
    GetRecruiterInterviewsAsync(int recruiterId);
}