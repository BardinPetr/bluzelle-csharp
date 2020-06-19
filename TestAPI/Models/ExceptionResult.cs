using System;

namespace TestAPI.Models
{
    internal class ExceptionResult
    {
        public ExceptionResult(Exception ex)
        {
             Error = ex.Message;
             Message = ex.GetType().ToString();
        }

        public string Message { get; }
        public string Error { get; }
    }
}