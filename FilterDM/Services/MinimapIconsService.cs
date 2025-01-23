using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDM.Services;
public class MinimapIconsService : IInit
{
    private readonly List<CroppedBitmap> _icons = [];

    private readonly Dictionary<string, Dictionary<string, List<CroppedBitmap>>> _iconsMap = [];


    public Task Init()
    {

        Dispatcher.UIThread.Invoke(() =>
        {
            string uri = "avares://FilterDM/Assets/Minimap_icons.png";
            Bitmap atlas = new Bitmap(AssetLoader.Open(new Uri(uri)));
            int countX = 14;
            int spriteSize = (int)atlas.Size.Width / countX;
            
            for (int y = 0; y < atlas.Size.Height; y += spriteSize)
            {
                for (int x = 0; x < atlas.Size.Width; x += spriteSize)
                {
                    PixelRect cropRect = new(x, y, spriteSize, spriteSize);
                    CroppedBitmap sptire = new CroppedBitmap(atlas, cropRect);
                    _icons.Add(sptire);
                }
            }

            List<CroppedBitmap> icons = [];
            icons.AddRange(_icons.Slice(14 * 3 + 4, 10 + 14 * 7));
            icons.AddRange(_icons.Slice(14 * 15 + 2, 12 + 14 * 19 + 10));

            List<CroppedBitmap> large = [];
            List<CroppedBitmap> med = [];
            List<CroppedBitmap> small = [];
            large.AddRange(icons.Where((icon, index) => index % 3 == 0));
            med.AddRange(icons.Where((icon, index) => index % 3 == 1));
            small.AddRange(icons.Where((icon, index) => index % 3 == 2));

            _iconsMap["Large"] = new();
            _iconsMap["Med"] = new();
            _iconsMap["Small"] = new();

            BuildShapes(_iconsMap["Large"], large);
            BuildShapes(_iconsMap["Med"], med);
            BuildShapes(_iconsMap["Small"], small);

        });
        return Task.CompletedTask;
    }

    private void BuildShapes(Dictionary<string, List<CroppedBitmap>> map, List<CroppedBitmap> src)
    {
        map["Circle"] = [.. src.Slice(0, 6), .. src.Slice(6 * 6, 5)];
        map["Diamond"] = [.. src.Slice(6, 6), .. src.Slice(6 * 6 + 5, 5)];
        map["Hexagon"] = [.. src.Slice(6 * 2, 6), .. src.Slice(6 * 7 + 4, 5)];
        map["Square"] = [.. src.Slice(6 * 3, 6), .. src.Slice(6 * 8 + 3, 5)];
        map["Stat"] = [.. src.Slice(6 * 4, 6), .. src.Slice(6 * 9 + 2, 5)];
        map["Triangle"] = [.. src.Slice(6 * 5, 6), .. src.Slice(6 * 10 + 1, 5)];
        map["Cross"] = src.Slice(6 * 11, 11);
        map["Moon"] = src.Slice(6 * 12 + 5, 11);
        map["RainDrop"] = src.Slice(6 * 14 + 4, 11);
        map["Kite"] = src.Slice(6 * 16 + 3, 11);
        map["Pentagon"] = src.Slice(6 * 18 + 2, 11);
        map["UpsideDownHouse"] = src.Slice(6 * 20 + 1, 11);
    }

    public List<CroppedBitmap> GetIcons() => _icons;

    public CroppedBitmap Get(string size, string shape, int color)
    {
        return _iconsMap[size][shape][color];
    }
    public List<CroppedBitmap> GetLarge()
    {
        List<CroppedBitmap> res = [];
        foreach (KeyValuePair<string, List<CroppedBitmap>> icon in _iconsMap["Large"])
        {
            res.AddRange(icon.Value);
        }

        return res;
    }
    public List<CroppedBitmap> GetMed()
    {
        List<CroppedBitmap> res = [];
        foreach (KeyValuePair<string, List<CroppedBitmap>> icon in _iconsMap["Med"])
        {
            res.AddRange(icon.Value);
        }
        return res;
    }
    public List<CroppedBitmap> GetSmall()
    {
        List<CroppedBitmap> res = [];
        foreach (KeyValuePair<string, List<CroppedBitmap>> icon in _iconsMap["Small"])
        {
            res.AddRange(icon.Value);
        }
        return res;
    }
}
