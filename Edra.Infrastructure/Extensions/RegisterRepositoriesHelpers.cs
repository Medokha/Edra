using Edra.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Edra.Infrastructure.Extensions
{
    public static class RegisterRepositoriesHelpers
    {
        public static void RegisterRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EdraDbContext>(option =>
                                         option.UseSqlServer(builder.Configuration.GetConnectionString("EdraConnection")));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<EdraDbContext>().AddDefaultTokenProviders();
        }
    }
}