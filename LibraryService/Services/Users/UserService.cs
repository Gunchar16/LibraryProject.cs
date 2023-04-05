using Library.Infrastructure;
using Library.Infrastructure.Dtos;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Interfaces;
using Library.Shared.Api.Response;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public UserService(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        { 
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<ApiServiceResponse<bool>> RegisterAsync(RegisterDto userDto)
        {
            var existingUser = await _userRepository.Query().FirstOrDefaultAsync(u => u.Email == userDto.Email
                                                                                        || u.Username == userDto.Username);

            if (existingUser != null)
                return new AlreadyExistsApiServiceResponse<bool>();

            var salt = GenerateSalt();
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = EncryptPassword(userDto.Password, salt),
                PasswordSalt = Convert.ToBase64String(salt)
            };

            await _userRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            return new SuccessApiServiceResponse<bool>(true);
        }

        public async Task<ApiServiceResponse<string>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.Query().FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                return new NotFoundApiServiceResponse<string>();

            var hashedPassword = EncryptPassword(loginDto.Password, Convert.FromBase64String(user.PasswordSalt));

            if (user.PasswordHash != hashedPassword)
                return new ValidationFailedApiServiceResponse<string>();

            var token = CreateToken(user, Guid.NewGuid().ToString());
            return new SuccessApiServiceResponse<string>(token);
        }
        private string CreateToken(User user, string jti)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, jti)

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[256 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            return salt;
        }
        private static string EncryptPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                         password: password,
                                                         salt: salt,
                                                         prf: KeyDerivationPrf.HMACSHA256,
                                                         iterationCount: 1000000,
                                                         numBytesRequested: 256 / 8));
        }
    }
}
