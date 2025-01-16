using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterCore.PoeData;
public class ItemClassDetails
{
    public string Name { get; set; }

    
   
}

public class ItemTypeDetails
{
    public string Name { get; set; }
    public string Tip { get; set; }


    private int _selections;
    public int GetSelectionConunt()
    {
        return _selections;
    }

    public void AddSelectionCount()
    {
        _selections++;
    }

    public void ClearSelection()
    {
        _selections = 0;
    }

    public ItemTypeDetails()
    {
            
    }
}
