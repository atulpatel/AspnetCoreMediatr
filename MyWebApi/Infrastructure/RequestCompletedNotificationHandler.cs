using MediatR;
using MembershipEligibilitySearch.Api.PipelineBehavior;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebApi.Infrastructure
{
    public class RequestCompletedNotificationHandler : INotificationHandler<RequestCompletedNotification>
    {
        public Task Handle(RequestCompletedNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(notification.StartTime);
                Console.WriteLine(notification.Request);
                Console.WriteLine(notification.Response);
            });
        }
    }
}