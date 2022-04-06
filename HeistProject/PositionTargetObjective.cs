// Decompiled with JetBrains decompiler

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
