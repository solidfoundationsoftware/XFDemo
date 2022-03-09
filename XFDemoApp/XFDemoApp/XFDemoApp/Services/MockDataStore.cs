using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XFDemoApp.Helpers;
using XFDemoApp.Models;

namespace XFDemoApp.Services
{
    public class MockDataStore : IDataStore<Listing>
    {
        readonly List<Listing> items;

        public MockDataStore()
        {
            items = new List<Listing>(JsonConvert.DeserializeObject<IEnumerable<Listing>>(EmbeddedResourceHelper.LoadResource("XFDemoApp.Resources.SampleInventory.json")));
        }

        public async Task<bool> AddItemAsync(Listing item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Listing item)
        {
            var oldItem = items.Where((Listing arg) => arg.PMListingId == item.PMListingId).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Listing arg) => arg.PMListingId == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Listing> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.PMListingId == id));
        }

        public async Task<IEnumerable<Listing>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items.Take(20));
        }
    }
}