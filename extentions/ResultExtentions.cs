using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Common;

namespace ToDoList.Extentions
{
    public static class ResultExtention
    {
        public static IActionResult ToAction<T>(this Result<T> result)
        {
            switch (result.Status)
               { 
                case ResultStatus.Ok: return new OkObjectResult(result.Value);
                case ResultStatus.NoContent: return new NoContentResult();
                case ResultStatus.NotFound: return new NotFoundObjectResult(result.Error);
                case ResultStatus.Unauthorized: return new UnauthorizedObjectResult(result.Error);
                case ResultStatus.BadRequest: return new BadRequestObjectResult(result.Error);
                case ResultStatus.Conflict: return new ConflictObjectResult(result.Error);
                case ResultStatus.Forbidden: return new ForbidResult();
                
            }
            return new BadRequestObjectResult(result.Error);
        }
    }
}