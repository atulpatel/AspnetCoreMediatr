using MediatR.Pipeline;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebApi.Infrastructure
{
    public class RequestStartTimePipelineBehavior<TRequest> : IRequestPreProcessor<TRequest>
    {
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (request is QueryBase)
                {
                    (request as QueryBase).StartExecutionTime = DateTime.Now;
                }
            });
        }
    }
}
