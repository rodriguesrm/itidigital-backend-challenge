using AutoFixture.Kernel;
using Iti.Backend.Challenge.Contract.Options;
using Iti.Backend.Challenge.Core.Ports;
using Iti.Backend.Challenge.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Iti.Backend.Challenge.Core.Tests.Core
{
    
    public class PasswordValidateServiceTest : GenericTestBase
    {

        public PasswordValidateServiceTest() : base() { }

        [Theory]
        [InlineData("", false, "PasswordEmpty")]
        [InlineData("aa", false, "RepeatedChar", "LetrasMaiusculas", "Numeros", "CaracteresEspeciais", "Tamanho")]
        [InlineData("ab", false, "LetrasMaiusculas", "Numeros", "CaracteresEspeciais", "Tamanho")]
        [InlineData("7182935", false, "LetrasMinusculas", "LetrasMaiusculas", "CaracteresEspeciais", "Tamanho")]
        [InlineData("71829357", false, "LetrasMinusculas", "LetrasMaiusculas", "CaracteresEspeciais", "RepeatedChar")]
        [InlineData("71829350", false, "LetrasMinusculas", "LetrasMaiusculas", "CaracteresEspeciais")]
        [InlineData("7182#350", false, "LetrasMinusculas", "LetrasMaiusculas")]
        [InlineData("AAAbbbCc", false, "RepeatedChar", "Numeros", "CaracteresEspeciais")]
        [InlineData("AbTp9!foo", false, "RepeatedChar")]
        [InlineData("AbTp9!foA", false, "RepeatedChar")]
        [InlineData("AbTp9! fok", false, "Espaco")]
        [InlineData("AbTp9!fok", true)]
        [InlineData("A1b2#350$", true)]
        public async Task ValidatePassword(string password, bool isValid, params string[] errorsList)
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

        #region Stubs

        private class PasswordValidateConfigurationProviderStub : IPasswordValidateConfigurationProvider
        {
            public Task<IDictionary<string, PasswordValidationRuleOption>> GetConfiguration()
            {

                IDictionary<string, PasswordValidationRuleOption> rules = new Dictionary<string, PasswordValidationRuleOption>
                {
                    { "LetrasMinusculas", new PasswordValidationRuleOption() { Name = "LetrasMinusculas", Regex = "[a-z]", IsValidWHenMatch = true, Message = "A senha deve conter ao menos uma letra minúscula" } },
                    { "LetrasMaiusculas", new PasswordValidationRuleOption() { Name = "LetrasMaiusculas", Regex = "[A-Z]", IsValidWHenMatch = true, Message = "A senha deve conter ao menos uma letra maiúscula" } },
                    { "Numeros", new PasswordValidationRuleOption() { Name = "Numeros", Regex = "[\\d]", IsValidWHenMatch = true, Message = "A senha deve conter ao menos uma letra minúscula" } },
                    { "CaracteresEspeciais", new PasswordValidationRuleOption() { Name = "CaracteresEspeciais", Regex = "[!@#$%^&*()-+]", IsValidWHenMatch = true, Message = "A senha deve conter ao menos um caracter especial válido, são válidos: ! @ # $ % ^ & * ( ) - +" } },
                    { "Espaco", new PasswordValidationRuleOption() { Name = "Espaco", Regex = "[\\s]", IsValidWHenMatch = false, Message = "A senha não deve conter caracteres em branco/espaço" } },
                    { "Tamanho", new PasswordValidationRuleOption() { Name = "Tamanho", Regex = "[\\w\\W\\d]{8,}$", IsValidWHenMatch = true, Message = "A senha deve conter no mínimo 8 posições" } }
                };
                return Task.FromResult(rules);
            }
        }

        #endregion

    }
}
