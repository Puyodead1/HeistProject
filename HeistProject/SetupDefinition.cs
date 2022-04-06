// Decompiled with JetBrains decompiler

using GTA.Math;
using System;

namespace HeistProject
{
  public class SetupDefinition
  {
    private ScriptedLogic _mainHeist;

    public Vector3? MapPosition { get; set; }

    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public HeistDefinition Parent { get; set; }

    public int MaxCapacity { get; set; }

    public string ScriptFilename { get; set; }

    public string ScriptType { get; set; }

    public void SetLogic(ScriptedLogic logic) => this._mainHeist = logic;

    public ScriptedLogic GetLogic()
    {
      ScriptedLogic instance = (ScriptedLogic) Activator.CreateInstance(this._mainHeist.GetType());
      instance.Id = this.Id;
      instance.IsFinale = false;
      return instance;
    }
  }
}
