using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationServices.Users
{
    public  class UserSearchQuery:SearchQuery
    {
        public bool IsSystemUser {  get; set; }
    }
}
