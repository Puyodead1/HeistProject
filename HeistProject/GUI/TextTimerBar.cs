// Decompiled with JetBrains decompiler

using GTA;
using NativeUI;
using System.Drawing;

namespace HeistProject.GUI
{
  public class TextTimerBar : TimerBarBase
  {
    public string Text { get; set; }

    public TextTimerBar(string label, string text)
      : base(label)
    {
      this.Text = text;
    }

    public override void Draw(int interval)
    {
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      Point safezoneBounds = UIMenu.GetSafezoneBounds();
      base.Draw(interval);
      ((UIText) new UIResText(this.Text, new Point((int) resolutionMantainRatio.Width - safezoneBounds.X - 10, (int) resolutionMantainRatio.Height - safezoneBounds.Y - (42 + 4 * interval)), 0.5f, Color.White, GTA.Font.ChaletLondon, UIResText.Alignment.Right)).Draw();
    }
  }
}
