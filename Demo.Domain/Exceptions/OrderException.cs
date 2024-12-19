using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Exceptions
{
    public static class OrderException
    {
        public class ProcessCheckoutException : BadRequestException
        {
            public ProcessCheckoutException():base("something wrong happen in checkout process")
            {

            }
        }

        public class UpdateOrderException : BadRequestException
        {
            public UpdateOrderException(Guid orderId):base($"something when wrong when update in order with {orderId}")
            {

            }
        }
    }
}
