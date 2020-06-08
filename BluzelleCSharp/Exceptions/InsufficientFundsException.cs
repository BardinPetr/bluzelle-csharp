using System;

namespace BluzelleCSharp.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string value) : base(value)
        {
        }
    }
}