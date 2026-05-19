using HireConnect.Application.External.DTOs;

namespace HireConnect.Application.External.Interfaces;

public interface IProfileApiClient
{
    Task<CandidateProfileDTO?>
    GetCandidateProfileAsync(int userId);
}