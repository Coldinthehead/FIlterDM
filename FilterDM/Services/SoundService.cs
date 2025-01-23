using Avalonia.Platform;
using NAudio.Wave;
using System;
using System.IO;

namespace FilterDM.Services;
public class SoundService : IDisposable
{
    private IWavePlayer? _outDevice;
    private WaveStream? _audioFileReader;
    private Stream? _currentSteam;

    public void Dispose() => Stop();

    private float _curVol;

    public void Play(int sample, int volume)
    {
        Stop();
        if (sample < 1 && sample > 16)
            return;
        string fname = "AlertSound" + sample + ".mp3";
        string uri = $"avares://FilterDM/Assets/Sounds/{fname}";
        Uri u = new Uri(uri);
        if (!AssetLoader.Exists(u))
        {
            return;
        }
        try
        {
            float vol = volume / 300.0f;
            _currentSteam = AssetLoader.Open(u);
            _outDevice = new WaveOutEvent();
            _audioFileReader = new Mp3FileReader(_currentSteam);
            _outDevice.Init(_audioFileReader);
            _outDevice.Volume = vol / 2;
            _outDevice.Play();
            _outDevice.PlaybackStopped += (sender, e) =>
            {
                if (e != null)
                {

                }
                Stop();
            };
        }
        catch (Exception ex)
        {
            Stop();
        }
    }

    private void Stop()
    {
        _outDevice?.Stop();
        _outDevice?.Dispose();
        _audioFileReader?.Dispose();
        _currentSteam?.Close();
        _outDevice = null;
        _audioFileReader = null;
        _currentSteam = null;
    }
}
