using Demo.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Entity
{
    [Table("Products")]
    public class Product:DomainEntity<Guid>,IAuditTable
    {
        [Column(TypeName ="nvarchar(1000)")]
        public string Name {  get; set; }

        [Column(TypeName ="nvarchar(max)")]
        public string? Detail {  get; set; }

        [Column(TypeName ="nvarchar(max)")]
        public string? Description {  get; set; }

        public string? ImageJson {  get; set; }

        public Guid CategoryId {  get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public ICollection<Variants> Variants { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public EntityStatus Status { get; set; }
    }
}
