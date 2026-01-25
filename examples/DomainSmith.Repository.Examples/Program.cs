using DomainSmith.Abstraction.Core.Maybe;
using DomainSmith.Repository.Examples.AggregateRoots;

Console.WriteLine("Start Test");

var owner = Owner.Create("John", "Nowak", "john.nowak@test.com").Value();

var ownerRepository = new OwnerRepository();

await ownerRepository.AddAsync(owner);

ownerRepository.Remove(owner);

internal class OwnerRepository : IOwnerRepository
{
    public Task<Maybe<Owner>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Owner>> GetAllAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Owner owner, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1000, cancellationToken);

        Console.WriteLine("OwnerAdded");
    }

    public void Remove(Owner id)
    {
        Task.Delay(1000);

        Console.WriteLine("OwnerRemoved");
    }
}