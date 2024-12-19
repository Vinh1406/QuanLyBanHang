using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Entity
{
    [Table("RolePermissions")]
    public class RolePermission:DomainEntity<Guid>
    {
        public Guid RoleId { get; set; }    
        public string PermissionCode { get; set; } = string.Empty;
    }
}
