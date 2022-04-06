// Decompiled with JetBrains decompiler

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
