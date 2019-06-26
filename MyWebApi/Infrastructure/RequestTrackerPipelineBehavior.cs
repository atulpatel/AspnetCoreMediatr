using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MembershipEligibilitySearch.Api.PipelineBehavior
{
    public class RequestTrackerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMediator _mediator;

        public RequestTrackerPipelineBehavior(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var starttime = DateTime.Now;
            var response = await next();

            await Task.Run(() =>
             {
                 _mediator.Publish(new RequestCompletedNotification
                 {
                     StartTime = starttime,
                     EndTime = DateTime.Now,
                     Request = request,
                     Response = response
                 }, cancellationToken);
             }, cancellationToken);
            return response;
        }
    }
}