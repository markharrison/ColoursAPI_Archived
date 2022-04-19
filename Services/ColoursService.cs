using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ColoursAPI.Models;

namespace ColoursAPI.Services
{
    public class ColoursService
    {
        private List<ColoursItem> _listColors = new() {  };  // This will only work for a single instance of the service ... to be replaced by datastore

        private IConfiguration _config;

        public ColoursService(IConfiguration config)
        {
            _ = Reset();

            _config = config;

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

            await UpdateById(1, new ColoursItem { Id = 1, Name = "blue", Data = null }); ;
            await UpdateById(2, new ColoursItem { Id = 2, Name = "darkblue", Data = null });
            await UpdateById(3, new ColoursItem { Id = 2, Name = "lightblue", Data = null });


            return null;

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
