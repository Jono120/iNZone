using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Olympic.AutoDataLayer;

namespace CommonLibrary
{
    [Table(BuildMode = BuildMode.DontBuild)]
    public class Participant : AutoDataSupport<Participant>
    {

        [Unique()] 
        [IncludeDB("ID")] 
        private Guid _id = Guid.NewGuid();

        public Guid ID
        { 
            get { return _id; }
        }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public String SecurityQuestion { get; set; }

        public String SecurityAnswer { get; set; }

        public String UserName { get; set; }

        public String Password { get; set; }

        public String Address1 { get; set; }

        public String Address2 { get; set; }

        public String Suburb { get; set; }

        public String Town { get; set; }

        public String PhoneNumber { get; set; }

        public bool KnowsCareer { get; set; }

        public MaleFemale Gender { get; set; }

        public DateTime DateCreated { get; set; }

        public String KioskID { get; set; }

        public enum MaleFemale
        {
            M,
            F        
        }
    }
}
