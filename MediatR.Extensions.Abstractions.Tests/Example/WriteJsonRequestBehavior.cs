using Microsoft.Extensions.Logging;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class WriteJsonRequestBehavior<TRequest> : RequestBehaviorBase<TRequest, Unit> where TRequest : IRequest<Unit>
    {
        public WriteJsonRequestBehavior(WriteJsonCommand<TRequest> cmd, PipelineContext ctx = null, ILogger log = null)
            : base(cmd, ctx, log)
        {
        }
    }

    public class WriteJsonRequestBehavior<TRequest, TResponse> : RequestBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public WriteJsonRequestBehavior(WriteJsonCommand<TRequest> cmd, PipelineContext ctx = null, ILogger log = null) 
            : base(cmd, ctx, log)
        {
        }
    }
}
