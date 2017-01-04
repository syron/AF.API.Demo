using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace af.apidemo.webapi.Models.Response
{
    public class Success<T>
    {
        public Success() { }
        public Success(T obj)
        {
            this.data = obj;
        }

        public T data { get; set; }
    }
}