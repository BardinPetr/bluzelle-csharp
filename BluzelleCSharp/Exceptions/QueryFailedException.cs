using System;

namespace BluzelleCSharp.Exceptions
{
    public class QueryFailedException : Exception
    {
        public QueryFailedException(string value) : base(value)
        {
        }
    }
}