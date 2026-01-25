# DomainSmith

[![CICD](https://github.com/lukasz-strus/DomainSmith/actions/workflows/main.yml/badge.svg)](https://github.com/lukasz-strus/DomainSmith/actions/workflows/main.yml)
[![codecov](https://codecov.io/gh/lukasz-strus/DomainSmith/branch/main/graph/badge.svg?token=DAD5PSBP23)](https://codecov.io/gh/lukasz-strus/DomainSmith)

Roslyn Source Generator for .NET that automates Domain-Driven Design patterns and Clean Architecture components.

## What this is

`DomainSmith` is a set of Roslyn source generators that generate DDD/Clean Architecture boilerplate **from simple domain types** annotated with attributes:

- `ValueObject` (immutability / value-based equality + factories and updates)
- `Entity` (ID + create/update API)
- `AggregateRoot` (ID + support for entity collections inside the aggregate)
- `Repository` (repository interface for an Aggregate Root)

The generator does not replace your domain logic. Its goal is to generate a consistent API and extensions (for example `Create(...)`, `Update(...)`, entity-collection helpers, etc.) from the fields/properties you already defined.

Packages in `src/` are independent and target `netstandard2.0` (generators are consumed by application projects). Additionally, `DomainSmith.Abstraction` provides shared DDD primitives (`ValueObject`, `Entity<TId>`, `AggregateRoot<TId>`) and a lightweight `Result` pattern.

## Projects in the solution

- `src/DomainSmith.Abstraction` – shared primitives, `Result`, `Maybe`, and helper attributes used by the generators.
- `src/DomainSmith.ValueObject` – Value Object generator (`[ValueObject]`).
- `src/DomainSmith.Entity` – Entity generator (`[Entity(typeof(...))]`).
- `src/DomainSmith.AggregateRoot` – Aggregate Root generator (`[AggregateRoot(typeof(...))]`) + `EntityCollections` support.
- `src/DomainSmith.Repository` – repository interface generator for Aggregate Roots.
- `examples/*` – usage examples.
- `tests/*` – generator tests.

## Shared primitives (Abstraction)

The generators assume (and the solution provides) base types:

- `DomainSmith.Abstraction.Core.Primitives.ValueObject`
- `DomainSmith.Abstraction.Core.Primitives.Entity<TId>`
- `DomainSmith.Abstraction.Core.Primitives.AggregateRoot<TId>`

The built-in `Result` pattern is available in `DomainSmith.Abstraction.Core.Result`:

- `Result` / `Result<T>` + `Error`

### Disabling and configuring generation

- `DomainSmith.Abstraction.Common.ExcludeFromGenerationAttribute`
  - mark a `property` / `field` to exclude it from the generated API.
- `DomainSmith.Abstraction.Common.NoResultPatternAttribute`
  - can be applied at assembly or class level to make the generator **not** generate `Result`-based APIs.

## How the generator decides what to generate

Each generator follows a similar pipeline:

1. Find a type (class/record) annotated with a specific attribute.
2. Read:
   - type name, namespace, and `using` directives (to make generated code match the source context),
   - a list of “augmentable” properties (typically public properties, excluding members annotated with `[ExcludeFromGeneration]`),
   - `Result pattern` configuration (`[NoResultPattern]`).
3. Generate code via a builder (`*Builder`) and add it to compilation as `*.g.cs`.

## Value Object (`[ValueObject]`)

Attribute: `DomainSmith.ValueObject.ValueObjectAttribute`

Generator input:

- a type (class/record) annotated with `[ValueObject]`
- properties on that type (excluding `[ExcludeFromGeneration]`)
- whether the `Result pattern` is enabled

What gets generated:

- code that provides a consistent way to create and update the object based on defined properties,
- factory/update methods like `Create(...)` and `Update(...)` (variants depend on `Result pattern`),
- an extension/partial API for the type (the generator also tracks “extension name” and type reference).

`Result pattern`:

- by default, the generator produces APIs returning `Result` / `Result<T>`.
- if you use `[NoResultPattern]`, the generator switches to APIs without `Result`.

## Entity (`[Entity(typeof(TId))]`)

Attribute: `DomainSmith.Entity.EntityAttribute`

Generator input:

- a class annotated with `[Entity(typeof(SomeIdType))]` – the generator reads the ID type from the attribute argument,
- ID metadata (for example whether it is a `record`/`class` and the underlying value type) is used to tailor generation,
- entity properties (excluding `[ExcludeFromGeneration]`),
- `Result pattern` configuration.

What gets generated:

- a consistent API for entity creation (`Create(...)`) and modification (`Update(...)`),
- integration with `Entity<TId>` (from `DomainSmith.Abstraction`),
- `Result`-based variants (default) or non-`Result` variants (with `[NoResultPattern]`).

## Aggregate Root (`[AggregateRoot(typeof(TId))]`)

Attribute: `DomainSmith.AggregateRoot.AggregateRootAttribute`

Generator input:

- a class annotated with `[AggregateRoot(typeof(SomeIdType))]` (similar to Entity),
- aggregate properties (excluding `[ExcludeFromGeneration]`),
- `Result pattern` configuration,
- **EntityCollections** – the generator analyzes the class and detects entity collections for which it should generate collection-management APIs.

### EntityCollections in AggregateRoot

If the generator detects that the aggregate has an entity collection (for example with a backing field), it generates:

- optionally, an `IReadOnlyCollection<T>` property exposing the collection,
- helper methods for collection management:
  	- `AddNewElementTo{CollectionName}(...)`
  - `UpdateElementIn{CollectionName}(id, ...)`
  - `DeleteElementFrom{CollectionName}(id)`

`Result pattern` variants:

- with `Result pattern`:
  - `Add...` returns `Result<TElement>`
  - `Update...` / `Delete...` return `Result`
  - when an element is not found, it returns `Result.Failure(new Error("{Aggregate}.{Collection}", "Not found"))`
- without `Result pattern`:
  - `Add...` returns `TElement?` (`null` on failure)
  - `Update...` / `Delete...` are `void` (silent failure, e.g., element not found)

Additionally, the generator detects whether the element entity itself uses the `Result pattern` (for example `TElement.Create(...)` may return `Result<TElement>`). In that case the generated code can:

- call `TElement.Create(...)`
- short-circuit and propagate the failure (or return `null`) if creation/update fails.

## Repository (for an Aggregate Root)

The repository generator is based on types annotated with `[AggregateRoot]`.

Input:

- an Aggregate Root class annotated with `[AggregateRoot(typeof(TId))]`
- the ID type `TId`

What gets generated:

- a repository interface named `I{AggregateRootName}Repository` (the output filename is set explicitly in the generator),
- method signatures tailored to the aggregate ID.

## Examples

The `examples/` directory contains projects showing minimal usage of the attributes and the generated output:

- `examples/DomainSmith.ValueObject.Examples`
- `examples/DomainSmith.Entity.Examples`
- `examples/DomainSmith.AggregateRoot.Examples`
- `examples/DomainSmith.Repository.Examples`

## Practical notes

- For source generators, the target type should typically be `partial` and have stable properties that define the generated API.
- If you do not want a member to participate in generation (technical fields, caches, etc.), mark it with `[ExcludeFromGeneration]`.
- If you do not use the `Result pattern`, add `[assembly: NoResultPattern]` or annotate individual types with `[NoResultPattern]`.
