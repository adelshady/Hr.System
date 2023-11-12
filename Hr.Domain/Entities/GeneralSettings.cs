using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Domain.Entities
{
    public class GeneralSettings
    {
        [Key]
        public int Id { get; set; }
        public int OvertimeHour { get; set; }
        public int DiscountHour { get; set; }

        public ICollection<Weekend> Weekends { get; set; }
        
       
        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

    }
}
