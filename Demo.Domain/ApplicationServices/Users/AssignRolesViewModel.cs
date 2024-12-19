using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationServices.Users
{
    public class AssignRolesViewModel
    {
        public string UserName {  get; set; }
        public List<string> RolesNames { get; set; }
    }
}
