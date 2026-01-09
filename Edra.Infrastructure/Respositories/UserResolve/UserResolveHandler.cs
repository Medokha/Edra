using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Edra.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Edra.Infrastructure.Repositories.UserResolve
{
    public class UserResolveHandler(IHttpContextAccessor httpContextAccessor) : IUserResolveHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public string? GetUserGuid() => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
