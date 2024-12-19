using Demo.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Entity
{
    [Table("OrderDetail")]
    public class OrderDetail:DomainEntity<Guid>,IAuditTable
    {
        public Guid UserId { get; set; }

        [Column(TypeName ="nvarchar(1000)")]
        public string VariantName {  get; set; }

        [Column(TypeName ="nvarchar(1000)")]
        public decimal PurchasePrice {  get; set; }

        public int Quantity {  get; set; }  

        public Guid VariantId { get; set; }

        public Guid OrderId {  get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }


        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public EntityStatus Status { get; set; }
    }
}
