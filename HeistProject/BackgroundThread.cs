// Decompiled with JetBrains decompiler

using GTA;
using NativeUI;
using System;
using System.Drawing;

namespace HeistProject
{
  public class BackgroundThread : Script
  {
    public static bool DrawLoadingIcon;
    private float _heading;

    public BackgroundThread() => this.Tick += (EventHandler) ((sender, args) =>
    {
      if (BackgroundThread.DrawLoadingIcon)
      {
        SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
        new Sprite("busy_spinner", "gtav_savingspinner_ib", new Point((int) ((double) resolutionMantainRatio.Width / 2.0) - 32, (int) ((double) resolutionMantainRatio.Height / 2.0) - 32), new Size(64, 64), this._heading, Color.White).Draw();
        this._heading += 6f;
      }
      if (BackgroundThread.CurrentScriptedLogic == null)
        return;
      BackgroundThread.CurrentScriptedLogic.BackgroundThreadUpdate();
    });

    public static ScriptedLogic CurrentScriptedLogic { get; set; }
  }
}
