using System;

namespace BluzelleCSharp.Exceptions
{
    public class KeyAlreadyExistsException : Exception
    {
        public KeyAlreadyExistsException() : base("Key already exists")
        {
        }
    }
}