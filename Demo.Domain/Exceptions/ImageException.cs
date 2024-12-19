using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public static class ImageException
    {
        public class UploadImageException : BadRequestException
        {
            public UploadImageException() : base("Something when wrong in upload ")
            {

            }
        }
        public class UpdateImageException : BadRequestException
        {
            public UpdateImageException(Guid imageId) : base($"something when wrong when update with {imageId}")
            {

            }
        }
        public class DeleteImageException : BadRequestException
        {
            public DeleteImageException(Guid imageId) : base($"Something when wrong delete with {imageId}")
            {
            }
        }

        public class ImageNotFoundException : NotFoundException
        {
            public ImageNotFoundException(Guid imageId):base($"something when wrong with {imageId} not found")
            {

            }
        }
    }
}
