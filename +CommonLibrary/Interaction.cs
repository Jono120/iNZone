using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Olympic.AutoDataLayer;

namespace CommonLibrary
{
    [Table(BuildMode = BuildMode.DontBuild)]
    public class Interaction : AutoDataSupport<Interaction>
    {
        [Unique()]
        [IncludeDB("ID")]
        private Guid _id = Guid.NewGuid();

        public Guid ID
        {
            get { return _id; }
        }

        public Guid ParticipantID { get; set; }

        public int PartnerID { get; set; }

        public bool? Subscribed { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
