using DomainSmith.AggregateRoot;

namespace DomainSmith.Repository.Examples.AggregateRoots;

[AggregateRoot(typeof(Guid))]
public partial class Owner
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
}