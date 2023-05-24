using ContactsApi.Data;
using ContactsApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;

namespace ContactsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;
        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
    }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetSingleContact([FromRoute] Guid id) { 
            var contactObj = await dbContext.Contacts.FindAsync(id);
            if(contactObj == null)
            {
                return NotFound();
            }
            return Ok(contactObj);
        }
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                    Address = addContactRequest.Address,
                    Email = addContactRequest.Email,
                    Phone = addContactRequest.Phone,
                    FullName = addContactRequest.FullName,

            };
           await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contactObj = await dbContext.Contacts.FindAsync(id);
            if (contactObj != null)
            {
                contactObj.Address = updateContactRequest.Address;
                contactObj.Email = updateContactRequest.Email;
                contactObj.Phone = updateContactRequest.Phone;
                contactObj.FullName = updateContactRequest.FullName;
                await dbContext.SaveChangesAsync();
                return Ok(contactObj);
            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contactObj = await dbContext.Contacts.FindAsync(id);
            if (contactObj == null)
            {
                return NotFound();
            }
            dbContext.Remove(contactObj);
            await dbContext.SaveChangesAsync();
            return Ok(contactObj);
        }
    }
}
