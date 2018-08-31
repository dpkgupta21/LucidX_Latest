using System;
using System.Collections.Generic;
using System.Text;

namespace LucidX.ResponseModels
{
    public class EmailCountResponse
    {
        public int inboxCount { get; set; }
        public int draftCount { get; set; }
        public int sentItemCount { get; set; }
        public int trashCount { get; set; }
        public int allocatedCount { get; set; }

    }
}
