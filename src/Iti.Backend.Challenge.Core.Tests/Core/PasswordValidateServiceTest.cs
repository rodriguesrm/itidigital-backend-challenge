using Iti.Backend.Challenge.Contract.Options;
using Iti.Backend.Challenge.Core.Ports;
using Iti.Backend.Challenge.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Iti.Backend.Challenge.Core.Tests.Core
{

    public class PasswordValidateServiceTest : GenericTestBase
    {

        public PasswordValidateServiceTest() : base() { }

        [Theory]
        [InlineData("", false, "PasswordEmpty")]
        [InlineData("aa", false, "RepeatedChar", "CapitalLetters", "Numbers", "SpecialCharacters", "Length")]
        [InlineData("ab", false, "CapitalLetters", "Numbers", "SpecialCharacters", "Length")]
        [InlineData("7182935", false, "SmallLetters", "CapitalLetters", "SpecialCharacters", "Length")]
        [InlineData("71829357", false, "SmallLetters", "CapitalLetters", "SpecialCharacters", "RepeatedChar")]
        [InlineData("71829350", false, "SmallLetters", "CapitalLetters", "SpecialCharacters")]
        [InlineData("7182#350", false, "SmallLetters", "CapitalLetters")]
        [InlineData("AAAbbbCc", false, "RepeatedChar", "Numbers", "SpecialCharacters")]
        [InlineData("AbTp9!foo", false, "RepeatedChar")]
        [InlineData("AbTp9!foA", false, "RepeatedChar")]
        [InlineData("AbTp9! fok", false, "Space")]
        [InlineData("AbTp9!fok", true)]
        [InlineData("A1b2#350$", true)]
        public async Task ValidatePassword_WhenConfiguraitonIsOk_ReturnResult(string password, bool isValid, params string[] errorsList)
        {

            IPasswordValidateConfigurationProvider provider = new PasswordValidateConfigurationProviderStub();
            PasswordValidateService service = new(provider);

            (bool isValidResult, IDictionary<string, string> errorsResult) = await service.ValidatePassword(password);

            Assert.Equal(isValid, isValidResult);
            Assert.Equal(errorsList.Length, errorsResult.Count);

            if (errorsList.Length > 0)
                foreach (string error in errorsList)
                    Assert.True(errorsResult.ContainsKey(error));

        }

        [Fact]
        public void ValidatePassword_WhenConfigurationNotExists_ThrowException()
        {

            string password = One<string>();

            IPasswordValidateConfigurationProvider provider = new PasswordValidateConfigurationProviderStub(false);
            PasswordValidateService service = new(provider);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.ValidatePassword(password));

        }

        #region Stubs

        private class PasswordValidateConfigurationProviderStub : IPasswordValidateConfigurationProvider
        {

            private bool _withResults;

            public PasswordValidateConfigurationProviderStub(bool withResults = true)
            {
                _withResults = withResults;
            }

            public Task<IDictionary<string, PasswordValidationRuleOption>> GetConfiguration()
            {
                IDictionary<string, PasswordValidationRuleOption> rules = new Dictionary<string, PasswordValidationRuleOption>();
                if (_withResults)
                {
                    rules = new Dictionary<string, PasswordValidationRuleOption>
                    {
                        { "SmallLetters", new PasswordValidationRuleOption() { Name = "SmallLetters", Regex = "[a-z]", IsValidWHenMatch = true, Message = "Password must contain at least one lowercase letter" } },
                        { "CapitalLetters", new PasswordValidationRuleOption() { Name = "CapitalLetters", Regex = "[A-Z]", IsValidWHenMatch = true, Message = "Password must contain at least one capital letter" } },
                        { "Numbers", new PasswordValidationRuleOption() { Name = "Numbers", Regex = "[\\d]", IsValidWHenMatch = true, Message = "Password must contain numbers" } },
                        { "SpecialCharacters", new PasswordValidationRuleOption() { Name = "SpecialCharacters", Regex = "[!@#$%^&*()-+]", IsValidWHenMatch = true, Message = "Password must contain at least one valid special character, the following are valid: ! @ # $ % ^ & * ( ) - +" } },
                        { "Length", new PasswordValidationRuleOption() { Name = "Length", Regex = "[\\w\\W\\d]{8,}$", IsValidWHenMatch = true, Message = "Password must have a minimum length of 8 characters" } },
                        { "Space", new PasswordValidationRuleOption() { Name = "Space", Regex = "[\\s]", IsValidWHenMatch = false, Message = "Password must not contain space characters" } }
                    };
                }
                return Task.FromResult(rules);
            }
        }

        #endregion

    }
}
