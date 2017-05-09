using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Olympic.AutoDataLayer;

namespace CommonLibrary
{
    [Table(BuildMode = BuildMode.DontBuild)]
    public class InteractionVideo : AutoDataSupport<InteractionVideo>
    {
        [Unique()]
        [IncludeDB("ID")]
        private Guid _id = Guid.NewGuid();

        public Guid ID
        {
            get { return _id; }
        }

        public Guid InteractionID { get; set; }

        public int VideoRating { get; set; }

        public int VideoID { get; set; }

        public String VideoName { get; set; }

        public DateTime DateCreated { get; set; }
       
    }
}
