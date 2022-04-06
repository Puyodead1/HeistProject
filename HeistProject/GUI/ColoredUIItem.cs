// Decompiled with JetBrains decompiler

using GTA;
using NativeUI;
using System.Drawing;

namespace HeistProject.GUI
{
  public class ColoredUIItem : UIMenuItem
  {
    private new UIResRectangle _rectangle;
    private new UIResText _text;
    private new Sprite _selectedSprite;
    private new Sprite _badgeLeft;
    private new Sprite _badgeRight;
    private new UIResText _labelText;

    public Color MainColor { get; set; }

    public Color HighlighColor { get; set; }

    public ColoredUIItem(string label, Color color, Color highlightColor)
      : base(label)
    {
      this.MainColor = color;
      this.HighlighColor = highlightColor;
      this.Init();
    }

    public ColoredUIItem(string label, string description, Color color, Color highlightColor)
      : base(label, description)
    {
      this.MainColor = color;
      this.HighlighColor = highlightColor;
      this.Init();
    }

    private void Init()
    {
      this._selectedSprite = new Sprite("commonmenu", "gradient_nav", new Point(0, 0), new Size(431, 38), 0.0f, this.HighlighColor);
      this._rectangle = new UIResRectangle(new Point(0, 0), new Size(431, 38), Color.FromArgb(150, 0, 0, 0));
      this._text = new UIResText(this.Text, new Point(8, 0), 0.33f, Color.WhiteSmoke, GTA.Font.ChaletLondon, UIResText.Alignment.Left);
      this.Description = this.Description;
      this._badgeLeft = new Sprite("commonmenu", "", new Point(0, 0), new Size(40, 40));
      this._badgeRight = new Sprite("commonmenu", "", new Point(0, 0), new Size(40, 40));
      this._labelText = new UIResText("", new Point(0, 0), 0.35f)
      {
        TextAlignment = UIResText.Alignment.Right
      };
    }

    public override void Position(int y)
    {
      UIResRectangle rectangle = this._rectangle;
      Point offset = this.Offset;
      int x = offset.X;
      int num = y + 144;
      offset = this.Offset;
      int y1 = offset.Y;
      int y2 = num + y1;
      Point point = new Point(x, y2);
      ((UIRectangle) rectangle).Position = point;
      this._selectedSprite.Position = new Point(0 + this.Offset.X, y + 144 + this.Offset.Y);
      ((UIText) this._text).Position = new Point(8 + this.Offset.X, y + 147 + this.Offset.Y);
      this._badgeLeft.Position = new Point(0 + this.Offset.X, y + 142 + this.Offset.Y);
      this._badgeRight.Position = new Point(385 + this.Offset.X, y + 142 + this.Offset.Y);
      ((UIText) this._labelText).Position = new Point(420 + this.Offset.X, y + 148 + this.Offset.Y);
    }

