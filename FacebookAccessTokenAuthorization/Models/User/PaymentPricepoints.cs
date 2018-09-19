using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class PaymentPricepoints
    {
        [JsonProperty(PropertyName="mobile")]
        public List<Mobile> Mobile;
    }
}
