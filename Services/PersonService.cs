using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmsAPI.Contract;
using SmsAPI.Data;
using SmsAPI.DTOs;
using SmsAPI.Models;

namespace SmsAPI.Services
{
    public class PersonService : IPersonService
    {
        private readonly AdmissionDbContext _context;
        public PersonService(AdmissionDbContext context)
        {
            _context = context;
        }
       
        public async Task<List<Person>> GetPersonAll()
        {
            var result =await _context.person.Where(s => s.Title != null).Take(100).ToListAsync();
            return result;
        }

        public async Task<List<PersonDto>> GetPersonAllCache()
        {
            var persons = await _context.person
                .Where(s => s.Title != null)
                .Take(100)
                .Select(s => new PersonDto
                {
                    BusinessEntityID = s.BusinessEntityID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    PersonType = s.PersonType,
                    NameStyle = s.NameStyle
                })
                .ToListAsync();

            return persons;
        }

        public async Task<Person?> GetPersonById(int id)
        {
            return await _context.person.FindAsync(id).AsTask();    
        }

        public async Task UpdatePerson(Person person)
        {
            _context.person.Update(person);
            await _context.SaveChangesAsync();
        }
    }
}
