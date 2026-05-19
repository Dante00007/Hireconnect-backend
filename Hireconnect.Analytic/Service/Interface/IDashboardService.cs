
using HireConnect.Analytic.DTO;

namespace HireConnect.Analytic.Service.Interface;

public interface IDashboardService
{
    Task<RecruiterDashboardDTO> GetRecruiterDashboardAsync(int recruiterId);
}