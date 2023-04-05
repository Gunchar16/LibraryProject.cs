using Library.Shared.Api.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shared.Api.Extensions
{
    public static class Extensions
    {
        public static ObjectResult ResponseResult(this ControllerBase controller, ApiServiceResponse response)
        {
            switch (response.State)
            {
                case ApiStatus.Ok:
                    return controller.Ok(response);
                case ApiStatus.NotFound:
                    return controller.NotFound(response);
                case ApiStatus.BadRequest:
                    return controller.BadRequest(response);
                case ApiStatus.AlreadyExists:
                    return controller.StatusCode((int)HttpStatusCode.Conflict, response);
                default:
                    return controller.StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
    }
}
