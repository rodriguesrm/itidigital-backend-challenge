using Iti.Backend.Challenge.HostApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Iti.Backend.Challenge.HostApi.Filter
{

    /// <summary>
    /// Validation filter of request models
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ValidateModelFilter : IActionFilter
    {

        ///<inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext ctx) { }

        ///<inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext ctx)
        {
            if (!ctx.ModelState.IsValid)
            {

                IEnumerable<GenericNotificationResponse> messages = ctx.ModelState
                    .Where(x => x.Value.Errors.Count() > 0)
                    .ToDictionary
                    (
                        k => k.Key,
                        v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    ).Select(itm => new GenericNotificationResponse(itm.Key, string.Join('|', itm.Value)))
                    .ToList();

                BadRequestObjectResult result = new BadRequestObjectResult(messages);
                ctx.Result = result;

            }
        }
    }


}
