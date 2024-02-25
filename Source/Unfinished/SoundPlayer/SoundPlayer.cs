using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading;

namespace Alternet.UI
{
    /// <summary>
    /// Controls playback of a sound from a audio file.
    /// </summary>
    [Serializable]
    [ToolboxItem(false)]
    public class SoundPlayer : BaseComponent, ISerializable
    {
        private const int defaultLoadTimeout = 10000;
        private const int blockSize = 1024;

        private static readonly object EventLoadCompleted = new();
        private static readonly object EventSoundLocationChanged = new();
        private static readonly object EventStreamChanged = new();
        private readonly SendOrPostCallback? loadAsyncOperationCompleted;

        private Uri? uri;
        private string? soundLocation = string.Empty;
        private int loadTimeout = defaultLoadTimeout;
        private ManualResetEvent semaphore = new(initialState: true);
        private Thread? copyThread;
        private int currentPos;
        private Stream? stream;
        private bool isLoadCompleted;
        private Exception? lastLoadException;
        private bool doesLoadAppearSynchronous;
        private byte[]? streamData;
        private AsyncOperation? asyncOperation;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundPlayer" /> class.
        /// </summary>
        public SoundPlayer()
        {
            loadAsyncOperationCompleted = LoadAsyncOperationCompleted;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundPlayer" /> class,
        /// and attaches the specified audio file.</summary>
        /// <param name="soundLocation">The location of a audio file to load.</param>
        /// <exception cref="UriFormatException">The URL value specified
        /// by <paramref name="soundLocation" /> cannot be resolved.</exception>
        public SoundPlayer(string soundLocation)
            : this()
        {
            soundLocation ??= string.Empty;
            SetupSoundLocation(soundLocation);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundPlayer" /> class,
        /// and attaches the audio file within the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">A <see cref="Stream" /> to an audio file.</param>
        public SoundPlayer(Stream stream)
            : this()
        {
            this.stream = stream;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundPlayer" /> class.
        /// </summary>
        /// <param name="serializationInfo">
        /// The <see cref="SerializationInfo" /> to be used for
        /// deserialization.
        /// </param>
        /// <param name="context">The destination to be used for deserialization.</param>
        /// <exception cref="UriFormatException">
        /// The <see cref="SoundPlayer.SoundLocation" /> specified in
        /// <paramref name="serializationInfo" /> cannot be resolved.</exception>
        protected SoundPlayer(SerializationInfo serializationInfo, StreamingContext context)
        {
            SerializationInfoEnumerator enumerator = serializationInfo.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                switch (current.Name)
                {
                    case "SoundLocation":
                        SetupSoundLocation((string?)current.Value);
                        break;
                    case "Stream":
                        stream = (Stream?)current.Value;
                        if (stream?.CanSeek ?? false)
                            stream.Seek(0L, SeekOrigin.Begin);
                        break;
                    case "LoadTimeout":
                        LoadTimeout = (int?)current.Value ?? 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Occurs when a audio file has been successfully or unsuccessfully loaded.
        /// </summary>
        public event AsyncCompletedEventHandler? LoadCompleted;

        /// <summary>
        /// Occurs when a new audio source path for this <see cref="SoundPlayer" />
        /// has been set.</summary>
        public event EventHandler? SoundLocationChanged;

        /// <summary>
        /// Occurs when a new <see cref="Stream" /> audio source for this
        /// <see cref="SoundPlayer" /> has been set.</summary>
        public event EventHandler? StreamChanged;

        /// <summary>
        /// Gets or sets the time (in milliseconds) in which the audio file must load.
        /// </summary>
        /// <returns>
        /// The number of milliseconds to wait. The default is 10000 (10 seconds).
        /// </returns>
        [DefaultValue(defaultLoadTimeout)]
        public int LoadTimeout
        {
            get
            {
                return loadTimeout;
            }

            set
            {
                value = Math.Max(0, value);
                loadTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the file path or URL of the audio file to load.
        /// </summary>
        /// <returns>
        /// The file path or URL from which to load an audio file,
        /// or <see cref="string.Empty" /> if no file path is present.
        /// The default is <see cref="string.Empty" />.</returns>
        public string SoundLocation
        {
            get
            {
                return soundLocation;
            }

            set
            {
                value ??= string.Empty;
                if (!soundLocation.Equals(value))
                {
                    SetupSoundLocation(value);
                    OnSoundLocationChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Stream" /> from which to load the audion file.
        /// </summary>
        /// <returns>
        /// A <see cref="Stream" /> from which to load the audio file,
        /// or <see langword="null" /> if no stream is available.
        /// The default is <see langword="null" />.
        /// </returns>
        [Browsable(false)]
        public Stream? Stream
        {
            get
            {
                if (uri != null)
                    return null;
                return stream;
            }

            set
            {
                if (stream != value)
                {
                    SetupStream(value);
                    OnStreamChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether loading of an audio file has successfully completed.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if an audio file is loaded;
        /// <see langword="false" /> if an audio file has not yet been loaded.
        /// </returns>
        public bool IsLoadCompleted => isLoadCompleted;

        /// <summary>
        /// Gets or sets the <see cref="object" /> that contains data about
        /// the <see cref="SoundPlayer" />.
        /// </summary>
        /// <returns>An <see cref="object" /> that contains data about
        /// the <see cref="SoundPlayer" />.</returns>
        public object? Tag { get; set; }

        /// <summary>
        /// Loads an audio file from a stream or a Web resource using a new thread.
        /// </summary>
        /// <exception cref="TimeoutException">
        /// The elapsed time during loading exceeds the time, in milliseconds,
        /// specified by <see cref="SoundPlayer.LoadTimeout" />.</exception>
        /// <exception cref="FileNotFoundException">The file specified by
        /// <see cref="SoundPlayer.SoundLocation" /> cannot be found.</exception>
        public void LoadAsync()
        {
            if (uri != null && uri.IsFile)
            {
                isLoadCompleted = true;
                FileInfo fileInfo = new FileInfo(uri.LocalPath);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException(
                        SR.GetString("SoundAPIFileDoesNotExist"),
                        soundLocation);
                }

                OnLoadCompleted(new AsyncCompletedEventArgs(null, cancelled: false, null));
            }
            else
            if (copyThread == null || copyThread.ThreadState != 0)
            {
                isLoadCompleted = false;
                streamData = null;
                currentPos = 0;
                asyncOperation = AsyncOperationManager.CreateOperation(null);
                LoadStream(loadSync: false);
            }
        }

        /// <summary>
        /// Loads a sound synchronously.
        /// </summary>
        /// <exception cref="TimeoutException">The elapsed time during loading exceeds
        /// the time, in milliseconds, specified by <see cref="SoundPlayer.LoadTimeout" />.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The file specified by <see cref="SoundPlayer.SoundLocation" /> cannot
        /// be found.</exception>
        public void Load()
        {
            if (uri != null && uri.IsFile)
            {
                FileInfo fileInfo = new FileInfo(uri.LocalPath);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException(
                        SR.GetString("SoundAPIFileDoesNotExist"),
                        soundLocation);
                }

                isLoadCompleted = true;
                OnLoadCompleted(new AsyncCompletedEventArgs(null, cancelled: false, null));
            }
            else
            {
                LoadSync();
            }
        }

        private void LoadAndPlay(int flags)
        {
            if (string.IsNullOrEmpty(soundLocation) && stream == null)
            {
                SystemSounds.Beep.Play();
                return;
            }

            if (uri != null && uri.IsFile)
            {
                string localPath = uri.LocalPath;
                isLoadCompleted = true;
                try
                {
                    ValidateSoundFile(localPath);
                    UnsafeNativeMethods.PlaySound(localPath, IntPtr.Zero, 2 | flags);
                    return;
                }
                finally
                {
                }
            }

            LoadSync();
            ValidateSoundData(streamData);
            try
            {
                UnsafeNativeMethods.PlaySound(streamData, IntPtr.Zero, 6 | flags);
            }
            finally
            {
            }
        }

        private void LoadSync()
        {
            if (!semaphore.WaitOne(LoadTimeout, exitContext: false))
            {
                if (copyThread != null)
                {
                    copyThread.Abort();
                }

                CleanupStreamData();
                throw new TimeoutException(SR.GetString("SoundAPILoadTimedOut"));
            }

            if (streamData != null)
            {
                return;
            }
            if (uri != null && !uri.IsFile && stream == null)
            {
                WebPermission webPermission = new WebPermission(NetworkAccess.Connect, uri.AbsolutePath);
                webPermission.Demand();
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Timeout = LoadTimeout;
                WebResponse response = webRequest.GetResponse();
                stream = response.GetResponseStream();
            }

            if (stream.CanSeek)
            {
                LoadStream(loadSync: true);
            }
            else
            {
                doesLoadAppearSynchronous = true;
                LoadStream(loadSync: false);
                if (!semaphore.WaitOne(LoadTimeout, exitContext: false))
                {
                    if (copyThread != null)
                    {
                        copyThread.Abort();
                    }

                    CleanupStreamData();
                    throw new TimeoutException(SR.GetString("SoundAPILoadTimedOut"));
                }

                doesLoadAppearSynchronous = false;
                if (lastLoadException != null)
                {
                    throw lastLoadException;
                }
            }

            copyThread = null;
        }

        private void LoadStream(bool loadSync)
        {
            if (loadSync && stream.CanSeek)
            {
                int num = (int)stream.Length;
                currentPos = 0;
                streamData = new byte[num];
                stream.Read(streamData, 0, num);
                isLoadCompleted = true;
                OnLoadCompleted(new AsyncCompletedEventArgs(null, cancelled: false, null));
            }
            else
            {
                semaphore.Reset();
                copyThread = new Thread(WorkerThread);
                copyThread.Start();
            }
        }

        /// <summary>Plays the audio file using a new thread, and loads the audio file first if it has not been loaded.</summary>
        /// <exception cref="T:System.ServiceProcess.TimeoutException">The elapsed time during loading exceeds the time, in milliseconds, specified by <see cref="P:System.Media.SoundPlayer.LoadTimeout" />.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <see cref="P:System.Media.SoundPlayer.SoundLocation" /> cannot be found.</exception>
        /// <exception cref="T:System.InvalidOperationException">The audio header is corrupted; the file specified by <see cref="P:System.Media.SoundPlayer.SoundLocation" /> is not a PCM audio file.</exception>
        public void Play()
        {
            LoadAndPlay(1);
        }

        /// <summary>Plays the audio file and loads the audio file first if it has not been loaded.</summary>
        /// <exception cref="T:System.ServiceProcess.TimeoutException">The elapsed time during loading exceeds the time, in milliseconds, specified by <see cref="P:System.Media.SoundPlayer.LoadTimeout" />.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <see cref="P:System.Media.SoundPlayer.SoundLocation" /> cannot be found.</exception>
        /// <exception cref="T:System.InvalidOperationException">The audio header is corrupted; the file specified by <see cref="P:System.Media.SoundPlayer.SoundLocation" /> is not a PCM .wav file.</exception>
        public void PlaySync()
        {
            LoadAndPlay(0);
        }

        /// <summary>Plays and loops the audio file using a new thread, and loads the audio file first if it has not been loaded.</summary>
        /// <exception cref="T:System.ServiceProcess.TimeoutException">The elapsed time during loading exceeds the time, in milliseconds, specified by <see cref="P:System.Media.SoundPlayer.LoadTimeout" />.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file specified by <see cref="P:System.Media.SoundPlayer.SoundLocation" /> cannot be found.</exception>
        /// <exception cref="T:System.InvalidOperationException">The audio header is corrupted; the file specified by <see cref="P:System.Media.SoundPlayer.SoundLocation" /> is not a PCM audio file.</exception>
        public void PlayLooping()
        {
            LoadAndPlay(9);
        }

        /// <summary>Stops playback of the sound if playback is occurring.</summary>
        public void Stop()
        {
            Native.WxOtherFactory.SoundStop();
        }

        /// <summary>Raises the <see cref="E:System.Media.SoundPlayer.LoadCompleted" /> event.</summary>
        /// <param name="e">An <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> that contains the event data.</param>
        protected virtual void OnLoadCompleted(AsyncCompletedEventArgs? e)
        {
            ((AsyncCompletedEventHandler)base.Events[EventLoadCompleted])?.Invoke(this, e);
        }

        /// <summary>Raises the <see cref="E:System.Media.SoundPlayer.SoundLocationChanged" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected virtual void OnSoundLocationChanged(EventArgs e)
        {
            ((EventHandler)base.Events[EventSoundLocationChanged])?.Invoke(this, e);
        }

        /// <summary>Raises the <see cref="E:System.Media.SoundPlayer.StreamChanged" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected virtual void OnStreamChanged(EventArgs e)
        {
            ((EventHandler)base.Events[EventStreamChanged])?.Invoke(this, e);
        }

        private static Uri? ResolveUri(string? partialUri)
        {
            Uri? uri = null;
            try
            {
                uri = new Uri(partialUri);
            }
            catch (UriFormatException)
            {
            }

            if (uri == null)
            {
                try
                {
                    uri = new Uri(Path.GetFullPath(partialUri));
                }
                catch (UriFormatException)
                {
                }
            }

            return uri;
        }

        private void SetupSoundLocation(string? soundLocation)
        {
            if (copyThread != null)
            {
                copyThread.Abort();
                CleanupStreamData();
            }

            uri = ResolveUri(soundLocation);
            this.soundLocation = soundLocation;
            stream = null;
            if (uri == null)
            {
                if (!string.IsNullOrEmpty(soundLocation))
                {
                    throw new UriFormatException(SR.GetString("SoundAPIBadSoundLocation"));
                }
            }
            else if (!uri.IsFile)
            {
                streamData = null;
                currentPos = 0;
                isLoadCompleted = false;
            }
        }

        private void SetupStream(Stream? stream)
        {
            if (copyThread != null)
            {
                copyThread.Abort();
                CleanupStreamData();
            }

            this.stream = stream;
            soundLocation = string.Empty;
            streamData = null;
            currentPos = 0;
            isLoadCompleted = false;
            if (stream != null)
            {
                uri = null;
            }
        }

        private void LoadAsyncOperationCompleted(object? arg)
        {
            OnLoadCompleted((AsyncCompletedEventArgs?)arg);
        }

        private void CleanupStreamData()
        {
            currentPos = 0;
            streamData = null;
            isLoadCompleted = false;
            lastLoadException = null;
            doesLoadAppearSynchronous = false;
            copyThread = null;
            semaphore.Set();
        }

        private void WorkerThread()
        {
            try
            {
                if (uri != null && !uri.IsFile && stream == null)
                {
                    WebRequest webRequest = WebRequest.Create(uri);
                    WebResponse response = webRequest.GetResponse();
                    stream = response.GetResponseStream();
                }

                streamData = new byte[1024];
                int num = stream.Read(streamData, currentPos, 1024);
                int num2 = num;
                while (num > 0)
                {
                    currentPos += num;
                    if (streamData.Length < currentPos + 1024)
                    {
                        byte[] destinationArray = new byte[streamData.Length * 2];
                        Array.Copy(streamData, destinationArray, streamData.Length);
                        streamData = destinationArray;
                    }

                    num = stream.Read(streamData, currentPos, 1024);
                    num2 += num;
                }

                lastLoadException = null;
            }
            catch (Exception ex)
            {
                lastLoadException = ex;
            }

            if (!doesLoadAppearSynchronous)
            {
                asyncOperation.PostOperationCompleted(loadAsyncOperationCompleted, new AsyncCompletedEventArgs(lastLoadException, cancelled: false, null));
            }

            isLoadCompleted = true;
            semaphore.Set();
        }

        private unsafe void ValidateSoundFile(string fileName)
        {
        }

        private void ValidateSoundData(byte[]? data)
        {
        }

        /// <summary>For a description of this member, see the <see cref="M:System.Runtime.Serialization.ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)" /> method.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (!string.IsNullOrEmpty(soundLocation))
            {
                info.AddValue("SoundLocation", soundLocation);
            }

            if (stream != null)
            {
                info.AddValue("Stream", stream);
            }

            info.AddValue("LoadTimeout", loadTimeout);
        }
    }
}
