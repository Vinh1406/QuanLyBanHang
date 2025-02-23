﻿using Demo.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Entity
{
    [Table("Order")]
    public class Order:DomainEntity<Guid>,IAuditTable
    {
        public Guid UserId { get; set; }

        [Column(TypeName ="nvarchar(1000)")]
        public string ShippingAddress {  get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal TotalAmount {  get; set; }

        public OrderStatus OrderStatus { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<OrderDetail> OrderDetail { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public EntityStatus Status { get; set; }

    }
}
