using MVCSinglePage.ApplicationServices.Dtos.PersonDtos;
using MVCSinglePage.ApplicationServices.Services.Contracts;
using MVCSinglePage.Models.Services.Contracts;
using MVCSinglePage.Models.DomainModels.PersonAggregates;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MVCSinglePage.ApplicationServices.Services
{
    public class PersonApplicationService : IPersonApplicationService
    {
        private readonly IPersonRepository _personRepository;
        #region [- ctor -]
        public PersonApplicationService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        #endregion

        #region [- Put() -]
        public async Task Put(PutPersonDto putPersonDto)
        {
            if (putPersonDto == null)
            {
                return;
            }
            var putPerson = new Person()
            {
                Id = putPersonDto.Id,
                FirstName = putPersonDto.FirstName,
                LastName = putPersonDto.LastName,
                Age = putPersonDto.Age,
            };
            await _personRepository.Update(putPerson);
        }

        #endregion

        #region [- GetPersonForDelete() -]
        public async Task<DeletePersonDto?> GetPersonDelete(GetPersonByIdDto getPersonByIdDto)
        {
            var person = await _personRepository.SelectPersonById(getPersonByIdDto);

            if (person == null)
            {
                return null;
            }

            var deletePersonDto = new DeletePersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age,
            };

            return deletePersonDto;
        }
        #endregion

        #region [- Delete() -]
        public async Task Delete(DeletePersonDto deletePersonDto)
        {
            var deletePerson = await _personRepository.SelectPersonById(
                new GetPersonByIdDto { Id = deletePersonDto.Id });
            if (deletePerson == null)
            {
                return;
            }

            await _personRepository.Delete(deletePerson);

        }
        #endregion

        #region [- Post() -]
        public async Task Post(PostPersonDto postPersonDto)
        {
            if (postPersonDto == null)
            {
                return;
            }
            var person = new Person()
            {
                Id = postPersonDto.Id,
                FirstName = postPersonDto.FirstName,
                LastName = postPersonDto.LastName,
                Age = postPersonDto.Age,
            };
            await _personRepository.Insert(person);
        }
        #endregion

        #region [- GetPersonDetail() -]
        public async Task<DetailPersonDto?> GetPersonDetail(GetPersonByIdDto getPersonDtoById)
        {

            var persons = await _personRepository.SelectPersonById(getPersonDtoById);
            if (persons == null)
            {
                return null;
            }
            var detailPesronDto = new DetailPersonDto
            {
                Id = persons.Id,
                FirstName = persons.FirstName,
                LastName = persons.LastName,
                Age = persons.Age,
            };
            return detailPesronDto;

        }
        #endregion

        #region [- GetAllPerson() -]
        public async Task<List<GetPersonDto>> GetAllPerson()
        {
            var persons = await _personRepository.SelectAll();
            var getPersonDtos = new List<GetPersonDto>();
            foreach (var person in persons)
            {
                var getPersonDto = new GetPersonDto()
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.Age,
                };
                getPersonDtos.Add(getPersonDto);
            }
            return getPersonDtos;
        }
        #endregion

        #region [- GetPersonById() -]
        public async Task<PutPersonDto?> GetPersonById(GetPersonByIdDto getPersonDtoById)
        {
            var person = await _personRepository.SelectPersonById(getPersonDtoById);
            if (person == null)
            {
                return null;
            }
            var putPersonDto = new PutPersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age
            };
            return putPersonDto;
        }
        #endregion

    }
}
