// Decompiled with JetBrains decompiler

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
