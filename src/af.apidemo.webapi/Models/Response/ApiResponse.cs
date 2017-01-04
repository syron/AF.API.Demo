using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace af.apidemo.webapi.Models.Response
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {

        }
        public ApiResponse(T obj)
        {
            Data = obj;
        }
        public ApiResponse(int code, string message)
        {
            Error = new Error(code, message);
        }
        public T Data { get; set; }
        public Error Error { get; set; }
    }
}