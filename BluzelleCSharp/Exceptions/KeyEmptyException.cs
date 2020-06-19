using System;

namespace BluzelleCSharp.Exceptions
{
    public class KeyEmptyException : Exception
    {
        public KeyEmptyException() : base("Key cannot be empty")
        {
            
        }
    }
}