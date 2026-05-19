
using Microsoft.AspNetCore.Mvc;
using HireConnect.Job.Models;
using HireConnect.Job.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using HireConnect.Job.DTO;
using System.Security.Claims;

namespace HireConnect.Job.Controllers
{
    [ApiController]
    [Route("api/job")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        public async Task<ActionResult> AddJob([FromBody] JobDTO job)
        {
            var postBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            await _jobService.AddJobAsync(postBy, job);
            return Ok(new { message = "Job posted successfully" });
        }


        [Authorize(Roles = "Recruiter")]
        [HttpPut("{jobId}")]
        public async Task<ActionResult> UpdateJob(int jobId, [FromBody] JobDTO jobdto)
        {
            var postBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var updatedJob = await _jobService.UpdateJobAsync(jobdto, jobId, postBy);
            return Ok(updatedJob);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJob(int id)
        {
            var recruiterIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            await _jobService.DeleteJobAsync(id, recruiterIdFromToken);
            return Ok(new { message = "Job deleted successfully" });
        }

        [Authorize(Roles = "Recruiter")]
        [HttpGet("myJobs")]
        public async Task<ActionResult<IEnumerable<JobResponseDTO>>> GetJobsByPostedBy()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID not found in token.");
            }

            int recruiterId = int.Parse(userIdClaim);
            var jobs = await _jobService.GetJobsByPostedByAsync(recruiterId);

            return Ok(jobs);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobResponseDTO>>> GetAllJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JobResponseDTO>> GetJobById(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<JobResponseDTO>>> SearchJobs(
            [FromQuery] string? title,
            [FromQuery] string? location,
            [FromQuery] double minSalary = 0,
            [FromQuery] double maxSalary = double.MaxValue)
        {
            var jobs = await _jobService.SearchJobsAsync(title ?? "", location ?? "", minSalary, maxSalary);
            return Ok(jobs);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<JobResponseDTO>>> GetJobsByCategory(string category)
        {
            var jobs = await _jobService.GetJobsByCategoryAsync(category);
            return Ok(jobs);
        }

        [HttpGet("location/{location}")]
        public async Task<ActionResult<IEnumerable<JobResponseDTO>>> GetJobsByLocation(string location)
        {
            var jobs = await _jobService.GetJobsByLocationAsync(location);
            return Ok(jobs);
        }


        //internal APIs for analytic service
        [HttpGet("internal/{jobId}")]
        public async Task<ActionResult<JobResponseInternalDTO>> GetJobByIdInternal(int jobId)
        {
            var job = await _jobService.GetJobByIdAsync(jobId);
            if (job == null) return NotFound();

            var jobInternal = new JobResponseInternalDTO
            {
                JobId = job.JobId,
                Title = job.Title,
                PostedBy = job.PostedBy,
                RecruiterName = job.PostedByName
            };

            return Ok(jobInternal);
        }

        [HttpGet("internal/myJobs/{recruiterId}")]
        public async Task<ActionResult<IEnumerable<JobSummaryInternalDTO>>> GetJobsByPostedByForInternal(int recruiterId)
        {
            var jobs = await _jobService.GetJobsByPostedByAsync(recruiterId);

            var jobsInternal = jobs.Select(job => new JobSummaryInternalDTO
            {
                JobId = job.JobId,
                Title = job.Title,
                Category = job.Category,
                Type = job.Type,
                Location = job.Location,
                CreatedAt = job.CreatedAt
            });
            return Ok(jobsInternal);
        }
    }
}