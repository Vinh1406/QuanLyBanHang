using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public static class CartException
    {
        public class CartItemNotFoundException : NotFoundException
        {
            public CartItemNotFoundException(Guid variantId):base($"the item with the id {variantId} war not found")
            {

            }
        }

        public class ProcessCartException : BadRequestException
        {
            public ProcessCartException():base("something when wrong")
            {

            }
        }

        public class OutOfStockException : BadRequestException
        {
            public OutOfStockException(string variantName) :base($"something when wrong when out of stock exception with {variantName}")
            {

            }
        }

        public class CartEmptyException : BadRequestException
        {
            public CartEmptyException():base("Cart empty with out")
            {

            }
        }
    }
}
