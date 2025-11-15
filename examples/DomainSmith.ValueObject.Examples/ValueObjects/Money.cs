namespace DomainSmith.ValueObject.Examples.ValueObjects;

[ValueObject]
public partial record Money
{
    public static int MaxAmount = 10000;
    public static int MinAmount = 0;
    public static int MaxCurrencyLength = 3;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "USD";

    static partial void OnCreating(ref decimal amount, ref string currency)
    {
        if (currency.Length > MaxCurrencyLength)
            currency = currency[..MaxCurrencyLength];
    }

    static partial void OnCreated(Money instance)
    {
        if (instance.Amount < MinAmount || instance.Amount > MaxAmount)
        {
            instance.DisallowCreate();
        }
        else
        {
            instance.AllowCreate();
        }
    }
}