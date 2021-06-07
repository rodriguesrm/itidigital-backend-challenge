using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.Contract
{

    /// <summary>
    /// Command result base contract
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CommandResult<TResponse>
    {

        /// <summary>
        /// Response data object
        /// </summary>
        public TResponse Response { get; set; }

        /// <summary>
        /// Errors dictionary
        /// </summary>
        public IDictionary<string, string> Errors { get; set; }

    }
}
