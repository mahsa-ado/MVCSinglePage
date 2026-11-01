using MVCSinglePage.ApplicationServices.Dtos.ProductDtos;

namespace MVCSinglePage.ApplicationServices.Services.Contracts
{
    public interface IProductApplicationService
    {
        #region [- Put() -]
        Task Put(PutProductDto putProductDto);
        #endregion

        #region [- GetProductForDelete() -]
        Task<DeleteProductDto?> GetProductDelete(GetProductByIdDto getProductByIdDto);
        #endregion

        #region [- Delete() -]
        Task Delete(DeleteProductDto deleteProductDto);
        #endregion

        #region [- Post() -]
        Task Post(PostProductDto postProductDto);

        #endregion

        #region [- GetProductDetail() -]
        Task<DetailProductDto?> GetProductDetail(GetProductByIdDto getProductByIdDto);

        #endregion

        #region [- GetAllProducts() -]
        Task<List<GetProductDto>> GetAllProducts();

        #endregion

        #region [- GetProductById() -]
        Task<PutProductDto?> GetProductById(GetProductByIdDto getProductByIdDto);
        #endregion

    }
}