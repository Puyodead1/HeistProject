// Decompiled with JetBrains decompiler
// Type: HeistProject.ScriptedLogic
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using System;
using System.Collections.Generic;

namespace HeistProject
{
  public abstract class ScriptedLogic
  {
    public bool HasFinished { get; set; }

    public bool HasWon { get; set; }

    public string Id { get; set; }

    public bool IsFinale { get; set; }

    public string FailureReason { get; set; }

    public Dictionary<string, Tuple<string, bool>> MissionChallenges { get; set; } = new Dictionary<string, Tuple<string, bool>>();

    public int StartTime { get; set; }

    public abstract void Update();

    public abstract void BackgroundThreadUpdate();

    public abstract void Start();

    public abstract void End();
  }
}
