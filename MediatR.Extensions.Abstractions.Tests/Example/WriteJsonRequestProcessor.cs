using Microsoft.Extensions.Logging;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class WriteJsonRequestProcessor<TRequest> : RequestProcessorBase<TRequest>
    {
        public WriteJsonRequestProcessor(WriteJsonCommand<TRequest> cmd, PipelineContext ctx = null, ILogger log = null) : base(cmd, ctx, log)
        {
        }
    }
}
