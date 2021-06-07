using Iti.Backend.Challenge.Contract.Options;
using Iti.Backend.Challenge.Core.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Iti.Backend.Challenge.Core.Services
{

    /// <summary>
    /// Password validate service
    /// </summary>
    public class PasswordValidateService : IPasswordValidateService
    {

        private readonly IPasswordValidateConfigurationProvider _provider;

        /// <summary>
        /// Create a new service instance
        /// </summary>
        /// <param name="provider">Password validate configuration provider instance</param>
        public PasswordValidateService(IPasswordValidateConfigurationProvider provider)
        {
            _provider = provider;
        }

        ///<inheritdoc/>
        public async Task<(bool, IDictionary<string, string>)> ValidatePassword(string password)
        {

            IDictionary<string, string> errors = new Dictionary<string, string>();
            IDictionary<string, PasswordValidationRuleOption> configurations = await _provider.GetConfiguration();

            if (password.Length == 0)
            {
                errors.Add("PasswordEmpty", "Password cannot be empty");
            }
            else
            {

                if (configurations?.Count == 0)
                    throw new InvalidOperationException("There are no password validation rules defined");

                foreach (KeyValuePair<string, PasswordValidationRuleOption> config in configurations)
                {
                    PasswordValidationRuleOption rule = config.Value;
                    bool isMatch = Regex.IsMatch(password, rule.Regex);
                    if (!(isMatch == rule.IsValidWHenMatch))
                        errors.Add(rule.Name, rule.Message);
                }

                //Check chars repetead
                bool isRepeatedChar =
                    password
                        .ToCharArray()
                        .ToList()
                        .GroupBy(g => g)
                        .Select(s => new { Character = s.Key, Count = s.Count() })
                        .Where(x => x.Count > 1)
                        .Any();

                if (isRepeatedChar)
                    errors.Add("RepeatedChar", "Password must not contain repeated characters");

            }

            return (errors.Count == 0, errors);

        }
    }
}
