using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ColoursAPI.Models;
using Microsoft.AspNetCore.Http;

namespace ColoursAPI.Services
{
    public class ColoursService
    {
        private List<ColoursItem> _listColors = new() {  };  // This will only work for a single instance of the service ... to be replaced by datastore

        private IConfiguration _config;

        public ColoursService(IConfiguration config)
        {
 
            _config = config;

            _ = Reset();

            return;
        }

        public async Task<List<ColoursItem>> GetAll()
        {
            await Task.Run(() => { });

            return _listColors;
        }


        public async Task<ColoursItem> GetById(int id)
        {
            ColoursItem _colourItem = _listColors.Find(x => x.Id == id);

            await Task.Run(() => { });

            return _colourItem;

        }

        public async Task<ColoursItem> GetByName(string pName)
        {
            await Task.Run(() => { });

            ColoursItem _colourItem = null;

            int idxName = _listColors.FindIndex(a => a.Name.ToLower() == pName.ToLower().Trim());
            if (idxName >= 0)
            {
                _colourItem = _listColors[idxName];
            }

            return _colourItem;

        }

        public async Task<ColoursItem> UpdateById(int id, ColoursItem coloursItemUpdate)
        {
            int idx = id;

            int idxName = _listColors.FindIndex(a => a.Name.ToLower() == coloursItemUpdate.Name.ToLower().Trim());
            if (idxName >= 0)
            {
                _listColors.RemoveAt(idxName);
            }

            if (idx > 0)
            {
                int idxId = _listColors.FindIndex(a => a.Id == idx);
                if (idxId >= 0)
                {
                    _listColors.RemoveAt(idxId);
                }
            }
            else
            {
                for (int i = 1; i <= 1000; i++)
                {
                    if (_listColors.Find(x => x.Id == i) == null)
                    {
                        idx = i;
                        break;
                    }
                }
            }

            coloursItemUpdate.Id = idx;
            coloursItemUpdate.Name = coloursItemUpdate.Name.ToLower().Trim();

            _listColors.Add(coloursItemUpdate);

            await Task.Run(() => { });

            return coloursItemUpdate;

        }

        public async Task<ColoursItem> DeleteById(int id)
        {
            int idxId = _listColors.FindIndex(a => a.Id == id);
            if (idxId >= 0)
            {
                _listColors.RemoveAt(idxId);
            }

            await Task.Run(() => { });

            return null;

        }

        public async Task<ColoursItem> DeleteAll()
        {
            _listColors.Clear();

            await Task.Run(() => { });

            return null;

        }

        public async Task<ColoursItem> Reset()
        {
            await DeleteAll();

            await UpdateById(1, new ColoursItem { Id = 1, Name = _config.GetValue<string>("Colour1"), Data = null }); 
            await UpdateById(2, new ColoursItem { Id = 2, Name = _config.GetValue<string>("Colour2"), Data = null });
            await UpdateById(3, new ColoursItem { Id = 2, Name = _config.GetValue<string>("Colour3"), Data = null });

            return null;

        }

        public string GetAppConfigInfo(HttpContext context)
        {
            string EchoData(string key, string value)
            {
                return key + ": <span class='echodata'>" + value + "</span><br/>";
            }

            string EchoDataBull(string key, string value)
            {
                return EchoData("&nbsp;&bull;&nbsp;" + key, value);
            }

            string strHtml = "";
            strHtml += "<html><head>";
            strHtml += "<style>";
            strHtml += "body { font-family: \"Segoe UI\",Roboto,\"Helvetica Neue\",Arial;}";
            strHtml += ".echodata { color: blue }";
            strHtml += "</style>";
            strHtml += "</head><body>";
            strHtml += "<h3>ColoursAPI - AppConfigInfo</h3>";

            strHtml += EchoData("OS Description", System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            strHtml += EchoData("Framework Description", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
            strHtml += EchoData("BuildIdentifier", _config.GetValue<string>("BuildIdentifier"));

            if (_config.GetValue<string>("AdminPW") == context.Request.Query["pw"].ToString())
            {
                strHtml += EchoData("ASPNETCORE_ENVIRONMENT", _config.GetValue<string>("ASPNETCORE_ENVIRONMENT"));
                strHtml += EchoData("ApplicationInsights ConnectionString", _config.GetValue<string>("ApplicationInsights:ConnectionString"));
                strHtml += EchoData("Default Colours", _config.GetValue<string>("Colour1") + " | " + _config.GetValue<string>("Colour2") + " | " + _config.GetValue<string>("Colour3"));             
            }

            strHtml += "RequestInfo: <br/>";
            strHtml += EchoDataBull("host", context.Request.Host.ToString());
            strHtml += EchoDataBull("ishttps", context.Request.IsHttps.ToString());
            strHtml += EchoDataBull("method", context.Request.Method.ToString());
            strHtml += EchoDataBull("path", context.Request.Path.ToString());
            strHtml += EchoDataBull("pathbase", context.Request.PathBase.ToString());
            strHtml += EchoDataBull("pathbase", context.Request.Protocol.ToString());
            strHtml += EchoDataBull("pathbase", context.Request.QueryString.ToString());
            strHtml += EchoDataBull("scheme", context.Request.Scheme.ToString());

            strHtml += "Headers: <br/>";
            foreach (var key in context.Request.Headers.Keys)
            {
                strHtml += EchoDataBull(key, $"{context.Request.Headers[key]}");
            }

            strHtml += "Connection:<br/>";
            strHtml += EchoDataBull("localipaddress", context.Connection.LocalIpAddress.ToString());
            strHtml += EchoDataBull("localport", context.Connection.LocalPort.ToString());
            strHtml += EchoDataBull("remoteipaddress", context.Connection.RemoteIpAddress.ToString());
            strHtml += EchoDataBull("remoteport", context.Connection.RemotePort.ToString());

            strHtml += "<hr/>";
            strHtml += "<a href='/'>Home</a>" + "<br/>";
            strHtml += "</body></html>";

            return strHtml;

        }
    }

}
