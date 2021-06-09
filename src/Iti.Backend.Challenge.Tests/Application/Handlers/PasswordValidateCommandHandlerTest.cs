using Iti.Backend.Challenge.Application.Handlers;
using Iti.Backend.Challenge.Contract;
using Iti.Backend.Challenge.Contract.Commands;
using Iti.Backend.Challenge.Core.Ports;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Iti.Backend.Challenge.Core.Tests.Application.Handlers
{

    public class PasswordValidateCommandHandlerTest : GenericTestBase
    {

        private static IList<string> _logs = new List<string>();

        public PasswordValidateCommandHandlerTest() : base() { }

        [Fact]
        public async Task HandleCommand_ResultNoExceptions()
        {

            _logs.Clear();

            Mock<IPasswordValidateService> service = new();
            service
                .Setup(_ => _.ValidatePassword(It.IsAny<string>()))
                .ReturnsAsync((true, new Dictionary<string, string>()));

            Mock<ILoggerFactory> loggerFactory = new();
            loggerFactory
                .Setup(_ => _.CreateLogger(It.IsAny<string>()))
                .Returns(new LoggerStub());

            PasswordValidateCommandHandler handler = new(service.Object, loggerFactory.Object);
            PasswordValidateCommands command = new("r5t6.P0O9");

            CommandResult<bool> handleResult = await handler.Handle(command, default);

            Assert.NotNull(handleResult);
            Assert.True(handleResult.Response);
            Assert.Equal(0, handleResult.Errors.Count);
            Assert.Equal(3, _logs.Count);

        }

        #region Stubs

        [ExcludeFromCodeCoverage]
        private class LoggerStub : ILogger, IDisposable
        {
            public IDisposable BeginScope<TState>(TState state)
                => this;

            public void Dispose() { /* DO NOTHING */ }

            public bool IsEnabled(LogLevel logLevel)
                => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                _logs.Add(state.ToString());
            }
        }


        #endregion

    }
}
