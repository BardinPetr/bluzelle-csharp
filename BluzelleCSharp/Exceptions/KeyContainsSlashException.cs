using System;

namespace BluzelleCSharp.Exceptions
{
    public class KeyContainsSlashException : Exception
    {
        public KeyContainsSlashException() : base("Key cannot contain a slash")
        {
        }
    }
}