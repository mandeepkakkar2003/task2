using System;

namespace Domain.Errors;

public class BadRequestException : DomainException
{
    public BadRequestException(string message) : base(message) { }
}
