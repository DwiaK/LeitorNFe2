using MudBlazor;
using MudBlazor.Utilities;

namespace LeitorNFe.App.Components.Index;

public partial class TestCard : MudComponentBase
{
    private string Classname =>
        new CssBuilder()
            .AddClass(Class)
            .Build();
}