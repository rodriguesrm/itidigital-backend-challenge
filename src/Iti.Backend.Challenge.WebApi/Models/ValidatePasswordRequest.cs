using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.WebApi.Models
{

    /// <summary>
    /// Validate password request model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ValidatePasswordRequest
    {

        /// <summary>
        /// Password to validade
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
