using System;
namespace GeoGames
{
    public class DeepLinkingConstants
    {
        public DeepLinkingConstants()
        {
        }

        public const string DataScheme = "http";
        public const string DataHost = "geogames.linknode.co.uk";
        public const string DataPathPrefix = "/fugitive";

        public const string DEFAULT_GAME = "GeoGames-Testing";

        public static string GenerateShareURL(string gameId)
        {
            return string.Format("{0}://{1}{2}/{3}", DataScheme, DataHost, DataPathPrefix, gameId);
        }
    }
}
