using Iti.Backend.Challenge.Contract;
using Iti.Backend.Challenge.Contract.Commands;
using Iti.Backend.Challenge.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Iti.Backend.Challenge.WebApi.Controllers
{

    /// <summary>
    /// Validate api controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {

        private readonly IMediator _mediator;

        /// <summary>
        /// Create a new api-controller instance
        /// </summary>
        /// <param name="mediator">Mediator object</param>
        public ValidateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Perform validate a password
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="detail">Indicates whether or not the return should be detailed</param>
        /// <response code="200">Successful request processing</response>
        /// <response code="400">Password is invalid and return details error validation (when ?detail=true)</response>
        /// <response code="500">Request processing failed</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidatePasswordDetailResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("password")]
        public async Task<IActionResult> ValidatePassword([FromBody] ValidatePasswordRequest request, [FromQuery] bool detail = false)
        {

            PasswordValidateCommands command = new(request.Password);
            CommandResult<bool> result = await _mediator.Send(command);
            
            if (detail && !result.Response)
            {
                ValidatePasswordDetailResponse errorDetails = new()
                {
                    IsValid = result.Response,
                    Errors = result.Errors.Select(err => err.Value).ToList()
                };
                return BadRequest(errorDetails);
            }

            return Ok(result.Response);

        }

    }

}
