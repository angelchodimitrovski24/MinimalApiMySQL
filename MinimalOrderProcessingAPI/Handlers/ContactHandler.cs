using Microsoft.EntityFrameworkCore;

namespace MinimalOrderProcessingAPI.Handlers
{
    public class ContactHandler
    {
        public async Task<IResult> GetAllContactsAsync(DataContext context)
        {
            var contacts = await context.Contacts.ToListAsync();
            return Results.Ok(contacts);
        }
        public async Task<IResult> GetContactByIdAsync(DataContext context, int id)
        {
            var contact = await context.Contacts.FindAsync(id);
            if (contact == null)
                return Results.NotFound("Contact not found.");

            return Results.Ok(contact);
        }

        public async Task<IResult> CreateContactAsync(DataContext context, Contact contact)
        {
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();
            return Results.Created($"/contacts/{contact.Id}", contact);
        }

        public async Task<IResult> UpdateContactAsync(DataContext context, int id, Contact updatedContact)
        {
            var existingContact = await context.Contacts.FindAsync(id);
            if (existingContact == null)
                return Results.NotFound("Contact not found.");

            existingContact.Name = updatedContact.Name;
            existingContact.Email = updatedContact.Email;

            await context.SaveChangesAsync();
            return Results.Ok(existingContact);
        }

        public async Task<IResult> DeleteContactAsync(DataContext context, int id)
        {
            var contact = await context.Contacts.FindAsync(id);
            if (contact == null)
                return Results.NotFound("Contact not found.");

            context.Contacts.Remove(contact);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }
    }
}
