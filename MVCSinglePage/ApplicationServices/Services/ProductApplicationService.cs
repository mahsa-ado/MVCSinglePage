
using MVCSinglePage.ApplicationServices.Dtos.ProductDtos;
using MVCSinglePage.ApplicationServices.Services.Contracts;
using MVCSinglePage.Models.DomainModels.ProductAggregates;
using MVCSinglePage.Models.Services.Contracts;

namespace MVCSinglePage.ApplicationServices.Services
{
    public class ProductApplicationService : IProductApplicationService
    {
        private readonly IProductRepository _productRepository;
        #region [-ctor-]
        public ProductApplicationService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region [- Put() -]
        public async Task Put(PutProductDto updateProductDto)
        {
            if (updateProductDto == null)
            {
                return;
            }
            var updateProduct = new Product()
            {
                Id = updateProductDto.Id,
                Title = updateProductDto.Title,
                UnitPrice = updateProductDto.UnitPrice,
                DescriptionRecord = updateProductDto.DescriptionRecord,
            };

            await _productRepository.Update(updateProduct);
        }
        #endregion

        #region [- Delete() -]
        public async Task Delete(DeleteProductDto deleteProductDto)
        {
            if (deleteProductDto == null || deleteProductDto.Id == Guid.Empty)
            {
                return;
            }
            var productDto = new Product()
            {
                Id = deleteProductDto.Id,
            };

            await _productRepository.Delete(productDto);
        }
        #endregion

        #region [- Post() -]
        public async Task Post(PostProductDto postProductDto)
        {
            if (postProductDto == null)
            {
                return;
            }
            var products = new Product()
            {
                Id = postProductDto.Id,
                Title = postProductDto.Title,
                UnitPrice = postProductDto.UnitPrice,
                DescriptionRecord = postProductDto.DescriptionRecord,
            };
            await _productRepository.Insert(products);
        }

        #endregion

        #region [- GetProductDetail() -]
        public async Task<DetailProductDto?> GetProductDetail(GetProductByIdDto getProductByIdDto)
        {
            var product = await _productRepository.SelectProductById(getProductByIdDto);
            if (product == null)
            {
                return null;
            }

            var productDto= new DetailProductDto()
            {
                Id = product.Id,
                Title = product.Title,
                UnitPrice = product.UnitPrice,
                DescriptionRecord = product.DescriptionRecord,
            };
            return productDto;
        }
        #endregion

        #region [- GetAllProducts() -]
        public async Task<List<GetProductDto>> GetAllProducts()
        {
            var products = await _productRepository.SelectAll();
            var getDto = new List<GetProductDto>();
            foreach (var product in products)
            {
                var productDto = new GetProductDto()
                {
                    Id = product.Id,
                    Title = product.Title,
                    UnitPrice = product.UnitPrice,
                    DescriptionRecord = product.DescriptionRecord,
                };
                getDto.Add(productDto);
            }
            return getDto;
        }


        #endregion

        #region [- GetProductById() -]
        public async Task<PutProductDto?> GetProductById(GetProductByIdDto getProductByIdDto)
        {

            var product = await _productRepository.SelectProductById(getProductByIdDto);

            if (product == null)
            {
                return null;
            }
            var productDto = new PutProductDto()
            {
                Id = product.Id,
                Title = product.Title,
                UnitPrice = product.UnitPrice,
                DescriptionRecord = product.DescriptionRecord,
            };

            return productDto;
        }

        #endregion

    }
}
