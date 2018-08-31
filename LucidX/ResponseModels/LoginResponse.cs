using System;
using System.Collections.Generic;
using System.Text;

namespace LucidX.ResponseModels
{
    public class LoginResponse
    {
        public string UserId{ get; set; }
        public string Name { get; set; }
        public string UserCompCode { get; set; }
        public string UserEmail { get; set; }
        public bool IsAuthenticate { get; set; }

    }
}
