using SmsAPI.DTOs;
using SmsAPI.Models;

namespace SmsAPI.Contract
{
    public interface IPersonService
    {
        Task<Person> GetPersonById(int id);
        Task<List<Person>> GetPersonAll();
        Task<List<PersonDto>> GetPersonAllCache();
        Task UpdatePerson(Person person);
    }
}
