using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.ViewModels
{
    public class Response
    {
        public Response()
        {
            Status = Status.Success;
            Message = Status.Success.ToString();
        }
        public Status Status { get; set; }
        public string Message { get; set; }
        public int TotalRecords { get; set; }
        public object Data { get; set; }
        public object Error { get; set; }
    }

    public enum Status
    {
        Success = 200,
        Created = 201,
        NoDataAvailable=204,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        InternalServerError = 500
    }
}
