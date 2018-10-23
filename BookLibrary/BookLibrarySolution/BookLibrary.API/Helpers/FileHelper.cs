using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace BookLibrary.API.Helpers
{
    public class FileHelper
    {
        public FileHelper() { }

        /* ****************************************************************************** */
        // CONVERSION
        /* ****************************************************************************** */
        #region Convert a JSON string into an Object of type T
        /// <summary>
        /// Convert a JSON string into an Object of type T
        /// </summary>
        /// <typeparam name="T">The type of object (ex: MyCustomClass)</typeparam>
        /// <param name="json">The JSON data in string format</param>
        /// <returns>An instance of the T object</returns>
        public T JSON_String_To_Object<T>(String json)
        {
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        #endregion

        #region Converts a JSON file to an Object of type T
        /// <summary>
        /// Converts a JSON file to an Object of type T
        /// </summary>
        /// <typeparam name="T">The type of object (ex: MyCustomClass)</typeparam>
        /// <param name="fName">File name to read from</param>
        /// <returns>An instance of the T object</returns>
        public T JSON_File_To_Object<T>(String fName)
        {
            String json = new StreamReader(fName, Encoding.UTF8).ReadToEnd();
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        #endregion

        #region Convert JSON string to XML document
        /// <summary>
        /// Convert JSON string to XML document
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>An XML document with data from the JSON string</returns>
        public XmlDocument JSON_String_To_XML_Document(String json)
        {
            return (XmlDocument)JsonConvert.DeserializeXmlNode(json);
        }
        #endregion

        #region Convert an XML string to an Object of type T
        /// <summary>
        /// Convert an XML string to an Object of type T
        /// </summary>
        /// <typeparam name="T">The type of object (ex: MyCustomClass)</typeparam>
        /// <param name="xml">XML data in string format</param>
        /// <returns>An instance of the T object</returns>
        public T XML_String_To_Object<T>(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            String json = JsonConvert.SerializeXmlNode(doc);
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        #endregion

        #region Converts an XML file to an Object of type T
        /// <summary>
        /// Converts an XML file to an Object of type T
        /// </summary>
        /// <typeparam name="T">The type of object (ex: MyCustomClass)</typeparam>
        /// <param name="fName">File name to read from</param>
        /// <returns>An instance of the T object</returns>
        public T XML_File_To_Object<T>(String fName)
        {
            String json = XML_File_To_JSON_String(fName);
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        #endregion

        /* ****************************************************************************** */
        // DESERIALIZATION
        /* ****************************************************************************** */
        #region Deserialize a JSON data string into an ExpandoObject
        /// <summary>
        /// Deserialize a JSON data string into an ExpandoObject
        /// </summary>
        /// <param name="json">JSON data string/param>
        /// <returns>ExpandoObject instance</returns>
        public ExpandoObject Deserialize_From_JSON_String(String json)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json);
            return obj;
        }
        #endregion

        #region Deserialize a JSON file into an ExpandoObject
        /// <summary>
        /// Deserialize a JSON file into an ExpandoObject
        /// </summary>
        /// <param name="fName">File name to read from</param>
        /// <returns>ExpandoObject instance</returns>
        public ExpandoObject Deserialize_From_JSON_File(String fName)
        {
            String json = new StreamReader(fName, Encoding.UTF8).ReadToEnd();
            return Deserialize_From_JSON_String(json);
        }
        #endregion

        #region Deserialize an XML file into an ExpandoObject
        /// <summary>
        /// Deserialize an XML file into an ExpandoObject
        /// </summary>
        /// <param name="fName">File name to read from</param>
        /// <returns>ExpandoObject instance</returns>
        public ExpandoObject Deserialize_From_XML_File(String fname)
        {
            String json = XML_File_To_JSON_String(fname);
            return Deserialize_From_JSON_String(json);
        }
        #endregion

        /* ****************************************************************************** */
        // DOWNLOADS
        /* ****************************************************************************** */
        #region Download JSON data from URL into an ExpandoObject
        /// <summary>
        /// Download JSON data from URL into an ExpandoObject
        /// </summary>
        /// <param name="url">URL to download from</param>
        /// <returns>ExpandoObject with JSON data retrieved from given URL</returns>
        public ExpandoObject JSON_URL_To_Expando_Object(String url)
        {
            String json = JSON_URL_To_JSON_String(url);
            return Deserialize_From_JSON_String(json);
        }
        #endregion

        #region Download XML data from URL into an ExpandoObject
        /// <summary>
        /// Download XML data from URL into an ExpandoObject
        /// </summary>
        /// <param name="url">URL to download from</param>
        /// <returns>ExpandoObject with XML data retrieved from given URL</returns>
        public ExpandoObject XML_URL_To_Expando_Object(String url)
        {
            String json = XML_URL_To_JSON_String(url);
            return Deserialize_From_JSON_String(json);
        }
        #endregion

        #region Downloads a JSON url into a JSON string
        /// <summary>
        /// Downloads a JSON url into a JSON string
        /// </summary>
        /// <param name="url">The URL to download from</param>
        /// <returns>JSON string data retrieved from the given URL</returns>
        public String JSON_URL_To_JSON_String(String url)
        {
            using (WebClient wc = new WebClient())
            {
                return wc.DownloadString(url);
            }
        }
        #endregion

        /* ****************************************************************************** */
        // SERIALIZATION
        /* ****************************************************************************** */
        #region Serializes an object into a JSON string
        /// <summary>
        /// Serializes an object into a JSON string
        /// </summary>
        /// <typeparam name="T">The type of object (ex: MyCustomClass)</typeparam>
        /// <param name="obj">The object to serialize</param>
        /// <returns>JSON string representation of the given object</returns>
        public String Object_To_JSON_String<T>(T obj)
        {
            String res = JsonConvert.SerializeObject(obj);
            return res;
        }
        #endregion

        #region Serializes XML string into JSON string
        /// <summary>
        /// Serializes XML string into JSON string
        /// </summary>
        /// <param name="xml">The XML string representation</param>
        /// <returns>JSON data retrieved from the given XML string</returns>
        public String XML_String_To_JSON_String(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return JsonConvert.SerializeXmlNode(doc);
        }
        #endregion

        #region Serializes an XML file into a JSON string
        /// <summary>
        /// Serializes an XML file into a JSON string
        /// </summary>
        /// <param name="fName">The XML file name</param>
        /// <returns>JSON string representation of the given XML document</returns>
        public String XML_File_To_JSON_String(String fName)
        {
            String xml = new StreamReader(fName, Encoding.UTF8).ReadToEnd();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return JsonConvert.SerializeXmlNode(doc);
        }
        #endregion

        #region Serializes an XML document into a JSON string
        /// <summary>
        /// Serializes an XML document into a JSON string
        /// </summary>
        /// <param name="doc">XML document to serialize</param>
        /// <returns>JSON string representation of the given XML document</returns>
        public String XML_Document_To_JSON_String(XmlDocument doc)
        {
            return JsonConvert.SerializeXmlNode(doc);
        }
        #endregion

        #region Serializes XML from given URL into JSON string
        /// <summary>
        /// Serializes XML from given URL into JSON string
        /// </summary>
        /// <param name="url">The URL to download from</param>
        /// <returns>JSON data retrieved from the given XML source</returns>
        public String XML_URL_To_JSON_String(String url)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(url);
            return XML_Document_To_JSON_String(doc);
        }
        #endregion
    }
}
