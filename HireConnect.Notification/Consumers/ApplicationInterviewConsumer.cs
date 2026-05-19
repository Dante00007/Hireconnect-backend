using MassTransit;

using HireConnect.Contracts.Events;
using HireConnect.Notification.Service.Interface;

namespace HireConnect.Notification.Consumers;

public class ApplicationInterviewConsumer
    : IConsumer<ApplicationInterviewEvent>
{
    private readonly INotificationService
        _notificationService;

    public ApplicationInterviewConsumer(
        INotificationService notificationService
    )
    {
        _notificationService =
            notificationService;
    }

    public async Task Consume(
        ConsumeContext<
            ApplicationInterviewEvent
        > context
    )
    {
        var message =
            context.Message;

        await _notificationService
            .CreateNotificationAsync(

                message.CandidateId,

                "Application Shortlisted",

                $"Your application for {message.JobTitle} moved to interview stage",

                "Application"

            );
    }
}