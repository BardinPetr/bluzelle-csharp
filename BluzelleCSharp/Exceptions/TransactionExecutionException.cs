using System;

namespace BluzelleCSharp.Exceptions
{
    public class TransactionExecutionException : Exception
    {
        public TransactionExecutionException(string err) : base(err)
        {
        }
    }
}