using Microsoft.AspNetCore.Mvc;
using HireConnect.Interview.Models;
using HireConnect.Interview.Service.Interface;
using HireConnect.Interview.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HireConnect.Interview.Controllers
{
    [ApiController]
    [Route("api/interviews")]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _interviewService;

        public InterviewController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        public async Task<ActionResult<InterviewResponseDTO>> Schedule([FromBody] InterviewDto interviewDTO)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user!);
            var createdInterview = await _interviewService.ScheduleInterviewAsync(interviewDTO, userId);
            return CreatedAtAction(nameof(GetByApplication),
                new { applicationId = createdInterview.ApplicationId }, createdInterview);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPut("{interviewId}/reschedule")]
        public async Task<ActionResult<InterviewResponseDTO>> Reschedule(int interviewId, [FromBody] RescheduleDto rescheduleDto)
        {

            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user!);
            var updated = await _interviewService.RescheduleInterviewAsync(interviewId, userId, rescheduleDto);
            return Ok(updated);


        }
        [Authorize(Roles = "Recruiter")]
        [HttpDelete("{interviewId}/cancel")]
        public async Task<ActionResult<InterviewResponseDTO>> Cancel(int interviewId)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user!);
            var result = await _interviewService.CancelInterviewAsync(interviewId, userId);
            return Ok(result);
        }


        [Authorize(Roles = "Candidate")]
        [HttpPut("{interviewId}/confirm")]
        public async Task<ActionResult<InterviewResponseDTO>> Confirm(int interviewId)
        {

            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(user!);
            var result = await _interviewService.ConfirmInterviewAsync(interviewId, userId);
            return Ok(result);

        }



        [Authorize(Roles = "Candidate")]
        [HttpGet("candidate")]
        public async Task<ActionResult<IEnumerable<InterviewResponseDTO>>> GetCandidateInterviews()
        {
            var candidateId =
                int.Parse(
                    User.FindFirst(
                        ClaimTypes.NameIdentifier
                    )!.Value
                );

            var interviews = await _interviewService.GetCandidateInterviewsAsync(candidateId);

            return Ok(interviews);
        }
        [Authorize(Roles = "Recruiter")]
        [HttpGet("recruiter")]
        public async Task<ActionResult<IEnumerable<InterviewResponseDTO>>> GetRecruiterInterviews()
        {
            var recruiterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var interviews = await _interviewService.GetRecruiterInterviewsAsync(recruiterId);

            return Ok(interviews);
        }

        [Authorize]
        [HttpGet("application/{applicationId}")]
        public async Task<ActionResult<IEnumerable<InterviewResponseDTO>>> GetByApplication(int applicationId)
        {
            var interviews = await _interviewService.GetByApplicationAsync(applicationId);
            return Ok(interviews);
        }


        //Only for internal use by analytic service, not exposed to external clients
        [HttpGet("internal/recruiter/{recruiterId}")]
        public async Task<ActionResult<IEnumerable<InterviewSummaryInternalDTO>>> GetRecruiterInterviewsForInternal(int recruiterId)
        {
            var interviews = await _interviewService.GetRecruiterInterviewsAsync(recruiterId);
            var interviewInternalResponses = new List<InterviewSummaryInternalDTO>();
            foreach (var interview in interviews)
            {
                interviewInternalResponses.Add(new InterviewSummaryInternalDTO
                {
                    InterviewId = interview.InterviewId,
                    ApplicationId = interview.ApplicationId,
                    CandidateName = interview.CandidateName,
                    JobTitle = interview.JobTitle,
                    ScheduledAt = interview.ScheduledAt,
                    Status = interview.Status
                });
            }
            return Ok(interviewInternalResponses);
        }
      
    }
}