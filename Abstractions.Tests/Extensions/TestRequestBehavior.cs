using Microsoft.Extensions.Logging;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class TestRequestBehavior<TRequest, TResponse> : RequestBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        // cmd is optional because not required by WriteJsonExtensionsTests...
        public TestRequestBehavior(ICommand<TRequest> cmd = null, PipelineContext ctx = null, ILogger log = null) : base(cmd, ctx, log)
        {
        }
    }
}
