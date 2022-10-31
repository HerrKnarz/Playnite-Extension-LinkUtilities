﻿using HtmlAgilityPack;
using LinkUtilities.Helper;
using LinkUtilities.Models;
using Playnite.SDK;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkUtilities.Linker
{
    /// <summary>
    /// Adds a link to MobyGames.
    /// </summary>
    class LinkHG101 : Link
    {
        public override string LinkName { get; } = "Hardcore Gaming 101";
        public override string BaseUrl { get; } = "http://www.hardcoregaming101.net/{0}";
        public override string SearchUrl { get; } = "http://www.hardcoregaming101.net/?s={0}";

        public override bool AddLink(Game game)
        {
            if (!LinkHelper.LinkExists(game, LinkName))
            {
                // MobyGames Links need the game name in lowercase without special characters and hyphens instead of white spaces.
                string gameName = game.Name.RemoveSpecialChars().CollapseWhitespaces().Replace(" ", "-").ToLower();

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

            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(string.Format(SearchUrl, searchTerm.UrlEncode()));

                HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes("//header[@class='entry-header']");

                if (htmlNodes != null && htmlNodes.Count > 0)
                {
                    foreach (HtmlNode node in htmlNodes)
                    {
                        HtmlNodeCollection reviewNodes = node.SelectNodes("./div[@class='index-entry-meta']/div[a='Review']");

                        if (reviewNodes != null && reviewNodes.Count > 0)
                        {
                            SearchResults.Add(new SearchResult
                            {
                                Name = node.SelectSingleNode("./h2/a").InnerHtml,
                                Url = node.SelectSingleNode("./h2/a").GetAttributeValue("href", ""),
                                Description = node.SelectNodes("./div[@class='index-entry-meta']/div/a").Select(tagNode => tagNode.InnerText).Aggregate((total, part) => total + ", " + part)
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error loading data from {LinkName}");
            }

            return base.SearchLink(searchTerm);
        }

        public LinkHG101(LinkUtilitiesSettings settings) : base(settings)
        {
        }
    }
}