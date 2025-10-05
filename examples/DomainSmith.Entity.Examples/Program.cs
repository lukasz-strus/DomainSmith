using DomainSmith.Entity.Examples.Entities;

var owner = Owner.Create("Łukasz", "Strus", "lukasz.strus@gmail.com",
    new Address("Ulica", "Warszawa", "mazowieckie", "00-000"));

Console.WriteLine("Id: " + owner?.Id.Value);
Console.WriteLine("Name: " + owner?.FirstName + " " + owner?.LastName);
Console.WriteLine("Email: " + owner?.Email);
Console.WriteLine("Address: " + (owner?.Address is not null
    ? $"{owner.Address.Street}, {owner.Address.City}, {owner.Address.State} {owner.Address.ZipCode}"
    : "No address provided"));

owner?.Update("Łukasz1", "Strus1", "lukasz1.strus@gmail.com",
    new Address("Ulica", "Warszawa", "mazowieckie", "00-000"));

Console.WriteLine("Id: " + owner?.Id.Value);
Console.WriteLine("Name: " + owner?.FirstName + " " + owner?.LastName);
Console.WriteLine("Email: " + owner?.Email);
Console.WriteLine("Address: " + (owner?.Address is not null
    ? $"{owner.Address.Street}, {owner.Address.City}, {owner.Address.State} {owner.Address.ZipCode}"
    : "No address provided"));

Console.WriteLine("Koniec Testu");

//TODO: 1. zamienić Create i Update na metody internal
//TODO: 2. zastanowić się czy dorobić metody typu SetFirstName, SetLastName itp. czy zostawić tylko Update/Create