﻿namespace BlogApi.Exceptions;

public class UnauthorizedException(string message) : Exception(message);