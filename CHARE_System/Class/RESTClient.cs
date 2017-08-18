using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Net.Http.Headers;

namespace CHARE_System.Class
{
    class RESTClient
    {
        public HttpClient client;
        public RESTClient()
        {
            Assembly assembly = this.GetType().Assembly;
            ResourceManager resourceManager = new ResourceManager("Resources.Strings", assembly);
            string apiAddress = resourceManager.GetString("RestAPIBaseAddress");

            client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}