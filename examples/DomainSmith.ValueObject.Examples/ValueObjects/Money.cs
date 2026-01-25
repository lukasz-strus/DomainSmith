using DomainSmith.Abstraction.Core.Result;

namespace DomainSmith.ValueObject.Examples.ValueObjects;

[ValueObject]
public partial record Money
{
    public static int MaxAmount = 10000;
    public static int MinAmount = 0;
    public static int MaxCurrencyLength = 3;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "USD";

    static partial void OnCreating(ref decimal amount, ref string currency, ref bool canCreate, ref Error error)
    {
        if (amount > MaxAmount)
        {
            canCreate = false;
            error = new Error("Money.Amount.ExceedsMax", $"Amount cannot exceed {MaxAmount}.");
            return;
        }

        if (amount < MinAmount)
        {
            canCreate = false;
            error = new Error("Money.Amount.BelowMin", $"Amount cannot be below {MinAmount}.");
            return;
        }

        if (currency.Length > MaxCurrencyLength)
        {
            canCreate = false;
            error = new Error("Money.Currency.ExceedsMaxLength",
                $"Currency cannot exceed {MaxCurrencyLength} characters.");
            return;
        }
    }
}