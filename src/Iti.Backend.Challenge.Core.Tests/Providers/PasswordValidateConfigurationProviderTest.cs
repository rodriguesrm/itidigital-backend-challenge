using AutoFixture;
using Iti.Backend.Challenge.Contract.Options;
using Iti.Backend.Challenge.Provider;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Iti.Backend.Challenge.Core.Tests.Providers
{

    public class PasswordValidateConfigurationProviderTest : GenericTestBase
    {

        public PasswordValidateConfigurationProviderTest() : base() { }

        [Fact]
        public void GetConfiguration_ReturnResult()
        {

            PasswordValidationRuleOption rule = new() { Name = A<string>(), Message = A<string>(), Regex = A<string>() };
            PasswordOption passwordOption = new() { ValidationRules = new List<PasswordValidationRuleOption>() { rule } };
            IOptions<PasswordOption> options = Mock.Of<IOptions<PasswordOption>>(opt => opt.Value == passwordOption);

            PasswordValidateConfigurationProvider provider = new(options);
            IDictionary<string, PasswordValidationRuleOption> result = provider.GetConfiguration().Result;

            Assert.Equal(1, result.Count);
            PasswordValidationRuleOption checkRule = result.FirstOrDefault().Value;
            Assert.NotNull(checkRule);
            Assert.Equal(rule.Name, checkRule.Name);
            Assert.Equal(rule.Regex, checkRule.Regex);
            Assert.Equal(rule.Message, checkRule.Message);
        }

    }
}
