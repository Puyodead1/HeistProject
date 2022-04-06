// Decompiled with JetBrains decompiler
// Type: HeistProject.CharacterCreator
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Native;
using NativeUI;
using System;
using System.Collections.Generic;

namespace HeistProject
{
  public class CharacterCreator
  {
    private MenuPool _pool;

    public CharacterCreator()
    {
      this._pool = new MenuPool();
      this.Create(Game.Player.Character);
    }

    public event CharacterEvent OnCharacterCreationComplete;

    private void CreateMenus()
    {
      UIMenu menu = new UIMenu("Character Creator", "MAIN MENU");
      this._pool.Add(menu);
      UIMenu uiMenu = new UIMenu("Character Creator", "EDIT FACE");
      this._pool.Add(uiMenu);
      UIMenuListItem uiMenuListItem = new UIMenuListItem("Gender", new List<object>()
      {
        (object) "Male",
        (object) "Female"
      }, 0);
      uiMenuListItem.OnListChanged += (ItemListEvent) ((sender, index) => this.SetPlayerModel(index == 0 ? new Model(PedHash.FreemodeMale01) : new Model(PedHash.FreemodeFemale01)));
      menu.AddItem((UIMenuItem) uiMenuListItem);
      UIMenuItem itemToBindTo = new UIMenuItem("Face");
      menu.AddItem(itemToBindTo);
      menu.BindMenuToItem(uiMenu, itemToBindTo);
      UIMenuItem uiMenuItem = new UIMenuItem("Complete");
      uiMenuItem.Activated += (ItemActivatedEvent) ((sender, item) =>
      {
        this._pool.CloseAllMenus();
        this.InvokeCharacterCreationComplete(CharacterData.GetPedCharacterData(Game.Player.Character));
      });
      menu.AddItem(uiMenuItem);
      this._pool.ToList().ForEach((Action<UIMenu>) (m => m.RefreshIndex()));
    }

    private void Create(Ped ped) => this.SetPlayerModel(new Model(PedHash.FreemodeMale01));

    public void Update() => this._pool.ProcessMenus();

    public void SetPlayerModel(Model model)
    {
      if (Game.Player.Character.Model.Hash == model.Hash)
        return;
      model.Request(10000);
      Function.Call(Hash.SET_PLAYER_MODEL, new InputArgument(Game.Player), (InputArgument) model.Hash);
    }

    protected virtual void InvokeCharacterCreationComplete(CharacterData data)
    {
      CharacterEvent creationComplete = this.OnCharacterCreationComplete;
      if (creationComplete == null)
        return;
      creationComplete(data);
    }
  }
}
