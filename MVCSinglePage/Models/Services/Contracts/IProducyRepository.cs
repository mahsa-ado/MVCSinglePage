using MVCSinglePage.ApplicationServices.Dtos.ProductDtos;
using MVCSinglePage.Models.DomainModels.ProductAggregates;

namespace MVCSinglePage.Models.Services.Contracts
{
    public interface IProductRepository
    {

        #region [- Update() -]
        Task Update(Product product);

        #endregion    

        #region [- Delete() -]
        Task Delete(Product product);

        #endregion     

        #region [- Insert() -]
        Task Insert(Product product);
        #endregion

        #region [- SelectAll() -]
        Task<List<Product>> SelectAll();
        #endregion

        #region [- SelectProductById() -]
        Task<Product?> SelectProductById(GetProductByIdDto getProductByIdDto);

        #endregion
    }
}
