namespace BlogApi.Exceptions;

public class NotFoundException(string message) : Exception(message);