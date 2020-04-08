using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace Media_Player
{
    public partial class MainWindow : Window
    {
        string mediaSource;
        DispatcherTimer timer;
        bool isPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_tick);
            this.Title = "GoPlayer";
            PauseBtn.Visibility = Visibility.Hidden;
            collapseGrid.Visibility = Visibility.Visible;
            listGrid.Visibility = Visibility.Hidden;
        }

        private void timer_tick(object sender, EventArgs e)
        {
            mediaSlider.Value = mediaElement.Position.TotalSeconds;
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                totalTime.Text = mediaElement.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if(isPlaying == false)
            {
                if (mediaList.SelectedItem != null)
                {
                    collapseGrid.Visibility = Visibility.Visible;
                    listGrid.Visibility = Visibility.Hidden;

                    mediaSource = mediaList.SelectedItem.ToString();
                    mediaElement.Source = new Uri(mediaSource);
                    mediaElement.LoadedBehavior = MediaState.Manual;
                    mediaElement.Play();
                    isPlaying = true;
                    PlayBtn.Visibility = Visibility.Hidden;
                    PauseBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Select Media file to  play");
                }
            }
            else
            {
                mediaElement.Pause();
                isPlaying = false;
                PlayBtn.Visibility = Visibility.Visible;
                PauseBtn.Visibility = Visibility.Hidden;
            }

        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }

        private void mediaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(mediaSlider.Value);
            terminated.Text = TimeSpan.FromSeconds(mediaSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void volumSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Volume = (double)volumSlider.Value;
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            collapseGrid.Visibility = Visibility.Hidden;
            listGrid.Visibility = Visibility.Visible;

            OpenFileDialog fileDialog = new OpenFileDialog();
            //fileDialog.Filter = "*.mpg|*.mpeg|*.avi|*.mp4|*.flv|*.WAV|*.AIFF|*.PCM|*.FLAC|*.ALAC|*.WMA|*.MP3|*.OGG|*.AAC|*";
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() == true)
            {
                mediaList.Items.Clear();
                var mediaFiles = fileDialog.FileNames;
                foreach (var file in mediaFiles)
                {
                    mediaList.Items.Add(file);
                }
            }
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan timeSpan = mediaElement.NaturalDuration.TimeSpan;
            mediaSlider.Maximum = timeSpan.TotalSeconds;
            timer.Start();
        }

        private void ForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position + TimeSpan.FromSeconds(5);
        }

        private void RewindBtn_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position - TimeSpan.FromSeconds(5);
        }

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.SpeedRatio = speedSlider.Value;
        }

        private void click_MouseDown(object sender, MouseButtonEventArgs e)
        {
            collapseGrid.Visibility = Visibility.Hidden;
            listGrid.Visibility = Visibility.Visible;
        }

        private void hideList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            listGrid.Visibility = Visibility.Hidden;
            collapseGrid.Visibility = Visibility.Visible;
        }
    }
}
