using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.Contract
{

    /// <summary>
    /// Command result base contract
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CommandResultBase<TResponse>
    {

        /// <summary>
        /// Indicates the success of the operation
        /// </summary>
        public bool Success { get; set; }


        /// <summary>
        /// Errors dictionary
        /// </summary>
        public IDictionary<string, string> Errors { get; set; }

        /// <summary>
        /// Response data object
        /// </summary>
        public TResponse Response { get; set; }

    }
}
