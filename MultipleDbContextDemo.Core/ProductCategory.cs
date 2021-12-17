using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleDbContextDemo
{
    [Table("ProductCategory")]
    public class ProductCategory : Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }

    }
}
