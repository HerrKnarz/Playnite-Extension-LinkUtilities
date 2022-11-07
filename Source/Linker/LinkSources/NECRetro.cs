﻿using LinkUtilities.Helper;
using LinkUtilities.Models;
using LinkUtilities.Models.MediaWiki;
using Playnite.SDK;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LinkUtilities.Linker
{
    /// <summary>
    /// Adds a link to NEC Retro.
    /// </summary>
    class LinkNECRetro : Link
    {
        public override string LinkName { get; } = "NEC Retro";
        public override string BaseUrl { get; } = "https://necretro.org/";
        public override string SearchUrl { get; } = "https://necretro.org/api.php?action=opensearch&format=xml&search={0}&limit=50";

        public override string GetGamePath(Game game)
        {
            // NEC Retro Links need the game with underscores instead of whitespaces and special characters simply encoded.
            return game.Name.CollapseWhitespaces().Replace(" ", "_").EscapeDataString();
        }

        public override List<GenericItemOption> SearchLink(string searchTerm)
        {
            SearchResults.Clear();

            try
            {
                WebClient client = new WebClient();

                client.Headers.Add("Accept", "application/xml");

                string xml = client.DownloadString(string.Format(SearchUrl, searchTerm.UrlEncode()));

                SearchSuggestion searchResults = xml.ParseXML<SearchSuggestion>();

                int counter = 0;

                foreach (SearchSuggestionItem item in searchResults.Section)
                {
                    // NEC Retro returns subbages to games in the results, so we simply count the shashes to filter them out.
                    if (item.Url.Value.Count(x => x == '/') < 4)
                    {
                        counter++;

                        SearchResults.Add(new SearchResult
                        {
                            Name = $"{counter}. {item.Text.Value}",
                            Url = item.Url.Value,
                            Description = ""
                        }

                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error loading data from {LinkName}");
            }

            return base.SearchLink(searchTerm);
        }

        public LinkNECRetro(LinkUtilities plugin) : base(plugin)
        {
        }
    }
}
