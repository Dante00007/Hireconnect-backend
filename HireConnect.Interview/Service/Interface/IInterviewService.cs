using HireConnect.Interview.DTO;


namespace HireConnect.Interview.Service.Interface
{
    public interface IInterviewService
    {
        Task<InterviewResponseDTO> ScheduleInterviewAsync(InterviewDto interviewdto,int userId);
        Task<InterviewResponseDTO> ConfirmInterviewAsync(int interviewId,int userId);
        Task<InterviewResponseDTO> RescheduleInterviewAsync(int interviewId,int userId ,RescheduleDto rescheduleDto);
        Task<InterviewResponseDTO> CancelInterviewAsync(int interviewId,int userId);

        Task<IEnumerable<InterviewResponseDTO>> GetCandidateInterviewsAsync(int candidateId);
        Task<IEnumerable<InterviewResponseDTO>> GetRecruiterInterviewsAsync(int recruiterId);
        Task<IEnumerable<InterviewResponseDTO>> GetByApplicationAsync(int applicationId);

        Task<InterviewResponseDTO?> GetByIdAsync(int interviewId);
    }
}