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

        private class GetWalletBalance
        {
            public Data data { get; set; }
            public bool error { get; set; }
            public object error_message { get; set; }
            public object error_code { get; set; }

            public class Data
            {
                public string address { get; set; }
                public string updated_at { get; set; }
                public string next_update_at { get; set; }
                public string quote_currency { get; set; }
                public int chain_id { get; set; }
                public Item[] items { get; set; }
                public object pagination { get; set; }
            }

            public class Item
            {
                public int contract_decimals { get; set; }
                public string contract_name { get; set; }
                public string contract_ticker_symbol { get; set; }
                public string contract_address { get; set; }
                public string[] supports_erc { get; set; }
                public string logo_url { get; set; }
                public DateTime? last_transferred_at { get; set; }
                public string type { get; set; }
                public string balance { get; set; }
                public string balance_24h { get; set; }
                public float quote_rate { get; set; }
                public float quote_rate_24h { get; set; }
                public float quote { get; set; }
                public float quote_24h { get; set; }
                public object nft_data { get; set; }
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

        public async Task<IEnumerable<Balance>> GetAddressBalance(string address, string chainId)
        {
            try
            {
                var response = await _client.GetAsync($"{chainId}/address/{address}/balances_v2/?quote-currency=USD&format=JSON&nft=false&no-nft-fetch=false&key={_apiKey}");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine(content);

                if (response.IsSuccessStatusCode)
                {
                    var deserialize = JsonSerializer.Deserialize<GetWalletBalance>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    var balances = new List<Balance>();

                    foreach (var item in deserialize.data.items)
                    {
                        var balance = new Balance
                        {
                            ContractAddress = item.contract_address,
                            ContractName = item.contract_name,
                            LogoUrl = item.logo_url,
                            Symbol = item.contract_ticker_symbol,
                            Type = item.type,
                            Value = decimal.Parse(item.balance) / (decimal)Math.Pow(10, item.contract_decimals),
                        };
                        balances.Add(balance);
                    }
                    return balances;
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
