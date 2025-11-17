using DomainSmith.Entity.Examples.Entities;

Console.WriteLine("Start Testu");

var owner = Owner.Create("Łukasz", "Strus", "lukasz.strus@gmail.com",
    new Address("Ulica", "Warszawa", "mazowieckie", "00-000"));

Console.WriteLine("Id: " + owner?.Id.Value);
Console.WriteLine("Name: " + owner?.FirstName + " " + owner?.LastName);
Console.WriteLine("Email: " + owner?.Email);
Console.WriteLine("Address: " + (owner?.Address is not null
    ? $"{owner.Address.Street}, {owner.Address.City}, {owner.Address.State} {owner.Address.ZipCode}"
    : "No address provided"));
Console.WriteLine("ModifiedAt: " + owner?.ModifiedAt);
Console.WriteLine("CreatedAt: " + owner?.CreatedAt);

Thread.Sleep(5000);
owner?.Update("Test", "Strus1", "lukasz1.strus@gmail.com",
    new Address("Ulica", "Warszawa", "mazowieckie", "00-000"));

Console.WriteLine("Id: " + owner?.Id.Value);
Console.WriteLine("Name: " + owner?.FirstName + " " + owner?.LastName);
Console.WriteLine("Email: " + owner?.Email);
Console.WriteLine("Address: " + (owner?.Address is not null
    ? $"{owner.Address.Street}, {owner.Address.City}, {owner.Address.State} {owner.Address.ZipCode}"
    : "No address provided"));
Console.WriteLine("ModifiedAt: " + owner?.ModifiedAt);
Console.WriteLine("CreatedAt: " + owner?.CreatedAt);

Console.WriteLine("Koniec Testu");