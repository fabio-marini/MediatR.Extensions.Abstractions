# MediatR.Extensions.Abstractions

This repository contains abstractions that can be used to build [MediatR](https://github.com/jbogard/MediatR) extensions, i.e. pipeline behaviors and request pre/post processors:

- [RequestBehaviorBase][1]: implements `IPipelineBehavior<TRequest, TResponse>` and takes an `ICommand<TRequest>`
- [ResponseBehaviorBase][2]: implements `IPipelineBehavior<TRequest, TResponse>` and takes an `ICommand<TResponse>`
- [RequestProcessorBase][3]: implements `IRequestPreProcessor<TRequest>` and takes an `ICommand<TRequest>`
- [ResponseProcessorBase][4]: implements `IRequestPostProcessor<TRequest, TResponse>` and takes an `ICommand<TResponse>`

There are three steps to build an extension:
1. create a generic command by implementing [ICommand&lt;TMessage&gt;][0] - `TMessage` is a MediatR request or response
2. extend the base classes described above and provide constructors to inject the command created above
3. if the command requires options, create an option class and inject it in the command constructor

:warning: The abstractions are designed to allow execution of the pipeline to continue in case of certain errors. This means that:
- exceptions raised by execution of the generic command are caught and logged, but not rethrown
- all other exceptions, i.e. those raised by cancellation and other components further down the chain, are not caught and will bubble up the stack

Keep this in mind if your extension fails without an exception! The error details will be in the logs...

An example is included with the tests project:
1. [WriteJsonCommand][5] implements ICommand&lt;TMessage&gt;
2. [WriteJsonRequestBehavior][6], [WriteJsonResponseBehavior][7], [WriteJsonRequestProcessor][8] and [WriteJsonResponseProcessor][9] extend the base classes and provide constructors to inject [WriteJsonCommand][5]
3. [WriteJsonOptions][10] are injected in the [WriteJsonCommand][5]

Processor extensions are picked up automatically by calling `AddMediatR()`, while behavior extensions must be added manually and in the desired order of execution. See [WriteJsonExtensionsTests][11] for an example.

 [0]: ./MediatR.Extensions.Abstractions/ICommand.cs
 [1]: ./MediatR.Extensions.Abstractions/RequestBehaviorBase.cs
 [2]: ./MediatR.Extensions.Abstractions/ResponseBehaviorBase.cs
 [3]: ./MediatR.Extensions.Abstractions/RequestProcessorBase.cs
 [4]: ./MediatR.Extensions.Abstractions/ResponseProcessorBase.cs
 [5]: ./MediatR.Extensions.Abstractions.Tests/Example/WriteJsonCommand.cs
 [6]: ./MediatR.Extensions.Abstractions.Tests/Example/WriteJsonRequestBehavior.cs
 [7]: ./MediatR.Extensions.Abstractions.Tests/Example/WriteJsonResponseBehavior.cs
 [8]: ./MediatR.Extensions.Abstractions.Tests/Example/WriteJsonRequestProcessor.cs
 [9]: ./MediatR.Extensions.Abstractions.Tests/Example/WriteJsonResponseProcessor.cs
[10]: ./MediatR.Extensions.Abstractions.Tests/Example/WriteJsonOptions.cs
[11]: ./MediatR.Extensions.Abstractions.Tests/Example/WriteJsonExtensionsTests.cs

