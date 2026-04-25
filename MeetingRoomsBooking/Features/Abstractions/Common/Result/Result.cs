using MeetingRoomsBooking.Features.Abstractions.Common.Errors;

namespace MeetingRoomsBooking.Features.Abstractions.Common.Result
{
    public class Result<T>
    {
        public ResultStatus Status { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }
        public bool IsSuccess => Status is ResultStatus.Ok or ResultStatus.Created;

        public static Result<T> Ok(T data) => new() { Status = ResultStatus.Ok, Data = data };
        public static Result<T> Created(T data) => new() { Status = ResultStatus.Created, Data = data };

        public static Result<T> Conflict(string code, string message) 
            => new() { Status = ResultStatus.Conflict, Error = new ApiError(code, message) };
        public static Result<T> BadRequest(string code, string message)
            => new() { Status = ResultStatus.BadRequest, Error = new ApiError(code, message) };
        public static Result<T> NotFound(string code, string message)
            => new() { Status = ResultStatus.NotFound, Error = new ApiError(code, message), };
        public static Result<T> Unauthorized(string code, string message) =>
            new() { Status = ResultStatus.Unauthorized, Error = new ApiError(code, message) };
    }
}
