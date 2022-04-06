// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.CategorySelectionMenu
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: D:\Steam\steamapps\common\Grand Theft Auto V\Scripts\HeistProject.dll

using Microsoft.CSharp.RuntimeBinder;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace HeistProject.GUI
{
    public class CategorySelectionMenu
    {
        public UIMenu ParentMenu;
        private bool HasBuiltItems;
        private Point _offset;

        public Dictionary<string, string[]> Items { get; private set; }

        public string ItemName { get; set; }

        public string CurrentSelectedItem { get; set; }

        public string CurrentSelectedCategory { get; set; }

        public int ItemOffset { get; set; }

        public event EventHandler SelectionChanged;

        public CategorySelectionMenu(
          Dictionary<string, string[]> items,
          string itemName,
          ref UIMenu parent,
          int itemOffset,
          Point offset)
        {
            this.Items = items;
            this.ItemName = itemName;
            this.ParentMenu = parent;
            this.ItemOffset = itemOffset;
            this._offset = offset;
        }

        public virtual void Build() => this.Build(this.Items.ElementAt<KeyValuePair<string, string[]>>(0).Key);

        public virtual void Build(string type)
        {
            if (this.HasBuiltItems)
            {
                this.ParentMenu.MenuItems.RemoveAt(this.ItemOffset + 1);
                this.ParentMenu.MenuItems.RemoveAt(this.ItemOffset);
            }
            this.HasBuiltItems = true;
            List<object> list = this.Items.Select<KeyValuePair<string, string[]>, string>((Func<KeyValuePair<string, string[]>, string>)(pair => pair.Key)).Cast<object>().ToList<object>();
            UIMenuListItem uiMenuListItem = new UIMenuListItem(this.ItemName + " Category", list, list.FindIndex(n => n.ToString() == type));
            uiMenuListItem.Offset = this._offset;
            uiMenuListItem.Parent = this.ParentMenu;
            uiMenuListItem.Position(this.ParentMenu.MenuItems.Count * 25 - 37);
            this.ParentMenu.MenuItems.Insert(this.ItemOffset, (UIMenuItem)uiMenuListItem);
            uiMenuListItem.OnListChanged += (sender, index) =>
            {
                EventHandler selectionChanged = this.SelectionChanged;
                if (selectionChanged == null)
                    return;
                selectionChanged((object)this, EventArgs.Empty);
            };
            UIMenuListItem itemListItem = new UIMenuListItem(this.ItemName, ((IEnumerable<string>)this.Items[type]).Select<string, object>((Func<string, object>)(s => (object)s)).ToList<object>(), 0);
            itemListItem.Offset = this._offset;
            itemListItem.Parent = this.ParentMenu;
            itemListItem.Position(this.ParentMenu.MenuItems.Count * 25 - 37);
            this.ParentMenu.MenuItems.Insert(this.ItemOffset + 1, (UIMenuItem)itemListItem);
            this.CurrentSelectedItem = (string)itemListItem.IndexToItem(0);
            this.CurrentSelectedCategory = type;
            itemListItem.OnListChanged += (sender, index) =>
            {
                this.CurrentSelectedItem = (string)itemListItem.IndexToItem(index);
                EventHandler selectionChanged = this.SelectionChanged;
                if (selectionChanged == null)
                    return;
                selectionChanged((object)this, EventArgs.Empty);
            };
            itemListItem.Activated += (ItemActivatedEvent)((sender, item) =>
            {
                ItemActivatedEvent itemSelected = this.ItemSelected;
                if (itemSelected == null)
                    return;
                itemSelected(this.ParentMenu, (UIMenuItem)itemListItem);
            });
        }

        public event ItemActivatedEvent ItemSelected;
    }
}
