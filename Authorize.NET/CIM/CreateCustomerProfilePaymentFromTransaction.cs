using AuthorizeNet.APICore;
//using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Util;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AuthorizeNet.CIM
{
    public class CreateCustomerProfilePaymentFromTransaction
    {

        private const int MaxResponseLength = 67108864;
        public ANetApiResponse CreateProfileFromTransaction(Environment env, createCustomerProfileFromTransactionRequest request)
        {
            ANetApiResponse response = null;
            if (null == request)
            {
                throw new ArgumentNullException("request");
            }
            //Logger.debug(string.Format("MerchantInfo->LoginId/TransactionKey: '{0}':'{1}'->{2}",
            //    request.merchantAuthentication.name, request.merchantAuthentication.ItemElementName, request.merchantAuthentication.Item));

            var postUrl = GetPostUrl(env);
            var webRequest = (HttpWebRequest)WebRequest.Create(postUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml";
            webRequest.KeepAlive = true;
            webRequest.Proxy = HttpUtility.SetProxyIfRequested(webRequest.Proxy);

            //set the http connection timeout 
            var httpConnectionTimeout = AuthorizeNet.Environment.getIntProperty(Constants.HttpConnectionTimeout);
            webRequest.Timeout = (httpConnectionTimeout != 0 ? httpConnectionTimeout : Constants.HttpConnectionDefaultTimeout);

            //set the time out to read/write from stream
            var httpReadWriteTimeout = AuthorizeNet.Environment.getIntProperty(Constants.HttpReadWriteTimeout);
            webRequest.ReadWriteTimeout = (httpReadWriteTimeout != 0 ? httpReadWriteTimeout : Constants.HttpReadWriteDefaultTimeout);

            var requestType = typeof(createCustomerProfileFromTransactionRequest);
            var serializer = new XmlSerializer(requestType);
            using (var writer = new XmlTextWriter(webRequest.GetRequestStream(), Encoding.UTF8))
            {
                serializer.Serialize(writer, request);
            }

            // Get the response
            String responseAsString = null;
            //Logger.debug(string.Format("Retreiving Response from Url: '{0}'", postUrl));

            // Set Tls to Tls1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var webResponse = webRequest.GetResponse())
            {
                //Logger.debug(string.Format("Received Response: '{0}'", webResponse));

                using (var responseStream = webResponse.GetResponseStream())
                {
                    if (null != responseStream)
                    {
                        var result = new StringBuilder();

                        using (var reader = new StreamReader(responseStream))
                        {
                            while (!reader.EndOfStream)
                            {
                                result.Append((char)reader.Read());

                                if (result.Length >= MaxResponseLength)
                                {
                                    throw new Exception("response is too long.");
                                }
                            }

                            responseAsString = result.Length > 0 ? result.ToString() : null;
                        }
                        //Logger.debug(string.Format("Response from Stream: '{0}'", responseAsString));
                    }
                }
            }
            if (null != responseAsString)
            {
                using (var memoryStreamForResponseAsString = new MemoryStream(Encoding.UTF8.GetBytes(responseAsString)))
                {
                    var responseType = typeof(createCustomerProfileResponse);
                    var deSerializer = new XmlSerializer(responseType);

                    Object deSerializedObject;
                    try
                    {
                        // try deserializing to the expected response type
                        deSerializedObject = deSerializer.Deserialize(memoryStreamForResponseAsString);
                    }
                    catch (Exception)
                    {
                        // probably a bad response, try if this is an error response
                        memoryStreamForResponseAsString.Seek(0, SeekOrigin.Begin); //start from beginning of stream
                        var genericDeserializer = new XmlSerializer(typeof(createCustomerProfileResponse));
                        deSerializedObject = genericDeserializer.Deserialize(memoryStreamForResponseAsString);
                    }

                    //if error response
                    if (deSerializedObject is Utility.ErrorResponse)
                    {
                        response = deSerializedObject as Utility.ErrorResponse;
                    }
                    else
                    {
                        //actual response of type expected
                        if (deSerializedObject is createCustomerProfileResponse)
                        {
                            response = deSerializedObject as createCustomerProfileResponse;
                        }
                        else if (deSerializedObject is ANetApiResponse) //generic response
                        {
                            response = deSerializedObject as ANetApiResponse;
                        }
                    }
                }
            }

            return response;

            //var httpApiResponse = AuthorizeNet.Util.HttpUtility.PostData<AuthorizeNet.Api.Controllers.Bases.ApiOperationBase.TQ, TS>(environment, GetApiRequest());
            //return _result;
        }

        private static Uri GetPostUrl(Environment env)
        {
            var postUrl = new Uri(env.getXmlBaseUrl() + "/xml/v1/request.api");
            //Logger.debug(string.Format("Creating PostRequest Url: '{0}'", postUrl));

            return postUrl;
        }
    }
}
