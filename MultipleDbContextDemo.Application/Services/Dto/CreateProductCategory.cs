using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleDbContextDemo.Services.Dto
{
    public class CreateProductCategory : Entity
    {
        [Required]
        //public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
