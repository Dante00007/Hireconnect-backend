
using System.Security.Claims;
using HireConnect.Profile.DTO;
using HireConnect.Profile.Service.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HireConnect.Profile.Controllers
{

    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IFileService _fileService;

        public ProfileController(IProfileService profileService, IFileService fileService)
        {
            _profileService = profileService;
            _fileService = fileService;
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost("candidate")]
        public async Task<IActionResult> AddCandidate([FromForm] CandidateProfileDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (string.IsNullOrEmpty(dto.FullName))
                return BadRequest(new { message = "Form data mapping failed. Check your Insomnia keys." });
            if (dto.Resume == null || dto.Resume.Length == 0)
                return BadRequest("Resume file is required.");

            await _profileService.AddCandidateProfileAsync(dto, userId);
            return Ok(new { message = "Candidate profile created successfully" });
        }

        [Authorize(Roles = "Candidate")]
        [HttpPut("candidate")]
        public async Task<IActionResult> UpdateCandidateProfile([FromForm] CandidateProfileDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _profileService.UpdateCandidateProfileAsync(dto, userId);
            return Ok(new { message = "Candidate profile updated successfully" });
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost("recruiter")]
        public async Task<IActionResult> AddRecruiter([FromForm] RecruiterProfileDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _profileService.AddRecruiterProfileAsync(dto, userId);
            return Ok(new { message = "Recruiter profile created successfully" });
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPut("recruiter")]
        public async Task<IActionResult> UpdateRecruiterProfile([FromForm] RecruiterProfileDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _profileService.UpdateRecruiterProfileAsync(dto, userId);
            return NoContent();
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserProfileDTO>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var profile = await _profileService.GetProfileByUserIdAsync(userId);

            return Ok(profile);
        }


        // DELETE: api/profile/{userId}
        [Authorize]
        [HttpDelete()]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _profileService.DeleteProfileAsync(userId);
            return Ok(new { message = "Profile deleted successfully" });
        }

        //Only for inter service communication, not exposed to frontend
        [HttpGet("internal/candidate/{userId}")]
        public async Task<ActionResult<CandidateProfileInternalDTO>> GetCandidateProfile(int userId)
        {
            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile == null) return NotFound(new { message = "Candidate profile not found" });

            var candidateProfile = new CandidateProfileInternalDTO
            {
                UserId = profile.UserId,
                FullName = profile.FullName,
                ResumeUrl = profile.ResumeUrl!
            };
            return Ok(candidateProfile);
        }

        [HttpGet("internal/recruiter/{userId}")]
        public async Task<ActionResult<RecruiterProfileInternalDTO>> GetRecruiterProfile(int userId)
        {
            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile == null) return NotFound(new { message = "Recruiter profile not found" });

            var recruiterProfile = new RecruiterProfileInternalDTO
            {
                UserId = profile.UserId,
                FullName = profile.FullName
            };
            return Ok(recruiterProfile);
        }
    }
}