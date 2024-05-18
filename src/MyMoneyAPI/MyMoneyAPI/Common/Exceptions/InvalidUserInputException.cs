namespace MyMoneyAPI.Common.Exceptions;

public class InvalidUserInputException : Exception
{
    public InvalidUserInputException() : base() { }

    public InvalidUserInputException(string message) : base(message) { }

    public InvalidUserInputException(string message, Exception innerException) : base(message, innerException) { }
}
