using Microsoft.AspNetCore.Mvc;
using ContactsApi.Models;
using ContactsApi.Data;
using BlazorContactsApp.Services;
using MongoDB.Driver;

namespace ContactsApi.Controllers
{

       [ApiController]
    [Route("api/[controller]")]

    public class ContactsController : ControllerBase
    {
                private readonly IMongoCollection<Contact> _contacts;

                public ContactsController(ContactService contactService)
                {
                    _contacts= contactService.Database.GetCollection<Contact>("contacts");
                }

                [HttpGet]
                public async Task<ActionResult<List<Contact>>> Get()
                {
                    var contacts = await _contacts.Find(contact => true).ToListAsync();
                    return Ok(contacts);
                }

                [HttpPost]
                public async Task<IActionResult> Post(Contact contact)
                {
                    await _contacts.InsertOneAsync(contact);
                    return Ok(contact);
                }

                [HttpDelete("{id}")]    
                public async Task<IActionResult> Delete(string id)
                {
                    var contact = await _contacts.Find(contact => contact.Id == id).FirstOrDefaultAsync();
                    if (contact == null)
                    {
                        return NotFound();
                    }
                   
                    await _contacts.DeleteOneAsync(contact => contact.Id == id);
                    return NoContent();
                }

    }

} 