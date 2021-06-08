using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.Contract.Options
{

    /// <summary>
    /// Password validation rule configuration option model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PasswordValidationRuleOption
    {

        /// <summary>
        /// Validation name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Regular expression to check password
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// Indicates whether it is valid when the regular expression results in a match or not
        /// </summary>
        public bool IsValidWhenMatch { get; set; } = true;

        /// <summary>
        /// Message to use for not match validation
        /// </summary>
        public string Message { get; set; }

    }
}
