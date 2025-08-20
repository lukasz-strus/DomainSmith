using DomainSmith.Entity;
using TestNamespace;

Console.WriteLine("Koniec Testu");

namespace TestNamespace
{
    [Entity(typeof(Guid))]
    public partial class TestEntity
    {
        public string Name { get; private set; }
        public string Name2 { get; private set; }
        public int Name3 { get; private set; }
        public DateTime Name4 { get; private set; }
        public TestRecord TestRecord { get; private set; }
    }

    public record TestRecord(string Value);
}