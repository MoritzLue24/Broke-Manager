namespace Application.Common;

public abstract class BaseException(string message) : Exception(message)
{
    public override string ToString()
        => this.GetType().ToString();
}

// Thrown on unrecoverable errors on invalid states that violates certain invariants
// for example on NotificationHandlers, we cant return a Result type -> unrecoverable
public class InvariantViolationException(string message) : BaseException(message);