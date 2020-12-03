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
        private List<ColoursItem> _listColors;  // This will only work for a single instance of the service ... to be replaced by datastore

        public ColoursService(IConfiguration config)
        {

            _listColors = new List<ColoursItem> {
                new ColoursItem {Id = 1, Name = "red" },
                new ColoursItem {Id = 2, Name = "yellow" },
                new ColoursItem {Id = 3, Name = "black" }
            };

            return;
        }

        public async Task<IEnumerable<ColoursItem>> GetAll()
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

        public async Task<IEnumerable<ColoursItem>> GetRandom()
        {
            await Task.Run(() => { });

            List<ColoursItem> _listRandomColors = new List<ColoursItem> {
                new ColoursItem {Id = 1, Name = "blue" },
                new ColoursItem {Id = 2, Name = "darkblue" },
                new ColoursItem {Id = 3, Name = "lightblue" }
            };

            return _listRandomColors;
        }

    }
}
