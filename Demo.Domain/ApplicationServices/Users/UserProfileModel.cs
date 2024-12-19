using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationServices.Users
{
    public class UserProfileModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<string> Permissions {  get; set; }

    }
}
