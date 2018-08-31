// ***********************************************************************
// Assembly         : Edu/.Droid
// Author           : PSI
// Created          : 06-24-2016
//
// Last Modified By : PSI
// Last Modified On : 08-06-2016
// ***********************************************************************
// <copyright file="WebServiceHandler.cs">
//     joe
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Text;
using Plugin.Connectivity;
using System.Threading.Tasks;
using LucidX.Constants;
using System.Security.Cryptography;
using LucidX.ResponseModels;
using System.Net.Http;

namespace LucidX.Webservices
{
    /// <summary>
    /// Class WebServiceHandler.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WebServiceHandler
    {
        /// <summary>
        /// The tag
        /// </summary>
        private static string TAG = "WebServiceHandler";


        /// <summary>
        /// Returns webservice result in Providing object
        /// </summary>
        /// <param name="Webservice_Method_Name">This is webservice method name</param>
        /// <param name="Method_Type">This is webservice method type like POST/GET</param>
        /// <param name="Request_Params">This is parameter which we send over server</param>
        /// <returns>Returns result of calling webservice in T type</returns>
		public async static Task<object> GetWebserviceResult(string Webservice_Method_Name,
		                                                     HttpMethod Method_Type, 
                                                             object Request_Params,
                                                             bool isXmlAlready= false)
        {
			object ParseResponse = null;
            try
            {
                var Response = default(HttpResponseMessage);

                HttpClient Client = new HttpClient();
                Client.MaxResponseContentBufferSize = 256000;

                TimeSpan timeoutSpan = TimeSpan.FromMilliseconds(120000);
                Client.Timeout = timeoutSpan;
                var Uri = new Uri(string.Format(WebserviceConstants.URL +
                                  Webservice_Method_Name, string.Empty));


                var isNetworkCheck = CrossConnectivity.Current.IsConnected;

                if (isNetworkCheck)
                {
                    if (Method_Type == HttpMethod.Get)
                    {
                        Response = await Client.GetAsync(Uri);
                    }
                    else if (Method_Type == HttpMethod.Post)
                    {
                        string requestXML = "";
                        if (isXmlAlready)
                        {
                            requestXML = Request_Params.ToString();
                        }
                        else {
                            requestXML = Utils.Utilities.ToXML(Request_Params);
                        }
						
                        var content = new StringContent(requestXML, Encoding.UTF8, "application/xml");
                        Response = await Client.PostAsync(Uri, content);
                    }
                    else if (Method_Type == HttpMethod.Delete)
                    {

                        Response = await Client.DeleteAsync(Uri);
                    }
                    var ResponseContent = await Response.Content.ReadAsStringAsync();
                    //Console.WriteLine("Response Of MethodName: " + Webservice_Method_Name +
                    //    " \n" + ResponseContent.ToString(), "WebServiceHandler | GetWebserviceResult");
                    if (Response.IsSuccessStatusCode)
                    {
                        // Deserialized Object by using Newtonsoft json api
						ParseResponse = Utils.Utilities.LoadFromXMLString(ResponseContent.ToString(), typeof(FinalResponse));

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return ParseResponse;
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

       
    }
}