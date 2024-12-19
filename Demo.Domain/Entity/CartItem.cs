using Demo.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Entity
{
    [Table("CartItems")]
    public class CartItem:DomainEntity<Guid>,IAuditTable
    {
        public Guid VariantId { get; set; }

        [Column(TypeName ="nvarchar(1000)")]
        public string VariantName {  get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal PuschasePrice {  get; set; }

        public Guid UserId { get; set; }

        public int Quantity {  get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public EntityStatus Status { get; set; }
    }
}
