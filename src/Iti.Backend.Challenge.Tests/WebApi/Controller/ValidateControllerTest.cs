using Iti.Backend.Challenge.Application.Handlers;
using Iti.Backend.Challenge.Contract;
using Iti.Backend.Challenge.Contract.Commands;
using Iti.Backend.Challenge.Core.Ports;
using Iti.Backend.Challenge.WebApi.Controllers;
using Iti.Backend.Challenge.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Iti.Backend.Challenge.Core.Tests.WebApi.Controller
{
    public class ValidateControllerTest : GenericTestBase
    {

        public ValidateControllerTest() : base() { }

        [Fact]
        public async Task ValidatePassword_WhenPasswordIsValid_ReturnOk()
        {
            ValidatePasswordRequest request = new() { Password = One<string>() };
            CommandResult<bool> result = new() { Response = true, Errors = new Dictionary<string, string>() };
            Mock<IMediator> mediator = new();
            mediator
                .Setup(_ => _.Send(It.Is<PasswordValidateCommands>(o => o.Password == request.Password), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(result));
            
            ValidateController controller = new(mediator.Object);
            IActionResult endpointResponse = await controller.ValidatePassword(request);
            Assert.NotNull(endpointResponse);
            Assert.IsType<OkObjectResult>(endpointResponse);
            var contentResult = ((OkObjectResult)endpointResponse).Value;
            Assert.IsType<bool>(contentResult);
            Assert.True((bool)contentResult);

        }

        [Fact]
        public async Task ValidatePassword_WhenPasswordIsInValid_And_DetailParameterIsFalse_ReturnFalse()
        {
            ValidatePasswordRequest request = new() { Password = One<string>() };
            CommandResult<bool> result = new() { Response = false, Errors = new Dictionary<string, string>() };
            Mock<IMediator> mediator = new();
            mediator
                .Setup(_ => _.Send(It.Is<PasswordValidateCommands>(o => o.Password == request.Password), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(result));

            ValidateController controller = new(mediator.Object);
            IActionResult endpointResponse = await controller.ValidatePassword(request);
            Assert.NotNull(endpointResponse);
            Assert.IsType<OkObjectResult>(endpointResponse);
            var contentResult = ((OkObjectResult)endpointResponse).Value;
            Assert.IsType<bool>(contentResult);
            Assert.False((bool)contentResult);

        }

        [Fact]
        public async Task ValidatePassword_WhenPasswordIsInValid_And_DetailParameterIsTrue_ReturnFalse()
        {
            ValidatePasswordRequest request = new() { Password = One<string>() };
            IDictionary<string, string> errors = new Dictionary<string, string>
            {
                { "Error1", One<string>() },
                { "Error2", One<string>() }
            };
            CommandResult<bool> result = new() { Response = false, Errors = errors };
            Mock<IMediator> mediator = new();
            mediator
                .Setup(_ => _.Send(It.Is<PasswordValidateCommands>(o => o.Password == request.Password), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(result));

            ValidateController controller = new(mediator.Object);
            IActionResult endpointResponse = await controller.ValidatePassword(request, true);
            Assert.NotNull(endpointResponse);
            Assert.IsType<BadRequestObjectResult>(endpointResponse);
            var contentResult = ((BadRequestObjectResult)endpointResponse).Value;
            Assert.IsType<ValidatePasswordDetailResponse>(contentResult);
            ValidatePasswordDetailResponse contentResponseTyped = contentResult as ValidatePasswordDetailResponse;
            Assert.NotNull(contentResponseTyped);
            Assert.False(contentResponseTyped.IsValid);
            Assert.Equal(2, contentResponseTyped.Errors.Count());
        }

    }
}
