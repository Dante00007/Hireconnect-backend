using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;

public class ApplicationAcceptedConsumer
    : IConsumer<ApplicationAcceptedEvent>
{
    private readonly INotificationService
        _notificationService;

    public ApplicationAcceptedConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            ApplicationAcceptedEvent
        > context
    )
    {
        var message =
            context.Message;

        await _notificationService
            .CreateNotificationAsync(

                message.CandidateId,

                "Application Accepted",

                $"Congratulations! Your application for {message.JobTitle} was accepted.",

                "Application"

            );
    }
}