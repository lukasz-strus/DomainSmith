using DomainSmith.ValueObject.Examples.ValueObjects;

//[assembly: NoResultPattern]

Console.WriteLine("Hello, World!");

var result = Money.Create(10.5m, "USD123");
var money = result.Value();

Console.WriteLine($"Money Amount: {money?.Amount}, Currency: {money?.Currency}");