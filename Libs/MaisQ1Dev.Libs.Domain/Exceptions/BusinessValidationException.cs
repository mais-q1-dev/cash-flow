namespace MaisQ1Dev.Libs.Domain.Exceptions;

public class BusinessValidationException : Exception
{
    public BusinessValidationException(IEnumerable<Error> businessValidations)
        : base("One or more validation failures have occurred.")
    {
        Errors = businessValidations;
    }

    public IEnumerable<Error> Errors { get; }
}