using System;

namespace LHRP.Api.Runtime.ErrorHandling.Errors
{
    public class RuntimeError
    {
        public RuntimeError(string message)
        {
            this.Message = message;
            this.Date = DateTime.Now;

        }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}