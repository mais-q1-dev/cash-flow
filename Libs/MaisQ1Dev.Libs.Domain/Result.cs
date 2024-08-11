using System.Diagnostics.CodeAnalysis;

namespace MaisQ1Dev.Libs.Domain;

public class Result
{
    protected internal Result(int code, List<Error> errors)
    {
        Code = code;
        if (errors.Count != 0)
            _errors.AddRange(errors);
    }

    public int Code { get; private set; }
    private List<Error> _errors = [];
    public IReadOnlyCollection<Error> Errors => _errors.AsReadOnly();

    public bool IsSuccess => Code >= 200 && Code <= 299;
    public bool IsFailure => !IsSuccess;

    public static Result Ok() => new(200, []);
    public static Result<TValue> Ok<TValue>(TValue value) => new(value, 200, []);
    
    public static Result<TValue> Created<TValue>(TValue value) => new(value, 201, []);
    
    public static Result<TValue> Accepted<TValue>(TValue value) => new(value, 202, []);
    
    public static Result NoContent() => new(204, []);

    public static Result NotFound(Error error) => new(404, [error]);
    public static Result<TValue> NotFound<TValue>(Error error) => new(default, 404, [error]);

    public static Result UnprocessableEntity(Error error) => new(422, [error]);
    public static Result UnprocessableEntity(List<Error> errors) => new(422, errors);
    public static Result<TValue> UnprocessableEntity<TValue>(Error error) => new(default, 422, [error]);
    public static Result<TValue> UnprocessableEntity<TValue>(List<Error> errors) => new(default, 422, errors);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, int code, List<Error> errors)
        : base(code, errors)
        => _value = value;

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result cann't be accessed.");
}
