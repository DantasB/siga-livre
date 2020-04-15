using System;
using System.Net;

namespace SigaClasses
{
    public class ImprovedWebClient : WebClient
    {

        CookieContainer cookies = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.AllowAutoRedirect = true;
            if (request is HttpWebRequest)
            {
                request.CookieContainer = cookies;

            }
            return request;
        }

    }
}
