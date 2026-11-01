using MVCSinglePage.ApplicationServices.Dtos.ProductDtos;
using MVCSinglePage.Models.DomainModels.ProductAggregates;
using MVCSinglePage.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using MVCSinglePage.Models;

namespace MVCSinglePage.Models.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProjectDbContext _projectDbContext;
        #region [- ctor -]
        public ProductRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }
        #endregion

        #region [- Update() -]
        public async Task Update(Product product)
        {
            try
            {
                _projectDbContext.Update(product);
                await _projectDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Delete() -]
        public async Task Delete(Product product)
        {
            try
            {
                _projectDbContext.Remove(product);
                await _projectDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Insert() -]
        public async Task Insert(Product product)
        {
            try
            {
                _projectDbContext.Add(product);
                await _projectDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- SelectAll() -]
        public async Task<List<Product>> SelectAll()
        {
            try
            {
                return await _projectDbContext.Product.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region [- SelectProductById() -]
        public async Task<Product?> SelectProductById(GetProductByIdDto getProductByIdDto)
        {
            try
            {
                return await _projectDbContext.Product.FindAsync(getProductByIdDto.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
