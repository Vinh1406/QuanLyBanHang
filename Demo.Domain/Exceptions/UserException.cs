using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public static class UserException
    {
        public class UserNotFoundException : NotFoundException
        {
            public UserNotFoundException() : base($"User Not Found") { }
        }
        public class RoleNotFoundException : NotFoundException
        {
            public RoleNotFoundException() : base("Role Not Found") { }
        }
        public class PasswordNotCorrectException : NotFoundException
        {
            public PasswordNotCorrectException():base("Password is not correct") { }
        }

        public class UnauthorizedException : BaseException
        {
            public UnauthorizedException():base("Unauthorized","you are not authorized")
            {
                StatusCode=StatusCodes.Status401Unauthorized;
            }
        }

        public class ForbiddenException : BaseException
        {
            public ForbiddenException():base("Forbidden","you are no allow to access resourse")
            {
                StatusCode=StatusCodes.Status403Forbidden;
            }
        }

        public class HandleUserException : BadRequestException
        {
            public HandleUserException(string message) : base(message) { }
        }

    }
}
