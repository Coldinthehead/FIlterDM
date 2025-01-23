using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using FilterDM.Constants;
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

            _iconsMap[IconSize.LARGE] = new();
            _iconsMap[IconSize.MEDIUM] = new();
            _iconsMap[IconSize.SMALL] = new();

            BuildShapes(_iconsMap[IconSize.LARGE], large);
            BuildShapes(_iconsMap[IconSize.MEDIUM], med);
            BuildShapes(_iconsMap[IconSize.SMALL], small);

        });
        return Task.CompletedTask;
    }

    private void BuildShapes(Dictionary<string, List<CroppedBitmap>> map, List<CroppedBitmap> src)
    {
        map[IconShapeConstants.CIRCLE] = [.. src.Slice(0, 6), .. src.Slice(6 * 6, 5)];
        map[IconShapeConstants.DIAMOND] = [.. src.Slice(6, 6), .. src.Slice(6 * 6 + 5, 5)];
        map[IconShapeConstants.HEXAGON] = [.. src.Slice(6 * 2, 6), .. src.Slice(6 * 7 + 4, 5)];
        map[IconShapeConstants.SQUARE] = [.. src.Slice(6 * 3, 6), .. src.Slice(6 * 8 + 3, 5)];
        map[IconShapeConstants.STAR] = [.. src.Slice(6 * 4, 6), .. src.Slice(6 * 9 + 2, 5)];
        map[IconShapeConstants.TRIANGLE] = [.. src.Slice(6 * 5, 6), .. src.Slice(6 * 10 + 1, 5)];
        map[IconShapeConstants.CROSS] = src.Slice(6 * 11, 11);
        map[IconShapeConstants.MOON] = src.Slice(6 * 12 + 5, 11);
        map[IconShapeConstants.RAINDROP] = src.Slice(6 * 14 + 4, 11);
        map[IconShapeConstants.KITE] = src.Slice(6 * 16 + 3, 11);
        map[IconShapeConstants.PENTAGON] = src.Slice(6 * 18 + 2, 11);
        map[IconShapeConstants.HOUSE] = src.Slice(6 * 20 + 1, 11);
    }

    public List<CroppedBitmap> GetIcons() => _icons;

    public CroppedBitmap Get(string size, string shape, int color)
    {
        return _iconsMap[size][shape][color];
    }
    public List<CroppedBitmap> GetLarge()
    {
        List<CroppedBitmap> res = [];
        foreach (KeyValuePair<string, List<CroppedBitmap>> icon in _iconsMap[IconSize.LARGE])
        {
            res.AddRange(icon.Value);
        }

        return res;
    }
    public List<CroppedBitmap> GetMed()
    {
        List<CroppedBitmap> res = [];
        foreach (KeyValuePair<string, List<CroppedBitmap>> icon in _iconsMap[IconSize.MEDIUM])
        {
            res.AddRange(icon.Value);
        }
        return res;
    }
    public List<CroppedBitmap> GetSmall()
    {
        List<CroppedBitmap> res = [];
        foreach (KeyValuePair<string, List<CroppedBitmap>> icon in _iconsMap[IconSize.SMALL])
        {
            res.AddRange(icon.Value);
        }
        return res;
    }
}
