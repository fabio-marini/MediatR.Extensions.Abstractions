using Microsoft.Extensions.Logging;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class WriteJsonResponseBehavior<TRequest, TResponse> : ResponseBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public WriteJsonResponseBehavior(WriteJsonCommand<TResponse> cmd, PipelineContext ctx = null, ILogger log = null)
            : base(cmd, ctx, log)
        {
        }
    }
}
