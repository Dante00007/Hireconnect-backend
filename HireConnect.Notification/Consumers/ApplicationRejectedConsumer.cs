using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;

public class ApplicationRejectedConsumer
    : IConsumer<ApplicationRejectedEvent>
{
    private readonly INotificationService
        _notificationService;

    public ApplicationRejectedConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            ApplicationRejectedEvent
        > context
    )
    {
        var message =
            context.Message;

        await _notificationService
            .CreateNotificationAsync(

                message.CandidateId,

                "Application Rejected",

                $"Your application for {message.JobTitle} was not selected",

                "Application"

            );
    }
}