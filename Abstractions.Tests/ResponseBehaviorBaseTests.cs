using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class ResponseBehaviorBaseTests
    {
        private readonly IServiceProvider svc;
        private readonly Mock<ILogger> log;
        private readonly Mock<PipelineContext> ctx;

        public ResponseBehaviorBaseTests()
        {
            log = new Mock<ILogger>();
            ctx = new Mock<PipelineContext>();

            var cmd = new Mock<ICommand<Unit>>();
            var qry = new Mock<ICommand<TestResult>>();

            svc = new ServiceCollection()

                .AddSingleton<Mock<ICommand<Unit>>>(sp => cmd)
                .AddSingleton<Mock<ICommand<TestResult>>>(sp => qry)

                .AddSingleton<ICommand<Unit>>(sp => cmd.Object)
                .AddSingleton<ICommand<TestResult>>(sp => qry.Object)

                .AddTransient<TestResponseBehavior<TestCommand, Unit>>()
                .AddTransient<TestResponseBehavior<TestQuery, TestResult>>()

                .AddTransient<PipelineContext>(sp => ctx.Object)
                .AddTransient<ILogger>(sp => log.Object)

                .BuildServiceProvider();
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { TestCommand.Default, Unit.Value };
            yield return new object[] { TestQuery.Default, TestResult.Default };
        }

        [Theory(DisplayName = "Behavior executes successfully"), MemberData(nameof(TestData))]
        public async Task Test1<TRequest, TResponse>(TRequest req, TResponse res) where TRequest : IRequest<TResponse>
        {
            var commandMock = svc.GetRequiredService<Mock<ICommand<TResponse>>>();

            commandMock.Setup(m => m.ExecuteAsync(res, CancellationToken.None)).Returns(Task.CompletedTask);

            var responseBehavior = svc.GetRequiredService<TestResponseBehavior<TRequest, TResponse>>();

            await responseBehavior.Handle(req, CancellationToken.None, () => Task.FromResult(res));

            var logInvocation = log.Invocations.Where(i => i.Method.Name == "Log").Single();

            logInvocation.Arguments.OfType<LogLevel>().Single().Should().Be(LogLevel.Information);
        }

        [Theory(DisplayName = "Behavior handles exceptions"), MemberData(nameof(TestData))]
        public async Task Test2<TRequest, TResponse>(TRequest req, TResponse res) where TRequest : IRequest<TResponse>
        {
            var commandMock = svc.GetRequiredService<Mock<ICommand<TResponse>>>();

            commandMock.Setup(m => m.ExecuteAsync(res, CancellationToken.None)).Throws(new ArgumentException());

            var responseBehavior = svc.GetRequiredService<TestResponseBehavior<TRequest, TResponse>>();

            await responseBehavior.Handle(req, CancellationToken.None, () => Task.FromResult(res));

            var logInvocation = log.Invocations.Where(i => i.Method.Name == "Log").Single();

            logInvocation.Arguments.OfType<LogLevel>().Single().Should().Be(LogLevel.Error);
            logInvocation.Arguments.OfType<ArgumentException>().Single();

            ctx.VerifyGet(m => m.Exceptions, Times.Once);
        }

        [Theory(DisplayName = "Behavior handles cancellations"), MemberData(nameof(TestData))]
        public async Task Test3<TRequest, TResponse>(TRequest req, TResponse res) where TRequest : IRequest<TResponse>
        {
            var responseBehavior = svc.GetRequiredService<TestResponseBehavior<TRequest, TResponse>>();

            var src = new CancellationTokenSource(0);

            Func<Task> act = async () => await responseBehavior.Handle(req, src.Token, () => Task.FromResult(res));

            await act.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}
