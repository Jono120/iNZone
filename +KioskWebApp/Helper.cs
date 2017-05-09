using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace KioskApplication
{
    public class Helper : System.Web.UI.Page
    {
        private const int MAX_NO_OF_MOVIES = 5;

        /// <summary>
        /// Returns all valid premier partners in a random order
        /// </summary>
        /// <returns></returns>
        public List<Partner> GetListOfPremierPartners()
        {
            List<Partner> partners = GetListOfPartners();

            partners.RemoveAll(delegate(Partner partner) { return !partner.IsPremier; });

            return partners;
        }

        /// <summary>
        /// Returns all valid partners ordered by premier partners first, in a random order
        /// </summary>
        /// <returns></returns>
        public List<Partner> GetListOfPartners()
        {
            List<Partner> partners = new List<Partner>();

            KioskVideos kioskVideos =
                Helper.GetKioskVideos(
                    Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/CategoriesConfig.xml"));

            string videosFolder = "~/" + ConfigurationManager.AppSettings["VideosFolderName"];

            foreach (Category category in kioskVideos.Category)
            {
                foreach (Customer customer in category.Partner)
                {
                    //Only add partner to the list if all its video/logo files are there
                    if (CheckRequiredMasterFilesExist(customer.Name))
                    {
                        string partnerConfigFilePath =
                        Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/" + customer.Name +
                                       "/Config.xml");

                        Partner partner = new Partner(GetPartnerID(partnerConfigFilePath), videosFolder + "\\" + customer.Name + "\\Movie1\\Thumbnail1" + ConfigurationManager.AppSettings["MovieThumbnailsFileExtension"], GetIsPremierPartner(partnerConfigFilePath), category.Name, customer.Name, GetAbbreviatedPartnerName(partnerConfigFilePath));

                        partners.Add(partner);
                    }
                }
            }


            Random random = new Random();
            List<Partner> randomPartners = new List<Partner>();

            // Randomise the list
            while (randomPartners.Count < partners.Count)
            {
                int randomNum = random.Next(0, partners.Count);

                if (!randomPartners.Contains(partners[randomNum]))
                {
                    randomPartners.Add(partners[randomNum]);
                }
            }

            randomPartners = (from p in randomPartners orderby p.IsPremier descending select p).ToList();

            return randomPartners;
        }

        /// <summary>
        /// Returns partner ID given path to the Config.xml file
        /// </summary>
        /// <param name="partnerConfigFilePath"></param>
        /// <returns>Partner ID</returns>
        public static int GetPartnerID(string partnerConfigFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(partnerConfigFilePath);

            XmlNodeList partnerIDList = doc.GetElementsByTagName("PartnerID");

            if (partnerIDList.Count != 1)
                return 0;

            return Convert.ToInt32(partnerIDList[0].InnerXml);
        }

        /// <summary>
        /// Returns whether partner is premier given path to the Config.xml file
        /// </summary>
        /// <param name="partnerConfigFilePath"></param>
        /// <returns>whether partner is premier </returns>
        public static bool GetIsPremierPartner(string partnerConfigFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(partnerConfigFilePath);

            XmlNodeList partnerIDList = doc.GetElementsByTagName("PremierPartner");

            if (partnerIDList.Count != 1)
                return false;

            if (partnerIDList[0].InnerXml.ToLower() == "yes")
                return true;

            return false;
        }

        /// <summary>
        /// Returns abbreviated partner name given path to Config.xml file
        /// </summary>
        /// <param name="partnerConfigFilePath"></param>
        /// <returns>Abbreviated partner name</returns>
        public static string GetAbbreviatedPartnerName(string partnerConfigFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(partnerConfigFilePath);

            XmlNodeList partnerIDList = doc.GetElementsByTagName("AbbreviatedPartnerName");

            if (partnerIDList.Count != 1)
                return "";

            return partnerIDList[0].InnerXml;
        }

        /// <summary>
        /// Returns abbreviated partner name given path to Config.xml file
        /// </summary>
        /// <param name="partnerConfigFilePath"></param>
        /// <returns>Abbreviated partner name for use in subscription question</returns>
        public static string GetPartnerNameForQuestion(string partnerConfigFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(partnerConfigFilePath);

            XmlNodeList partnerIDList = doc.GetElementsByTagName("AbbreviatedPartnerNameForQuestion");

            if (partnerIDList.Count != 1)
                return "";

            return partnerIDList[0].InnerXml;
        }

        /// <summary>
        /// Returns abbreviated partner name given path to Config.xml file
        /// </summary>
        /// <param name="partnerConfigFilePath"></param>
        /// <returns>Abbreviated partner name for use in subscription question</returns>
        public static string GetPartnerNameForSubscribeYes(string partnerConfigFilePath)
        {
            string shortName = GetPartnerNameForQuestion(partnerConfigFilePath);

            if (shortName.Length >= 2 && shortName.Substring(0, 1) != "'")
            {
                return (shortName.Substring(0, 1).ToUpper() + shortName.Substring(1, shortName.Length - 1)).Replace('"','\"');
            }

            return shortName.Replace("'", "\'");
        }

        /// <summary>
        /// Returns abbreviated partner name given path to Config.xml file
        /// </summary>
        /// <param name="partnerConfigFilePath"></param>
        /// <returns>Abbreviated partner name for use in subscription question</returns>
        public static string GetPartnerNameForSubscribeNo(string partnerConfigFilePath)
        {
            string shortName = GetPartnerNameForQuestion(partnerConfigFilePath);

            if(shortName.ToLower().Contains("the "))
            {
                return (shortName.Substring(0, 1).ToLower() + shortName.Substring(1, shortName.Length - 1)).Replace('"', '\"');
            }

            return shortName.Replace("'", "\'");
        }

        /// <summary>
        /// Returns video display name
        /// </summary>
        /// <param name="partnerConfigFilePath"></param>
        /// <param name="videoID"></param>
        /// <returns></returns>
        public static string GetVideoName(string partnerConfigFilePath, int videoID)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(partnerConfigFilePath);

            XmlNodeList masterMovieList = doc.GetElementsByTagName("Movie" + videoID + "Name");

            if (masterMovieList.Count != 1)
                return "";

            return masterMovieList[0].InnerXml;
        }

        public static KioskVideos GetKioskVideos(string categoriesConfigFilePath)
        {
            if (File.Exists(categoriesConfigFilePath))
            {
                XmlReader reader = new XmlTextReader(categoriesConfigFilePath);

                XmlSerializer xs = new XmlSerializer(typeof(KioskVideos));
                return (KioskVideos)xs.Deserialize(reader);
            }

            return null;
        }

        public static JobSearchServiceProviders GetJobSearchServiceProviders(string jobSearchServiceProvidersFilePath)
        {
            if (File.Exists(jobSearchServiceProvidersFilePath))
            {
                XmlReader reader = new XmlTextReader(jobSearchServiceProvidersFilePath);

                XmlSerializer xs = new XmlSerializer(typeof(JobSearchServiceProviders));
                return (JobSearchServiceProviders)xs.Deserialize(reader);
            }

            return null;
        }

        /// <summary>
        /// Generates random indexes for randomising partner videos on landing page
        /// </summary>
        /// <param name="categoriesConfigFilePath"></param>
        /// <returns></returns>
        public static string GenerateRandomVideosOrder(string categoriesConfigFilePath)
        {
            string randomVideosOrder = "";

            if (File.Exists(categoriesConfigFilePath))
            {
                XmlReader reader = new XmlTextReader(categoriesConfigFilePath);

                XmlSerializer xs = new XmlSerializer(typeof(KioskVideos));
                KioskVideos kioskVideos = (KioskVideos)xs.Deserialize(reader);
               
                foreach(Category category in kioskVideos.Category)
                {
                    int numOfPartners = category.Partner.Count;

                    List<int> randomNumbers = new List<int>();

                    Random random = new Random();

                    while(randomNumbers.Count < numOfPartners)
                    {
                        int randomNum = random.Next(0, numOfPartners);
                        
                        //Make sure random numbers don't repeat
                        if (!randomNumbers.Contains(randomNum))
                        {
                            randomNumbers.Add(randomNum);
                            randomVideosOrder += randomNum + ",";
                        }
                    }

                    randomVideosOrder += "|";
                }
            }
            return randomVideosOrder.Replace(",|", "|");
        }

        public static String SerializeKioskVideos(Object obj)
        {
            try
            {

                String XmlizedString = null;

                MemoryStream memoryStream = new MemoryStream();

                XmlSerializer xs = new XmlSerializer(typeof(KioskVideos));

                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, obj);

                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;

                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

                return XmlizedString;

            }

            catch (Exception e)
            {

                System.Console.WriteLine(e);

                return null;

            }


        }

        private static String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();

            String constructedString = encoding.GetString(characters);

            return (constructedString);

        }

        /// <summary>
        /// Checks whether all the necessary files exist in the Master file movie folder of a partner.
        /// </summary>
        /// <param name="partnerFolderName"></param>
        /// <returns>False if file or directory don't exist, otherwise true</returns>
        public bool CheckRequiredMasterFilesExist(string partnerFolderName)
        {
            string videosPathAtServer = Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"]);
            string largeThumbFile = videosPathAtServer + "/" + partnerFolderName + "/Movie1/Thumbnail1" + ConfigurationManager.AppSettings["MovieThumbnailsFileExtension"];
            string masterMovieFile = videosPathAtServer + "/" + partnerFolderName + "/Movie1/Movie1.flv";
            string thumbMovieFile = videosPathAtServer + "/" + partnerFolderName + "/Movie1/ThumbMovie1.flv";
            string bigLogoFile = videosPathAtServer + "/" + partnerFolderName + "/BigLogo" + ConfigurationManager.AppSettings["LogoFileExtension"];
            string smallLogoFile = videosPathAtServer + "/" + partnerFolderName + "/SmallLogo" + ConfigurationManager.AppSettings["LogoFileExtension"];
            string configFile = videosPathAtServer + "/" + partnerFolderName + "/Config.xml";

            return (File.Exists(largeThumbFile) && File.Exists(masterMovieFile) && File.Exists(thumbMovieFile) && File.Exists(bigLogoFile) && File.Exists(smallLogoFile) && File.Exists(configFile));
        }

        /// <summary>
        /// Checks whether all the necessary files exist in a partner movie folder
        /// </summary>
        /// <param name="partnerFolderName"></param>
        /// <returns></returns>
        public bool CheckRequiredMovieFilesExist(string partnerFolderName, int movieNumber)
        {
            string videosPathAtServer = Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"]);
            string largeThumbFile = videosPathAtServer + "/" + partnerFolderName + "/Movie" + movieNumber + "/Thumbnail" + movieNumber + ConfigurationManager.AppSettings["MovieThumbnailsFileExtension"];
            string movieFile = videosPathAtServer + "/" + partnerFolderName + "/Movie" + movieNumber + "/Movie" + movieNumber + ".flv";

            return (File.Exists(largeThumbFile) && File.Exists(movieFile));
        }

        public List<int> GetValidPartnerMovies(string partnerFolderName)
        {
            List<int> validMovies = new List<int>();

            for (int i = 0; i < MAX_NO_OF_MOVIES; i++)
            {
                if (CheckRequiredMovieFilesExist(partnerFolderName, i + 1))
                {
                    validMovies.Add(i + 1);
                }
            }

            return validMovies;
        }
    }


    public class Partner
    {
        /// <summary>
        /// The ID of the partner
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The path to the master thumbnail
        /// </summary>
        public string Movie1ThumbnailPath { get; set; }

        /// <summary>
        /// Whether partner is a premier partner
        /// </summary>
        public bool IsPremier { get; set; }

        /// <summary>
        /// Category that partner belongs to
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Folder name of partner
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Abbreviated name of partner
        /// </summary>
        public string AbbreviatedName { get; set; }

        public Partner(int partnerID, string movie1ThumbnailPath, bool isPremierPartner, string category, string partnerName, string abbreviatedName)
        {
            ID = partnerID;
            Movie1ThumbnailPath = movie1ThumbnailPath;
            IsPremier = isPremierPartner;
            Category = category;
            Name = partnerName;
            AbbreviatedName = abbreviatedName;
        }
    }
}