    public override void Draw()
    {
      ((UIRectangle) this._rectangle).Size = new Size(431 + this.Parent.WidthOffset, 38);
      this._selectedSprite.Size = new Size(431 + this.Parent.WidthOffset, 38);
      if (this.Hovered && !this.Selected)
      {
        ((UIRectangle) this._rectangle).Color = Color.FromArgb(20, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        ((UIRectangle) this._rectangle).Draw();
      }
      if (this.Selected)
      {
        this._selectedSprite.Color = this.HighlighColor;
        this._selectedSprite.Draw();
      }
      else
      {
        this._selectedSprite.Color = this.MainColor;
        this._selectedSprite.Draw();
      }
      ((UIText) this._text).Color = this.Enabled ? (this.Selected ? Color.Black : Color.WhiteSmoke) : Color.FromArgb(163, 159, 148);
      if (this.LeftBadge != UIMenuItem.BadgeStyle.None)
      {
        ((UIText) this._text).Position = new Point(35 + this.Offset.X, ((UIText) this._text).Position.Y);
        this._badgeLeft.TextureDict = this.BadgeToSpriteLib(this.LeftBadge);
        this._badgeLeft.TextureName = this.BadgeToSpriteName(this.LeftBadge, this.Selected);
        this._badgeLeft.Color = this.BadgeToColor(this.LeftBadge, this.Selected);
        this._badgeLeft.Draw();
      }
      else
        ((UIText) this._text).Position = new Point(8 + this.Offset.X, ((UIText) this._text).Position.Y);
      if (this.RightBadge != UIMenuItem.BadgeStyle.None)
      {
        this._badgeRight.Position = new Point(385 + this.Offset.X + this.Parent.WidthOffset, this._badgeRight.Position.Y);
        this._badgeRight.TextureDict = this.BadgeToSpriteLib(this.RightBadge);
        this._badgeRight.TextureName = this.BadgeToSpriteName(this.RightBadge, this.Selected);
        this._badgeRight.Color = this.BadgeToColor(this.RightBadge, this.Selected);
        this._badgeRight.Draw();
      }
      if (!string.IsNullOrWhiteSpace(this.RightLabel))
      {
        ((UIText) this._labelText).Position = new Point(420 + this.Offset.X + this.Parent.WidthOffset, ((UIText) this._labelText).Position.Y);
        ((UIText) this._labelText).Caption = this.RightLabel;
        ((UIText) this._labelText).Color = ((UIText) this._text).Color = this.Enabled ? (this.Selected ? Color.Black : Color.WhiteSmoke) : Color.FromArgb(163, 159, 148);
        ((UIText) this._labelText).Draw();
      }
      ((UIText) this._text).Draw();
    }

    public override void SetLeftBadge(UIMenuItem.BadgeStyle badge) => this.LeftBadge = badge;

    public override void SetRightBadge(UIMenuItem.BadgeStyle badge) => this.RightBadge = badge;

    public override void SetRightLabel(string text) => this.RightLabel = text;

    public new string RightLabel { get; private set; }

    public new UIMenuItem.BadgeStyle LeftBadge { get; private set; }

    public new UIMenuItem.BadgeStyle RightBadge { get; private set; }

    private string BadgeToSpriteLib(UIMenuItem.BadgeStyle badge) => "commonmenu";

    private string BadgeToSpriteName(UIMenuItem.BadgeStyle badge, bool selected)
    {
      switch (badge)
      {
        case UIMenuItem.BadgeStyle.None:
          return "";
        case UIMenuItem.BadgeStyle.BronzeMedal:
          return "mp_medal_bronze";
        case UIMenuItem.BadgeStyle.GoldMedal:
          return "mp_medal_gold";
        case UIMenuItem.BadgeStyle.SilverMedal:
          return "medal_silver";
        case UIMenuItem.BadgeStyle.Alert:
          return "mp_alerttriangle";
        case UIMenuItem.BadgeStyle.Crown:
          return "mp_hostcrown";
        case UIMenuItem.BadgeStyle.Ammo:
          return !selected ? "shop_ammo_icon_a" : "shop_ammo_icon_b";
        case UIMenuItem.BadgeStyle.Armour:
          return !selected ? "shop_armour_icon_a" : "shop_armour_icon_b";
        case UIMenuItem.BadgeStyle.Barber:
          return !selected ? "shop_barber_icon_a" : "shop_barber_icon_b";
        case UIMenuItem.BadgeStyle.Clothes:
          return !selected ? "shop_clothing_icon_a" : "shop_clothing_icon_b";
        case UIMenuItem.BadgeStyle.Franklin:
          return !selected ? "shop_franklin_icon_a" : "shop_franklin_icon_b";
        case UIMenuItem.BadgeStyle.Bike:
          return !selected ? "shop_garage_bike_icon_a" : "shop_garage_bike_icon_b";
        case UIMenuItem.BadgeStyle.Car:
          return !selected ? "shop_garage_icon_a" : "shop_garage_icon_b";
        case UIMenuItem.BadgeStyle.Gun:
          return !selected ? "shop_gunclub_icon_a" : "shop_gunclub_icon_b";
        case UIMenuItem.BadgeStyle.Heart:
          return !selected ? "shop_health_icon_a" : "shop_health_icon_b";
        case UIMenuItem.BadgeStyle.Makeup:
          return !selected ? "shop_makeup_icon_a" : "shop_makeup_icon_b";
        case UIMenuItem.BadgeStyle.Mask:
          return !selected ? "shop_mask_icon_a" : "shop_mask_icon_b";
        case UIMenuItem.BadgeStyle.Michael:
          return !selected ? "shop_michael_icon_a" : "shop_michael_icon_b";
        case UIMenuItem.BadgeStyle.Star:
          return "shop_new_star";
        case UIMenuItem.BadgeStyle.Tatoo:
          return !selected ? "shop_tattoos_icon_" : "shop_tattoos_icon_b";
        case UIMenuItem.BadgeStyle.Trevor:
          return !selected ? "shop_trevor_icon_a" : "shop_trevor_icon_b";
        case UIMenuItem.BadgeStyle.Lock:
          return "shop_lock";
        case UIMenuItem.BadgeStyle.Tick:
          return "shop_tick_icon";
        default:
          return "";
      }
    }

    private Color BadgeToColor(UIMenuItem.BadgeStyle badge, bool selected) => (badge == UIMenuItem.BadgeStyle.Crown || badge == UIMenuItem.BadgeStyle.Lock || badge == UIMenuItem.BadgeStyle.Tick) && selected ? Color.FromArgb((int) byte.MaxValue, 0, 0, 0) : Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
  }
}
