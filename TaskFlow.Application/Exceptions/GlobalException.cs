namespace TaskFlow.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {

    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {

    }
}

public class ValidationAppException : Exception
{
    public IEnumerable<string> Errors { get; }
    public ValidationAppException(string message, IEnumerable<string> errors) : base(message)
    {
        Errors = errors;
    }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Unauthorized") : base(message)
    {

    }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "Forbidden") : base(message)
    {

    }
}
