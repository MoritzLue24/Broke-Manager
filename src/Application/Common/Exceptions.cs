namespace Application.Common;

// Thrown on unrecoverable errors on invalid states that violates certain invariants
// for example on NotificationHandlers, we cant return a Result type -> unrecoverable
public class InvariantViolationException(string message) : Exception(message);