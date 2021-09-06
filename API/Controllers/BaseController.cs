using System;
using System.Threading.Tasks;
using Infrastructure.Core.Result;
using Infrastructure.Error;
using Infrastructure.Result;
using Microsoft.AspNetCore.Mvc;
using Models.Core.Error;

namespace API.Controllers
{
    public class BaseController : Controller
    {
        public async Task<ActionResult> ReturnResult<T, TM>(Task<T> task) where T : ResultContainer<TM>
        {
            var result = await task;
            
            if (result.ErrorType.HasValue)
            {
                switch (result.ErrorType)
                {
                    case ErrorType.NotFound:
                        return NotFound();
                    case ErrorType.BadRequest:
                        return BadRequest();
                }
            }

            if (result.Data == null) 
                return NoContent();
            
            return Ok(result.Data);
        }
        
        /// <summary>
        /// Response
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Container With result</returns>
        protected async Task<ActionResult<T>> ProcessResultAsync<T>(Func<Task<Result<T>>> func)
        {
            try
            {
                var res = await func();
                if(res.IsSuccess)
                    return Ok(res.Some);

                return BadRequest(res.Error);
            }
            catch (Exception e)
            {
                return BadRequest(Result<T>.FromIError(new ExceptionError(e)));
            }
        }
        
        /// <summary>
        /// Response
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Container With result</returns>
        protected ActionResult<T> ProcessResult<T>(Func<Result<T>> func)
        {
            try
            {
                var res = func();
                if(res.IsSuccess)
                    return Ok(res.Some);

                return BadRequest(res.Error);
            }
            catch (Exception e)
            {
                return BadRequest(Result<T>.FromIError(new ExceptionError(e)));
            }
        }
    }
}