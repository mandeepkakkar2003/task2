namespace Domain.Errors;
using System;

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }
}
