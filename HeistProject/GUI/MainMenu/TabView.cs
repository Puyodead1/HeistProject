// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.MainMenu.TabView
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Native;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace HeistProject.GUI.MainMenu
{
  public class TabView
  {
    public int Index;
    private bool _visible;
    private Scaleform _sc;

    public TabView(string title)
    {
      this.Title = title;
      this.Tabs = new List<TabItem>();
      this.Name = Game.Player.Name;
      this.IsControlInTabs = true;
    }

    public string Title { get; set; }

    public Sprite Photo { get; set; }

    public string Name { get; set; }

    public string Money { get; set; }

    public string MoneySubtitle { get; set; }

    public List<TabItem> Tabs { get; set; }

    public bool IsControlInTabs { get; set; }

    public event EventHandler OnMenuClose;

    public bool Visible
    {
      get => this._visible;
      set
      {
        this._visible = value;
        if (value)
          Function.Call(Hash._START_SCREEN_EFFECT, (InputArgument) "MinigameTransitionIn", (InputArgument) 0, (InputArgument) true);
        else
          Function.Call(Hash._STOP_SCREEN_EFFECT, (InputArgument) "MinigameTransitionIn");
      }
    }

    public void ProcessMouse()
    {
    }

    public void ShowInstructionalButtons()
    {
      if (this._sc == null)
      {
        this._sc = new Scaleform(0);
        this._sc.Load("instructional_buttons");
      }
      this._sc.CallFunction("CLEAR_ALL");
      this._sc.CallFunction("TOGGLE_MOUSE_BUTTONS", (object) 0);
      this._sc.CallFunction("CREATE_CONTAINER");
      this._sc.CallFunction("SET_DATA_SLOT", (object) 0, (object) Function.Call<string>(Hash._0x0499D7B09FC9B407, (InputArgument) 2, (InputArgument) 176, (InputArgument) 0), (object) "Select");
      this._sc.CallFunction("SET_DATA_SLOT", (object) 1, (object) Function.Call<string>(Hash._0x0499D7B09FC9B407, (InputArgument) 2, (InputArgument) 177, (InputArgument) 0), (object) "Back");
      this._sc.CallFunction("SET_DATA_SLOT", (object) 2, (object) Function.Call<string>(Hash._0x0499D7B09FC9B407, (InputArgument) 2, (InputArgument) 206, (InputArgument) 0), (object) "");
      this._sc.CallFunction("SET_DATA_SLOT", (object) 3, (object) Function.Call<string>(Hash._0x0499D7B09FC9B407, (InputArgument) 2, (InputArgument) 205, (InputArgument) 0), (object) "Browse");
    }

    public void ProcessControls()
    {
      Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, (InputArgument) 0);
      if (Game.IsControlJustPressed(0, Control.PhoneLeft) && this.IsControlInTabs)
      {
        this.Tabs[this.Index].Active = false;
        this.Tabs[this.Index].Focused = false;
        this.Tabs[this.Index].Visible = false;
        this.Index = (1000 - 1000 % this.Tabs.Count + this.Index - 1) % this.Tabs.Count;
        this.Tabs[this.Index].Active = true;
        this.Tabs[this.Index].Focused = false;
        this.Tabs[this.Index].Visible = true;
        Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "NAV_UP_DOWN", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
      }
      else if (Game.IsControlJustPressed(0, Control.PhoneRight) && this.IsControlInTabs)
      {
        this.Tabs[this.Index].Active = false;
        this.Tabs[this.Index].Focused = false;
        this.Tabs[this.Index].Visible = false;
        this.Index = (1000 - 1000 % this.Tabs.Count + this.Index + 1) % this.Tabs.Count;
        this.Tabs[this.Index].Active = true;
        this.Tabs[this.Index].Focused = false;
        this.Tabs[this.Index].Visible = true;
        Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "NAV_UP_DOWN", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
      }
      else if (Game.IsControlJustPressed(0, Control.FrontendAccept) && this.IsControlInTabs)
      {
        if (this.Tabs[this.Index].CanBeFocused)
        {
          this.Tabs[this.Index].Focused = true;
          this.Tabs[this.Index].JustOpened = true;
          this.IsControlInTabs = false;
        }
        else
        {
          this.Tabs[this.Index].JustOpened = true;
          this.Tabs[this.Index].OnActivated();
        }
        Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "SELECT", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
      }
      else if (Game.IsControlJustPressed(0, Control.PhoneCancel) && !this.IsControlInTabs)
      {
        this.Tabs[this.Index].Focused = false;
        this.IsControlInTabs = true;
        Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "BACK", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
      }
      else if (Game.IsControlJustPressed(0, Control.PhoneCancel) && this.IsControlInTabs)
      {
        this.Visible = false;
        Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "BACK", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
        EventHandler onMenuClose = this.OnMenuClose;
        if (onMenuClose == null)
          return;
        onMenuClose((object) this, EventArgs.Empty);
      }
      else if (Game.IsControlJustPressed(0, Control.FrontendLb))
      {
        this.Tabs[this.Index].Active = false;
        this.Tabs[this.Index].Focused = false;
        this.Tabs[this.Index].Visible = false;
        this.Index = (1000 - 1000 % this.Tabs.Count + this.Index - 1) % this.Tabs.Count;
        this.Tabs[this.Index].Active = true;
        this.Tabs[this.Index].Focused = false;
        this.Tabs[this.Index].Visible = true;
        this.IsControlInTabs = true;
        Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "NAV_UP_DOWN", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
      }
      else
      {
        if (!Game.IsControlJustPressed(0, Control.FrontendRb))
          return;
        this.Tabs[this.Index].Active = false;
        this.Tabs[this.Index].Focused = false;
        this.Tabs[this.Index].Visible = false;
        this.Index = (1000 - 1000 % this.Tabs.Count + this.Index + 1) % this.Tabs.Count;
        this.Tabs[this.Index].Active = true;
        this.Tabs[this.Index].Focused = false;
        this.Tabs[this.Index].Visible = true;
        this.IsControlInTabs = true;
        Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "NAV_UP_DOWN", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
      }
    }

    public void RefreshIndex()
    {
      this.Index = (1000 - 1000 % this.Tabs.Count) % this.Tabs.Count;
      this.Tabs[this.Index].Active = true;
      this.Tabs[this.Index].Focused = false;
      this.Tabs[this.Index].Visible = true;
    }

    public void Update()
    {
      if (!this.Visible)
        return;
      this.ShowInstructionalButtons();
      Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
      this.ProcessControls();
      this.ProcessMouse();
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      Point left = new Point(300, 180);
      ((UIText) new UIResText(this.Title, new Point(left.X, left.Y - 80), 1f, Color.White, GTA.Font.ChaletComprimeCologne, UIResText.Alignment.Left)
      {
        DropShadow = true
      }).Draw();
      if (this.Photo == null)
      {
        new Sprite("char_multiplayer", "char_multiplayer", new Point((int) resolutionMantainRatio.Width - left.X - 64, left.Y - 80), new Size(64, 64)).Draw();
      }
      else
      {
        this.Photo.Position = new Point((int) resolutionMantainRatio.Width - left.X - 100, left.Y - 80);
        this.Photo.Size = new Size(64, 64);
        this.Photo.Draw();
      }
      ((UIText) new UIResText(this.Name, new Point((int) resolutionMantainRatio.Width - left.X - 70, left.Y - 95), 0.7f, Color.White, GTA.Font.ChaletComprimeCologne, UIResText.Alignment.Right)
      {
        DropShadow = true
      }).Draw();
      string money = this.Money;
      if (string.IsNullOrEmpty(this.Money))
        money = DateTime.Now.ToString();
      ((UIText) new UIResText(money, new Point((int) resolutionMantainRatio.Width - left.X - 70, left.Y - 60), 0.4f, Color.White, GTA.Font.ChaletComprimeCologne, UIResText.Alignment.Right)
      {
        DropShadow = true
      }).Draw();
      ((UIText) new UIResText(this.MoneySubtitle, new Point((int) resolutionMantainRatio.Width - left.X - 70, left.Y - 40), 0.4f, Color.White, GTA.Font.ChaletComprimeCologne, UIResText.Alignment.Right)
      {
        DropShadow = true
      }).Draw();
      for (int index = 0; index < this.Tabs.Count; ++index)
      {
        int width = (int) ((double) resolutionMantainRatio.Width - (double) (2 * left.X) - 20.0) / 5;
        Color baseColor = this.Tabs[index].Active ? Color.White : Color.Black;
        ((UIRectangle) new UIResRectangle(left.AddPoints(new Point((width + 5) * index, 0)), new Size(width, 40), Color.FromArgb(this.Tabs[index].Active ? (int) byte.MaxValue : 200, baseColor))).Draw();
        ((UIText) new UIResText(this.Tabs[index].Title.ToUpper(), left.AddPoints(new Point(width / 2 + (width + 5) * index, 5)), 0.35f, this.Tabs[index].Active ? Color.Black : Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Centered)).Draw();
        if (this.Tabs[index].Active)
          ((UIRectangle) new UIResRectangle(left.SubtractPoints(new Point(-((width + 5) * index), 10)), new Size(width, 10), Color.DodgerBlue)).Draw();
      }
      this.Tabs[this.Index].Draw();
      this._sc.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", (object) -1);
      this._sc.Render2D();
    }
  }
}
