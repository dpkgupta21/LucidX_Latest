using System;

namespace LucidX.ResponseModels
{
    public class AccountOrdersResponse
    {
        public int CompCode { get; set; }
        public string AccountCode { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string CountryCode { get; set; }
        public int StateID { get; set; }
        public string City { get; set; }
        public string ContactPerson { get; set; }
        public string Telephone { get; set; }
    }
}
