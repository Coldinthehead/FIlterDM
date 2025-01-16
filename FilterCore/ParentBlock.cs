using System.Text;

namespace FilterCore;

public class ParentBlock
{
    public readonly List<ItemBlock> ChildBlocks = [];

    public readonly string Name;

    public ParentBlock(string name)
    {
        Name = name;
    }

    public ParentBlock AddChildBlock(ItemBlock block)
    {
        ChildBlocks.Add(block);
        block.ParentName = Name;
        return this;
    }

    public string DumpString()
    {
        StringBuilder sb = new();
        sb.Append('#').AppendLine(new string('=', 50));
        sb.AppendLine($"# {Name}");
        sb.Append('#').AppendLine(new string('=', 50));
        foreach (ItemBlock block in ChildBlocks)
        {
            sb.AppendLine();
            sb.Append(block.DumpFilterString());
        }
        sb.AppendLine();
        return sb.ToString();
    }
}
