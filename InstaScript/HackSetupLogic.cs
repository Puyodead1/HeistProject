// Decompiled with JetBrains decompiler
// Type: InstaScript.HackSetupLogic
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstaScript
{
  public class HackSetupLogic : ScriptedLogic
  {
    private int _heistStart;
    private int _msgsShown;
    private bool _oldInCar;
    private Vehicle _decoy;
    private Vehicle _realTarget;
    private Vector3 _vehspawn = new Vector3(455.87f, -3063.9f, 5.72f);
    private Vector3 _targetPos = new Vector3(75.17f, 165.64f, 104.24f);
    private float _targetHeading = 251.45f;
    private Vector3 _decoyTargetPos = new Vector3(87.47f, 166.79f, 103.12f);
    private Vector3 _firstGotoTarget = new Vector3(181.73f, 217.13f, 104.52f);
    private Vector3 _vanDestroyPos = new Vector3(-1152.78f, -1564.77f, 3.19f);
    private int _swapStart;
    private Blip _destBlip;
    private List<Ped> _firstEnemies;
    private List<Entity> _extraEntities;
    private bool _hasTakenOutShitAi;

    private int Stage { get; set; }

    public override void Start()
    {
      this._extraEntities = new List<Entity>();
      Model model = new Model(VehicleHash.GBurrito2);
      model.Request(10000);
      this._decoy = World.CreateVehicle(model, this._vehspawn, 2.6f);
      this._decoy.PrimaryColor = VehicleColor.MatteBlack;
      this._decoy.SecondaryColor = VehicleColor.MatteRed;
      this._realTarget = World.CreateVehicle(model, this._targetPos, this._targetHeading);
      this._realTarget.PrimaryColor = VehicleColor.MatteBlack;
      this._realTarget.SecondaryColor = VehicleColor.MatteRed;
      model.MarkAsNoLongerNeeded();
      Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) Game.Player.Character, (InputArgument) 883325847, (InputArgument) 10000, (InputArgument) true, (InputArgument) true);
      this.Stage = 0;
    }

    public override void End()
    {
      UI.ShowSubtitle(" ");
      if (this._firstEnemies != null)
      {
        for (int index = 0; index < this._firstEnemies.Count; ++index)
        {
          if ((Entity) this._firstEnemies[index] != (Entity) null)
            this._firstEnemies[index].Delete();
        }
      }
      if (this._extraEntities != null)
      {
        foreach (Entity extraEntity in this._extraEntities)
        {
          if (extraEntity != (Entity) null)
            extraEntity.Delete();
        }
      }
      if ((Entity) this._realTarget != (Entity) null)
        this._realTarget.Delete();
      if ((Entity) this._decoy != (Entity) null)
        this._decoy.Delete();
      if (!Blip.op_Inequality(this._destBlip, (Blip) null))
        return;
      this._destBlip.Remove();
    }

    public override void Update()
    {
      Game.MaxWantedLevel = 0;
      Game.Player.WantedLevel = 0;
      if (this.Stage == 0)
      {
        UI.ShowSubtitle("Get in the ~b~decoy van.", 120000);
        this._decoy.AddBlip();
        this._decoy.CurrentBlip.Color = BlipColor.Blue;
        ++this.Stage;
      }
      if (this.Stage == 1 && Game.Player.Character.IsInVehicle(this._decoy))
      {
        this._decoy.CurrentBlip.Remove();
        UI.ShowSubtitle("Go to the ~y~rival crew hideout.", 120000);
        this._destBlip = World.CreateBlip(this._firstGotoTarget);
        this._destBlip.ShowRoute = true;
        this._destBlip.Color = BlipColor.Yellow;
        ++this.Stage;
      }
      if (this.Stage == 2)
      {
        Util.DrawEntryMarker(this._firstGotoTarget);
        if (Game.Player.Character.IsInRangeOf(this._firstGotoTarget, 2.5f))
        {
          this._destBlip.Remove();
          UI.ShowSubtitle("Take out ~r~the rival heist crew~w~.", 120000);
          ScriptHandler.Log("STARTING HACK SETUP " + (object) DateTime.Now);
          ScriptHandler.Log("Game version: " + (object) Game.Version);
          this._firstEnemies = new List<Ped>();
          Model model = new Model(2119136831);
          model.Request(10000);
          ScriptHandler.Log("Enemy model loaded: " + model.IsLoaded.ToString());
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(156.1619f, 167.4264f, 105.0338f), 53.08382f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(161.0561f, 152.3708f, 105.1776f), 128.3731f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(154.9187f, 167.8838f, 105.0087f), -90.24311f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(136.6799f, 169.4423f, 105.025f), 136.3335f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(135.7046f, 168.672f, 105.0151f), -98.85367f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(124.1728f, 166.8464f, 104.7268f), -158.9687f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(114.6501f, 170.3525f, 112.4553f), -179.9646f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(1909f / 16f, 167.9247f, 116.3537f), -143.5587f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(94.40309f, 177.9885f, 104.6231f), 48.69602f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(93.48317f, 179.479f, 104.6365f), 167.0626f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(91.52485f, 186.9985f, 116.5235f), 174.7394f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(78.79584f, 182.9568f, 109.7947f), -137.8035f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(73.56261f, 154.2949f, 104.7115f), 1.16687f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(72.56553f, 155.2483f, 104.7283f), -55.1218f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(69.36771f, 166.1006f, 112.9878f), -84.67198f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(55.18499f, 168.6938f, 126.4878f), -99.82393f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(72.68143f, 184.4065f, 104.8874f), 117.357f));
          this._firstEnemies.Add(World.CreatePed(model, new Vector3(71.57777f, 186.2894f, 104.9099f), -167.0066f));
          ScriptHandler.Log("Enemies null: " + (object) this._firstEnemies.Count<Ped>((Func<Ped, bool>) (e => (Entity) e == (Entity) null)));
          model.MarkAsNoLongerNeeded();
          string[] strArray = new string[4]
          {
            "WORLD_HUMAN_AA_SMOKE",
            "WORLD_HUMAN_DRINKING",
            "WORLD_HUMAN_DRUG_DEALER",
            "WORLD_HUMAN_PARTYING"
          };
          int num = World.AddRelationshipGroup("PACIFIC_HACK_ENEMIES");
          World.SetRelationshipBetweenGroups(Relationship.Hate, num, Game.Player.Character.RelationshipGroup);
          World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, num);
          ScriptHandler.Log("Preparing enemies...");
          foreach (Ped firstEnemy in this._firstEnemies)
          {
            if (!((Entity) firstEnemy == (Entity) null))
            {
              firstEnemy.RelationshipGroup = num;
              Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, (InputArgument) firstEnemy.Handle, (InputArgument) 1);
              firstEnemy.BlockPermanentEvents = false;
              Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) firstEnemy, (InputArgument) 324215364, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
              Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) firstEnemy, (InputArgument) 453432689, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
              firstEnemy.AddBlip();
              firstEnemy.CurrentBlip.Sprite = BlipSprite.Enemy;
              firstEnemy.CurrentBlip.Color = BlipColor.Red;
              firstEnemy.CurrentBlip.Scale = 0.7f;
              firstEnemy.CurrentBlip.IsShortRange = true;
              if (Util.SharedRandom.Next(10) > 4)
              {
                Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) firstEnemy, (InputArgument) -2084633992, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
                Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) firstEnemy, (InputArgument) 100416529, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
              }
              if (Util.SharedRandom.Next(100) > 30)
                Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, (InputArgument) firstEnemy, (InputArgument) strArray[Util.SharedRandom.Next(strArray.Length)], (InputArgument) 0, (InputArgument) false);
            }
          }
          ++this.Stage;
        }
      }
      if (this.Stage == 3 && this._firstEnemies != null && this._firstEnemies.All<Ped>((Func<Ped, bool>) (e => e.IsDead)))
      {
        ++this.Stage;
        this._decoy.AddBlip();
        this._decoy.CurrentBlip.Color = BlipColor.Blue;
        UI.ShowSubtitle("Get in the ~b~decoy vehicle.", 120000);
      }
      if (this.Stage == 4 && Game.Player.Character.IsInVehicle(this._decoy))
      {
        this._decoy.CurrentBlip.Remove();
        UI.ShowSubtitle("Get the ~b~decoy vehicle~w~ near the ~y~target vehicle~w~.", 120000);
        this._destBlip = World.CreateBlip(this._decoyTargetPos);
        this._destBlip.Color = BlipColor.Yellow;
        ++this.Stage;
      }
      if (this.Stage == 5)
        Util.DrawEntryMarker(this._decoyTargetPos, 2f);
      if (this.Stage == 5 && Game.Player.Character.IsInRangeOf(this._decoyTargetPos, 3f) && Game.Player.Character.IsInVehicle(this._decoy) && (double) Game.Player.Character.CurrentVehicle.Speed < 1.0)
      {
        Ped ped = ((IEnumerable<Ped>) EntryPoint.Team).FirstOrDefault<Ped>((Func<Ped, bool>) (t => (Entity) t != (Entity) Game.Player.Character && t.IsAlive));
        if ((Entity) ped != (Entity) null)
        {
          ped.BlockPermanentEvents = true;
          TaskSequence sequence = new TaskSequence();
          sequence.AddTask.LeaveVehicle();
          sequence.AddTask.GoTo(this._realTarget.Position, false, 10000);
          sequence.AddTask.EnterVehicle(this._realTarget, VehicleSeat.Driver);
          sequence.AddTask.Wait(10000);
          Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, (InputArgument) 0, (InputArgument) this._realTarget.Handle, (InputArgument) Util.Warehouse.X, (InputArgument) Util.Warehouse.Y, (InputArgument) Util.Warehouse.Z, (InputArgument) 10f, (InputArgument) 786468, (InputArgument) 5f);
          sequence.Close();
          ped.Task.PerformSequence(sequence);
        }
        UI.ShowSubtitle("Lure the ~r~rival heist crew~w~ to ~y~Aguja Street.", 120000);
        this._destBlip.Remove();
        this._destBlip = World.CreateBlip(this._vanDestroyPos);
        this._destBlip.Color = BlipColor.Yellow;
        this._destBlip.ShowRoute = true;
        this._swapStart = Game.GameTime;
        if (this._firstEnemies != null)
        {
          foreach (Entity firstEnemy in this._firstEnemies)
            firstEnemy.Delete();
        }
        ++this.Stage;
      }
      if (this.Stage == 6)
        Util.DrawEntryMarker(this._vanDestroyPos, 2f);
      if (this.Stage == 6 && Game.Player.Character.IsInVehicle(this._decoy) && Game.Player.Character.IsInRangeOf(this._vanDestroyPos, 2f))
      {
        this._destBlip.Remove();
        UI.ShowSubtitle("Destroy the ~r~van.", 120000);
        this._decoy.AddBlip();
        this._decoy.CurrentBlip.Color = BlipColor.Red;
        ++this.Stage;
      }
      if (this.Stage != 7 || !((Entity) this._decoy != (Entity) null) || !this._decoy.IsDead)
        return;
      Script.Wait(2000);
      this.HasWon = true;
      this.HasFinished = true;
    }

    public override void BackgroundThreadUpdate()
    {
      if (this.Stage >= 6 && !this._hasTakenOutShitAi && (Entity) this._realTarget.GetPedOnSeat(VehicleSeat.Driver) != (Entity) null && !Game.Player.Character.IsInRangeOf(this._realTarget.Position, 100f))
      {
        Ped pedOnSeat = this._realTarget.GetPedOnSeat(VehicleSeat.Driver);
        if ((Entity) pedOnSeat != (Entity) Game.Player.Character)
        {
          Vector3 positionOnStreet = World.GetNextPositionOnStreet(pedOnSeat.Position);
          pedOnSeat.Position = positionOnStreet;
          Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, (InputArgument) pedOnSeat.Handle, (InputArgument) this._realTarget.Handle, (InputArgument) Util.Warehouse.X, (InputArgument) Util.Warehouse.Y, (InputArgument) Util.Warehouse.Z, (InputArgument) 10f, (InputArgument) 786468, (InputArgument) 5f);
          this._hasTakenOutShitAi = true;
        }
      }
      if (this.Stage == 6 && Game.GameTime - this._swapStart > 20000 && !this.HasFinished)
      {
        this._swapStart = Game.GameTime;
        this.SpawnChasingDudes();
      }
      if (this._realTarget.IsDead)
      {
        this.FailureReason = "The target vehicle has been destroyed.";
        this.HasFinished = true;
      }
      if (this._decoy.IsDead && this.Stage <= 6)
      {
        this.FailureReason = "The decoy has been destroyed before you lured the gang out.";
        this.HasFinished = true;
      }
      bool flag = true;
      for (int index = 1; index < EntryPoint.Team.Length; ++index)
        flag = flag && EntryPoint.Team[index].IsDead;
      if (flag)
      {
        this.FailureReason = "All team members have died.";
        this.HasFinished = true;
      }
      if (this._firstEnemies == null)
        return;
      foreach (Ped ped in this._firstEnemies.Where<Ped>((Func<Ped, bool>) (p => (uint) p.CurrentBlip.Alpha > 0U)))
      {
        if (ped.IsDead)
        {
          Function.Call(Hash.SET_PED_TO_INFORM_RESPECTED_FRIENDS, (InputArgument) ped.Handle, (InputArgument) 200f, (InputArgument) 16);
          ped.CurrentBlip.Alpha = 0;
        }
      }
    }

    private void SpawnChasingDudes()
    {
      Vector3 positionOnStreet = World.GetNextPositionOnStreet(Game.Player.Character.Position + Game.Player.Character.ForwardVector * 100f);
      Vehicle vehicle = World.CreateVehicle(new Model(VehicleHash.Dominator), positionOnStreet);
      this._extraEntities.Add((Entity) vehicle);
      Ped ped1 = World.CreatePed(new Model(-9308122), positionOnStreet);
      Ped ped2 = World.CreatePed(new Model(2119136831), positionOnStreet);
      int num = World.AddRelationshipGroup("PACIFIC_HACK_ENEMIES");
      ped1.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, (InputArgument) ped1.Handle, (InputArgument) 3);
      ped1.BlockPermanentEvents = false;
      Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) ped1, (InputArgument) 324215364, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
      ped2.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, (InputArgument) ped2.Handle, (InputArgument) 3);
      ped2.BlockPermanentEvents = false;
      Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) ped2, (InputArgument) 324215364, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
      ped1.Task.WarpIntoVehicle(vehicle, VehicleSeat.Driver);
      ped2.Task.WarpIntoVehicle(vehicle, VehicleSeat.Passenger);
      Script.Wait(1000);
      Function.Call(Hash.TASK_VEHICLE_CHASE, (InputArgument) ped1.Handle, (InputArgument) Game.Player.Character.Handle);
      ped1.AddBlip();
      ped1.CurrentBlip.Sprite = BlipSprite.Enemy;
      ped1.CurrentBlip.Color = BlipColor.Red;
      ped1.CurrentBlip.IsShortRange = true;
      ped1.CurrentBlip.Scale = 0.7f;
      ped2.AddBlip();
      ped2.CurrentBlip.Sprite = BlipSprite.Enemy;
      ped2.CurrentBlip.Color = BlipColor.Red;
      ped2.CurrentBlip.IsShortRange = true;
      ped2.CurrentBlip.Scale = 0.7f;
      this._extraEntities.Add((Entity) ped1);
      this._extraEntities.Add((Entity) ped2);
      this._firstEnemies.Add(ped1);
      this._firstEnemies.Add(ped2);
    }
  }
}
