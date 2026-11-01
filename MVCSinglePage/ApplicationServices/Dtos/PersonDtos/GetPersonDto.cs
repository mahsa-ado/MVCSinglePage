namespace MVCSinglePage.ApplicationServices.Dtos.PersonDtos
{
    public class GetPersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
