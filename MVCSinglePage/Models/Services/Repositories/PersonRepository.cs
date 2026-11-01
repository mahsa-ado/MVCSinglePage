using MVCSinglePage.ApplicationServices.Dtos.PersonDtos;
using MVCSinglePage.Models.DomainModels.PersonAggregates;
using MVCSinglePage.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;


namespace MVCSinglePage.Models.Services.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ProjectDbContext _projectDbContext;

        #region [- Ctor -]
        public PersonRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }
        #endregion

        #region [- Update() -]
        public async Task Update(Person person)
        {
            try
            {
                _projectDbContext.Update(person);
                await _projectDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Delete() -]
        public async Task Delete(Person person)
        {
            try
            {
                _projectDbContext.Remove(person);
                await _projectDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region [- Insert() -]
        public async Task Insert(Person person)
        {
            try
            {
                _projectDbContext.Add(person);
                await _projectDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- SelectAllPerson() -]
        public async Task<List<Person>> SelectAll()
        {
            try
            {
                return await _projectDbContext.Person.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region [- SelectPersonById() -]
        public async Task<Person?> SelectPersonById(GetPersonByIdDto getPersonDtoById)
        {

            try
            {
                return await _projectDbContext.Person.FindAsync(getPersonDtoById.Id);


            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
    }
}
