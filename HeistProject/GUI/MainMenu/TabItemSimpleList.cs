// Decompiled with JetBrains decompiler

using GTA;
using NativeUI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HeistProject.GUI.MainMenu
{
  public class TabItemSimpleList : TabItem
  {
    public TabItemSimpleList(string title, System.Collections.Generic.Dictionary<string, string> dict)
      : base(title)
    {
      this.Dictionary = dict;
      this.DrawBg = false;
    }

    public System.Collections.Generic.Dictionary<string, string> Dictionary { get; set; }

    public override void Draw()
    {
      base.Draw();
      UIMenu.GetScreenResolutionMantainRatio();
      int alpha1 = this.Focused || !this.CanBeFocused ? 180 : 60;
      int alpha2 = this.Focused || !this.CanBeFocused ? 200 : 90;
      int alpha3 = this.Focused || !this.CanBeFocused ? (int) byte.MaxValue : 150;
      int x1 = this.BottomRight.X;
      Point point1 = this.TopLeft;
      int x2 = point1.X;
      int width = x1 - x2;
      for (int index = 0; index < this.Dictionary.Count; ++index)
      {
        point1 = this.TopLeft;
        int x3 = point1.X;
        point1 = this.TopLeft;
        int y1 = point1.Y + 40 * index;
        ((UIRectangle) new UIResRectangle(new Point(x3, y1), new Size(width, 40), index % 2 == 0 ? Color.FromArgb(alpha1, 0, 0, 0) : Color.FromArgb(alpha2, 0, 0, 0))).Draw();
        KeyValuePair<string, string> keyValuePair = this.Dictionary.ElementAt<KeyValuePair<string, string>>(index);
        string key = keyValuePair.Key;
        point1 = this.TopLeft;
        int x4 = point1.X + 6;
        point1 = this.TopLeft;
        int y2 = point1.Y + 5 + 40 * index;
        Point position = new Point(x4, y2);
        Color color1 = Color.FromArgb(alpha3, Color.White);
        ((UIText) new UIResText(key, position, 0.35f, color1)).Draw();
        string str = keyValuePair.Value;
        point1 = this.BottomRight;
        int x5 = point1.X - 6;
        point1 = this.TopLeft;
        int y3 = point1.Y + 5 + 40 * index;
        Point point2 = new Point(x5, y3);
        Color color2 = Color.FromArgb(alpha3, Color.White);
        ((UIText) new UIResText(str, point2, 0.35f, color2, GTA.Font.ChaletLondon, UIResText.Alignment.Right)).Draw();
      }
    }
  }
}
