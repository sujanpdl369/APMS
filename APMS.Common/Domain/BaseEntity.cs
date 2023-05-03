using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Common.Domain
{
    public class BaseEntity
    {
        [Key]
        public int RegisterId { get; set; }
    }
}
