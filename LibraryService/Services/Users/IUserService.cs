using Library.Infrastructure.Dtos;
using Library.Shared.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Services.Users
{
    public interface IUserService
    {
        Task<ApiServiceResponse<bool>> RegisterAsync(RegisterDto userDto);
        Task<ApiServiceResponse<string>> LoginAsync(LoginDto loginDto);
    }
}
