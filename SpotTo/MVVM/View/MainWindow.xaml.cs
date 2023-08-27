using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotTo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private SpotifyAPIHelper _spotifyAPIHelper = new SpotifyAPIHelper();


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private async void FetchPlaylist_Click(object sender, RoutedEventArgs e)
        {
            // Extract playlist ID from URL
            var playlistId = ExtractPlaylistIdFromUrl(PlaylistUrlTextBox.Text);

            if (!string.IsNullOrEmpty(playlistId))
            {
                var songs = await _spotifyAPIHelper.GetSongsFromPlaylist(playlistId);
                SongsListBox.ItemsSource = songs;
                SongCountLabel.Content = $"Songs: {songs.Count()}"; // Update the label with song count

            }
            else
            {
                MessageBox.Show("Invalid Playlist URL");
            }


        }


        public static string ExtractPlaylistIdFromUrl(string url)
        {
            Uri uri;

            if (!Uri.TryCreate(url, UriKind.Absolute, out uri) || uri.Host != "open.spotify.com" || !uri.AbsolutePath.StartsWith("/playlist/"))
            {
                throw new ArgumentException("Invalid Spotify playlist URL.");
            }

            string path = uri.AbsolutePath;
            string[] parts = path.Split('/');
            if (parts.Length > 2)
            {
                return parts[2];
            }
            else
            {
                throw new ArgumentException("Invalid Spotify playlist URL.");
            }
        }

        private void PlaylistUrlTextBox_TextInput(object sender, TextChangedEventArgs e)
        {

        }


    }
}
