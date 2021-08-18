using System.Threading.Tasks;
using Infrastructure.Error;
using Infrastructure.Result;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<ActionResult> ReturnResult<T, TM>(Task<T> task) where T : ResultContainer<TM>
        {
            var result = await task;
            
            if (result.ErrorType.HasValue)
            {
                if (result.ErrorType == ErrorType.NotFound) 
                    return NotFound();
                
                if (result.ErrorType == ErrorType.BadRequest) 
                    return BadRequest();
            }

            if (result.Data == null) 
                return NoContent();
            
            return Ok(result.Data);
        }
    }
}