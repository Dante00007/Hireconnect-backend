using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;
public class InterviewRescheduledConsumer
    : IConsumer<InterviewRescheduledEvent>
{
    private readonly INotificationService
        _notificationService;

    public InterviewRescheduledConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            InterviewRescheduledEvent
        > context
    )
    {
        var message =
            context.Message;

        await _notificationService
            .CreateNotificationAsync(

                message.CandidateId,

                "Interview Rescheduled",

                $"Interview for {message.JobTitle} was rescheduled to {message.NewDateTime:g}",

                "Interview"

            );
    }
}