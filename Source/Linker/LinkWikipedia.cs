﻿using LinkUtilities.Helper;
using LinkUtilities.Models;
using LinkUtilities.Models.MediaWiki;
using Playnite.SDK;
using Playnite.SDK.Models;
using System.Collections.Generic;
using System.Net;

namespace LinkUtilities.Linker
{
    /// <summary>
    /// Adds a link to Wikipedia.
    /// </summary>
    class LinkWikipedia : Link
    {
        public override string LinkName { get; } = "Wikipedia";
        public override string BaseUrl { get; } = "https://en.wikipedia.org/wiki/{0}";
        public override string SearchUrl { get; } = "https://en.wikipedia.org/w/api.php?action=opensearch&format=xml&search={0}&limit=50";

        public override bool AddLink(Game game)
        {
            if (!LinkHelper.LinkExists(game, LinkName))
            {
                // PCGamingWiki Links need the game simply encoded.
                string gameName = System.Uri.EscapeDataString(game.Name);

                LinkUrl = string.Format(BaseUrl, gameName);

                if (LinkHelper.CheckUrl(LinkUrl))
                {
                    return LinkHelper.AddLink(game, LinkName, LinkUrl, Settings);
                }
                else
                {
                    LinkUrl = string.Empty;

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override List<GenericItemOption> SearchLink(string searchTerm)
        {
            SearchResults.Clear();

            searchTerm = WebUtility.UrlEncode(searchTerm);

            WebClient client = new WebClient();

            client.Headers.Add("Accept", "application/xml");

            string xml = client.DownloadString(string.Format(SearchUrl, searchTerm));

            SearchSuggestion searchResults = xml.ParseXML<SearchSuggestion>();

            int counter = 0;

            foreach (SearchSuggestionItem item in searchResults.Section)
            {
                counter++;

                SearchResults.Add(new SearchResult
                {
                    Name = counter.ToString() + ". " + item.Text.Value,
                    Url = item.Url.Value,
                    Description = ""
                }
                );
            }

            return base.SearchLink(searchTerm);
        }

        public LinkWikipedia(LinkUtilitiesSettings settings) : base(settings)
        {
        }
    }
}