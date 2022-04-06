// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.MainMenu.TabSubmenuItem
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Native;
using NativeUI;
using System.Collections.Generic;
using System.Drawing;

namespace HeistProject.GUI.MainMenu
{
  public class TabSubmenuItem : TabItem
  {
    public TabSubmenuItem(string name, IEnumerable<TabItem> items)
      : base(name)
    {
      this.DrawBg = false;
      this.CanBeFocused = true;
      this.Items = new List<TabItem>(items);
      this.IsInList = true;
    }

    public List<TabItem> Items { get; set; }

    public int Index { get; set; }

    public bool IsInList { get; set; }

    public void ProcessControls()
    {
      if (this.JustOpened)
      {
        this.JustOpened = false;
      }
      else
      {
        if (!this.Focused)
          return;
        if (Game.IsControlJustPressed(0, Control.PhoneSelect) && this.Focused)
        {
          Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "SELECT", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
          this.Items[this.Index].OnActivated();
        }
        if (Game.IsControlJustPressed(0, Control.FrontendUp) || Game.IsControlJustPressed(0, Control.MoveUpOnly))
        {
          this.Index = (1000 - 1000 % this.Items.Count + this.Index - 1) % this.Items.Count;
          Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "NAV_UP_DOWN", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
        }
        else
        {
          if (!Game.IsControlJustPressed(0, Control.FrontendDown) && !Game.IsControlJustPressed(0, Control.MoveDownOnly))
            return;
          this.Index = (1000 - 1000 % this.Items.Count + this.Index + 1) % this.Items.Count;
          Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "NAV_UP_DOWN", (InputArgument) "HUD_FRONTEND_DEFAULT_SOUNDSET", (InputArgument) 1);
        }
      }
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      base.Draw();
      this.ProcessControls();
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      int num1 = this.Focused ? 1 : 0;
      int alpha1 = this.Focused ? 200 : 100;
      int alpha2 = this.Focused ? (int) byte.MaxValue : 150;
      float num2 = resolutionMantainRatio.Width - (float) (this.SafeSize.X * 2);
      int num3 = (int) ((double) num2 * 0.681800007820129);
      Size size = new Size((int) num2 - (num3 + 3), 40);
      for (int index = 0; index < this.Items.Count; ++index)
      {
        ((UIRectangle) new UIResRectangle(this.SafeSize.AddPoints(new Point(0, (size.Height + 3) * index)), size, this.Index != index || !this.Focused ? Color.FromArgb(alpha1, Color.Black) : Color.FromArgb(alpha2, Color.White))).Draw();
        ((UIText) new UIResText(this.Items[index].Title, this.SafeSize.AddPoints(new Point(6, 5 + (size.Height + 3) * index)), 0.35f, Color.FromArgb(alpha2, this.Index != index || !this.Focused ? Color.White : Color.Black))).Draw();
      }
      this.Items[this.Index].Visible = true;
      this.Items[this.Index].FadeInWhenFocused = true;
      this.Items[this.Index].CanBeFocused = true;
      this.Items[this.Index].Focused = this.Focused;
      this.Items[this.Index].UseDynamicPositionment = false;
      this.Items[this.Index].SafeSize = this.SafeSize.AddPoints(new Point((int) num2 - num3, 0));
      this.Items[this.Index].TopLeft = this.SafeSize.AddPoints(new Point((int) num2 - num3, 0));
      this.Items[this.Index].BottomRight = new Point((int) resolutionMantainRatio.Width - this.SafeSize.X, (int) resolutionMantainRatio.Height - this.SafeSize.Y);
      this.Items[this.Index].Draw();
    }
  }
}
