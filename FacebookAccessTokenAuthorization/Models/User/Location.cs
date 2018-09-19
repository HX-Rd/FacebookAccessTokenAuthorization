using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class Location
    {
        [JsonProperty(PropertyName = "city")]
        public string City;
        [JsonProperty(PropertyName = "city_id")]
        public long? CityId;
        [JsonProperty(PropertyName = "country")]
        public string Country;
        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode;
        [JsonProperty(PropertyName = "latitude")]
        public string Latitude;
        [JsonProperty(PropertyName = "located_in")]
        public string LocatedIn;
        [JsonProperty(PropertyName = "longitude")]
        public long? Longitude;
        [JsonProperty(PropertyName = "name")]
        public string Name;
        [JsonProperty(PropertyName = "region")]
        public string Region;
        [JsonProperty(PropertyName = "region_id")]
        public long? RegionId;
        [JsonProperty(PropertyName = "state")]
        public string State;
        [JsonProperty(PropertyName = "street")]
        public string Street;
        [JsonProperty(PropertyName = "zip")]
        public string Zip;
    }
}
