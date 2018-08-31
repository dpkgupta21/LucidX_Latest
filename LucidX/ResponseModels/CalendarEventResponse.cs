using System;
using System.Collections.Generic;
using System.Text;

namespace LucidX.ResponseModels
{
    public class CalendarEventResponse
    {
        public string EntryId { get; set; }
        public string CompCode { get; set; }
        public string AccountCode { get; set; }
        public string NotesTypeId { get; set; }
        public int EntryTypeId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public string AssignedTo { get; set; }
        public string Completed { get; set; }
        public bool IsPublic { get; set; }
        public string AccountId { get; set; }
       
    }
}
