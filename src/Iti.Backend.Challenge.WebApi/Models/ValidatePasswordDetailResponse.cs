using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.WebApi.Models
{

    /// <summary>
    /// Validate password detail response
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ValidatePasswordDetailResponse
    {

        /// <summary>
        /// Password valid indicator
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Errors validation list
        /// </summary>
        public IEnumerable<string> Errors{ get; set; }

    }
}
