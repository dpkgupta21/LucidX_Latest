using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LucidX.ResponseModels
{
    [Serializable]
    [XmlRoot(ElementName = "LedgerOrder")]
    public class LedgerOrder
    {
        public int CompCode { get; set; }
        public string AccountCode { get; set; }
        public string PresetCode { get; set; }
        public int TransactionID { get; set; }
        public int TabID { get; set; }
        public string TransactionReference { get; set; }
        public string TransPeriod { get; set; }
        public string TransDate { get; set; }
        public int JournalNo { get; set; }
        public int JournalLine { get; set; }
        public string LineDescription { get; set; }
        public decimal BaseAmount { get; set; }
        public string DrCr { get; set; }
        public string MatchedFlag { get; set; }
        public string MatchedDate { get; set; }
        public string MatchedPeriod { get; set; }
        public int MatchedRef { get; set; }
        public decimal ReportingAmount { get; set; }
        public decimal ReportingCurrencyRate { get; set; }
        public bool IsActualCurrency { get; set; }
        public string DueDate { get; set; }
        public string BA_01 { get; set; }
        public string BA_02 { get; set; }
        public string BA_03 { get; set; }
        public string BA_04 { get; set; }
        public string BA_05 { get; set; }
        public string BA_06 { get; set; }
        public string BA_07 { get; set; }
        public string BA_08 { get; set; }
        public string BA_09 { get; set; }
        public string BA_10 { get; set; }
        public string BA_11 { get; set; }
        public string BA_12 { get; set; }
        public string BA_13 { get; set; }
        public string BA_14 { get; set; }
        public string BA_15 { get; set; }
        public string BA_16 { get; set; }
        public string BA_17 { get; set; }
        public string BA_18 { get; set; }
        public string BA_19 { get; set; }
        public string BA_20 { get; set; }
        public int ProcessedBy { get; set; }
        public string ProcessedDate { get; set; }
        public string ProcessedPeriod { get; set; }
        public int AccountId { get; set; }
        public List<LedgerOrderItem> LedgerOrderItems { get; set; }

        // Added new 
        public string AccountName { get; set; }
        public bool ISForexRecord { get; set; }
        public decimal CompleteTotal { get; set; }

        public string CountryCode { get; set; }

    }
}
