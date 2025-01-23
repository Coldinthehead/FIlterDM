using Avalonia.Platform;
using NAudio.Wave;
using System;

namespace FilterDM.Services;
public class SoundService : IDisposable
{
    private IWavePlayer? _outDevice;
    private WaveStream? _audioFileReader;

    public void Dispose() => Stop();

    public void Play(string fname)
    {
        Stop();

        string uri = $"avares://FilterDM/Assets/Sounds/{fname}";

        using var stream = AssetLoader.Open(new Uri(uri));
        if (stream == null)
        {
            return;
        }
        _outDevice = new WaveOutEvent();
        _audioFileReader = new Mp3FileReader(stream);
        _outDevice.Init(_audioFileReader);
        _outDevice.Volume = 0.5f;
        _outDevice.Play();
        _outDevice.PlaybackStopped += (sender, e) => Stop();
    }

    private void Stop()
    {
        _outDevice?.Stop();
        _outDevice?.Dispose();
        _audioFileReader?.Dispose();
        _outDevice = null;
        _audioFileReader = null;
    }
}
