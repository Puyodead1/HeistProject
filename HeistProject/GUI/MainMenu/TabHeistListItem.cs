// Decompiled with JetBrains decompiler

using GTA;
using GTA.Native;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace HeistProject.GUI.MainMenu
{
  public class TabHeistListItem : TabItem
  {
    protected const int MaxItemsPerView = 15;
    protected int _minItem;
    protected int _maxItem;

    public TabHeistListItem(string name, IEnumerable<HeistDefinition> list)
      : base(name)
    {
      this.FadeInWhenFocused = true;
      this.DrawBg = false;
      this._noLogo = new Sprite("gtav_online", "rockstarlogo256", new Point(), new Size(512, 256));
      this._maxItem = 15;
      this._minItem = 0;
      this.CanBeFocused = true;
      this.Heists = new List<HeistDefinition>(list);
    }

    public event HeistProject.GUI.MainMenu.OnItemSelect OnItemSelect;

    public List<HeistDefinition> Heists { get; set; }

    public int Index { get; set; }

    protected Sprite _noLogo { get; set; }

    public void ProcessControls()
    {
      if (!this.Focused)
        return;
      if (this.JustOpened)
      {
        this.JustOpened = false;
      }
      else
      {
        if (Game.IsControlJustPressed(0, Control.PhoneSelect))
        {
          Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
          HeistProject.GUI.MainMenu.OnItemSelect onItemSelect = this.OnItemSelect;
          if (onItemSelect != null)
            onItemSelect(this.Heists[this.Index]);
        }
        if (Game.IsControlJustPressed(0, Control.FrontendUp) || Game.IsControlJustPressed(0, Control.MoveUpOnly))
        {
          this.Index = (1000 - 1000 % this.Heists.Count + this.Index - 1) % this.Heists.Count;
          Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
          if (this.Heists.Count <= 15)
            return;
          if (this.Index < this._minItem)
          {
            --this._minItem;
            --this._maxItem;
          }
          if (this.Index != this.Heists.Count - 1)
            return;
          this._minItem = this.Heists.Count - 15;
          this._maxItem = this.Heists.Count;
        }
        else
        {
          if (!Game.IsControlJustPressed(0, Control.FrontendDown) && !Game.IsControlJustPressed(0, Control.MoveDownOnly))
            return;
          this.Index = (1000 - 1000 % this.Heists.Count + this.Index + 1) % this.Heists.Count;
          Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", 1);
          if (this.Heists.Count <= 15)
            return;
          if (this.Index >= this._maxItem)
          {
            ++this._maxItem;
            ++this._minItem;
          }
          if (this.Index != 0)
            return;
          this._minItem = 0;
          this._maxItem = 15;
        }
      }
    }

    public override void Draw()
    {
      this.ProcessControls();
      base.Draw();
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      Size size1 = new Size((int) (resolutionMantainRatio.Width - (float) (this.SafeSize.X * 2)) - 515, 40);
      int alpha1 = this.Focused ? 120 : 30;
      int alpha2 = this.Focused ? 200 : 100;
      int alpha3 = this.Focused ? (int) byte.MaxValue : 150;
      int num1 = 0;
      for (int minItem = this._minItem; minItem < Math.Min(this.Heists.Count, this._maxItem); ++minItem)
      {
        ((UIRectangle) new UIResRectangle(this.SafeSize.AddPoints(new Point(0, (size1.Height + 3) * num1)), size1, this.Index != minItem || !this.Focused ? Color.FromArgb(alpha2, Color.Black) : Color.FromArgb(alpha3, Color.White))).Draw();
        ((UIText) new UIResText(this.Heists[minItem].Name, this.SafeSize.AddPoints(new Point(6, 5 + (size1.Height + 3) * num1)), 0.35f, Color.FromArgb(alpha3, this.Index != minItem || !this.Focused ? Color.White : Color.Black))).Draw();
        ++num1;
      }
      Point safeSize;
      if (string.IsNullOrEmpty(this.Heists[this.Index].Logo) || !this.Heists[this.Index].AssetTranslation.ContainsKey(this.Heists[this.Index].Logo))
      {
        this._noLogo.Position = new Point((int) resolutionMantainRatio.Width - this.SafeSize.X - 512, this.SafeSize.Y);
        this._noLogo.Color = Color.FromArgb(alpha2, 0, 0, 0);
        this._noLogo.Draw();
      }
      else
      {
        string path = this.Heists[this.Index].AssetTranslation[this.Heists[this.Index].Logo];
        int x = (int) resolutionMantainRatio.Width - this.SafeSize.X - 512;
        safeSize = this.SafeSize;
        int y = safeSize.Y;
        Point position = new Point(x, y);
        Size size2 = new Size(512, 256);
        Sprite.DrawTexture(path, position, size2);
      }
      int width1 = (int) resolutionMantainRatio.Width;
      safeSize = this.SafeSize;
      int x1 = safeSize.X;
      int x2 = width1 - x1 - 512;
      safeSize = this.SafeSize;
      int y1 = safeSize.Y + 256;
      ((UIRectangle) new UIResRectangle(new Point(x2, y1), new Size(512, 40), Color.FromArgb(alpha3, Color.Black))).Draw();
      string name = this.Heists[this.Index].Name;
      int width2 = (int) resolutionMantainRatio.Width;
      safeSize = this.SafeSize;
      int x3 = safeSize.X;
      int x4 = width2 - x3 - 4;
      safeSize = this.SafeSize;
      int y2 = safeSize.Y + 260;
      Point point1 = new Point(x4, y2);
      Color color1 = Color.FromArgb(alpha3, Color.White);
      ((UIText) new UIResText(name, point1, 0.5f, color1, GTA.Font.HouseScript, UIResText.Alignment.Right)).Draw();
      int num2 = 6;
      for (int index = 0; index < num2; ++index)
      {
        int width3 = (int) resolutionMantainRatio.Width;
        safeSize = this.SafeSize;
        int x5 = safeSize.X;
        int x6 = width3 - x5 - 512;
        safeSize = this.SafeSize;
        int y3 = safeSize.Y + 256 + 40 + 40 * index;
        ((UIRectangle) new UIResRectangle(new Point(x6, y3), new Size(512, 40), index % 2 == 0 ? Color.FromArgb(alpha1, 0, 0, 0) : Color.FromArgb(alpha2, 0, 0, 0))).Draw();
        string str1 = "";
        string str2 = "";
        int num3;
        switch (index)
        {
          case 0:
            str1 = "Author";
            str2 = this.Heists[this.Index].Author ?? "Unknown";
            break;
          case 1:
            str1 = "Players";
            num3 = this.Heists[this.Index].MaxCapacity + 1;
            str2 = num3.ToString();
            break;
          case 2:
            str1 = "Setups";
            num3 = this.Heists[this.Index].Setups.Count;
            str2 = num3.ToString();
            break;
          case 3:
            str1 = "Setup Cost";
            num3 = this.Heists[this.Index].SetupCost;
            str2 = num3.ToString("C0");
            break;
          case 4:
            str1 = "Potential Payout";
            num3 = this.Heists[this.Index].Payout;
            str2 = num3.ToString("C0");
            break;
          case 5:
            str1 = "Type";
            str2 = "Heist";
            break;
        }
        string caption = str1;
        int width4 = (int) resolutionMantainRatio.Width;
        safeSize = this.SafeSize;
        int x7 = safeSize.X;
        int x8 = width4 - x7 - 506;
        safeSize = this.SafeSize;
        int y4 = safeSize.Y + 260 + 42 + 40 * index;
        Point position = new Point(x8, y4);
        Color color2 = Color.FromArgb(alpha3, Color.White);
        ((UIText) new UIResText(caption, position, 0.35f, color2)).Draw();
        string str3 = str2;
        int width5 = (int) resolutionMantainRatio.Width;
        safeSize = this.SafeSize;
        int x9 = safeSize.X;
        int x10 = width5 - x9 - 6;
        safeSize = this.SafeSize;
        int y5 = safeSize.Y + 260 + 42 + 40 * index;
        Point point2 = new Point(x10, y5);
        Color color3 = Color.FromArgb(alpha3, Color.White);
        ((UIText) new UIResText(str3, point2, 0.35f, color3, GTA.Font.ChaletLondon, UIResText.Alignment.Right)).Draw();
      }
      int width6 = (int) resolutionMantainRatio.Width;
      safeSize = this.SafeSize;
      int x11 = safeSize.X;
      int x12 = width6 - x11 - 512;
      safeSize = this.SafeSize;
      int y6 = safeSize.Y + 256 + 42 + 40 * num2;
      ((UIRectangle) new UIResRectangle(new Point(x12, y6), new Size(512, 2), Color.FromArgb(alpha3, Color.White))).Draw();
      string description = this.Heists[this.Index].Description;
      int width7 = (int) resolutionMantainRatio.Width;
      safeSize = this.SafeSize;
      int x13 = safeSize.X;
      int x14 = width7 - x13 - 508;
      safeSize = this.SafeSize;
      int y7 = safeSize.Y + 256 + 45 + 40 * num2 + 4;
      Point position1 = new Point(x14, y7);
      Color color4 = Color.FromArgb(alpha3, Color.White);
      ((UIText) new UIResText(description, position1, 0.35f, color4)
      {
        WordWrap = new Size(508, 0)
      }).Draw();
      int width8 = (int) resolutionMantainRatio.Width;
      safeSize = this.SafeSize;
      int x15 = safeSize.X;
      int x16 = width8 - x15 - 512;
      safeSize = this.SafeSize;
      int y8 = safeSize.Y + 256 + 44 + 40 * num2;
      ((UIRectangle) new UIResRectangle(new Point(x16, y8), new Size(512, 45 * (StringMeasurer.MeasureString(this.Heists[this.Index].Description) / 500)), Color.FromArgb(alpha2, 0, 0, 0))).Draw();
    }
  }
}
