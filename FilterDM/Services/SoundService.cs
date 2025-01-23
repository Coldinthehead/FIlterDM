﻿using Avalonia.Platform;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilterDM.Services;
internal class SoundService : IDisposable
{
    private IWavePlayer? _outDevice;
    private WaveStream? _audioFileReader;

    public void Dispose() => Stop();

    public void Play(string fname)
    {
        Stop();

        string uri = $"avares://FilterDM/Assets/Sounds/{fname}";

        using var stream = AssetLoader.Open(new Uri(uri));
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
