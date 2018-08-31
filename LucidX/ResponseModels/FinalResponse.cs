using System;
using System.Net;
using System.Data;

namespace LucidX.ResponseModels
{
    public class FinalResponse
    {

        public string ErrrorMessageFromDal { get; set; }
        public bool ISFailed { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Object ResultDoc { get; set; }
        public DataSet ResultDs { get; set; }
        public DataTable ResultDt { get; set; }

    }
}
