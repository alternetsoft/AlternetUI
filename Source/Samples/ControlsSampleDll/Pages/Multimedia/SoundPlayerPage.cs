using System;
using System.IO;
using System.Linq;
using Alternet.UI;

namespace ControlsSample
{
    internal partial class SoundPlayerPage : VerticalStackPanel
    {
        private static readonly string ResPrefix = $"Resources/Sounds/Wav/";
        internal static readonly string audioButton = $"{ResPrefix}button-124476.wav";
        internal static readonly string audioNotification1 = $"{ResPrefix}notification-sound-7062.wav";
        internal static readonly string audioNotification2
            = $"{ResPrefix}notifications-sound-127856.wav";
        internal static readonly string audioTap = $"{ResPrefix}tap-notification-180637.wav";
        internal static readonly string audioDogGrowl = $"{ResPrefix}doggrowl.wav";
        internal static readonly string audioTinkALink = $"{ResPrefix}tinkalink2.wav";       
        
        internal static readonly string audioCustom = "Open audio file (*.wav)...";

        private SimpleSoundPlayer? player;
        
        private readonly EnumPicker selectComboBox = new()
        {
            Margin = 5,
        };

        public SoundPlayerPage()
        {
            Padding = 10;

            new Label("Select sound to play:")
            {
                Margin = 5,
                HorizontalAlignment = HorizontalAlignment.Left,
                Parent = this,
            };

            List<string> audioFiles = new()
            {
                audioNotification2,
                audioButton,
                audioDogGrowl,
                audioTinkALink,
                audioCustom,
            };

            selectComboBox.AddRange(audioFiles);
            selectComboBox.Value = audioNotification2;
            selectComboBox.Parent = this;
            selectComboBox.ValueChanged += SelectComboBox_SelectedItemChanged;

            var stackPanel = new VerticalStackPanel(this);

            var playButton = new Button("Play", Play);
            var stopButton = new Button("Stop", Stop);

            new ControlSet(playButton, stopButton)
            .Margin(5).HorizontalAlignment(HorizontalAlignment.Left).Parent(this).SuggestedWidthToMax();
        }

        private void CreatePlayer(string? url = null)
        {
            if(url is null)
            {
                if (selectComboBox.Value is not string selectedUrl)
                    return;
                url = Path.Combine(CommonUtils.GetAppFolder(), selectedUrl);
            }

            player = new(url);
            App.Log($"SimpleSoundPlayer created. Is ok: {player.IsOk}");
        }

        private void Play()
        {
            if (player is null)
                CreatePlayer();

            if (player is null || !player.IsOk)
            {
                App.Log($"SimpleSoundPlayer.Play. Is ok: {false}");
                return;
            }

            var result = player?.Play() ?? false;
            App.Log($"SimpleSoundPlayer.Play. Is ok: {result}");
        }

        private void Stop()
        {
            player?.Stop();
        }

        private void SelectComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            if (selectComboBox.Value is not string url)
                return;

            if (url == audioCustom)
            {
                var dialog = OpenFileDialog.Default;
                dialog.FileMustExist = true;
                dialog.Filter = "Audio Files (*.wav)|*.wav";

                dialog.ShowAsync(this.ParentWindow, () =>
                {
                    if (File.Exists(dialog.FileName))
                    {
                        Stop();
                        player?.Dispose();
                        CreatePlayer(dialog.FileName);
                        Play();
                    }
                    else
                        App.Log($"File not found: {dialog.FileName}");
                });
            }
            else
            {
                Stop();
                player?.Dispose();
                CreatePlayer();
                Play();
            }
        }
    }
}