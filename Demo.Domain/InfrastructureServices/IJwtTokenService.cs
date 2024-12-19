using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.InfrastructureServices
{
    public  interface IJwtTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
    }
}
