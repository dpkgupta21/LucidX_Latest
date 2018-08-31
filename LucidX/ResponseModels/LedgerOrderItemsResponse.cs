using System;

namespace LucidX.ResponseModels
{
    public class LedgerOrderItemsResponse
    {
        public int CompCode { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public int AccountId { get; set; }
        public int TransactionID { get; set; }
        public int TabID { get; set; }
        public int JournalNo { get; set; }
        public int JournalLine { get; set; }
        public bool IsActualCurrency { get; set; }
        public string LineDescription { get; set; }
        public decimal BaseAmount { get; set; }
        public int MatchedRef { get; set; }
        public int ReportingAmount { get; set; }
        public int ReportingCurrencyRate { get; set; }
        public int ProcessedBy { get; set; }   
        public decimal TaxAmount { get; set; }
        
     
    
    }
}
