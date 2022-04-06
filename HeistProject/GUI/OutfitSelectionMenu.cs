// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.OutfitSelectionMenu
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject.Data;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HeistProject.GUI
{
  public class OutfitSelectionMenu
  {
    public Camera MainCamera;
    public UIMenu MainMenu;
    public bool IsInMenu;
    private Ped[] _team;
    private Vector3[] _pedPos = new Vector3[4]
    {
      new Vector3(566.39f, -3124.4f, 17.77f),
      new Vector3(566.03f, -3122.74f, 17.77f),
      new Vector3(566.15f, -3125.28f, 17.77f),
      new Vector3(566.73f, -3123.43f, 17.77f)
    };
    private bool _matchOutfits;
    private bool _matchMasks;
    private float _pedRot = 275.29f;
    private Vector3 _camPos = new Vector3(568.96f, -3124.21f, 18.77f);

    public string Name { get; set; }

    public string Description { get; set; }

    public OutfitSelectionMenu(string name, string description, params Ped[] teammates)
    {
      this.IsInMenu = true;
      this.MainMenu = new UIMenu("", "", new Point(0, 40));
      this.MainMenu.SetBannerType(new UIResRectangle());
      this.MainMenu.MouseEdgeEnabled = false;
      this.MainMenu.MouseControlsEnabled = false;
      this.MainMenu.ResetKey(UIMenu.MenuControls.Back);
      this._team = teammates;
      this.Name = name;
      this.Description = description;
      for (int index = 0; index < this._team.Length; ++index)
      {
        this._team[index].Position = this._pedPos[index];
        this._team[index].Heading = this._pedRot;
        this._team[index].FreezePosition = true;
      }
      World.DestroyAllCameras();
      this.MainCamera = World.CreateCamera(this._camPos, new Vector3(), 60f);
      this.MainCamera.PointAt((Entity) this._team[0]);
      World.RenderingCamera = this.MainCamera;
      Game.Player.CanControlCharacter = false;
      this.PopulateMenu();
    }

    private void PopulateMenu()
    {
      this.MainMenu.Clear();
      CategorySelectionMenu cat1 = new CategorySelectionMenu(Outfits.ToDictionary(), "Outfit", ref this.MainMenu, 0, new Point(0, 40));
      cat1.Build();
      cat1.SelectionChanged += (EventHandler) ((sender, args) => this.SetTeamOutfit(Outfits.GetOutfit(cat1.CurrentSelectedCategory, cat1.CurrentSelectedItem), cat1.CurrentSelectedCategory));
      cat1.ItemSelected += (ItemActivatedEvent) ((sender, item) => this.SetPedOutfit(Game.Player.Character, Outfits.GetOutfit(cat1.CurrentSelectedCategory, cat1.CurrentSelectedItem)));
      this.SetTeamOutfit(Outfits.GetOutfit(cat1.CurrentSelectedCategory, cat1.CurrentSelectedItem), cat1.CurrentSelectedCategory);
      CategorySelectionMenu cat2 = new CategorySelectionMenu(Masks.ToDictionary(), "Mask", ref this.MainMenu, 2, new Point(0, 40));
      cat2.Build();
      cat2.SelectionChanged += (EventHandler) ((sender, args) => this.SetTeamMask(Masks.GetMask(cat2.CurrentSelectedCategory, cat2.CurrentSelectedItem), cat2.CurrentSelectedCategory));
      cat2.ItemSelected += (ItemActivatedEvent) ((sender, item) => this.SetPedMask(Game.Player.Character, Masks.GetMask(cat2.CurrentSelectedCategory, cat2.CurrentSelectedItem)));
      this.SetTeamMask(Masks.GetMask(cat2.CurrentSelectedCategory, cat2.CurrentSelectedItem), cat2.CurrentSelectedCategory);
      UIMenuCheckboxItem menuCheckboxItem1 = new UIMenuCheckboxItem("Match Outfits", false);
      menuCheckboxItem1.CheckboxEvent += (ItemCheckboxEvent) ((sender, @checked) => this._matchOutfits = @checked);
      this.MainMenu.AddItem((UIMenuItem) menuCheckboxItem1);
      UIMenuCheckboxItem menuCheckboxItem2 = new UIMenuCheckboxItem("Match Masks", false);
      menuCheckboxItem2.CheckboxEvent += (ItemCheckboxEvent) ((sender, @checked) => this._matchMasks = @checked);
      this.MainMenu.AddItem((UIMenuItem) menuCheckboxItem2);
      ColoredUIItem coloredUiItem = new ColoredUIItem("~w~Start", Color.DarkGreen, Color.ForestGreen);
      coloredUiItem.Activated += (ItemActivatedEvent) ((sender, item) =>
      {
        this.IsInMenu = false;
        Function.Call(Hash._START_SCREEN_EFFECT, (InputArgument) "MinigameTransitionIn", (InputArgument) 0, (InputArgument) true);
        Script.Wait(500);
        Game.FadeScreenOut(1000);
        Script.Wait(1000);
        Function.Call(Hash._STOP_SCREEN_EFFECT, (InputArgument) "MinigameTransitionIn");
        World.RenderingCamera = (Camera) null;
        Game.Player.CanControlCharacter = true;
        EventHandler selectionComplete = this.OnSelectionComplete;
        if (selectionComplete != null)
          selectionComplete((object) this, EventArgs.Empty);
        for (int index = 0; index < this._team.Length; ++index)
          this._team[index].FreezePosition = false;
      });
      this.MainMenu.AddItem((UIMenuItem) coloredUiItem);
      this.MainMenu.RefreshIndex();
      this.MainMenu.Visible = true;
    }

    private bool IsOutfitValid(Ped ped, TexturedOutfit fit)
    {
      if (Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, (InputArgument) ped.Handle, (InputArgument) Outfit.LOWER_ID, (InputArgument) fit.Lower, (InputArgument) fit.LowerTexture))
      {
        if (Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, (InputArgument) ped.Handle, (InputArgument) Outfit.UPPER_ID, (InputArgument) fit.Upper, (InputArgument) fit.UpperTexture))
        {
          if (Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, (InputArgument) ped.Handle, (InputArgument) Outfit.ACCESSORY_ID, (InputArgument) fit.Accessory, (InputArgument) fit.AccessoryTexture))
          {
            if (Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, (InputArgument) ped.Handle, (InputArgument) Outfit.SHIRT_OVERLAY_ID, (InputArgument) fit.ShirtOverlay, (InputArgument) fit.ShirtOverlayTexture))
              return Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, (InputArgument) ped.Handle, (InputArgument) Outfit.SHOES_ID, (InputArgument) fit.Shoes, (InputArgument) fit.ShoesTexture);
          }
        }
      }
      return false;
    }

    private void SetTeamOutfit(Outfit outfit, string category)
    {
      foreach (Ped ped in this._team)
      {
        if ((Entity) ped == (Entity) Game.Player.Character)
          this.SetPedOutfit(ped, outfit);
        else if (!this._matchOutfits)
          this.SetPedOutfit(ped, Outfits.GetRandomOutfitFromCategory(category));
        else
          this.SetPedOutfit(ped, outfit);
      }
    }

    private void SetPedOutfit(Ped ped, Outfit outfit)
    {
      if (outfit.StaticTexture)
      {
        Util.SetPedComponentVariation(ped, Outfit.UPPER_ID, outfit.Upper, 0);
        Util.SetPedComponentVariation(ped, Outfit.LOWER_ID, outfit.Lower, 0);
        Util.SetPedComponentVariation(ped, Outfit.ACCESSORY_ID, outfit.Accessory, 0);
        Util.SetPedComponentVariation(ped, Outfit.SHIRT_OVERLAY_ID, outfit.ShirtOverlay, 0);
        Util.SetPedComponentVariation(ped, Outfit.SHOES_ID, outfit.Shoes, 0);
      }
      else if (outfit.MatchColors)
      {
        int texture = Util.SharedRandom.Next(new List<int>()
        {
          Util.GetComponentTextureVariations(ped, Outfit.LOWER_ID, outfit.Lower),
          Util.GetComponentTextureVariations(ped, Outfit.ACCESSORY_ID, outfit.Accessory),
          Util.GetComponentTextureVariations(ped, Outfit.SHIRT_OVERLAY_ID, outfit.ShirtOverlay)
        }.Min());
        Util.SetPedComponentVariation(ped, Outfit.UPPER_ID, outfit.Upper, Util.SharedRandom.Next(Util.GetComponentTextureVariations(ped, Outfit.UPPER_ID, outfit.Upper)));
        Util.SetPedComponentVariation(ped, Outfit.LOWER_ID, outfit.Lower, texture);
        Util.SetPedComponentVariation(ped, Outfit.ACCESSORY_ID, outfit.Accessory, texture);
        Util.SetPedComponentVariation(ped, Outfit.SHIRT_OVERLAY_ID, outfit.ShirtOverlay, texture);
        Util.SetPedComponentVariation(ped, Outfit.SHOES_ID, outfit.Shoes, Util.SharedRandom.Next(Util.GetComponentTextureVariations(ped, Outfit.SHOES_ID, outfit.Shoes)));
      }
      else
      {
        int num = 0;
        int upperText;
        int lowerText;
        int accText;
        int overlayText;
        int shoesText;
        do
        {
          Util.SetPedComponentVariation(ped, Outfit.UPPER_ID, outfit.Upper, upperText = Util.SharedRandom.Next(Util.GetComponentTextureVariations(ped, Outfit.UPPER_ID, outfit.Upper)));
          Util.SetPedComponentVariation(ped, Outfit.LOWER_ID, outfit.Lower, lowerText = Util.SharedRandom.Next(Util.GetComponentTextureVariations(ped, Outfit.LOWER_ID, outfit.Lower)));
          Util.SetPedComponentVariation(ped, Outfit.ACCESSORY_ID, outfit.Accessory, accText = Util.SharedRandom.Next(Util.GetComponentTextureVariations(ped, Outfit.ACCESSORY_ID, outfit.Accessory)));
          Util.SetPedComponentVariation(ped, Outfit.SHIRT_OVERLAY_ID, outfit.ShirtOverlay, overlayText = Util.SharedRandom.Next(Util.GetComponentTextureVariations(ped, Outfit.SHIRT_OVERLAY_ID, outfit.ShirtOverlay)));
          Util.SetPedComponentVariation(ped, Outfit.SHOES_ID, outfit.Shoes, shoesText = Util.SharedRandom.Next(Util.GetComponentTextureVariations(ped, Outfit.SHOES_ID, outfit.Shoes)));
          ++num;
        }
        while (!this.IsOutfitValid(ped, new TexturedOutfit(outfit, upperText, lowerText, shoesText, accText, overlayText)) && num < 10);
      }
    }

    private void SetTeamMask(Mask mask, string category)
    {
      foreach (Ped ped in this._team)
      {
        if ((Entity) ped == (Entity) Game.Player.Character)
        {
          Util.SetPedMask(ped, mask.Id, mask.Texture == -1 ? Util.SharedRandom.Next(Util.GetMaskTextureVariations(ped, mask.Id)) : mask.Texture);
        }
        else
        {
          if (!this._matchMasks)
            mask = Masks.GetRandomMaskFromCategory(category);
          Util.SetPedMask(ped, mask.Id, mask.Texture == -1 ? Util.SharedRandom.Next(Util.GetMaskTextureVariations(ped, mask.Id)) : mask.Texture);
        }
      }
    }

    private void SetPedMask(Ped ped, Mask mask) => Util.SetPedMask(ped, mask.Id, mask.Texture == -1 ? Util.SharedRandom.Next(Util.GetMaskTextureVariations(ped, mask.Id)) : mask.Texture);

    public event EventHandler OnSelectionComplete;

    public void Update()
    {
      if (!this.IsInMenu)
        return;
      this.MainMenu.ProcessControl();
      this.MainMenu.ProcessMouse();
      this.MainMenu.Draw();
      Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
      Point safezoneBounds = UIMenu.GetSafezoneBounds();
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      ((UIText) new UIResText(this.Name, new Point(safezoneBounds.X, safezoneBounds.Y), 0.8f, Color.White, GTA.Font.ChaletComprimeCologne, UIResText.Alignment.Left)
      {
        Outline = true
      }).Draw();
      ((UIText) new UIResText(this.Description, new Point(safezoneBounds.X, 50 + safezoneBounds.Y), 0.4f, Color.White, GTA.Font.ChaletComprimeCologne, UIResText.Alignment.Left)
      {
        WordWrap = new Size(Convert.ToInt32(resolutionMantainRatio.Width) - safezoneBounds.X * 2, 0),
        Outline = true
      }).Draw();
      ((UIRectangle) new UIResRectangle(new Point(safezoneBounds.X + 435, safezoneBounds.Y + 107), new Size(1200, 37), Color.Green)).Draw();
      ((UIText) new UIResText("TEAM", new Point(safezoneBounds.X + 1000, safezoneBounds.Y + 110), 0.35f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Centered)).Draw();
      ((UIRectangle) new UIResRectangle(new Point(safezoneBounds.X, safezoneBounds.Y + 107), new Size(432, 37), Color.Green)).Draw();
      ((UIText) new UIResText("OPTIONS", new Point(safezoneBounds.X + 200, safezoneBounds.Y + 110), 0.35f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Centered)).Draw();
    }
  }
}
