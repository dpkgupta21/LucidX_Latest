using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class InboxEmailApiParams
    {

        public int mailCount { get; set; }
        public int mailTypeId { get; set; }
        public int filterIndex { get; set; }
        public string filterEmail { get; set; }
        public bool blnShowblank { get; set; }
        public string intUserID { get; set; }
        public string connectionName { get; set; }
    }
}
