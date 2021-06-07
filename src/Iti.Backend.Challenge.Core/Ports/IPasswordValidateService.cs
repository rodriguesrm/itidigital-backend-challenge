using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iti.Backend.Challenge.Core.Ports
{

    /// <summary>
    /// Password validate service interface contract
    /// </summary>
    public interface IPasswordValidateService
    {

        /// <summary>
        /// Validate password
        /// </summary>
        /// <param name="password">Password to validade</param>
        public Task<(bool, IDictionary<string, string>)> ValidatePassword(string password);

    }
}
