using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public abstract class NotFoundException : BaseException
    {
        protected NotFoundException(string message) : base("not Found", message)
        {
            StatusCode =StatusCodes.Status404NotFound;
        }
    }
}
