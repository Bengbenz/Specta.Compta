using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Client.ViewModels;

using Dawn;

namespace Beng.Specta.Compta.Client.Layout.Sidebar
{
    public class SidebarState
    {
        public static readonly int MobileWidth = 767;
        private ILogger<SidebarState> Logger { get; }
        private NavigationManager NavigationManager { get; }
        public bool IsMinimized { get; private set; }
        public event Action OnChange;

        public SidebarState(ILogger<SidebarState> logger, NavigationManager navigationManager)
        {
            Logger = logger;
            NavigationManager = navigationManager;
        }

        public void ToggleMinimized()
        {
            IsMinimized = !IsMinimized;
            NotifyStateChanged();
        }

        public void UpdateMinimized(bool value)
        {
            IsMinimized = value;
            NotifyStateChanged();
        }

        public void ToggleExpandedGroup(ModuleInfoVm menuItem)
        {
            Guard.Argument(menuItem, nameof(menuItem)).NotNull();

            menuItem.IsExpanded = !menuItem.IsExpanded;
            NotifyStateChanged();
        }

        public void SetActiveItem(ModuleInfoVm menuItem, ModuleInfoVm groupItem)
        {
            if (menuItem == null)
            {
                string currentUrlWithoutBase = NavigationManager.Uri.Substring(NavigationManager.BaseUri.Length - 1);

                menuItem = GeMenuItems().SingleOrDefault(x => x.Meta.Url.OriginalString ==  currentUrlWithoutBase);
                groupItem = GetGroupItems().SingleOrDefault(x => currentUrlWithoutBase.StartsWith(x.Meta.Url.OriginalString, StringComparison.InvariantCulture));
            }

            if (menuItem != null)
            {
                menuItem.IsActive = true;
            }
            var otherItems = GeMenuItems().Except(new List<ModuleInfoVm> { menuItem });
            foreach (var item in otherItems)
            {
                item.IsActive = false;
            }

            var otherGroups = GetGroupItems();
            if (groupItem != null)
            {
                groupItem.IsExpanded = true;
                otherGroups = otherGroups.Except(new List<ModuleInfoVm> { groupItem }).ToList();
            }
            foreach (var group in otherGroups)
            {
                group.IsExpanded = false;
            }

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        private IReadOnlyCollection<ModuleInfoVm> GetGroupItems() =>
            MenuItems.Where(x => x.Children.Any()).ToList();


        private IReadOnlyCollection<ModuleInfoVm> GeMenuItems()
        {
            var rootNodes = MenuItems.Where(x => !x.Children.Any());
            return rootNodes.Union(MenuItems.SelectMany(x => x.Children)).ToList();
        }

        public IReadOnlyCollection<ModuleInfoVm> MenuItems { get; } = new List<ModuleInfoVm>
        {
            new ModuleInfoVm("1", "Dashboard", new MetaInfoVm("/", "vuestic-iconset vuestic-iconset-dashboard")),
            new ModuleInfoVm("2", "Projets", new MetaInfoVm("/projects", "vuestic-iconset vuestic-iconset-statistics")),
            new ModuleInfoVm("3", "Budget", new MetaInfoVm("/budget", "vuestic-iconset vuestic-iconset-forms")),
            new ModuleInfoVm("4", "Finances", new MetaInfoVm("/funding", "vuestic-iconset vuestic-iconset-tables"))
            {
                Children = new List<ModuleInfoVm>
                {
                    new ModuleInfoVm("4-1", "Plan de Financement", new MetaInfoVm("/funding-plan")),
                    new ModuleInfoVm("4-2", "Contrats", new MetaInfoVm("/funding-contracts"))
                }
            },
            new ModuleInfoVm("5", "Comptabilité", new MetaInfoVm("/accounting", "vuestic-iconset vuestic-iconset-files"))
            {
                Children = new List<ModuleInfoVm>
                {
                    new ModuleInfoVm("5-1", "Intégration", new MetaInfoVm("/accounting-integration")),
                    new ModuleInfoVm("5-2", "Plan Analytique", new MetaInfoVm("/accounting-analytic-plan"))
                }
            },
            new ModuleInfoVm("6", "Trésorerie", new MetaInfoVm("/treasury", "vuestic-iconset vuestic-iconset-ui-elements")),
            new ModuleInfoVm("7", "Administration", new MetaInfoVm("/settings", "vuestic-iconset vuestic-iconset-settings"))
            {
                Children = new List<ModuleInfoVm>
                {
                    new ModuleInfoVm("7-1", "Utilisateurs", new MetaInfoVm("/settings-users")),
                    new ModuleInfoVm("7-2", "Rôles", new MetaInfoVm("/settings-roles")),
                    new ModuleInfoVm("7-3", "Permissions", new MetaInfoVm("/settings-permissions"))
                }
            }
        };
    }
}