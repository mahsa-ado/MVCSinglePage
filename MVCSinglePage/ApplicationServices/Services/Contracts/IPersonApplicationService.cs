using MVCSinglePage.ApplicationServices.Dtos.PersonDtos;

namespace MVCSinglePage.ApplicationServices.Services.Contracts
{
    public interface IPersonApplicationService
    {
        #region [- Put() -]
        Task Put(PutPersonDto putPersonDto);
        #endregion

        #region [- Delete() -]
        Task Delete(DeletePersonDto deletePersonDto);
        #endregion

        #region [- Post() -]
        Task Post(PostPersonDto postPersonDto);
        #endregion

        #region [- GetPersonDetail() -]
        Task<DetailPersonDto?> GetPersonDetail(GetPersonByIdDto getPersonDtoById);
        #endregion

        #region [- GetAllPerson() -]
        Task<List<GetPersonDto>> GetAllPerson();

        #endregion

        #region [- GetPersonById() -]
        Task<PutPersonDto?> GetPersonById(GetPersonByIdDto getPersonDtoById);
        #endregion

    }
}