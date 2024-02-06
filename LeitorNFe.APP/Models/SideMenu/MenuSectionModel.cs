using System.Collections.Generic;

namespace LeitorNFe.App.Models.SideMenu;

public class MenuSectionModel
{
    public string Title { get; set; }
    public List<MenuSectionItemModel> SectionItems { get; set; }
}