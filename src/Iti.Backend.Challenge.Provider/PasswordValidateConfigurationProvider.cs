using Iti.Backend.Challenge.Contract.Options;
using Iti.Backend.Challenge.Core.Ports;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Iti.Backend.Challenge.Provider
{

    /// <summary>
    /// Password validation configuration provider
    /// </summary>
    public class PasswordValidateConfigurationProvider : IPasswordValidateConfigurationProvider
    {

        private readonly PasswordOption _passwordOptions;

        /// <summary>
        /// Create a new provider instance
        /// </summary>
        /// <param name="passwordOptions">Options for password configuration</param>
        public PasswordValidateConfigurationProvider(IOptions<PasswordOption> passwordOptions)
        {
            _passwordOptions = passwordOptions?.Value;
        }

        ///<inheritdoc/>
        public Task<IDictionary<string, PasswordValidationRuleOption>> GetConfiguration()
        {
            IDictionary<string, PasswordValidationRuleOption> result = new Dictionary<string, PasswordValidationRuleOption>();

            if (_passwordOptions != null)
                result = _passwordOptions.ValidationRules?.ToDictionary(k => k.Regex, v => v);

            return Task.FromResult(result);
        }

    }
}
