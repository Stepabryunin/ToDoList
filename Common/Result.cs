using Microsoft.AspNetCore.Http.HttpResults;

namespace ToDoList.Common
{
    public class Result<T>
    {
        public bool Success{get;set;}
        public ResultStatus Status {get;set;}
        public T? Value {get;set;}
        public string? Error {get;set;}
        public Result(T val)
        {
            Success = true;
            Status= ResultStatus.Ok;
            Value = val;
            Error = null;
        }
        public Result(ResultStatus status, string error)
        {
            Success=false;
            Status = status;
            Error = error;
        }
        private Result()
        {
            Success = true;
            Status = ResultStatus.NoContent;
            Error=null;
        }
        public static Result<T> NoContent() => new();
        public static Result<T> Forbidden()
        {
            var res = new Result<T>();
            res.Status=ResultStatus.Forbidden;
            return res;
        }


    }
    public enum ResultStatus
    {
        Ok,
        NotFound,
        Unauthorized,
        BadRequest,
        Conflict,
        NoContent,
        Forbidden
    }
}