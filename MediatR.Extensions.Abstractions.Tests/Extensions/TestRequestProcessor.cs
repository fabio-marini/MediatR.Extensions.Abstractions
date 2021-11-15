using Microsoft.Extensions.Logging;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class TestRequestProcessor<TRequest> : RequestProcessorBase<TRequest>
    {
        // cmd is optional because not required by WriteJsonExtensionsTests...
        public TestRequestProcessor(ICommand<TRequest> cmd = null, PipelineContext ctx = null, ILogger log = null) : base(cmd, ctx, log)
        {
        }
    }
}
