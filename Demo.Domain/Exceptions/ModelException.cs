using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public static class ModelException
    {
        public class ModelNotValidException : BadRequestException
        {
            public ModelNotValidException(string message) : base(message) { }
        }
    }
}
