using System;

namespace BluzelleCSharp.Exceptions
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException() : base("key not found")
        {
        }
    }
}