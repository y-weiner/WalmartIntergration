

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WalmartIntergration.Models
{
    public class WalmartItem
    {
        [JsonProperty("mart")]
        public string Mart { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("wpid")] 
        public string Wpid { get; set; }

        [JsonProperty("upc")]
        public string Upc { get; set; }

        [JsonProperty("gtin")]
        public string Gtin { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("shelf")]
        public string Shelf { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("publishedStatus")]
        public string PublishedStatus { get; set; }

        [JsonProperty("unpublishedReasons")]
        public Reasons UnpublishedReasons { get; set; }

        [JsonProperty("lifeCycleStatus")]
        public string LifeCycleStatus { get; set; }

        public string[] GetShelfs()
        {
            string[] values = new string[0];
            try
            {
                if (!string.IsNullOrEmpty(this.Shelf))
                {
                    values = JArray.Parse(this.Shelf).ToObject<string[]>();
                }
            } catch (Exception e){}
            return values;
        }
    }
    
    public class Price
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        public override string ToString()
        {
            return $"{Currency} ${Amount}";
        }
    }

    public class Reasons
    {
        [JsonProperty("reason")]
        public string[] Reason { get; set; }
    }
}
