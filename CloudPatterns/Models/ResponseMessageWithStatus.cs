using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPatterns.Models
{
    public class ResponseMessageWithStatus
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ResponseMessageWithStatus(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
