// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.CategorySelectionMenu
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

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
      List<object> list = this.Items.Select<KeyValuePair<string, string[]>, string>((Func<KeyValuePair<string, string[]>, string>) (pair => pair.Key)).Cast<object>().ToList<object>();
      UIMenuListItem uiMenuListItem = new UIMenuListItem(this.ItemName + " Category", list, list.FindIndex((Predicate<object>) (n =>
      {
        // ISSUE: reference to a compiler-generated field
        if (CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (CategorySelectionMenu)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p2 = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (CategorySelectionMenu), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string, object> target2 = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string, object>> p1 = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (CategorySelectionMenu), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__0.Target((CallSite) CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__0, n);
        string str = type;
        object obj2 = target2((CallSite) p1, obj1, str);
        return target1((CallSite) p2, obj2);
      })));
      uiMenuListItem.Offset = this._offset;
      uiMenuListItem.Parent = this.ParentMenu;
      uiMenuListItem.Position(this.ParentMenu.MenuItems.Count * 25 - 37);
      this.ParentMenu.MenuItems.Insert(this.ItemOffset, (UIMenuItem) uiMenuListItem);
      uiMenuListItem.OnListChanged += (ItemListEvent) ((sender, index) =>
      {
        // ISSUE: reference to a compiler-generated field
        if (CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (CategorySelectionMenu)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p4 = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (CategorySelectionMenu), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__3.Target((CallSite) CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__3, sender.IndexToItem(sender.Index));
        this.Build(target((CallSite) p4, obj));
        EventHandler selectionChanged = this.SelectionChanged;
        if (selectionChanged == null)
          return;
        selectionChanged((object) this, EventArgs.Empty);
      });
      UIMenuListItem itemListItem = new UIMenuListItem(this.ItemName, ((IEnumerable<string>) this.Items[type]).Select<string, object>((Func<string, object>) (s => (object) s)).ToList<object>(), 0);
      itemListItem.Offset = this._offset;
      itemListItem.Parent = this.ParentMenu;
      itemListItem.Position(this.ParentMenu.MenuItems.Count * 25 - 37);
      this.ParentMenu.MenuItems.Insert(this.ItemOffset + 1, (UIMenuItem) itemListItem);
      // ISSUE: reference to a compiler-generated field
      if (CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (CategorySelectionMenu)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.CurrentSelectedItem = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__5.Target((CallSite) CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__5, itemListItem.IndexToItem(0));
      this.CurrentSelectedCategory = type;
      itemListItem.OnListChanged += (ItemListEvent) ((sender, index) =>
      {
        // ISSUE: reference to a compiler-generated field
        if (CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (CategorySelectionMenu)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.CurrentSelectedItem = CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__6.Target((CallSite) CategorySelectionMenu.\u003C\u003Eo__28.\u003C\u003Ep__6, itemListItem.IndexToItem(index));
        EventHandler selectionChanged = this.SelectionChanged;
        if (selectionChanged == null)
          return;
        selectionChanged((object) this, EventArgs.Empty);
      });
      itemListItem.Activated += (ItemActivatedEvent) ((sender, item) =>
      {
        ItemActivatedEvent itemSelected = this.ItemSelected;
        if (itemSelected == null)
          return;
        itemSelected(this.ParentMenu, (UIMenuItem) itemListItem);
      });
    }

    public event ItemActivatedEvent ItemSelected;
  }
}
