namespace Microservice.Banking.Common;

public class ValidationException : Exception
{
  public ValidationException(string message) : this(message, null) { }
  public ValidationException(string message, params ValidationError[] errors) : base(message, null)
  {
    this.Errors = errors ?? Enumerable.Empty<ValidationError>();
  }

  public IEnumerable<ValidationError> Errors { get; private set; }

}