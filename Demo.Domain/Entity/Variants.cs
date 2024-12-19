using Demo.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Entity
{
    [Table("Variants")]
    public class Variants:DomainEntity<Guid>,IAuditTable
    {
        [Column(TypeName ="nvarchar(1000)")]
        public string Name {  get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Price {  get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPrice {  get; set; }

        public int Quantity {  get; set; }

        public string? ImageJson {  get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }


        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public EntityStatus Status { get; set; }
    }
}
