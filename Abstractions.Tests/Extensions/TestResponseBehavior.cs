using Microsoft.Extensions.Logging;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class TestResponseBehavior<TRequest, TResponse> : ResponseBehaviorBase<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        // cmd is optional because not required by WriteJsonExtensionsTests...
        public TestResponseBehavior(ICommand<TResponse> cmd = null, PipelineContext ctx = null, ILogger log = null) : base(cmd, ctx, log)
        {
        }
    }
}
