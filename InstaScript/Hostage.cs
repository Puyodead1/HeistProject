// Decompiled with JetBrains decompiler
using GTA;
using GTA.Native;
using HeistProject;

namespace InstaScript
{
  public class Hostage
  {
    public static string GetAnimNameFromIndex(int indx)
    {
      switch (indx)
      {
        case 0:
          return "cashier_a";
        case 1:
          return "cashier_b";
        case 2:
          return "ped_a";
        case 3:
          return "ped_b";
        case 4:
          return "ped_c";
        case 5:
          return "ped_d";
        case 6:
          return "ped_e";
        case 7:
          return "ped_f";
        case 8:
          return "ped_g";
        case 9:
          return "ped_h";
        default:
          return (string) null;
      }
    }

    public Hostage(PosInfo info, int index)
    {
      int num = World.AddRelationshipGroup("PACIFIC_STANDARD_FINALE_HOSTAGES");
      World.SetRelationshipBetweenGroups(Relationship.Companion, Game.Player.Character.RelationshipGroup, num);
      World.SetRelationshipBetweenGroups(Relationship.Companion, num, Game.Player.Character.RelationshipGroup);
      this.Position = info;
      this.Ped = World.CreateRandomPed(info.Position);
      this.Ped.Heading = info.Heading;
      this.Ped.RelationshipGroup = num;
      this.AnimSet = Hostage.GetAnimNameFromIndex(index);
      Function.Call(Hash.TASK_PLAY_ANIM, this.Ped.Handle, PacificFinale.LoadDict("anim@heists@ornate_bank@hostages@intro"), ("intro_loop_" + this.AnimSet), 8f, 1f, -1, 1, -8f, 0, 0, 0);
    }

    public bool CanRebel()
    {
      if (this.AnimSet == "cashier_a")
        return true;
      return this.AnimSet.StartsWith("ped") && !this.AnimSet.EndsWith("e") && !this.AnimSet.EndsWith("f") && !this.AnimSet.EndsWith("h");
    }

    public bool HasGunForRebelling()
    {
      if (!this.AnimSet.StartsWith("ped"))
        return false;
      return this.AnimSet.EndsWith("b") || this.AnimSet.EndsWith("g");
    }

    public void Clean() => this.Ped.Delete();

    public void Scream()
    {
      string[] strArray = new string[3]
      {
        "GENERIC_FRIGHTENED_HIGH",
        "GENERIC_SHOCKED_HIGH",
        "GENERIC_SHOCKED_MED"
      };
      Function.Call(Hash._PLAY_AMBIENT_SPEECH1, this.Ped.Handle, strArray[Util.SharedRandom.Next(strArray.Length)], "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL", 1);
    }

    public void Spook()
    {
      TaskSequence sequence = new TaskSequence();
      Function.Call(Hash.TASK_PLAY_ANIM, 0, PacificFinale.LoadDict("anim@heists@ornate_bank@hostages@intro"), ("intro_" + this.AnimSet), 8f, 1f, -1, 0, -8f, 0, 0, 0);
      Function.Call(Hash.TASK_PLAY_ANIM, 0, PacificFinale.LoadDict("anim@heists@ornate_bank@hostages@" + this.AnimSet + "@"), "idle", 8f, 1f, -1, 1, -8f, 0, 0, 0);
      sequence.Close();
      this.Ped.Task.PerformSequence(sequence);
      sequence.Dispose();
      this.IsSpooked = true;
    }

    public void Rebel()
    {
      if (!this.CanRebel())
        return;
      TaskSequence sequence = new TaskSequence();
      Function.Call(Hash.TASK_PLAY_ANIM, 0, PacificFinale.LoadDict("anim@heists@ornate_bank@hostages@" + this.AnimSet + "@"), "reach", 8f, 1f, -1, 0, -8f, 0, 0, 0);
      Function.Call(Hash.TASK_PLAY_ANIM, 0, PacificFinale.LoadDict("anim@heists@ornate_bank@hostages@" + this.AnimSet + "@"), "pass", 8f, 1f, -1, 0, -8f, 0, 0, 0);
      if (this.HasGunForRebelling())
      {
        int num = World.AddRelationshipGroup("PACIFIC_STANDARD_FINALE_REBEL");
        World.SetRelationshipBetweenGroups(Relationship.Neutral, num, Game.Player.Character.RelationshipGroup);
        World.SetRelationshipBetweenGroups(Relationship.Neutral, Game.Player.Character.RelationshipGroup, num);
        this.Ped.RelationshipGroup = num;
        Function.Call(Hash.GIVE_WEAPON_TO_PED, this.Ped, 453432689, 1000, true, true);
        sequence.AddTask.ShootAt(EntryPoint.Team[Util.SharedRandom.Next(EntryPoint.Team.Length)], -1, FiringPattern.FullAuto);
        this.Scream();
      }
      else
        Function.Call(Hash.TASK_PLAY_ANIM, 0, PacificFinale.LoadDict("anim@heists@ornate_bank@hostages@" + this.AnimSet + "@"), "idle", 8f, 1f, -1, 1, -8f, 0, 0, 0);
      sequence.Close();
      this.HasRebeled = true;
      this.Ped.Task.PerformSequence(sequence);
      sequence.Dispose();
    }

    public bool IsSpooked { get; set; }

    public Ped Ped { get; set; }

    public PosInfo Position { get; set; }

    public string AnimSet { get; set; }

    public bool HasRebeled { get; set; }
  }
}
