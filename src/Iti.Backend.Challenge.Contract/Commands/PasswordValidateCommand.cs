using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.Contract.Commands
{

    /// <summary>
    /// Validate password command contract
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PasswordValidateCommands : IRequest<CommandResult<bool>>
    {

        public PasswordValidateCommands() : this(null) { }

        public PasswordValidateCommands(string password)
        {
            Password = password;
        }

        /// <summary>
        /// Password to validate
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResult<bool> Response { get; set; }

    }
}
