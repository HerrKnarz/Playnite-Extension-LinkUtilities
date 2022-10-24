﻿using LinkUtilities.Linker;
using Playnite.SDK.Models;

namespace LinkUtilities.LinkActions
{
    /// <summary>
    /// Adds a link to the game store page of the library (e.g. steam or gog) the game is part of.
    /// </summary>
    public class AddLibraryLinks : ILinkAction
    {
        /// <summary>
        /// contains all game libraries that have a link to a store page that can be added.
        /// </summary>
        private readonly Libraries libraries;

        public string ProgressMessage { get; } = "LOCLinkUtilitiesLibraryLinkProgress";
        public string ResultMessage { get; } = "LOCLinkUtilitiesAddedMessage";
        public LinkUtilitiesSettings Settings { get; set; }

        public AddLibraryLinks(LinkUtilitiesSettings settings)
        {
            Settings = settings;

            libraries = new Libraries(Settings);
        }

        public bool Execute(Game game)
        {
            LibraryLink library;
            bool result = false;

            // Find the library of the game and add a link, if possible.
            library = libraries.Find(x => x.LibraryId == game.PluginId);

            if (library is object)
            {
                result = library.AddLink(game);
            }
            return result;
        }
    }
}