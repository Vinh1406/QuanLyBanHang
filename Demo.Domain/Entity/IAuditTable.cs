using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain.Enum;

namespace Demo.Domain.Entity
{
    internal interface IAuditTable
    {
        public  DateTime? CreatedDate {  get; set; }

        public Guid? CreatedBy {  get; set; }

        public Guid? UpdatedBy {  get; set; }

        public DateTime? UpdatedDate { get; set; }

        public EntityStatus Status { get; set; }

    }
}
