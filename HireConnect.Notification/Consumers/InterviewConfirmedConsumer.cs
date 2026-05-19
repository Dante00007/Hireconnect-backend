using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;

public class InterviewConfirmedConsumer
    : IConsumer<InterviewConfirmedEvent>
{
    private readonly INotificationService
        _notificationService;

    public InterviewConfirmedConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            InterviewConfirmedEvent
        > context
    )
    {
        var message =
            context.Message;

        await _notificationService
            .CreateNotificationAsync(

                message.RecruiterId,

                "Interview Confirmed",

                $"{message.CandidateName} confirmed interview for {message.JobTitle}",

                "Interview"

            );
    }
}