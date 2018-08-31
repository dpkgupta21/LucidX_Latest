using System;

namespace LucidX.ResponseModels
{
    public class ShowTaxRatesResponse
    {
        public string CountryCode { get; set; }

        public int StateID { get; set; }

        public int CityId { get; set; }
        public int TaxTypeID { get; set; }
        public string FinancialYear { get; set; }
        public string DateFrom { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public decimal TaxRatePercent { get; set; }
        public bool IsDefaultCode { get; set; }
        public int TaxID { get; set; }
    }
}
