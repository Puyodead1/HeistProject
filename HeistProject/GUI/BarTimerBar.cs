// Decompiled with JetBrains decompiler

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
