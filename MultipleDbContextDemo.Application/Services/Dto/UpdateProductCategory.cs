using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleDbContextDemo.Services.Dto
{
    public class UpdateProductCategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
