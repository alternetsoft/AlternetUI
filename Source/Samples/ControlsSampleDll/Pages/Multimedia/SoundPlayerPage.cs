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
        internal static readonly string audioNotification2 = $"{ResPrefix}notifications-sound-127856.wav";
        internal static readonly string audioTap = $"{ResPrefix}tap-notification-180637.wav";
        internal static readonly string audioDogGrowl = $"{ResPrefix}doggrowl.wav";
        internal static readonly string audioTinkALink = $"{ResPrefix}tinkalink2.wav";       
        internal static readonly string audioCustom = "Open audio file (*.wav)...";

        private SimpleSoundPlayer? player;
        private OpenFileDialog? dialog;
        
        private readonly ComboBox selectComboBox = new()
        {
            Margin = 5,
        };

        public SoundPlayerPage()
        {
            Padding = 10;

            selectComboBox.IsEditable = false;
            selectComboBox.Add(audioNotification2);
            selectComboBox.Add(audioButton);
            selectComboBox.Add(audioCustom);
            selectComboBox.Add(audioDogGrowl);
            selectComboBox.Add(audioTinkALink);

            selectComboBox.SelectedItem = audioNotification2;
            selectComboBox.Parent = this;

            selectComboBox.SelectedItemChanged += SelectComboBox_SelectedItemChanged;

            AddVerticalStackPanel().AddButtons(
                ("Play", Play),
                ("Stop", Stop))
            .Margin(5).HorizontalAlignment(HorizontalAlignment.Left).SuggestedWidthToMax();
        }

        private void CreatePlayer(string? url = null)
        {
            if(url is null)
            {
                if (selectComboBox.SelectedItem is not string selectedUrl)
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
            if (selectComboBox.SelectedItem is not string url)
                return;

            if (url == audioCustom)
            {
                dialog ??= new OpenFileDialog();
                dialog.FileMustExist = true;
                dialog.Filter = "Audio Files (*.wav)|*.wav";
                var result = dialog.ShowModal(this.ParentWindow);
                if (result == ModalResult.Accepted)
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
                }
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