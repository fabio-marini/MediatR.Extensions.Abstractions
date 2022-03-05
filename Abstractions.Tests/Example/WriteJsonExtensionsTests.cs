using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace MediatR.Extensions.Abstractions.Tests.Example
{
    public class WriteJsonExtensionsTests
    {
        [Fact(DisplayName = "WriteJson Extensions")]
        public async Task Test()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WriteJson");

            var serviceProvider = new ServiceCollection()

                .AddMediatR(this.GetType())

                .AddOptions<WriteJsonOptions>()
                .Configure(opt => opt.Path = path)
                .Services

                .AddTransient<WriteJsonCommand<TestQuery>>()
                .AddTransient<WriteJsonCommand<TestResult>>()

                // pre/post processors are picked up automatically by AddMediatR() above
                .AddTransient<IPipelineBehavior<TestQuery, TestResult>, WriteJsonRequestBehavior<TestQuery, TestResult>>()
                .AddTransient<IPipelineBehavior<TestQuery, TestResult>, WriteJsonResponseBehavior<TestQuery, TestResult>>()

                .BuildServiceProvider();

            if (Directory.Exists(path) == true)
            {
                Directory.Delete(path, recursive: true);
            }

            Directory.CreateDirectory(path);

            var mediator = serviceProvider.GetRequiredService<IMediator>();

            var req = new TestQuery { Message = "Hello world! :)" };

            var res = await mediator.Send(req);

            res.Length.Should().Be(req.Message.Length);

            // 2 x requests and 2 x responses
            Directory.GetFiles(path).Should().HaveCount(4);
        }
    }
}
