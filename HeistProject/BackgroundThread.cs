// Decompiled with JetBrains decompiler
// Type: HeistProject.BackgroundThread
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

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
