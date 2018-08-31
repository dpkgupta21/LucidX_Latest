using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class CalendarEventsAPIParams
    {
        public string assignedTo { get; set; }
        public string calendarType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string connectionName { get; set; }
    }
}
