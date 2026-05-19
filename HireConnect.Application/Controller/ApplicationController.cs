using Microsoft.AspNetCore.Mvc;

using HireConnect.Application.Models;
using HireConnect.Application.Service.Interface;
using HireConnect.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HireConnect.Application.Controllers
{

    [ApiController]
    [Route("api/applications")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _appService;

        public ApplicationController(IApplicationService appService)
        {
            _appService = appService;
        }

        [Authorize(Roles = "Candidate")]
        [HttpPost("submit")]
        public async Task<ActionResult<ApplicationResponseDTO>> Submit([FromBody] ApplicationSubmitDTO applicationdto)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID not found in token.");
            }
            int candidateId = int.Parse(userIdClaim);
            var result = await _appService.SubmitApplicationAsync(applicationdto, candidateId);
            return CreatedAtAction(nameof(GetById), new { applicationId = result.ApplicationId }, result);

        }

       
        [Authorize(Roles = "Recruiter")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var recruiterIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(recruiterIdClaim))
            {
                return Unauthorized("Recruiter ID not found in token.");
            }
            int recruiterId = int.Parse(recruiterIdClaim);

            var result = await _appService.UpdateStatusAsync(id, status, recruiterId);
            return Ok(result);
        }

        [Authorize(Roles = "Candidate")]
        [HttpDelete("{applicationId}")]
        public async Task<ActionResult> Withdraw(int applicationId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID not found in token.");
            }
            int candidateId = int.Parse(userIdClaim);
            await _appService.WithdrawApplicationAsync(applicationId, candidateId);
            return Ok(new { message = "Application withdrawn successfully" });
        }

        [Authorize(Roles = "Candidate")]
        [HttpGet("candidate")]
        public async Task<ActionResult<IEnumerable<ApplicationResponseDTO>>> GetByCandidate()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID not found in token.");
            }
            int candidateId = int.Parse(userIdClaim);
            if (candidateId <= 0)
            {
                return BadRequest("Invalid candidate ID.");
            }
            var applications = await _appService.GetByCandidateIdAsync(candidateId);
            return Ok(applications);
        }


        [HttpGet("job/{jobId}")]
        public async Task<ActionResult<IEnumerable<ApplicationResponseDTO>>> GetByJob(int jobId)
        {
            var applications = await _appService.GetByJobIdAsync(jobId);
            return Ok(applications);
        }

        [Authorize(Roles = "Recruiter,Candidate ")]
        [HttpGet("{applicationId}")]
        public async Task<ActionResult<ApplicationResponseDTO>> GetById(int applicationId)
        {
            var application = await _appService.GetByIdAsync(applicationId);
            if (application == null) return NotFound();
            return Ok(application);
        }



        //internal api for fetching candidate details in application summary for recruiter dashboard

        [HttpGet("internal/{applicationId}")]
        public async Task<ActionResult<ApplicationInternalResponseDTO>> GetInternalDetails(int applicationId)
        {
            var application = await _appService.GetByIdAsync(applicationId);
            if (application == null) return NotFound();
            var internalDetails = new ApplicationInternalResponseDTO
            {
                ApplicationId = application.ApplicationId,
                CandidateId = application.CandidateId,
                CandidateName = application.CandidateName,
                JobTitle = application.JobTitle
            };
            return Ok(internalDetails);
        }
        [HttpGet("internal/job/{jobId}")]
        public async Task<ActionResult<IEnumerable<ApplicationSummaryInternalDTO>>> GetByJobForInternal(int jobId)
        {
            var applications = await _appService.GetByJobIdAsync(jobId);
            var internalSummaries = applications.Select(app => new ApplicationSummaryInternalDTO
            {
                ApplicationId = app.ApplicationId,
                JobId = jobId,
                CandidateId = app.CandidateId,
                CandidateName = app.CandidateName,
                JobTitle = app.JobTitle,
                Status = app.Status,
                AppliedAt = app.AppliedAt
            }).ToList();
            if (internalSummaries == null)
            {
                return NotFound();
            }
            return Ok(internalSummaries);
        }

    }
}