

using System.Collections.Generic;

namespace LucidX.Droid.Source.Models
{
    public class GroupMenuModel
    {
        public string groupMenuIcon{ get; set; }
        public string menuName { get; set; }

        public List<ChildMenuModel> submenuList { get; set; }

    }
}