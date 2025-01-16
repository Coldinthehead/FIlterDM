using FilterCore.Enums;

namespace FilterCore.Decorations.Builder;

public interface IIconColorSelector
{
    public IIconShapeSelector WithColor(StaticGameColor color);
}


public interface IIconShapeSelector
{
    public IIconSizeSelector WithShape(MinimapIconShape shape);
}

public interface IIconSizeSelector
{
    public DecorationBuilder WithSize(MinimapIconSize size);
}
