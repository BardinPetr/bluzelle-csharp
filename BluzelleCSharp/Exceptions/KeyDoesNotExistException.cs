using System;

namespace BluzelleCSharp.Exceptions
{
    public class KeyDoesNotExistException : Exception
    {
        public KeyDoesNotExistException(string id) : base("Key does not exist" + (id.Length == 0 ? "" : $" {id}"))
        {
        }
    }
}