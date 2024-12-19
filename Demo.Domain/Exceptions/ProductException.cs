using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public static class ProductException
    {
        public class ProductNotFoundException : NotFoundException
        {
            public ProductNotFoundException(Guid productId):base($"the product with the id {productId} not found ")
            {

            }
        }
        public class CrerateProductException : BadRequestException
        {
            public CrerateProductException(string productName) : base($"Something when wrong when create {productName} war not exception")
            {

            }
        }

        public class UpdateProductException:BadRequestException
        {
            public UpdateProductException(string productId):base($"Something when wrong when update {productId}")
            {

            }
        }
        public class DeleteProductException : BadRequestException
        {
            public DeleteProductException(Guid productId):base($"Something when wrong when delete {productId}")
            {

            }
        }

        public class CreateReviewException : BadRequestException
        {
            public CreateReviewException():base("Something when wrong when create review ")
            {

            }
        }

    }
}
