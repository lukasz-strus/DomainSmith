using DomainSmith.Entity;
using TestNamespace;


var testEntity = TestEntity.Create("Test1");

Console.WriteLine(testEntity.Name);

testEntity.Update("Test");

Console.WriteLine(testEntity.Name);
Console.WriteLine("Koniec Testu");

namespace TestNamespace
{
    [Entity(typeof(Guid))]
    public partial class TestEntity
    {
        public string Name { get; private set; }
    }
}