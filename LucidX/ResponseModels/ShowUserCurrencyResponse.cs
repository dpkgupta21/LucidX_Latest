using System;

namespace LucidX.ResponseModels
{
    public class ShowUserCurrencyResponse
    {
        public int CurrencyCodeID { get; set; }

        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyUnit { get; set; }
        public string CurrencyName { get; set; }
        public bool RSS_Data { get; set; }
    }
}
