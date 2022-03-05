using Microsoft.Extensions.Logging;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class WriteJsonResponseProcessor<TRequest, TResponse> : ResponseProcessorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public WriteJsonResponseProcessor(WriteJsonCommand<TResponse> cmd, PipelineContext ctx = null, ILogger log = null) : base(cmd, ctx, log)
        {
        }
    }
}
