using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.HostApi.Filter
{

    /// <summary>
    /// Filter global da aplicação
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class GlobalFilters
    {

        /// <summary>
        /// Configure filters
        /// </summary>
        /// <param name="opt">Mvc options object instance</param>
        public static void Configure(MvcOptions opt)
        {
            opt.Filters.Add<ValidateModelFilter>();
        }
    }

}
