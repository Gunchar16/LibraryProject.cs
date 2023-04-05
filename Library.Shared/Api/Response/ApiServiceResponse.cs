using Library.Shared.Api.Response;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Library.Shared.Api.Response
{
    public abstract class ApiServiceResponse
    {
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public ApiStatus State { get; set; }


    }

    public class ApiServiceResponse<T> : ApiServiceResponse
    {
        public ApiServiceResponse() { }
        public ApiServiceResponse(T data, ApiServiceResponse response)
        {
            base.State = response.State;
            base.ErrorCode = response.ErrorCode;
            base.Message = response.Message;
            Data = data;
        }
        public T Data { get; protected set; }
    }

    public class ValidationFailedApiServiceResponse : ApiServiceResponse
    {
        public ValidationFailedApiServiceResponse()
        {
            State = ApiStatus.ValidationFailed;
            ErrorCode = ResponseErrorCode.ValidationFail;
            Message = $"Input validation failed";
        }

        public ValidationFailedApiServiceResponse(string param, string errorCode = ResponseErrorCode.ValidationFail)
        {
            State = ApiStatus.ValidationFailed;
            ErrorCode = errorCode;
            Message = $"Invalid parameter '{param}'";
        }
    }
    public class ValidationFailedApiServiceResponse<T> : ApiServiceResponse<T>
    {
        public ValidationFailedApiServiceResponse(string message = null, string errorCode = ResponseErrorCode.ValidationFail)
        {
            State = ApiStatus.ValidationFailed;
            ErrorCode = errorCode;
            Message = message;
        }
    }




    public class SuccessApiServiceResponse : ApiServiceResponse
    {
        public SuccessApiServiceResponse(string message = null)
        {
            State = ApiStatus.Ok;
            Message = message;
        }
    }

    public class SuccessApiServiceResponse<T> : ApiServiceResponse<T>
    {
        public SuccessApiServiceResponse(T data, string message = null, bool isAuth = false)
        {
            State = ApiStatus.Ok;
            Data = data;
            Message = message;
        }
    }

    public class AlreadyExistsApiServiceResponse<T> : ApiServiceResponse<T>
    {
        public AlreadyExistsApiServiceResponse(string message = null, string errorCode = ResponseErrorCode.AlreadyExists)
        {
            State = ApiStatus.AlreadyExists;
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public class NotFoundApiServiceResponse<T> : ApiServiceResponse<T>
    {
        public NotFoundApiServiceResponse(string message = null, string errorCode = ResponseErrorCode.NotFound)
        {
            State = ApiStatus.NotFound;
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public class BadRequestApiServiceResponse<T> : ApiServiceResponse<T>
    {
        public BadRequestApiServiceResponse(T data, string message = null, string errorCode = ResponseErrorCode.BadRequest, List<string> validationErrors = null, bool isAuth = false)
        {
            State = ApiStatus.BadRequest;
            ErrorCode = errorCode;
            Message = message;
            Data = data;
        }
    }

    public class BadRequestApiServiceResponse : ApiServiceResponse
    {
        public BadRequestApiServiceResponse(string message = null, string errorCode = ResponseErrorCode.BadRequest)
        {
            State = ApiStatus.BadRequest;
            ErrorCode = errorCode;
            Message = message;
        }
    }
    public enum ApiStatus
    {
        Ok,
        NotFound,
        ValidationFailed,
        BadRequest,
        AlreadyExists
    }
}
