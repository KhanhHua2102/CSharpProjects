using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task? Main()
    {
        using var db = new PhoneBookContext();

        bool exit = false;
        do
        {
            Console.WriteLine("\nWelcome to your PhoneBook");
            Console.WriteLine("Enter 0 to Exit");
            Console.WriteLine("Enter 1 to list all contacts");
            Console.WriteLine("Enter 2 to add new contact");
            Console.WriteLine("Enter 3 to update a contact's name");
            Console.WriteLine("Enter 4 to remove a contact");
            Console.WriteLine("Enter 5 to find a contact through phone number");
            Console.WriteLine("Enter 6 to delete all contacts");
            Console.WriteLine("Enter 7 to Send an email");

            decimal parsedValue = decimal.Parse(GetInput());
            int option = Convert.ToInt32(parsedValue);

            switch (option)
            {
                case 0:
                    exit = true;
                    break;

                case 1:
                    Console.WriteLine("\nList all contacts");
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
                    break;

                case 2:
                    Console.WriteLine("\nPlease enter your name:");
                    string name = GetInput();

                    string? email = null;
                    do
                    {
                        Console.WriteLine("\nPlease enter a valid email address!");
                        email = GetInput();
                    }
                    while (!EmailIsValid(email));

                    string? phoneNumber = null;
                    do
                    {
                        Console.WriteLine("\nPlease enter a valid phone number");
                        phoneNumber = GetInput();
                    }
                    while (!PhoneNumberIsValid(phoneNumber));

                    Console.WriteLine("\nAdd new contact");
                    await AddContact(db, name, email, phoneNumber);

                    break;

                case 3:
                    Console.WriteLine("Enter the phone number you want to change detail");
                    phoneNumber = GetInput();
                    Console.WriteLine("\nPlease enter new name:");
                    name = GetInput();
                    Console.WriteLine($"\nUpdate contact's name to {name}");
                    await UpdateContact(db, phoneNumber, name);
                    break;

                case 4:
                    Console.WriteLine("\nEnter the phone number you want to remove");
                    phoneNumber = GetInput();

                    bool success = await RemoveContact(db, phoneNumber);

                    if (success)
                    {
                        Console.WriteLine("Contact removed");
                    }
                    else
                    {
                        Console.WriteLine("Issue occurred");
                    }

                    break;

                case 5:
                    Console.WriteLine("\nEnter the phone number you want to find");
                    phoneNumber = GetInput();
                    Contact? contact = await FindContact(db, phoneNumber);
                    if (contact != null)
                    {
                        Console.WriteLine($"\nFound contact: Name={contact.Name}, Email={contact.Email}, Phone={contact.PhoneNumber}");
                    }
                    else
                    {
                        Console.WriteLine("Contact not found");
                    }
                    break;

                case 6:
                    await DeleteAllContacts(db);
                    Console.WriteLine("\nDeleted all contacts.");
                    break;

                case 7:
                    Console.WriteLine("\nLook for the email by their phone number:");
                    phoneNumber = GetInput();

                    string? toEmail = null;
                    contact = await FindContact(db, phoneNumber);
                    if (contact != null)
                    {
                        Console.WriteLine($"\nFound contact: Name={contact.Name}, Email={contact.Email}, Phone={contact.PhoneNumber}");

                        toEmail = contact.Email;
                        string subject = "Test mail";
                        string body = "This is a test mail only.";
                        SendEmail(toEmail, subject, body);
                        Console.WriteLine("Email sent!");
                    }
                    else
                    {
                        Console.WriteLine("Contact not found, please try again with another phone number");
                    }
                    break;
            }
        }
        while (!exit);
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
        Contact? contact = await db.Contacts
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

    static async Task<bool> RemoveContact(PhoneBookContext db, string phoneNumber)
    {
        Contact? contact = await db.Contacts
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);

        if (contact != null)
        {
            db.Remove(contact);
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

    static bool PhoneNumberIsValid(string phoneNumber)
    {
        if (phoneNumber.StartsWith('0') && phoneNumber.Length == 10)
        {
            return true;
        }

        return false;
    }

    static bool EmailIsValid(string email)
    {
        if (email.Contains('@'))
        {
            return true;
        }

        return false;
    }

    static string GetInput()
    {
        var input = Console.ReadLine();
        while (input == null)
        {
            Console.WriteLine("Please try again!");
            input = Console.ReadLine();
        }

        return input;
    }

    static void SendEmail(string toEmail, string subject, string body)
    {
        var fromAddress = new MailAddress("mail@gmail.com", "Henry Hua");
        var toAddress = new MailAddress(toEmail);
        const string fromPassword = "joby buwf yhcw xmuw"; //  Example password (use App Password for Gmail client)
        const string smtpHost = "smtp.gmail.com"; // Example host for Gmail client
        const int smtpPort = 587;

        var smtp = new SmtpClient
        {
            Host = smtpHost,
            Port = smtpPort,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        })
        {
            smtp.Send(message);
        }
    }
}
