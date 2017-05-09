using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace KioskApplication
{
///     <summary>
///     Class used for deserialising the CategoriesConfig.xml file 
     
///     The xml structure is like this:
///         <JobSearchServiceProviders xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
///             <Provider Name="Find a Job" ImageName="findAJob-logo.gif" Url="http://job-bank.workandincome.govt.nz/find-a-job/search.aspx"/>
///             <Provider Name="Seek" ImageName="seek-logo.png" Url="http://www.seek.co.nz/?tracking=jobs:nz:sm:sem_brand:google_nz:ggl:txt_ad:brand_slsearch&gclid=CKH9kczC068CFcyGpAodAhhxfA"/>
///             <Provider Name="Trade me" ImageName="trade_me-logo.gif" Url="http://www.trademe.co.nz/jobs?gclid=CNrkteLC068CFWlLpgodYg1gcQ"/>
///         </JobSearchServiceProviders>
///     </summary>

    public class JobSearchServiceProviders
    {
        [XmlElement(ElementName = "Provider")]
        public List<Provider> Provider
        {
            get;
            set;
        }
    }

    public class Provider
    {
        /// <summary>
        /// Name of the Provider
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Image under Images folder 
        /// </summary>
        [XmlAttribute]
        public string ImageName { get; set; }

        [XmlAttribute]
        public string Url { get; set; }

        [XmlAttribute]
        public string ImageWidth { get; set; }

        [XmlAttribute]
        public string ImageHeight { get; set; }
    }

}
