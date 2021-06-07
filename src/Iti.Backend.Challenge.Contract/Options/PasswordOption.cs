using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.Contract.Options
{

    /// <summary>
    /// Password options configuration model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PasswordOption
    {

        /// <summary>
        /// Password validation rules
        /// </summary>
        public IEnumerable<PasswordValidationRuleOption> ValidationRules { get; set; }

    }
}
