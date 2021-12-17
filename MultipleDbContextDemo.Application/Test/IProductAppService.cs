using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleDbContextDemo.Test
{
    public interface IProductAppService : IApplicationService
    {
        List<ProductCategory> GetProductCategory();

    }
}
