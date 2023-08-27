    using SpotifyAPI.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

public class SpotifyAPIHelper
{
    private readonly string _clientId = "";
    private readonly string _clientSecret = "";
    private SpotifyClient _spotifyClient;

    public SpotifyAPIHelper()
    {
        ConfigureClient();
    }

    private void ConfigureClient()
    {
        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(_clientId, _clientSecret);

        // Use non-generic version
        var responseTask = new OAuthClient(config).RequestToken(request);
        var response = responseTask.Result as ClientCredentialsTokenResponse;

        if (response != null)
        {
            _spotifyClient = new SpotifyClient(config.WithToken(response.AccessToken));
        }
        else
        {
            throw new Exception("Failed to obtain Spotify token.");
        }
    }


    public async Task<List<string>> GetSongsFromPlaylist(string playlistId)
    {
        try
        {
            var songs = new List<string>();
            var itemsPerPage = 100;
            var currentOffset = 0;

            while (true)
            {
                var request = new PlaylistGetItemsRequest { Limit = itemsPerPage, Offset = currentOffset };
                var playlistItems = await _spotifyClient.Playlists.GetItems(playlistId, request);

                if (playlistItems.Items.Count == 0)
                {
                    break;
                }

                foreach (var item in playlistItems.Items)
                {
                    if (item.Track is FullTrack track)
                    {
                        songs.Add($"{track.Name} - {track.Artists.FirstOrDefault()?.Name}");
                    }
                }

                currentOffset += itemsPerPage;
            }

            return songs;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching songs from playlist: {ex.Message}");
            return new List<string>();
        }
    }



}
