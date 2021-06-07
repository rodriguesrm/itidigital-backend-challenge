using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.Contract.Commands
{

    /// <summary>
    /// Validate password command contract
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PasswordValidateCommands : IRequest<CommandResultBase<bool>>
    {

        /// <summary>
        /// Password to validate
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Response data 
        /// </summary>
        public CommandResultBase<bool> Response { get; set; }

    }
}
