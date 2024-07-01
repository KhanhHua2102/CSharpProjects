using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class PhoneBookContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public string DbPath { get; }

    public PhoneBookContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "phonebook.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer($"Data Source=localhost,1433;Database=PhoneBook.db;User Id=sa;Password=koos5CISH-lict;TrustServerCertificate=true;");
}

public class Contact
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Email: {Email}, Phone: {PhoneNumber}";
    }
}
