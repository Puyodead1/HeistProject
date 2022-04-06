// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.MissionPassedScreen
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Native;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace HeistProject.GUI
{
  public class MissionPassedScreen
  {
    private List<Tuple<string, string, MissionPassedScreen.TickboxState>> _items = new List<Tuple<string, string, MissionPassedScreen.TickboxState>>();
    private int _completionRate;
    private MissionPassedScreen.Medal _medal;

    public string Title { get; set; }

    public bool Visible { get; set; }

    public bool IsHeist { get; set; }

    public MissionPassedScreen(
      string title,
      int completionRate,
      MissionPassedScreen.Medal medal,
      bool heist)
    {
      this.Title = title;
      this._completionRate = completionRate;
      this._medal = medal;
      this.IsHeist = heist;
      this.Visible = false;
    }

    public void AddItem(string label, string status, MissionPassedScreen.TickboxState state) => this._items.Add(new Tuple<string, string, MissionPassedScreen.TickboxState>(label, status, state));

    public void Show()
    {
      this.Visible = true;
      Function.Call(Hash._START_SCREEN_EFFECT, (InputArgument) "DeathFailOut", (InputArgument) -1, (InputArgument) 1);
      while (!Game.IsControlJustPressed(0, Control.FrontendAccept))
      {
        SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
        int int32 = Convert.ToInt32(resolutionMantainRatio.Width / 2f);
        new Sprite("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(resolutionMantainRatio.Width), 450 + this._items.Count * 40), 0.0f, Color.FromArgb(200, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)).Draw();
        ((UIText) new UIResText((this.IsHeist ? "heist" : "setup") + " passed", new Point(int32, 100), 2.5f, Color.FromArgb((int) byte.MaxValue, 199, 168, 87), GTA.Font.Pricedown, UIResText.Alignment.Centered)).Draw();
        ((UIText) new UIResText(this.Title, new Point(int32, 230), 0.5f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Centered)).Draw();
        ((UIRectangle) new UIResRectangle(new Point(int32 - 300, 290), new Size(600, 2), Color.White)).Draw();
        for (int index = 0; index < this._items.Count; ++index)
        {
          ((UIText) new UIResText(this._items[index].Item1, new Point(int32 - 230, 300 + 40 * index), 0.35f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Left)).Draw();
          ((UIText) new UIResText(this._items[index].Item2, new Point(this._items[index].Item3 == MissionPassedScreen.TickboxState.None ? int32 + 265 : int32 + 230, 300 + 40 * index), 0.35f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Right)).Draw();
          if (this._items[index].Item3 != MissionPassedScreen.TickboxState.None)
          {
            string textureName = "shop_box_blank";
            switch (this._items[index].Item3)
            {
              case MissionPassedScreen.TickboxState.Tick:
                textureName = "shop_box_tick";
                break;
              case MissionPassedScreen.TickboxState.Cross:
                textureName = "shop_box_cross";
                break;
            }
            new Sprite("commonmenu", textureName, new Point(int32 + 230, 290 + 40 * index), new Size(48, 48)).Draw();
          }
        }
        ((UIRectangle) new UIResRectangle(new Point(int32 - 300, 300 + 40 * this._items.Count), new Size(600, 2), Color.White)).Draw();
        ((UIText) new UIResText("Completion", new Point(int32 - 150, 320 + 40 * this._items.Count), 0.4f)).Draw();
        ((UIText) new UIResText(this._completionRate.ToString() + "%", new Point(int32 + 150, 320 + 40 * this._items.Count), 0.4f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Right)).Draw();
        string textureName1 = "bronzemedal";
        switch (this._medal)
        {
          case MissionPassedScreen.Medal.Silver:
            textureName1 = "silvermedal";
            break;
          case MissionPassedScreen.Medal.Gold:
            textureName1 = "goldmedal";
            break;
        }
        new Sprite("mpmissionend", textureName1, new Point(int32 + 150, 320 + 40 * this._items.Count), new Size(32, 32)).Draw();
        Scaleform scaleform = new Scaleform(0);
        scaleform.Load("instructional_buttons");
        scaleform.CallFunction("CLEAR_ALL");
        scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", (object) 0);
        scaleform.CallFunction("CREATE_CONTAINER");
        scaleform.CallFunction("SET_DATA_SLOT", (object) 0, (object) Function.Call<string>(Hash._0x0499D7B09FC9B407, (InputArgument) 2, (InputArgument) 201, (InputArgument) 0), (object) "Continue");
        scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", (object) -1);
        scaleform.Render2D();
        Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, (InputArgument) 0);
        Function.Call(Hash.ENABLE_CONTROL_ACTION, (InputArgument) 0, (InputArgument) 201);
        Script.Yield();
      }
      Function.Call(Hash._STOP_ALL_SCREEN_EFFECTS);
    }

    public enum Medal
    {
      Bronze,
      Silver,
      Gold,
    }

    public enum TickboxState
    {
      None,
      Empty,
      Tick,
      Cross,
    }
  }
}
