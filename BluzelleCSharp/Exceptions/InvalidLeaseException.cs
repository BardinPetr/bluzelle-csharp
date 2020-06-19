using System;

namespace BluzelleCSharp.Exceptions
{
    public class InvalidLeaseException : Exception
    {
        public InvalidLeaseException() : base("Invalid lease time")
        {
        }
    }
}