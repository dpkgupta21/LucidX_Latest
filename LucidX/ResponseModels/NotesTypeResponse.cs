namespace LucidX.ResponseModels
{
    public class NotesTypeResponse
    {
        public int NotesTypeId { get; set; }
        public string NotesTypeName { get; set; }
        public bool IsSystem { get; set; }
        public string eResourceNo { get; set; }

        public bool IsSelected{ get; set; }

    }
}
