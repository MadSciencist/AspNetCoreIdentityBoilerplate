using System;
using System.Collections.Generic;
using AspNetCoreIdentityBoilerplate.Models;

namespace AspNetCoreIdentityBoilerplate.Infrastructure
{
    public interface ITokenBuilder
    {
        (string token, DateTime expring) Build(AppUser user, IEnumerable<string> roles);
    }
}