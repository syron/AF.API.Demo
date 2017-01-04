using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace af.apidemo.webapi.Models.Response
{
    public class Error
    {
        public Error() { }
        public Error(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }
}