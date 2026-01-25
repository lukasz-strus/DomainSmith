using System.Text;
using DomainSmith.AggregateRoot.Generators.Models;

namespace DomainSmith.AggregateRoot.Generators;

internal static class EntityCollectionsMembersBuilder
{
    internal static void AppendRequiredUsings(StringBuilder usings, bool isResultPattern)
    {
        usings.Append("\nusing System.Linq;");

        if (!isResultPattern)
            return;

        usings.Append("\nusing DomainSmith.Abstraction.Common;");
        usings.Append("\nusing DomainSmith.Abstraction.Core.Result;");
    }

    internal static string Build(
        IReadOnlyList<EntityCollectionInfo> entityCollections,
        bool isResultPattern,
        string aggregateRootClassName)
    {
        if (entityCollections.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();

        foreach (var c in entityCollections)
        {
            sb.AppendLine();

            if (c.GenerateProperty)
            {
                sb.AppendLine(
                    $"\tpublic IReadOnlyCollection<{c.ElementType}> {c.PropertyName} => {c.BackingFieldName};");
                sb.AppendLine();
            }

            var addName = $"AddNewElementTo{c.PropertyName}";
            var updateName = $"UpdateElementIn{c.PropertyName}";
            var deleteName = $"DeleteElementFrom{c.PropertyName}";

            var argsDecl = string.Join(", ", c.CtorArgs.Select(a => $"{a.Type} {a.Name}"));
            var argsCall = string.Join(", ", c.CtorArgs.Select(a => a.Name));

            if (isResultPattern)
            {
                AppendOwnerWithResultPattern(sb, c, aggregateRootClassName, addName, updateName, deleteName, argsDecl,
                    argsCall);
            }
            else
            {
                AppendOwnerWithoutResultPattern(sb, c, addName, updateName, deleteName, argsDecl, argsCall);
            }
        }

        return sb.ToString();
    }

    private static void AppendOwnerWithResultPattern(
        StringBuilder sb,
        EntityCollectionInfo c,
        string aggregateRootClassName,
        string addName,
        string updateName,
        string deleteName,
        string argsDecl,
        string argsCall)
    {
        if (c.ElementIsResultPattern)
        {
            sb.AppendLine($"\tpublic Result<{c.ElementType}> {addName}({argsDecl})");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvar result = {c.ElementType}.Create({argsCall});");
            sb.AppendLine("\t\tif (result.IsFailure)");
            sb.AppendLine("\t\t\treturn result;");
            sb.AppendLine();
            sb.AppendLine("\t\tvar entity = result.Value();");
            sb.AppendLine($"\t\t{c.BackingFieldName}.Add(entity);");
            sb.AppendLine();
            sb.AppendLine("\t\treturn entity;");
            sb.AppendLine("\t}");
            sb.AppendLine();

            sb.AppendLine($"\tpublic Result {updateName}({c.ElementIdType} id, {argsDecl})");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvar entity = {c.BackingFieldName}.FirstOrDefault(a => a.Id == id);");
            sb.AppendLine("\t\tif (entity is null)");
            sb.AppendLine(
                $"\t\t\treturn Result.Failure(new Error(\"{aggregateRootClassName}.{c.PropertyName}\", \"Not found\"));");
            sb.AppendLine();
            sb.AppendLine($"\t\tvar result = entity.Update({argsCall});");
            sb.AppendLine();
            sb.AppendLine("\t\treturn result;");
            sb.AppendLine("\t}");
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine($"\tpublic Result<{c.ElementType}> {addName}({argsDecl})");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvar entity = {c.ElementType}.Create({argsCall});");
            sb.AppendLine("\t\tif (entity is null)");
            sb.AppendLine(
                $"\t\t\treturn Result.Failure<{c.ElementType}>(new Error(\"{c.ElementType}.Create\", \"Creation failed\"));");
            sb.AppendLine();
            sb.AppendLine($"\t\t{c.BackingFieldName}.Add(entity);");
            sb.AppendLine();
            sb.AppendLine("\t\treturn entity;");
            sb.AppendLine("\t}");
            sb.AppendLine();

            sb.AppendLine($"\tpublic Result {updateName}({c.ElementIdType} id, {argsDecl})");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvar entity = {c.BackingFieldName}.FirstOrDefault(a => a.Id == id);");
            sb.AppendLine("\t\tif (entity is null)");
            sb.AppendLine(
                $"\t\t\treturn Result.Failure(new Error(\"{aggregateRootClassName}.{c.PropertyName}\", \"Not found\"));");
            sb.AppendLine();
            sb.AppendLine($"\t\tentity.Update({argsCall});");
            sb.AppendLine();
            sb.AppendLine("\t\treturn Result.Success();");
            sb.AppendLine("\t}");
            sb.AppendLine();
        }

        sb.AppendLine($"\tpublic Result {deleteName}({c.ElementIdType} id)");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\tvar entity = {c.BackingFieldName}.FirstOrDefault(a => a.Id == id);");
        sb.AppendLine("\t\tif (entity is null)");
        sb.AppendLine(
            $"\t\t\treturn Result.Failure(new Error(\"{aggregateRootClassName}.{c.PropertyName}\", \"Not found\"));");
        sb.AppendLine();
        sb.AppendLine($"\t\t{c.BackingFieldName}.Remove(entity);");
        sb.AppendLine();
        sb.AppendLine("\t\treturn Result.Success();");
        sb.AppendLine("\t}");
    }

    private static void AppendOwnerWithoutResultPattern(
        StringBuilder sb,
        EntityCollectionInfo c,
        string addName,
        string updateName,
        string deleteName,
        string argsDecl,
        string argsCall)
    {
        if (c.ElementIsResultPattern)
        {
            sb.AppendLine($"\tpublic {c.ElementType}? {addName}({argsDecl})");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvar result = {c.ElementType}.Create({argsCall});");
            sb.AppendLine("\t\tif (result.IsFailure)");
            sb.AppendLine("\t\t\treturn null;");
            sb.AppendLine();
            sb.AppendLine("\t\tvar entity = result.Value();");
            sb.AppendLine($"\t\t{c.BackingFieldName}.Add(entity);");
            sb.AppendLine();
            sb.AppendLine("\t\treturn entity;");
            sb.AppendLine("\t}");
            sb.AppendLine();

            sb.AppendLine($"\tpublic void {updateName}({c.ElementIdType} id, {argsDecl})");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvar entity = {c.BackingFieldName}.FirstOrDefault(a => a.Id == id);");
            sb.AppendLine("\t\tif (entity is null)");
            sb.AppendLine("\t\t\treturn;");
            sb.AppendLine();
            sb.AppendLine($"\t\tvar result = entity.Update({argsCall});");
            sb.AppendLine("\t\tif (result.IsFailure)");
            sb.AppendLine("\t\t\treturn;");
            sb.AppendLine("\t}");
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine($"\tpublic {c.ElementType}? {addName}({argsDecl})");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvar entity = {c.ElementType}.Create({argsCall});");
            sb.AppendLine("\t\tif (entity is null)");
            sb.AppendLine("\t\t\treturn null;");
            sb.AppendLine();
            sb.AppendLine($"\t\t{c.BackingFieldName}.Add(entity);");
            sb.AppendLine();
            sb.AppendLine("\t\treturn entity;");
            sb.AppendLine("\t}");
            sb.AppendLine();

            sb.AppendLine($"\tpublic void {updateName}({c.ElementIdType} id, {argsDecl})");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tvar entity = {c.BackingFieldName}.FirstOrDefault(a => a.Id == id);");
            sb.AppendLine("\t\tif (entity is null)");
            sb.AppendLine("\t\t\treturn;");
            sb.AppendLine();
            sb.AppendLine($"\t\tentity.Update({argsCall});");
            sb.AppendLine("\t}");
            sb.AppendLine();
        }

        sb.AppendLine($"\tpublic void {deleteName}({c.ElementIdType} id)");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\tvar entity = {c.BackingFieldName}.FirstOrDefault(a => a.Id == id);");
        sb.AppendLine("\t\tif (entity is null)");
        sb.AppendLine("\t\t\treturn;");
        sb.AppendLine();
        sb.AppendLine($"\t\t{c.BackingFieldName}.Remove(entity);");
        sb.AppendLine("\t}");
    }
}