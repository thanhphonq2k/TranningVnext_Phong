using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Abp.Domain.Repositories;
using Abp.EntityFramework;
using MultipleDbContextDemo.EntityFramework;
using Abp.UI;
using MultipleDbContextDemo.Services.Dto;

namespace MultipleDbContextDemo.Test
{
    public class ProductAppService : MultipleDbContextDemoAppServiceBase, IProductAppService
    {
        private readonly string _connectionString;
        private readonly IRepository<ProductCategory> _productCategoryRepository; //in the second db
        private readonly IDbContextProvider<MySecondDbContext> _mySecondDbContext;

        public ProductAppService(string connectionString, IRepository<ProductCategory> productCategoryRepository, IDbContextProvider<MySecondDbContext> mySecondDbContext)
        {
            _connectionString = connectionString;
            _productCategoryRepository = productCategoryRepository;
            _mySecondDbContext = mySecondDbContext;
        }

        //Entity
        public void CreateProductCategory(CreateProductCategory input)
        {
            try
            {
                var productCategory = new ProductCategory()
                {
                    Id = input.Id,
                    Name = input.Name,
                    Active = input.Active
                };
                _productCategoryRepository.Insert(productCategory);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateProductCategory(UpdateProductCategory input)
        {
            try
            {
                var productCategory = _productCategoryRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
                if (productCategory != null)
                {
                    productCategory.Name = input.Name;
                    productCategory.Active = input.Active;
                    _productCategoryRepository.UpdateAsync(productCategory);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteProductCategory(int id)
        {
            try
            {
                var productCategory = _productCategoryRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
                if (productCategory != null)
                {
                    _productCategoryRepository.Delete(productCategory);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductCategoryDto DetailProductCategory(int id)
        {
            try
            {
                ProductCategoryDto productCategoryDto = new ProductCategoryDto();
                if (id != 0)
                {
                    var productCategory = _productCategoryRepository.GetAll().Where(x => x.Id == id).Select(y => new ProductCategoryDto
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Active = y.Active
                    }).FirstOrDefault();
                    productCategoryDto.Id = productCategory.Id;
                    productCategoryDto.Name = productCategory.Name;
                    productCategoryDto.Active = productCategory.Active;
                }
                return productCategoryDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProductCategory> GetProductCategory()
        {
            try
            {
                var lstproductCategory = _productCategoryRepository.GetAllList().Select(x => new ProductCategory
                {
                    Id = x.Id,
                    Name = x.Name,
                    Active = x.Active
                }).ToList();
                return lstproductCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Linq
        public List<ProductCategory> GetProductCategoryLinq()
        {
            try
            {
                var productCategory = (from c in _mySecondDbContext.GetDbContext().ProductCategories

                                       select new ProductCategory
                                       {
                                           Id = c.Id,
                                           Name = c.Name,
                                           Active = c.Active
                                       })
                           .ToList();
                var ProductCategory = productCategory.ToList().Select(r => new ProductCategory
                {
                    Id = r.Id,
                    Name = r.Name,
                    Active = r.Active
                }).ToList();
                return ProductCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreateProductCategoryLinq(CreateProductCategory input)
        {
            try
            {
                ProductCategory productCategory = new ProductCategory();
                productCategory.Id = input.Id;
                productCategory.Name = input.Name;
                productCategory.Active = input.Active;

                var ProductCategory = _mySecondDbContext.GetDbContext().ProductCategories.Add(productCategory);

                _productCategoryRepository.InsertAsync(ProductCategory);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateProductCategoryLinq(UpdateProductCategory input)
        {
            try
            {
                var productCategory = (from c in _mySecondDbContext.GetDbContext().ProductCategories
                                       where c.Id == input.Id
                                       select c).FirstOrDefault();
                if (productCategory.Id != 0)
                {
                    productCategory.Name = input.Name;
                    productCategory.Active = input.Active;
                    _productCategoryRepository.Update(productCategory);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteProductCategoryLinq(int id)
        {
            try
            {
                var productCategory = (from c in _mySecondDbContext.GetDbContext().ProductCategories
                                       where c.Id == id
                                       select c).FirstOrDefault();
                if (productCategory.Id != 0)
                {
                    _productCategoryRepository.Delete(productCategory);

                    //_repository
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductCategoryDto DetailProductCategoryLinq(int id)
        {
            try
            {
                ProductCategoryDto productCategoryDto = new ProductCategoryDto();
                var productCategory = (from c in _mySecondDbContext.GetDbContext().ProductCategories
                                       where c.Id == id
                                       select new ProductCategoryDto()
                                       {
                                           Id = c.Id,
                                           Name = c.Name,
                                           Active = c.Active
                                       }).First();

                productCategoryDto.Id = productCategory.Id;
                productCategoryDto.Name = productCategory.Name;
                productCategoryDto.Active = productCategory.Active;

                return productCategoryDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Sql

        public List<ProductCategory> GetCategorySql()
        {
            try
            {
                var sql = "select * from ProductCategory";
                var getAll = _mySecondDbContext.GetDbContext().Database.SqlQuery<ProductCategory>(sql).ToList();
                return getAll;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public int CreateSql(CreateProductCategory input)
        {
            try
            {
                ProductCategory cate = new ProductCategory();
                cate.Name = input.Name;
                cate.Active = input.Active;

                var sql = "insert into ProductCategory(Name,Active) values(@Name,@Active)";
                var name = new SqlParameter("@Name", cate.Name);
                var active = new SqlParameter("@Active", cate.Active);
                var insert = _mySecondDbContext.GetDbContext().Database.ExecuteSqlCommand(sql, name, active);
                return insert;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public int EditSql(UpdateProductCategory input)
        {
            try
            {
                var getId = "select * from ProductCategory where Id=@id";
                var id = new SqlParameter("@id", input.Id);
                var cate = _mySecondDbContext.GetDbContext().Database.SqlQuery<ProductCategory>(getId, id).FirstOrDefault();

                var sql = "update ProductCategory set Name=@name,Active=@active where Id=@id ";
                var cateid = new SqlParameter("id", cate.Id);
                var name = new SqlParameter("name", input.Name);
                var active = new SqlParameter("active", input.Active);
                var update = _mySecondDbContext.GetDbContext().Database.ExecuteSqlCommand(sql, name, active, cateid);
                return update;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public bool DeleteSql(int id)
        {
            try
            {
                var getId = "select * from ProductCategory where Id=@id";
                var cid = new SqlParameter("@id", id);
                var cate = _mySecondDbContext.GetDbContext().Database.SqlQuery<ProductCategory>(getId, cid).FirstOrDefault();
                if (cate.Id != 0)
                {
                    var sql = "delete ProductCategory where Id=@id";
                    var cateId = new SqlParameter("id", cate.Id);
                    var delete = _mySecondDbContext.GetDbContext().Database.ExecuteSqlCommand(sql, cateId);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public ProductCategoryDto DetailSql(int id)
        {
            try
            {
                var getId = "select Id,Name,Active from Category where Id=@id";
                var cid = new SqlParameter("@id", id);
                var cate = _mySecondDbContext.GetDbContext().Database.SqlQuery<ProductCategory>(getId, cid).FirstOrDefault();

                ProductCategoryDto productCategoryDto = new ProductCategoryDto();
                productCategoryDto.Id = cate.Id;
                productCategoryDto.Name = cate.Name;
                productCategoryDto.Active = cate.Active;
                return productCategoryDto;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
