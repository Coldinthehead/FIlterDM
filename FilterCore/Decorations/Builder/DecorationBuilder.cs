using FilterCore.Enums;
using System.Drawing;

namespace FilterCore.Decorations.Builder;

public class DecorationBuilder
{
    private List<DecorationProperty> _properties = [];

    private Color _textColor;
    private Color _borderColor;

    private bool _useTextColor;
    private bool _useBorderColor;


    private DecorationBuilder() { }

    public static DecorationBuilder Start()
    {
        return new DecorationBuilder();
    }

    public static DecorationBuilder GenericTierOne()
    {
        return Start()
                .WithSize(45)
                .WithBackground(Color.Orange)
                .WithPrimaryColor(Color.Black)
                .WithComboColor(StaticGameColor.Orange)
                .WithPermanentBeam()
                .WithShape(MinimapIconShape.Circle)
                .WithSize(MinimapIconSize.Large)
                .WithValuableSound();
    }

    public static DecorationBuilder GenericTierZero()
    {
        return Start()
                    .WithSize(45)
                    .WithBackground(Color.White)
                    .WithPrimaryColor(Color.Red)
                    .WithComboColor(StaticGameColor.Red)
                    .WithPermanentBeam()
                    .WithShape(MinimapIconShape.Diamond)
                    .WithSize(MinimapIconSize.Large)
                    .WithExceptionalSound();
    }
    public List<DecorationProperty> Build()
    {
        if (_useTextColor)
        {
            _properties.Add(DecorationProperty.TextColor(_textColor));
        }
        if (_useBorderColor)
        {
            _properties.Add(DecorationProperty.BorderColor(_borderColor));
        }

        return _properties;
    }

    public DecorationBuilder WithPrimaryColor(Color color)
    {
        _textColor = color;
        _useTextColor = true;
        _borderColor = color;
        _useBorderColor = true;
        return this;
    }

    public DecorationBuilder WithBackground(Color color)
    {
        _properties.Add(DecorationProperty.BackgroundColor(color));
        return this;
    }

    public DecorationBuilder ExcludeTextColor()
    {
        _useTextColor = false;
        return this;
    }

    public DecorationBuilder ExcludeBorderColor()
    {
        _useBorderColor = false;
        return this;
    }

    public IBeamBuilder WithBeam(StaticGameColor color)
    {
        return new BeamBuilder(this, color);
    }

    public IIconColorSelector WithIcon()
    {
        return new MinimapIconBuilder(this);
    }

    public DecorationBuilder WithSize(int size)
    {
        _properties.Add(DecorationProperty.FontSize(size));
        return this;
    }

    public DecorationBuilder WithExceptionalSound()
    {
        _properties.Add(DecorationProperty.AlertSound(Sound.ShFlex, 300));
        return this;
    }

    public DecorationBuilder WithValuableSound()
    {
        _properties.Add(DecorationProperty.AlertSound(Sound.HallKickCurrency, 300));
        return this;
    }

    public IComboIconBeamBuilder WithComboColor(StaticGameColor color)
    {
        return new ComboIconBeamBuilder(this, color);
    }

    public static DecorationBuilder Splinter()
    {
        return Start()
            .WithSize(45)
            .WithPrimaryColor(Color.Black)
            .WithBackground(Color.FromArgb(125, 255, 255, 255));
    }

    public class ComboIconBeamBuilder : IComboIconBeamBuilder, IIconSizeSelector, IIconShapeSelector
    {
        private readonly StaticGameColor _color;
        private MinimapIconSize _iconSize;
        private MinimapIconShape _shape;
        private bool _pernamentBeam;

        private readonly DecorationBuilder _parent;

        public ComboIconBeamBuilder(DecorationBuilder parent, StaticGameColor color)
        {
            _parent = parent;
            _color = color;
        }

        IIconShapeSelector IComboIconBeamBuilder.WithPermanentBeam()
        {
            _pernamentBeam = true;
            return this;
        }
        IIconShapeSelector IComboIconBeamBuilder.WithTempBeam()
        {
            _pernamentBeam = false;
            return this;
        }

        DecorationBuilder IIconSizeSelector.WithSize(MinimapIconSize size)
        {
            _iconSize = size;
            if (_pernamentBeam)
            {
                _parent._properties.Add(DecorationProperty.PermanentBeam(_color));
            }
            else
            {
                _parent._properties.Add(DecorationProperty.TempBeam(_color));
            }
            _parent._properties.Add(DecorationProperty.MinimapIcon(_iconSize, _color, _shape));
            return _parent;
        }

        public IIconSizeSelector WithShape(MinimapIconShape shape)
        {
            _shape = shape;
            return this;
        }
    }

    public class MinimapIconBuilder : IIconSizeSelector, IIconShapeSelector, IIconColorSelector
    {
        private readonly DecorationBuilder _parent;

        private StaticGameColor _color;
        private MinimapIconSize _size;
        private MinimapIconShape _shape;

        public MinimapIconBuilder(DecorationBuilder parent)
        {
            _parent = parent;
        }

        DecorationBuilder IIconSizeSelector.WithSize(MinimapIconSize size)
        {
            _size = size;
            _parent._properties.Add(DecorationProperty.MinimapIcon(_size, _color, _shape));
            return _parent;
        }
        IIconShapeSelector IIconColorSelector.WithColor(StaticGameColor color)
        {
            _color = color;
            return this;
        }
        public IIconSizeSelector WithShape(MinimapIconShape shape)
        {
            _shape = shape;
            return this;
        }
    }

    public class BeamBuilder : IBeamBuilder
    {
        private readonly DecorationBuilder _parent;
        private readonly StaticGameColor _color;

        public BeamBuilder(DecorationBuilder parent, StaticGameColor _color)
        {
            _parent = parent;
        }

        DecorationBuilder IBeamBuilder.Permanent()
        {
            _parent._properties.Add(DecorationProperty.PermanentBeam(_color));
            return _parent;
        }
        DecorationBuilder IBeamBuilder.Temp()
        {
            _parent._properties.Add(DecorationProperty.TempBeam(_color));
            return _parent;
        }
    }
}
