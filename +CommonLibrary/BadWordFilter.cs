using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Olympic.AutoDataLayer;

namespace CommonLibrary
{
	[Table(BuildMode = BuildMode.DontBuild)]
	public class BadWordFilterClass : AutoDataSupport<BadWordFilterClass>
	{
		[Unique()]
        [IncludeDB("ID")]

		public int ID { get; set; }

		public String BadWord { get; set; }

		public Boolean UseWildCard { get; set; }

		public DateTime DateCreated { get; set; }
	}
}
