// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.TimerBarBase
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using NativeUI;
using System.Drawing;

namespace HeistProject.GUI
{
  public abstract class TimerBarBase
  {
    public string Label { get; set; }

    public TimerBarBase(string label) => this.Label = label;

    public virtual void Draw(int interval)
    {
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      Point safezoneBounds = UIMenu.GetSafezoneBounds();
      ((UIText) new UIResText(this.Label, new Point((int) resolutionMantainRatio.Width - safezoneBounds.X - 180, (int) resolutionMantainRatio.Height - safezoneBounds.Y - (30 + 4 * interval)), 0.3f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Right)).Draw();
      new Sprite("timerbars", "all_black_bg", new Point((int) resolutionMantainRatio.Width - safezoneBounds.X - 298, (int) resolutionMantainRatio.Height - safezoneBounds.Y - (40 + 4 * interval)), new Size(300, 37), 0.0f, Color.FromArgb(180, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)).Draw();
      UI.HideHudComponentThisFrame((HudComponent) 7);
      UI.HideHudComponentThisFrame((HudComponent) 9);
      UI.HideHudComponentThisFrame((HudComponent) 6);
    }
  }
}
