using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class AddCalendarEventsAPIParams
    {
        public string entryId { get; set; }
        public string compCode { get; set; }
        public string accCode { get; set; }
        public string notesTypeId { get; set; }
        public int entryTypeId { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string subject { get; set; }
        public string details { get; set; }
        public string assignedTo { get; set; }
        public string summaryItemId { get; set; }
        public bool isCompleted { get; set; }
        public bool isPublic { get; set; }
        public string accountId { get; set; }
        public string connectionName { get; set; }
    }
}
