namespace Domain.Errors;
using System;

public class ForbiddenException : DomainException
{
    public ForbiddenException(string message) : base(message) { }
}
