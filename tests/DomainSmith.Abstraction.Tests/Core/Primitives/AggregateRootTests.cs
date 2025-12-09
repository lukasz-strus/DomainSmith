using System.Reflection;
using DomainSmith.Abstraction.Core.Primitives;
using FluentAssertions;

namespace DomainSmith.Abstraction.Tests.Core.Primitives
{
    internal sealed class TestAggregateRoot : AggregateRoot<Guid>
    {
        public TestAggregateRoot() : base()
        {
        }

        public TestAggregateRoot(Guid id) : base(id)
        {
        }
    }

    public class AggregateRootTests
    {
        [Fact]
        public void Parameterless_constructor_should_create_instance()
        {
            // Act
            var aggregate = new TestAggregateRoot();

            // Assert
            aggregate.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_with_id_should_create_instance_and_set_id_if_available()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var aggregate = new TestAggregateRoot(id);

            // Assert
            aggregate.Should().NotBeNull();

            var idProp = aggregate.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (idProp == null || !idProp.CanRead) return;
            var value = idProp.GetValue(aggregate);

            value.Should().BeOfType<Guid>();
            value.Should().Be(id);
        }

        [Fact]
        public void AggregateRoot_is_abstract_and_has_protected_constructors()
        {
            // Assert
            var type = typeof(AggregateRoot<>);
            type.GetTypeInfo().IsAbstract.Should().BeTrue();

            var protectedCtors = type.GetTypeInfo().DeclaredConstructors;
            var constructorInfos = protectedCtors as ConstructorInfo[] ?? protectedCtors.ToArray();
            constructorInfos.Should().Contain(c => c.IsFamily && c.GetParameters().Length == 0);
            constructorInfos.Should().Contain(c => c.IsFamily && c.GetParameters().Length == 1);
        }
    }
}