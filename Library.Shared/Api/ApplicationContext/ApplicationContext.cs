using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shared.Api.ApplicationContext
{
    public class ApplicationContext
    {
        public int? UserId { get; init; }
        public string UserRole { get; init; }
        public string? Token { get; init; }
        public string? ClientIpAddress { get; init; }
        public int Tier { get; init; }
        public ApplicationContext(string? clientIpAddress)
        {
            ClientIpAddress = clientIpAddress;
        }

        public ApplicationContext(int userId, string role, string token, string? clientIpAddress)
        {
            UserId = userId;
            UserRole = role;
            Token = token;
            ClientIpAddress = clientIpAddress;
        }
    }
}
