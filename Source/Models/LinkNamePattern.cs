using LinkUtilities.Helper;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace LinkUtilities.Models
{
    /// <summary>
    /// Pattern to find the right link name for a given URL and link title combination
    /// </summary>
    public class LinkNamePattern
    {
        /// <summary>
        /// Position used to sort the patterns
        /// </summary>
        [JsonProperty("position")]
        public int? Position { get; set; }

        /// <summary>
        /// Name to use for the link if the patterns match
        /// </summary>
        [JsonProperty("linkName")]
        public string LinkName { get; set; }

        /// <summary>
        /// Pattern the URL has to match. Can contain wildcards * (zero or more characters) or ? (exactly one character).
        /// </summary>
        [JsonProperty("urlPattern")]
        public string UrlPattern { get; set; }

        /// <summary>
        /// Pattern the link title has to match. Can contain wildcards * (zero or more characters) or ? (exactly one character).
        /// </summary>
        [JsonProperty("namePattern")]
        public string NamePattern { get; set; }

        /// <summary>
        /// If true only one of both patterns has to match. If false both have to match.
        /// </summary>
        [JsonProperty("partialMatch")]
        public bool PartialMatch { get; set; }

        /// <summary>
        /// Regular expression of the UrlPattern
        /// </summary>
        [JsonIgnore]
        public string UrlRegEx { get => ParseHelper.WildCardToRegular(UrlPattern); }

        /// <summary>
        /// Regular expression of the NamePattern
        /// </summary>
        [JsonIgnore]
        public string NameRegEx { get => ParseHelper.WildCardToRegular(NamePattern); }

        public bool LinkMatch(string linkName, string linkUrl)
        {
            if (PartialMatch)
            {
                return ((!string.IsNullOrWhiteSpace(NamePattern) && Regex.IsMatch(linkName, NameRegEx)) ||
                    (!string.IsNullOrWhiteSpace(UrlPattern) && Regex.IsMatch(linkUrl, UrlRegEx)));
            }
            else
            {
                return (string.IsNullOrWhiteSpace(NamePattern) || Regex.IsMatch(linkName, NameRegEx)) &&
                    (string.IsNullOrWhiteSpace(UrlPattern) || Regex.IsMatch(linkUrl, UrlRegEx));
            }
        }
    }
}