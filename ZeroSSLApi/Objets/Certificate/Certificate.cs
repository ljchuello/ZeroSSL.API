using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZeroSSLApi.Objets.Certificate
{
    public class Certificate
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("common_name", NullValueHandling = NullValueHandling.Ignore)]
        public string CommonName { get; set; } = string.Empty;

        [JsonProperty("additional_domains", NullValueHandling = NullValueHandling.Ignore)]
        public string AdditionalDomains { get; set; } = string.Empty;

        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public string Created { get; set; } = string.Empty;

        [JsonProperty("expires", NullValueHandling = NullValueHandling.Ignore)]
        public string Expires { get; set; } = string.Empty;

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("validation_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ValidationType { get; set; } = string.Empty;

        [JsonProperty("validation_emails", NullValueHandling = NullValueHandling.Ignore)]
        public string ValidationEmails { get; set; } = string.Empty;

        [JsonProperty("replacement_for", NullValueHandling = NullValueHandling.Ignore)]
        public string ReplacementFor { get; set; } = string.Empty;

        [JsonProperty("fingerprint_sha1", NullValueHandling = NullValueHandling.Ignore)]
        public string FingerprintSha1 { get; set; } = string.Empty;

        //[JsonProperty("brand_validation", NullValueHandling = NullValueHandling.Ignore)]
        //public object BrandValidation { get; set; }

        [JsonProperty("validation", NullValueHandling = NullValueHandling.Ignore)]
        public Validation Validation { get; set; } = new Validation();
    }

    public class Validation
    {
        [JsonProperty("other_methods", NullValueHandling = NullValueHandling.Ignore)]
        public OtherMethods OtherMethods { get; set; } = new OtherMethods();
    }

    public class OtherMethods
    {
        [JsonProperty("domainDotCom", NullValueHandling = NullValueHandling.Ignore)]
        public DomainDotCom DomainDotCom { get; set; } = new DomainDotCom();
    }

    public class DomainDotCom
    {
        [JsonProperty("file_validation_url_http", NullValueHandling = NullValueHandling.Ignore)]
        public string FileValidationUrlHttp { get; set; } = string.Empty;

        [JsonProperty("file_validation_url_https", NullValueHandling = NullValueHandling.Ignore)]
        public string FileValidationUrlHttps { get; set; } = string.Empty;

        [JsonProperty("file_validation_content", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> FileValidationContent { get; set; } = new List<string>();

        [JsonProperty("cname_validation_p1", NullValueHandling = NullValueHandling.Ignore)]
        public string CnameValidationP1 { get; set; } = string.Empty;

        [JsonProperty("cname_validation_p2", NullValueHandling = NullValueHandling.Ignore)]
        public string CnameValidationP2 { get; set; } = string.Empty;
    }
}
