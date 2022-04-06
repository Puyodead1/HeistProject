// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.MenuSetup
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA.Math;
using System;

namespace HeistProject.GUI
{
  public class MenuSetup
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public bool Available { get; set; }

    public bool Complete { get; set; }

    public bool Highlighted { get; set; }

    public string Texture { get; set; }

    public string Photo1 { get; set; }

    public string Photo2 { get; set; }

    public string Photo3 { get; set; }

    public Vector3? Position { get; set; }

    public event EventHandler Activated;

    internal void OnActivated()
    {
      EventHandler activated = this.Activated;
      if (activated == null)
        return;
      activated((object) this, EventArgs.Empty);
    }
  }
}
