using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace LucidX.Utils
{
    public class Utilities
    {
        public const string CALENDAR_DATE_FORMAT = "yyyy-MM-dd";
        public const string RECEIVED_DATE_FORMAT_FROM_WEBSERVICE = "yyyyMMdd";

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        public static string ToXML(object prams)
        {
            StringWriter writer = new Utf8StringWriter();
            var serializer = new XmlSerializer(prams.GetType());
            serializer.Serialize(writer, prams);
            return writer.ToString();
        }

        public static object LoadFromXMLString(string xmlText, Type toType)
        {
            //var stringReader = new System.IO.StringReader(xmlText);
            //var serializer = new XmlSerializer(typeof(object));
            //return serializer.Deserialize(stringReader);
            XmlSerializer ser = new XmlSerializer(toType);

            using (StringReader sr = new StringReader(xmlText))

                return ser.Deserialize(sr);
        }




        public static string GetEncryptedToken(string stringToEncrypt)
        {
            TripleDESCryptoServiceProvider des;
            MD5CryptoServiceProvider hashmd5;
            byte[] bytPwdhash, bytBuff;
            string encrypted = "";
            string original = stringToEncrypt;
            hashmd5 = new MD5CryptoServiceProvider();
            bytPwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes("EncryptDecrypt"));
            hashmd5 = null;
            des = new TripleDESCryptoServiceProvider();
            des.Key = bytPwdhash;
            des.Mode = CipherMode.ECB;
            bytBuff = ASCIIEncoding.ASCII.GetBytes(original);
            encrypted = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(bytBuff, 0, bytBuff.Length));
            des = null;
            return encrypted;
        }

        public static string DateIntoWebserviceFormat(DateTime dateTime)
        {
            return dateTime.ToString(CALENDAR_DATE_FORMAT);
        }

        public static DateTime FormatStringIntoDateTime(string dateString)
        {
            try
            {
                return DateTime.ParseExact(dateString, RECEIVED_DATE_FORMAT_FROM_WEBSERVICE, CultureInfo.InvariantCulture);
            }catch(Exception ex)
            {
                return DateTime.Now;
            }
        }

        public static string ShowDateInFormat(string dateString)
        {
            return DateIntoWebserviceFormat(FormatStringIntoDateTime(dateString));
        }

        public static string ShowCurrentDateInFormat()
        {
            return DateIntoWebserviceFormat(DateTime.Now);
        }

        public static string GetDateForWebserviceTransDate(string dateString)
        {
            DateTime dateTime=  DateTime.ParseExact(dateString, CALENDAR_DATE_FORMAT, CultureInfo.InvariantCulture);
            return dateTime.ToString(RECEIVED_DATE_FORMAT_FROM_WEBSERVICE);
        }

    }
}
