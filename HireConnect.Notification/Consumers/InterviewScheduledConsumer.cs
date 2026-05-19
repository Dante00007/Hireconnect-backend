using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;

public class InterviewScheduledConsumer
    : IConsumer<InterviewScheduledEvent>
{
    private readonly INotificationService
        _notificationService;

    public InterviewScheduledConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            InterviewScheduledEvent
        > context
    )
    {
        var message =
            context.Message;

        await _notificationService
            .CreateNotificationAsync(

                message.CandidateId,

                "Interview Scheduled",

                $"Interview scheduled for {message.JobTitle} on {message.ScheduledAt:g}",

                "Interview"

            );
    }
}