using Library.Infrastructure.Dtos;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Interfaces;
using Library.Service.Services.Users;
using Library.Shared.Api.ApplicationContext;
using Library.Shared.Api.Extensions;
using Library.Shared.Api.Filters;
using Library.Shared.Api.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly ApplicationContext _context;

        public AuthController(IConfiguration config, IUserService userService, ApplicationContext context)
        {
            _config = config;
            _userService = userService;
            _context = context;
        }

        [HttpPost("register")]
        [ValidateModel]
        public async Task<ActionResult<ApiServiceResponse<bool>>> Register([FromBody] RegisterDto registerDto)
            => this.ResponseResult(await _userService.RegisterAsync(registerDto));

        [HttpPost("login")]
        [ValidateModel]
        public async Task<ActionResult<ApiServiceResponse<string>>> Login([FromBody] LoginDto loginDto)
            => this.ResponseResult(await _userService.LoginAsync(loginDto));
    }
}