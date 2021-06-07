using Iti.Backend.Challenge.Contract;
using Iti.Backend.Challenge.Contract.Commands;
using Iti.Backend.Challenge.Core.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Iti.Backend.Challenge.Application.Handlers
{
    public class PasswordValidateCommandHandler : IRequestHandler<PasswordValidateCommands, CommandResult<bool>>
    {

        private readonly IPasswordValidateService _service;
        private readonly ILogger<PasswordValidateCommandHandler> _logger;

        /// <summary>
        /// Craete a new service instance
        /// </summary>
        /// <param name="service">Password validate service object instance</param>
        /// <param name="loggerFactory">Logger factor object</param>
        public PasswordValidateCommandHandler(IPasswordValidateService service, ILoggerFactory loggerFactory)
        {
            _service = service;
            _logger = loggerFactory.CreateLogger<PasswordValidateCommandHandler>();
        }

        /// <summary>
        /// Handle PasswordValidateCommands
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete</param>
        public async Task<CommandResult<bool>> Handle(PasswordValidateCommands request, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"{GetType().Name} HANDLER START");

            (bool success, IDictionary<string, string> errors) = await _service.ValidatePassword(request.Password);

            _logger.LogInformation($"Password is {(success ? "valid": "invalid")}");
            CommandResult<bool> result = new CommandResult<bool>()
            {
                Response = success,
                Errors = errors
            };

            _logger.LogInformation($"{GetType().Name} HANDLER END");

            return result;

        }
    }
}
