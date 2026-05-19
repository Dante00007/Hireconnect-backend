using HireConnect.Analytic.DTO;
using HireConnect.Analytic.External.DTO;
using HireConnect.Analytic.External.Interfaces;
using HireConnect.Analytic.Service.Interface;

public class DashboardService
    : IDashboardService
{
    private readonly IJobApiClient _jobApiClient;

    private readonly IApplicationApiClient _applicationApiClient;

    private readonly IInterviewApiClient _interviewApiClient;

    public DashboardService(IJobApiClient jobApiClient, IApplicationApiClient applicationApiClient, IInterviewApiClient interviewApiClient)
    {
        _jobApiClient = jobApiClient;

        _applicationApiClient = applicationApiClient;

        _interviewApiClient = interviewApiClient;
    }

    public async Task<RecruiterDashboardDTO> GetRecruiterDashboardAsync(int recruiterId)
    {
        // ======================
        // JOBS
        // ======================

        var jobs = await _jobApiClient.GetRecruiterJobsAsync(recruiterId);

        // ======================
        // APPLICATIONS
        // ======================

        var applications = new List<ApplicationSummaryDTO>();

        foreach (var job in jobs)
        {
            var jobApplications = await _applicationApiClient.GetApplicationsByJobAsync(job.JobId);

            applications.AddRange(jobApplications);
        }

        // ======================
        // INTERVIEWS
        // ======================

        var interviews = await _interviewApiClient.GetRecruiterInterviewsAsync(recruiterId);

        // ======================
        // BUILD DASHBOARD
        // ======================

        var dashboard =
            new RecruiterDashboardDTO
            {
                TotalJobs =
                    jobs.Count,

                ActiveJobs =
                    jobs.Count,

                TotalApplications =
                    applications.Count,

                TotalInterviews =
                    interviews.Count,

                AcceptedCandidates =
                    applications.Count(a =>
                        a.Status == "Accepted"
                    ),

                ScheduledInterviews =
                    interviews.Count(i =>
                        i.Status == "Scheduled"
                    )
            };

        // ======================
        // STATUS BREAKDOWN
        // ======================

        dashboard.ApplicationStatusBreakdown = 
                    applications
                        .GroupBy(a => a.Status)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Count()
                        );

        // ======================
        // RECENT APPLICATIONS
        // ======================

        dashboard.RecentApplications =
                applications
                    .OrderByDescending(a =>
                        a.AppliedAt
                    )
                    .Take(5)
                    .Select(a =>
                        new RecentApplicationDTO
                        {
                            ApplicationId =
                                a.ApplicationId,

                            CandidateName =
                                a.CandidateName,

                            JobTitle =
                                a.JobTitle,

                            Status =
                                a.Status,

                            AppliedAt =
                                a.AppliedAt
                        }
                    )
                    .ToList();

        return dashboard;
    }
}