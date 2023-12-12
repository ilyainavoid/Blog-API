namespace BlogApi.Exceptions;

public class ForbiddenException(string message) : Exception(message);