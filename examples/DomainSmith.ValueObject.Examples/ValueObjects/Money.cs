namespace DomainSmith.ValueObject.Examples.ValueObjects;

[ValueObject]
public partial record Money
{
    public static int MaxAmount = 10000;
    public static int MinAmount = 0;
    public static int MaxCurrencyLength = 3;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "USD";
}