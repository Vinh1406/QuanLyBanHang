using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public static class VariantException
    {
        public class VariantNotFoundException : NotFoundException
        {
            public VariantNotFoundException(Guid variantId)
                : base($"the variant with the id {variantId} war not found")
            {
            }
        }

        public class CreateVariantException : BadRequestException
        {
            public CreateVariantException(string variantName) 
                : base( $"something when wrong when create variant {variantName} ")
            {

            }
        }
        public class UpdatetVariantException: BadRequestException
        {
            public UpdatetVariantException(Guid variantId):base($"Something when wrong when update variant {variantId}")
            {

            }
        }
        public class DeleteVariantException:BadRequestException
        {
            public DeleteVariantException(Guid variantId):base($"Something when wrong when delete variant {variantId}")
            {

            }
        }
    }
}
