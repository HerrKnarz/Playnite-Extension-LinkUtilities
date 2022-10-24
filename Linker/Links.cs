﻿using System.Collections.Generic;

namespace LinkUtilities.Linker
{
    /// <summary>
    /// List of all website Links that can be added.
    /// </summary>
    public class Links : List<Link>
    {
        public Links(LinkUtilitiesSettings settings)
        {
            Add(new LinkMobyGames(settings));
            Add(new LinkPCGamingWiki(settings));
        }
    }
}