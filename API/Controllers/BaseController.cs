using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Mvc;
using Models.Error;
using Models.User;

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
    }
}