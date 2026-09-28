using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Application.Dto;

namespace PAK_Zameen.Filters
{
    public class ValidateModelStateFilter : IActionFilter
    {
        /// <summary>
        /// After the action method is called and before the result is executed (before view render)
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// Method call just before the action method is called.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var response = new ResponseDto<List<string>>();
                var list = new List<string>();

                foreach (var k in context.ModelState.Keys)
                {
                    foreach (var e in context.ModelState[k].Errors)
                    {
                        var message = string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage;
                        list.Add(message);
                        System.Console.WriteLine(message);
                    }
                }

                response.Errors = list;
                context.Result = new OkObjectResult(response);
            }
        }
    }
}
