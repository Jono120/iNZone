using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace KioskApplication
{
    /// <summary>
    /// Class used for deserialising the CategoriesConfig.xml file 
    /// 
    /// The xml structure is like this:
    ///      <?xml version="1.0" encoding="utf-8"?>
    ///  <KioskVideos xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    ///      <Category Name="Commercial" UnselectedTabImageName="tab_over.png" SelectedTabImageName="tab_over.png">
    ///           <Partner>
    ///               <Name>Partner1</Name>
    ///           </Partner>
    ///           <Partner>
    ///               <Name>Partner2</Name>
    ///           </Partner>
    ///           <Partner>
    ///              <Name>Partner3</Name>
    ///           </Partner>
    ///           <Partner>
    ///              <Name>Partner4</Name>
    ///           </Partner>
    ///      </Category>	
    ///  </KioskVideos>
    /// </summary>

    public class KioskVideos
    {
        [XmlElement(ElementName = "Category")]
        public List<Category> Category
        {
            get;
            set;
        }
    }

    public class Category
    {
        /// <summary>
        /// Name of the category
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Image under Images folder that displays when tab is selected
        /// </summary>
        [XmlAttribute]
        public string UnselectedTabImageName { get; set; }

        /// <summary>
        /// Image under Images folder that displays when tab is unselected
        /// </summary>
        [XmlAttribute]
        public string SelectedTabImageName { get; set; }

        [XmlElement(ElementName = "Partner")]
        public List<Customer> Partner { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
    }
}
