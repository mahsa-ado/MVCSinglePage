using MVCSinglePage.ApplicationServices.Dtos.PersonDtos;
using MVCSinglePage.Models.DomainModels.PersonAggregates;

namespace MVCSinglePage.Models.Services.Contracts
{
    public interface IPersonRepository
    {
        #region [- Update() -]
        Task Update(Person person);
        #endregion

        #region [- Delete() -]
        Task Delete(Person person);
        #endregion

        #region [- Insert() -]
        Task Insert(Person person);
        #endregion

        #region [- SelectAll() -]
        Task<List<Person>> SelectAll();

        #endregion

        #region [- SelectPersonById() -]
        Task<Person?> SelectPersonById(GetPersonByIdDto getPersonDtoById);
        #endregion
    }

}
