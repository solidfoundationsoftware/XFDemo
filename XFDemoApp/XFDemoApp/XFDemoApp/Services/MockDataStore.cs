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

        public async Task<IEnumerable<Listing>> GetItemsAsync(ListingSortOrder sortKey = ListingSortOrder.None)
        {
            return await Task.FromResult(OrderListings(items, sortKey));
        }

        public async Task<IEnumerable<Listing>> SearchItemsAsync(string searchTerm, ListingSortOrder sortKey)
        {
            IEnumerable<Listing> query = from item in items select item;

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(s => s.ListingTitle.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);

            return await Task.FromResult(OrderListings(query, sortKey));
        }

        private IEnumerable<Listing> OrderListings(IEnumerable<Listing> query, ListingSortOrder sortKey)
        {
            if (query == null) return query;

            switch (sortKey)
            {
                case ListingSortOrder.None:
                    break;
                case ListingSortOrder.TitleAscending:
                    query = query.OrderBy(p => p.ListingTitle);
                    break;
                case ListingSortOrder.TitleDescending:
                    query = query.OrderByDescending(p => p.ListingTitle);
                    break;
                case ListingSortOrder.PriceAscending:
                    query = query.OrderBy(p => p.ListingPrice);
                    break;
                case ListingSortOrder.PriceDescending:
                    query = query.OrderByDescending(p => p.ListingPrice);
                    break;
                default:
                    break;
            }

            return query;
        }
    }
}