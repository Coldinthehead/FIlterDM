using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterDM.ViewModels;
public partial class ErrorViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _message;
}
