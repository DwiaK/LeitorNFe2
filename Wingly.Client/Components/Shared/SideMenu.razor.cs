using Microsoft.AspNetCore.Components;
using MudBlazor;
using LeitorNFe.App.Models;
using LeitorNFe.App.Models.SideMenu;
using System.Collections.Generic;

namespace LeitorNFe.App.Components.Shared;

public partial class SideMenu
{
    private List<MenuSectionModel> _menuSections = new()
    {
        new MenuSectionModel
        {
            Title = "Menu",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    Title = "Início",
                    Icon = Icons.Material.Filled.Home,
                    Href = "/"
                }
            }
        },

        new MenuSectionModel
        {
            Title = "Nota Fiscal",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    IsParent = true,
                    Title = "Importação",
                    Icon = Icons.Material.Filled.Build,
                    PageStatus = PageStatus.Concluido,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Manual",
                            Href = "/importacao/manual",
                            PageStatus = PageStatus.Concluido
                        },

                        new()
                        {
                            Title = "Automática",
                            Href = "/importacao/automatica",
                            PageStatus = PageStatus.Concluido
                        },
                    }
                }
            }
        },

        new MenuSectionModel
        {
            Title = "Informações",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    Title = "Social",
                    Icon = Icons.Material.Filled.PeopleAlt,
                    Href = "/social",
                    PageStatus = PageStatus.Concluido
                },
            }
        }
    };

    [EditorRequired][Parameter] public bool SideMenuDrawerOpen { get; set; }
    [EditorRequired][Parameter] public EventCallback<bool> SideMenuDrawerOpenChanged { get; set; }
    [EditorRequired][Parameter] public bool CanMiniSideMenuDrawer { get; set; }
    [EditorRequired][Parameter] public EventCallback<bool> CanMiniSideMenuDrawerChanged { get; set; }
}