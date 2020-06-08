using System;

namespace TestAPI
{
    internal class ExceptionResult
    {
        public ExceptionResult(Exception ex)
        {
            Message = ex.Message;
            Error = ex.GetType().ToString();
        }

        public string Message { get; }
        public string Error { get; }
    }
}