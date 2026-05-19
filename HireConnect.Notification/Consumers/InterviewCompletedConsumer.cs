using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;
public class InterviewCompletedConsumer
    : IConsumer<InterviewCompletedEvent>
{
    private readonly INotificationService
        _notificationService;

    public InterviewCompletedConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            InterviewCompletedEvent
        > context
    )
    {
        var message =
            context.Message;

        // Candidate Notification
        await _notificationService
            .CreateNotificationAsync(

                message.CandidateId,

                "Interview Completed",

                $"Interview completed for {message.JobTitle}",

                "Interview"

            );

        // Recruiter Notification
        await _notificationService
            .CreateNotificationAsync(

                message.RecruiterId,

                "Interview Completed",

                $"Interview completed for {message.JobTitle}",

                "Interview"

            );
    }
}