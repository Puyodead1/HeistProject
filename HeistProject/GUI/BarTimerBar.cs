// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.BarTimerBar
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using NativeUI;
using System.Drawing;

namespace HeistProject.GUI
{
  public class BarTimerBar : TimerBarBase
  {
    public string Text { get; set; }

    public float Percentage { get; set; }

    public Color BackgroundColor { get; set; }

    public Color ForegroundColor { get; set; }

    public BarTimerBar(string label)
      : base(label)
    {
      this.BackgroundColor = Color.DarkRed;
      this.ForegroundColor = Color.Red;
    }

    public override void Draw(int interval)
    {
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      Point safezoneBounds = UIMenu.GetSafezoneBounds();
      base.Draw(interval);
      Point pos = new Point((int) resolutionMantainRatio.Width - safezoneBounds.X - 160, (int) resolutionMantainRatio.Height - safezoneBounds.Y - (28 + 4 * interval));
      ((UIRectangle) new UIResRectangle(pos, new Size(150, 15), this.BackgroundColor)).Draw();
      ((UIRectangle) new UIResRectangle(pos, new Size((int) (150.0 * (double) this.Percentage), 15), this.ForegroundColor)).Draw();
    }
  }
}
