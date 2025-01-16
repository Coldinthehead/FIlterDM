using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterDM.ViewModels.EditPage.Decorators;
public class TextSizeDecoratorViewModel : ModifierViewModelBase
{
    public TextSizeDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
    }
}
