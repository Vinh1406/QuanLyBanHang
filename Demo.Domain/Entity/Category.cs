using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain.Enum;

namespace Demo.Domain.Entity
{
    [Table("Categories")]
    public class Category:DomainEntity<Guid>, IAuditTable
    {
        [Column(TypeName ="nvarchar(1000)")]
        public string Name {  get; set; }
        public string? ImageJson {  get; set; }
        public  ICollection<Product> Products { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public EntityStatus Status { get; set; }
    }
}
