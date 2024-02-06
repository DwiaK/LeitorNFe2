namespace LeitorNFe.SharedKernel;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "O valor do resultado é nulo.");

    public static readonly Error ConditionNotMet = new("Error.ConditionNotMet", "A condição especificada não foi satisfeita.");

    public static implicit operator Result(Error error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);
}
