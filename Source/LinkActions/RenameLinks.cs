﻿using LinkUtilities.Settings;
using Playnite.SDK;
using Playnite.SDK.Models;

namespace LinkUtilities.LinkActions
{
    /// <summary>
    /// Class to rename links based on patterns.
    /// </summary>
    public class RenameLinks : LinkAction
    {
        public override string ProgressMessage { get; } = "LOCLinkUtilitiesProgressRenameLinks";

        public override string ResultMessage { get; } = "LOCLinkUtilitiesDialogRenamedMessage";

        /// <summary>
        /// List of patterns to find the links to rename based on URL or link name
        /// </summary>
        public LinkNamePatterns RenamePatterns { get; set; }

        public RenameLinks(LinkUtilities plugin) : base(plugin)
        {
        }

        public override bool Execute(Game game, string actionModifier = "")
        {
            bool mustUpdate = false;

            if (game.Links != null && game.Links.Count > 0)
            {
                foreach (Link link in game.Links)
                {
                    string linkName = link.Name;

                    if (RenamePatterns.LinkMatch(ref linkName, link.Url))
                    {
                        if (linkName != link.Name)
                        {
                            API.Instance.MainView.UIDispatcher.Invoke(delegate
                            {
                                link.Name = linkName;
                            });

                            mustUpdate = true;
                        }
                    }
                }

                if (mustUpdate)
                {
                    API.Instance.Database.Games.Update(game);
                }
            }
            return mustUpdate;
        }
    }
}