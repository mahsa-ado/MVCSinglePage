namespace MVCSinglePage.ApplicationServices.Dtos.ProductDtos
{
    public class PostProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal UnitPrice { get; set; }
        public string DescriptionRecord { get; set; }
    }
}
