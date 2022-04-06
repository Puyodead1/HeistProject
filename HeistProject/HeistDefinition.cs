// Decompiled with JetBrains decompiler
// Type: HeistProject.HeistDefinition
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA.Math;
using System;
using System.Collections.Generic;

namespace HeistProject
{
  public class HeistDefinition
  {
    private ScriptedLogic _mainHeist;

    internal Dictionary<string, string> AssetTranslation { get; set; }

    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Author { get; set; }

    public int MaxCapacity { get; set; }

    public Vector3? MapPosition { get; set; }

    public List<string> Assets { get; set; }

    public string Logo { get; set; }

    public int Payout { get; set; }

    public int SetupCost { get; set; }

    public string ScriptType { get; set; }

    public string ScriptFilename { get; set; }

    public List<SetupDefinition> Setups { get; set; }

    public void SetLogic(ScriptedLogic logic) => this._mainHeist = logic;

    public ScriptedLogic GetLogic()
    {
      ScriptedLogic instance = (ScriptedLogic) Activator.CreateInstance(this._mainHeist.GetType());
      instance.Id = this.Id;
      instance.IsFinale = true;
      return instance;
    }
  }
}
