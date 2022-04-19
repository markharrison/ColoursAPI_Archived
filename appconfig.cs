using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ColoursAPI
{
    public class AppConfig
    {
        private string _AdminPWVal;
        private IConfiguration _config;

        public AppConfig(IConfiguration config)
        {
            _AdminPWVal = config.GetValue<string>("AdminPW");
            _config = config;

        }
 
        public string AdminPW
        {
            get => this._AdminPWVal;
            set => this._AdminPWVal = value;
        }
        public string GetAppConfigInfo()
        {
            string strAppConfigInfoHtml = "";
            strAppConfigInfoHtml += "<html><head>";
            strAppConfigInfoHtml += "<style>";
            strAppConfigInfoHtml += "body { font-family: \"Segoe UI\",Roboto,\"Helvetica Neue\",Arial;}";
            strAppConfigInfoHtml += "</style>";
            strAppConfigInfoHtml += "</head><body>";
            strAppConfigInfoHtml += "<h3>ColoursAPI - AppConfigInfo </h3>";        
            strAppConfigInfoHtml += "OS Description: " + System.Runtime.InteropServices.RuntimeInformation.OSDescription + "<br/>";
            strAppConfigInfoHtml += "Framework Description: " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription + "<br/>";
            strAppConfigInfoHtml += "ASPNETCORE_ENVIRONMENT: " + _config.GetValue<string>("ASPNETCORE_ENVIRONMENT") + "<br/>";
            strAppConfigInfoHtml += "InstrumentationKey: " + _config.GetValue<string>("ApplicationInsights:InstrumentationKey") + "<br/>";
            strAppConfigInfoHtml += "BuildIdentifier: " + _config.GetValue<string>("BuildIdentifier") + "<br/><br/>";
            strAppConfigInfoHtml += "<a href='/'>Home</a>" + "<br/>";
            strAppConfigInfoHtml += "<hr></body></html>";

            return strAppConfigInfoHtml;
        }
    }

}
