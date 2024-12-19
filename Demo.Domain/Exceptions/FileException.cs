using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public static class FileException
    {
        public class FileExceptionNotFoundException : NotFoundException
        {
            public FileExceptionNotFoundException(string path) : base($"the file with the path {path} not found ")
            {

            }
        }
    }
}
