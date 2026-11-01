namespace MVCSinglePage.ApplicationServices.Dtos.ProductDtos
{
    public class DetailProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal UnitPrice { get; set; }
        public string DescriptionRecord { get; set; }
    }
}
