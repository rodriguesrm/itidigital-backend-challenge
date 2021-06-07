using Iti.Backend.Challenge.Contract.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iti.Backend.Challenge.Core.Ports
{

    /// <summary>
    /// Password validation configuration provider contract interdace
    /// </summary>
    public interface IPasswordValidateConfigurationProvider
    {

        /// <summary>
        /// Get configuration rows
        /// </summary>
        Task<IDictionary<string, PasswordValidationRuleOption>> GetConfiguration();

    }
}
