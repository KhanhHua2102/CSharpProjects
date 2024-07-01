using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task? Main()
    {
        using var db = new PhoneBookContext();

        string name = "Henry";
        string email = "henry@mail.com";
        string phoneNumber = "0421857998";

        await DeleteAllContacts(db);
        Console.WriteLine("Deleted all contacts.");

        Console.WriteLine("Add new contact");
        await AddContact(db, name, email, phoneNumber);

        Console.WriteLine("Print all contacts");
        var contactList = await GetAllContacts(db);
        if (contactList.Count == 0)
        {
            Console.WriteLine("No contacts");
        }
        else
        {
            foreach (Contact record in contactList)
            {
                Console.WriteLine(record);
            }
        }

        // Update contact's name
        var contact = await FindContact(db, phoneNumber);
        if (contact != null)
        {
            Console.WriteLine($"Found contact: Name={contact.Name}, Email={contact.Email}, Phone={contact.PhoneNumber}");
        }
        else
        {
            Console.WriteLine("Contact not found");
        }

        Console.WriteLine("Update contact's name");
        await UpdateContact(db, phoneNumber, "Henry Hua");

        contact = await FindContact(db, phoneNumber);
        if (contact != null)
        {
            Console.WriteLine($"Found contact: Name={contact.Name}, Email={contact.Email}, Phone={contact.PhoneNumber}");
        }
        else
        {
            Console.WriteLine("Contact not found");
        }

    }


    static async Task AddContact(PhoneBookContext db, string name, string email, string phoneNumber)
    {
        Contact? existingContact = await FindContact(db, phoneNumber);
        if (existingContact == null)
        {
            Contact contact = new() { Name = name, PhoneNumber = phoneNumber, Email = email };
            db.Contacts.Add(contact);
            await db.SaveChangesAsync();
            Console.WriteLine("New contact added.");
        }
        else
        {
            Console.WriteLine("Existing contact found.");
        }
    }

    static async Task<Contact?> FindContact(PhoneBookContext db, string phoneNumber)
    {
        var contact = await db.Contacts
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        return contact;
    }

    static async Task<bool> UpdateContact(PhoneBookContext db, string phoneNumber, string newName)
    {
        var contact = await db.Contacts
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        if (contact != null)
        {
            contact.Name = newName;
            await db.SaveChangesAsync();
            return true;
        }
        Console.WriteLine("Contact not found.");
        return false;
    }

    static async Task<List<Contact>> GetAllContacts(PhoneBookContext db)
    {
        var contacts = await db.Contacts.ToListAsync();

        return contacts;
    }

    static async Task DeleteAllContacts(PhoneBookContext db)
    {
        var allContacts = await db.Contacts.ToListAsync();
        db.RemoveRange(db.Contacts);
        await db.SaveChangesAsync();

        db.ChangeTracker.Clear();
    }
}
