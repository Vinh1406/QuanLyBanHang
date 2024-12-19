using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Entity
{
    public class DomainEntity<Tkey>
    {
        [Key]
        public Tkey Id { get; set; }
    }
}
