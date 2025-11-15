using DomainSmith.ValueObject.Examples.ValueObjects;

Console.WriteLine("Hello, World!");

var money = Money.Create(10.5m, "USD123");

Console.WriteLine($"Money Amount: {money?.Amount}, Currency: {money?.Currency}");