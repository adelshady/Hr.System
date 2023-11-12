using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTOs
{
    public class PublicHolidaysDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "PublicHoliday name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Public Holiday name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string  Date { get; set; }
    }
}
