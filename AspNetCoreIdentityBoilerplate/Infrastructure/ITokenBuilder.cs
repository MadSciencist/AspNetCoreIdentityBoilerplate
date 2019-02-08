using System;
using AspNetCoreIdentityBoilerplate.Models;

namespace AspNetCoreIdentityBoilerplate.Infrastructure
{
    public interface ITokenBuilder
    {
        (string token, DateTime expring) BuildToken(AppUser user);
    }
}