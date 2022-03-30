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

        private class GetWalletBalanceResponse
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
                public float? quote_rate { get; set; }
                public float? quote_rate_24h { get; set; }
                public float quote { get; set; }
                public float? quote_24h { get; set; }
                public object nft_data { get; set; }
            }
        }

        private class GetPriceResponse
        {
            public Data data { get; set; }
            public bool error { get; set; }
            public object error_message { get; set; }
            public object error_code { get; set; }

            public class Data
            {
                public string updated_at { get; set; }
                public Item[] items { get; set; }
                public Pagination pagination { get; set; }
            }

            public class Pagination
            {
                public bool has_more { get; set; }
                public int page_number { get; set; }
                public int page_size { get; set; }
                public int total_count { get; set; }
            }

            public class Item
            {
                public int contract_decimals { get; set; }
                public string contract_name { get; set; }
                public string contract_ticker_symbol { get; set; }
                public string contract_address { get; set; }
                public object supports_erc { get; set; }
                public string logo_url { get; set; }
                public decimal quote_rate { get; set; }
                public int rank { get; set; }
            }

        }

        private class GetTransactionResponse
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
                public Pagination pagination { get; set; }
            }

            public class Pagination
            {
                public bool has_more { get; set; }
                public int page_number { get; set; }
                public int page_size { get; set; }
                public object total_count { get; set; }
            }

            public class Item
            {
                public DateTime block_signed_at { get; set; }
                public int block_height { get; set; }
                public string tx_hash { get; set; }
                public int tx_offset { get; set; }
                public bool successful { get; set; }
                public string from_address { get; set; }
                public object from_address_label { get; set; }
                public string to_address { get; set; }
                public string to_address_label { get; set; }
                public string value { get; set; }
                public float value_quote { get; set; }
                public int gas_offered { get; set; }
                public int gas_spent { get; set; }
                public long gas_price { get; set; }
                public float gas_quote { get; set; }
                public float gas_quote_rate { get; set; }
                public Log_Events[] log_events { get; set; }
            }

            public class Log_Events
            {
                public DateTime block_signed_at { get; set; }
                public int block_height { get; set; }
                public int tx_offset { get; set; }
                public int log_offset { get; set; }
                public string tx_hash { get; set; }
                public string[] raw_log_topics { get; set; }
                public int sender_contract_decimals { get; set; }
                public string sender_name { get; set; }
                public string sender_contract_ticker_symbol { get; set; }
                public string sender_address { get; set; }
                public string sender_address_label { get; set; }
                public string sender_logo_url { get; set; }
                public string raw_log_data { get; set; }
                public Decoded decoded { get; set; }
            }

            public class Decoded
            {
                public string name { get; set; }
                public string signature { get; set; }
                public Param[] _params { get; set; }
            }

            public class Param
            {
                public string name { get; set; }
                public string type { get; set; }
                public bool indexed { get; set; }
                public bool decoded { get; set; }
                public string value { get; set; }
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

                if (response.IsSuccessStatusCode)
                {
                    var deserialize = JsonSerializer.Deserialize<GetWalletBalanceResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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

        public async Task<decimal> GetPrice(string symbol)
        {
            try
            {
                var response = await _client.GetAsync($"pricing/tickers/?quote-currency=USD&format=JSON&tickers={symbol}&page-number=&key={_apiKey}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var deserialize = JsonSerializer.Deserialize<GetPriceResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (deserialize.data.items.Any())
                    {
                        return deserialize.data.items.OrderBy(u => u.rank).FirstOrDefault().quote_rate;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }

        public async Task<DateTime?> GetWalletFirstActivity(string address, string chainId)
        {
            try
            {
                var response = await _client.GetAsync($"{chainId}/address/{address}/transactions_v2/?quote-currency=USD&format=JSON&block-signed-at-asc=true&no-logs=false&key={_apiKey}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var deserialize = JsonSerializer.Deserialize<GetTransactionResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    var firstDate = deserialize.data.items.FirstOrDefault().block_signed_at;
                    return firstDate;
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
