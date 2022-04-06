// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.TextTimerBar
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

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
