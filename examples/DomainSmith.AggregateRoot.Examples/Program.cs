using DomainSmith.AggregateRoot.Examples.AggregateRoots;

Console.WriteLine("Start Testu");

var result = Owner.Create("Łukasz", "Strus", "lukasz.strus@gmail.com",
    new Address("Ulica", "Warszawa", "mazowieckie", "00-000"));

var owner = result.Value();

var newCarResult = owner.AddNewElementToNewCars("Audi", "A6", 250000m);

var newCar = newCarResult.Value();

var oldCarResult = owner.AddNewElementToOldCars("BMW", "X5", 200000m);

var oldCar = oldCarResult.Value();

Console.WriteLine($"Owner created: {owner.FirstName} {owner.LastName}, email: {owner.Email}");
Console.WriteLine($"New Car added: {newCar.Name} {newCar.Type}, price: {newCar.Price}");
Console.WriteLine($"Old Car added: {oldCar.Name} {oldCar.Type}, price: {oldCar.Price}");