using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;

public class ApplicationSubmittedConsumer
    : IConsumer<ApplicationSubmittedEvent>
{
    private readonly INotificationService
        _notificationService;

    public ApplicationSubmittedConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            ApplicationSubmittedEvent
        > context
    )
    {
        var message =
            context.Message;

        await _notificationService
            .CreateNotificationAsync(

                message.RecruiterId,

                "New Application",

                $"{message.CandidateName} applied for {message.JobTitle}",

                "Application"

            );
    }
}