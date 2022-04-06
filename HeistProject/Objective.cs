// Decompiled with JetBrains decompiler

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
