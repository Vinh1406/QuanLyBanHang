using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Entity
{
    [Table("Permissions")]
    public  class Permission:DomainEntity<Guid>
    {
        public string Code {  get; set; }=string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Index {  get; set; }
        public string ParentCode { get; set; } = string.Empty;
    }
}
