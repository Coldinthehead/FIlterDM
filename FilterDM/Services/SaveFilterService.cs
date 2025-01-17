using FilterDM.Models;
using FilterDM.ViewModels.EditPage;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.Services;
public class SaveFilterService
{
    public void SaveModel(FilterModel model, ObservableCollection<BlockDetailsViewModel> blocks)
    {
        model.LastSaveDate = DateTime.Now;
        model.Blocks.Clear();
        foreach (BlockDetailsViewModel block in blocks)
        {
            model.AddBlock(block.GetModel());
        }
    }
}
