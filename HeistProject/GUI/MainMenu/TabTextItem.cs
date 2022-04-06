// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.MainMenu.TabTextItem
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using NativeUI;
using System.Drawing;

namespace HeistProject.GUI.MainMenu
{
  public class TabTextItem : TabItem
  {
    public string TextTitle { get; set; }

    public string Text { get; set; }

    public int WordWrap { get; set; }

    public TabTextItem(string name, string title)
      : base(name)
    {
      this.TextTitle = title;
    }

    public TabTextItem(string name, string title, string text)
      : base(name)
    {
      this.TextTitle = title;
      this.Text = text;
    }

    public override void Draw()
    {
      base.Draw();
      UIMenu.GetScreenResolutionMantainRatio();
      int alpha = this.Focused || !this.CanBeFocused ? (int) byte.MaxValue : 200;
      if (!string.IsNullOrEmpty(this.TextTitle))
        ((UIText) new UIResText(this.TextTitle, this.SafeSize.AddPoints(new Point(40, 20)), 1.5f, Color.FromArgb(alpha, Color.White))).Draw();
      if (string.IsNullOrEmpty(this.Text))
        return;
      int num;
      if (this.WordWrap != 0)
      {
        num = this.WordWrap;
      }
      else
      {
        Point point = this.BottomRight;
        int x1 = point.X;
        point = this.TopLeft;
        int x2 = point.X;
        num = x1 - x2 - 40;
      }
      int width = num;
      ((UIText) new UIResText(this.Text, this.SafeSize.AddPoints(new Point(40, 150)), 0.4f, Color.FromArgb(alpha, Color.White))
      {
        WordWrap = new Size(width, 0)
      }).Draw();
    }
  }
}
