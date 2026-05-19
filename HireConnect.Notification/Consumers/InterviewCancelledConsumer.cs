using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;

public class InterviewCancelledConsumer
    : IConsumer<InterviewCancelledEvent>
{
    private readonly INotificationService
        _notificationService;

    public InterviewCancelledConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            InterviewCancelledEvent
        > context
    )
    {
        var message =
            context.Message;

        await _notificationService
            .CreateNotificationAsync(

                message.CandidateId,

                "Interview Cancelled",

                $"Interview for {message.JobTitle} was cancelled",

                "Interview"

            );
    }
}