using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization.Models
{
    public class FacebookUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id;
        [JsonProperty(PropertyName = "about")]
        public string About;
        [JsonProperty(PropertyName = "address")]
        public Location Address;
        [JsonProperty(PropertyName = "age_range")]
        public AgeRange AgeRange;
        [JsonProperty(PropertyName = "admin_notes")]
        public List<PageAdminNotes> AdminNotes;
        [JsonProperty(PropertyName = "birthday")]
        public string Birthday;
        [JsonProperty(PropertyName = "email")]
        public string Email;
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName;
        [JsonProperty(PropertyName = "hometown")]
        public dynamic Hometown;
        [JsonProperty(PropertyName = "install_type")]
        public string InstallType;
        [JsonProperty(PropertyName = "installed")]
        public bool? Installed;
        [JsonProperty(PropertyName = "is_famedeeplinkinguser")]
        public bool? IsFamedeeplinkinguser;
        [JsonProperty(PropertyName = "last_name")]
        public string LastName;
        [JsonProperty(PropertyName = "location")]
        public dynamic Location;
        [JsonProperty(PropertyName = "middle_name")]
        public string MiddleName;
        [JsonProperty(PropertyName = "name")]
        public string Name;
        [JsonProperty(PropertyName = "name_format")]
        public string NameFormat;
        [JsonProperty(PropertyName = "payment_pricepoints")]
        public PaymentPricepoints PaymentPricepoints;
        [JsonProperty(PropertyName = "security_settings")]
        public SecuritySettings SecuritySettings;
        [JsonProperty(PropertyName = "test_group")]
        public long? TestGroup;
        [JsonProperty(PropertyName = "video_upload_limits")]
        public VideoUploadLimits VideoUploadLimits;
        [JsonProperty(PropertyName = "viewer_can_send_gift")]
        public bool? ViewerCanSendGift;
        [JsonProperty(PropertyName = "can_review_measurement_request")]
        public bool? CanReviewMeasurementRequest;
        [JsonProperty(PropertyName = "context")]
        public UserContext Context;
        [JsonProperty(PropertyName = "employee_number")]
        public string EmployeeNumber;
        [JsonProperty(PropertyName = "favorite_athletes")]
        public List<Experience> FavoriteAthletes;
        [JsonProperty(PropertyName = "favorite_teams")]
        public List<Experience> FavoriteTeams;
        [JsonProperty(PropertyName = "gender")]
        public string Gender;
        [JsonProperty(PropertyName = "inspirational_people")]
        public List<Experience> InspirationalPeople;
        [JsonProperty(PropertyName = "is_shared_login")]
        public bool? IsSharedLogin;
        [JsonProperty(PropertyName = "labels")]
        public List<PageLabel> Labels;
        [JsonProperty(PropertyName = "languages")]
        public List<Experience> Languages;
        [JsonProperty(PropertyName = "link")]
        public string Link;
        [JsonProperty(PropertyName = "meeting_for")]
        public List<string> MeetingFor;
        [JsonProperty(PropertyName = "political")]
        public string Political;
        [JsonProperty(PropertyName = "profile_pic")]
        public string ProfilePic;
        [JsonProperty(PropertyName = "public_key")]
        public string PublicKey;
        [JsonProperty(PropertyName = "quotes")]
        public string Quotes;
        [JsonProperty(PropertyName = "relationship_status")]
        public string RelationshipStatus;
        [JsonProperty(PropertyName = "religion")]
        public string Religion;
        [JsonProperty(PropertyName = "shared_login_upgrade_required_by")]
        public DateTime SharedLoginUpgradeRequiredBy;
        [JsonProperty(PropertyName = "short_name")]
        public string ShortName;
        [JsonProperty(PropertyName = "significant_other")]
        public FacebookUser SignificantOther;
        [JsonProperty(PropertyName = "sports")]
        public List<Experience> Sports;
        [JsonProperty(PropertyName = "token_for_business")]
        public string TokenForBusiness;
    }


}
