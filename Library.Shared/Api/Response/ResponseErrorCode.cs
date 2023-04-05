using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shared.Api.Response
{
    public class ResponseErrorCode
    {
        public const string ValidationFail = "VALIDATION_FAIL";
        public const string NotFound = "NOT_FOUND";
        public const string AlreadyExists = "ALREADY_EXISTS";
        public const string BadRequest = "BAD_REQUEST";
    }
}
