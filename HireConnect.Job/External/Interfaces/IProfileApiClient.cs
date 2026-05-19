using HireConnect.Job.External.DTO;

namespace HireConnect.Job.External.Interfaces;

public interface IProfileApiClient
{
    Task<RecruiterProfileInternalDTO?>
    GetRecruiterProfileAsync(int userId);
}