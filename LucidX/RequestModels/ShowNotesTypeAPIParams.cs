using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class ShowNotesTypeAPIParams
    {
        public string connectionName { get; set; }
    }
}
