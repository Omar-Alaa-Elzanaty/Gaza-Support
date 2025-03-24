using FluentValidation.Results;
using Gaza_Support.Domains.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Gaza_Support.Domains.Dtos.ResponseDtos
{
    public class BaseResponse<T>
    {
        public bool Status => (int)StatusCode >= 200 && (int)StatusCode <= 200;
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }

        public static BaseResponse<T> Success()
        {
            return new()
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        public static BaseResponse<T> Success(T data, string? message = null)
        {
            return new()
            {
                Data = data,
                Message = message,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static BaseResponse<T> Success(string message)
        {
            return new()
            {
                StatusCode = HttpStatusCode.OK,
                Message = message
            };
        }

        public static BaseResponse<T> Success(T data, string message, HttpStatusCode statusCode)
        {
            return new()
            {
                Data = data,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static BaseResponse<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new()
            {
                Message = message,
                StatusCode = statusCode
            };
        }

        public static BaseResponse<T> ValidationFailure(IEnumerable<ValidationFailure> errors)
        {
            return new()
            {
                Errors = errors.GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity
            };
        }

        public static BaseResponse<T> ValidationFailure(IEnumerable<ValidationFailure> errors, string message)
        {
            return new()
            {
                Errors = errors.GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = message
            };
        }

        public static BaseResponse<T> ValidationFailure(IEnumerable<IdentityError> errors)
        {
            return new()
            {
                Errors = errors.GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity
            };
        }

        public static BaseResponse<T> ValidationFailure(IEnumerable<IdentityError> errors, string message)
        {
            return new()
            {
                Errors = errors.GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = message
            };
        }
    }
}
