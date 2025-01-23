using Avalonia.Platform;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;

namespace FilterDM.Services;
public class SoundService : IDisposable
{
    private readonly IWavePlayer _outDevice = new WaveOutEvent()
    {
        Volume = 0.5f
    };
    private readonly MixingSampleProvider mixer;


    private DateTime _lastStartTime;


    public void Dispose() => _outDevice?.Dispose();

    public SoundService()
    {
        mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        mixer.ReadFully = true;
        _outDevice.Init(mixer);
        _outDevice.Play();
        _lastStartTime = DateTime.Now;
    }

    public void Play(int sample, int volume)
    {
        if (DateTime.Now - _lastStartTime < TimeSpan.FromSeconds(1))
        {
            return;
        }
        _lastStartTime = DateTime.Now;
        if (sample < 1 && sample > 16)
            return;
        string fname = "AlertSound" + sample + ".mp3";
        string uri = $"avares://FilterDM/Assets/Sounds/{fname}";
        Uri u = new Uri(uri);
        if (!AssetLoader.Exists(u))
        {
            return;
        }
        float vol = volume / 300.0f;
        var stream = AssetLoader.Open(u);
        var reader = new Mp3FileReader(stream);
        var volumeC = new WaveChannel32(reader)
        {
            Volume = vol,
        };
        mixer.AddMixerInput(volumeC);

    }
}

