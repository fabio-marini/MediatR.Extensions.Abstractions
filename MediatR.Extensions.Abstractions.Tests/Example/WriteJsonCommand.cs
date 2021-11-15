using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Extensions.Abstractions.Tests
{
    public class WriteJsonCommand<TMessage> : ICommand<TMessage>
    {
        private readonly IOptions<WriteJsonOptions> opt;

        public WriteJsonCommand(IOptions<WriteJsonOptions> opt)
        {
            this.opt = opt;
        }

        public async Task ExecuteAsync(TMessage message, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(message);

            var fullName = Path.Combine(opt.Value.Path, Guid.NewGuid().ToString() + ".json");

            await File.WriteAllTextAsync(fullName, json, cancellationToken);
        }
    }
}
