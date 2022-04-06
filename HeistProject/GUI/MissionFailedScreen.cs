// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.MissionFailedScreen
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Native;
using NativeUI;
using System;
using System.Drawing;

namespace HeistProject.GUI
{
  public class MissionFailedScreen
  {
    public string Reason { get; set; }

    public bool Visible { get; set; }

    public bool Setup { get; set; }

    public MissionFailedScreen(string reason, bool setup)
    {
      this.Reason = reason;
      this.Setup = setup;
      this.Visible = false;
    }

    public void Show()
    {
      Function.Call(Hash._START_SCREEN_EFFECT, (InputArgument) "DeathFailOut", (InputArgument) -1, (InputArgument) 1);
      while (!Game.IsControlJustPressed(0, Control.FrontendAccept))
      {
        Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, (InputArgument) 0);
        Function.Call(Hash.ENABLE_CONTROL_ACTION, (InputArgument) 0, (InputArgument) 201);
        SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
        int int32 = Convert.ToInt32(resolutionMantainRatio.Width / 2f);
        new Sprite("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(resolutionMantainRatio.Width), 300), 0.0f, Color.FromArgb(230, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)).Draw();
        ((UIText) new UIResText((this.Setup ? "setup" : "heist") + " failed", new Point(int32, 100), 2.5f, Color.FromArgb((int) byte.MaxValue, 148, 27, 46), GTA.Font.Pricedown, UIResText.Alignment.Centered)).Draw();
        ((UIText) new UIResText(this.Reason, new Point(int32, 230), 0.5f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Centered)).Draw();
        Scaleform scaleform = new Scaleform(0);
        scaleform.Load("instructional_buttons");
        scaleform.CallFunction("CLEAR_ALL");
        scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", (object) 0);
        scaleform.CallFunction("CREATE_CONTAINER");
        scaleform.CallFunction("SET_DATA_SLOT", (object) 0, (object) Function.Call<string>(Hash._0x0499D7B09FC9B407, (InputArgument) 2, (InputArgument) 201, (InputArgument) 0), (object) "Continue");
        scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", (object) -1);
        scaleform.Render2D();
        Script.Yield();
      }
      Game.PlaySound("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
      Function.Call(Hash._STOP_ALL_SCREEN_EFFECTS);
    }
  }
}
