using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public class CovalentService : ICovalentService
    {
        private readonly HttpClient _client;

        private readonly string _apiKey = "ckey_b9362aff21fe42a7a6d6f6b4f94";

        public CovalentService()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.covalenthq.com/v1/"),
            };
        }

        private class GetAllChainResponse
        {
            public Data data { get; set; }
            public bool error { get; set; }
            public object error_message { get; set; }
            public object error_code { get; set; }

            public class Data
            {
                public string updated_at { get; set; }
                public Item[] items { get; set; }
                public object pagination { get; set; }
            }

            public class Item
            {
                public string name { get; set; }
                public string chain_id { get; set; }
                public bool is_testnet { get; set; }
                public string db_schema_name { get; set; }
                public string label { get; set; }
                public string logo_url { get; set; }
            }
        }

        public async Task<IEnumerable<Chain>> GetAllChain()
        {
            try
            {
                var response = await _client.GetAsync($"chains/?key={_apiKey}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var deserialize = JsonSerializer.Deserialize<GetAllChainResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var chains = new List<Chain>();

                    foreach (var item in deserialize.data.items)
                    {
                        var chain = new Chain
                        {
                            CovalentId = item.chain_id,
                            IsTestnet = item.is_testnet,
                            Lable = item.label,
                            LogoUrl = item.logo_url,
                            Name = item.name
                        };
                        chains.Add(chain);
                    }

                    return chains;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}
