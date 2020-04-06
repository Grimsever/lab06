#nullable enable
using System;

namespace Lab06.MVC.Domain.OwException
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string? paramName) : base($"User by {paramName} can`t be null")
        {
        }
    }
}