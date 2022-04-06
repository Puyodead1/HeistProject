// Decompiled with JetBrains decompiler
// Type: HeistProject.Objective
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

namespace HeistProject
{
  public abstract class Objective
  {
    private HeistDefinition HeistParent;
    private SetupDefinition SetupParent;

    public bool Completed { get; set; }

    public string Description { get; set; }

    public abstract void Start();

    public void Stop()
    {
    }

    public Objective(HeistDefinition heist) => this.HeistParent = heist;

    public Objective(SetupDefinition setup) => this.SetupParent = setup;
  }
}
