using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/contacts")]
public class ContactsController : ControllerBase
{
    private static List<Contact> _contacts = new List<Contact>();

    [HttpGet]
    public IEnumerable<Contact> Get()
    {
        return _contacts;
    }

    [HttpPost]
    public ActionResult<Contact> Create(Contact contact)
    {
        _contacts.Add(contact);
        return contact;
    }

    [HttpPut("{id}")]
    public ActionResult<Contact> Update(int id, Contact contact)
    {
        var existingContact = _contacts.Find(c => c.Id == id);
        if (existingContact == null)
            return NotFound();

        existingContact.Nome = contact.Nome;
        existingContact.Email = contact.Email;
        existingContact.Telefone = contact.Telefone;

        return existingContact;
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var contact = _contacts.Find(c => c.Id == id);
        if (contact == null)
            return NotFound();

        _contacts.Remove(contact);
        return NoContent();
    }
}
