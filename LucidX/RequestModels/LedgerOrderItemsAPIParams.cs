using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class LedgerOrderItemsAPIParams
    {
        public int compCode { get; set; }
        public int journalNo { get; set; }    
        public string connectionName { get; set; }
        
    }
}
