// Decompiled with JetBrains decompiler

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
