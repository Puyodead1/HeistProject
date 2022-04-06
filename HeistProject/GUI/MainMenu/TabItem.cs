// Decompiled with JetBrains decompiler

using GTA;
using NativeUI;
using System;
using System.Drawing;

namespace HeistProject.GUI.MainMenu
{
  public class TabItem
  {
    public bool DrawBg;
    protected Sprite RockstarTile;

    public TabItem(string name)
    {
      this.RockstarTile = new Sprite("pause_menu_sp_content", "rockstartilebmp", new Point(), new Size(64, 64), 0.0f, Color.FromArgb(40, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.Title = name;
      this.DrawBg = true;
      this.UseDynamicPositionment = true;
    }

    public bool Visible { get; set; }

    public bool Focused { get; set; }

    public string Title { get; set; }

    public bool Active { get; set; }

    public bool JustOpened { get; set; }

    public bool CanBeFocused { get; set; }

    public Point TopLeft { get; set; }

    public Point BottomRight { get; set; }

    public Point SafeSize { get; set; }

    public bool UseDynamicPositionment { get; set; }

    public event EventHandler Activated;

    public bool FadeInWhenFocused { get; set; }

    public void OnActivated()
    {
      EventHandler activated = this.Activated;
      if (activated == null)
        return;
      activated((object) this, EventArgs.Empty);
    }

    public virtual void Draw()
    {
      if (!this.Visible)
        return;
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      if (this.UseDynamicPositionment)
      {
        this.SafeSize = new Point(300, 240);
        Point safeSize = this.SafeSize;
        int x1 = safeSize.X;
        safeSize = this.SafeSize;
        int y = safeSize.Y;
        this.TopLeft = new Point(x1, y);
        int width = (int) resolutionMantainRatio.Width;
        safeSize = this.SafeSize;
        int x2 = safeSize.X;
        this.BottomRight = new Point(width - x2, (int) resolutionMantainRatio.Height - this.SafeSize.Y);
      }
      Size size = new Size(this.BottomRight.SubtractPoints(this.TopLeft));
      if (!this.DrawBg)
        return;
      ((UIRectangle) new UIResRectangle(this.TopLeft, size, Color.FromArgb(this.Focused || !this.FadeInWhenFocused ? 200 : 120, 0, 0, 0))).Draw();
      int num1 = 100;
      this.RockstarTile.Size = new Size(num1, num1);
      int num2 = size.Width / num1;
      int duration = 4;
      for (int index = 0; index < num2 * duration; ++index)
      {
        this.RockstarTile.Position = this.TopLeft.AddPoints(new Point(num1 * (index % num2), num1 * (index / num2)));
        this.RockstarTile.Color = Color.FromArgb((int) Util.LinearFloatLerp(40f, 0.0f, index / num2, duration), (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        this.RockstarTile.Draw();
      }
    }
  }
}
