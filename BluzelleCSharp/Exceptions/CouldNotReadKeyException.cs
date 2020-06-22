using System;

namespace BluzelleCSharp.Exceptions
{
    public class CouldNotReadKeyException : Exception
    {
        public CouldNotReadKeyException() : base("could not read key")
        {
        }
    }
}