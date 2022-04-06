// Decompiled with JetBrains decompiler
// Type: HeistProject.PositionTargetObjective
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;

namespace HeistProject
{
  public class PositionTargetObjective : Objective
  {
    private Vector3 _target;
    private float _range;

    public PositionTargetObjective(
      HeistDefinition heist,
      Vector3 target,
      string description,
      float range)
      : base(heist)
    {
      this._target = target;
      this.Description = description;
      this._range = range;
    }

    public PositionTargetObjective(
      SetupDefinition heist,
      Vector3 target,
      string description,
      float range)
      : base(heist)
    {
      this._target = target;
      this.Description = description;
      this._range = range;
    }

    public override void Start()
    {
      while (!Game.Player.Character.IsInRangeOf(this._target, this._range))
        Script.Yield();
      this.Completed = true;
    }
  }
}
