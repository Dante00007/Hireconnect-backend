namespace HireConnect.Analytics.Models
{
    public class AnalyticsSummary
    {
        public int TotalJobs { get; set; }
        public int TotalApplications { get; set; }
        public int ShortlistedCount { get; set; }
        public int OfferedCount { get; set; }
        public int RejectedCount { get; set; }
        public double AvgTimeToHireDays { get; set; }
        public double ViewToApplyRatio { get; set; }

        public override string ToString()
        {
            return $"Jobs: {TotalJobs}, Apps: {TotalApplications}, Shortlisted: {ShortlistedCount}";
        }
    }
}