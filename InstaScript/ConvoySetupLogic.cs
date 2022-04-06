// Decompiled with JetBrains decompiler
// Type: InstaScript.ConvoySetupLogic
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
  public class ConvoySetupLogic : ScriptedLogic
  {
    private Vector3 _calafiaBridge = new Vector3(-209.2f, 4214.17f, 43.73f);
    private Vector3 _barnPos = new Vector3(1978.39f, 5171.1f, 46.26f);
    private float _convoyHeading = 132.98f;
    private Vector3 _convoy1Pos = new Vector3(199.43f, 4447.51f, 71.64f);
    private Vector3 _convoy2Pos = new Vector3(211.11f, 4457.68f, 69.97f);
    private Vector3 _convoy3Pos = new Vector3(229.83f, 4477.29f, 68.02f);
    private Vector3 _savagePos = new Vector3(249.06f, 4308.54f, 42.12f);
    private Vector3 _convoyDestination = new Vector3(-197.28f, 3759.54f, 41f);
    private bool _oldInCar;
    private bool _oldEnemies;
    private int _stealStart;
    private Blip _destBlip;
    private Vehicle _barracks;
    private Vehicle _insurgent;
    private List<Entity> _cleanup;
    private List<Ped> _enemies;
    private Ped _insurgetDriver;
    private Ped _insurgentShooter;
    private bool _shootingModeSet;

    private int Stage { get; set; }

    public override void Start()
    {
      this._cleanup = new List<Entity>();
      this._enemies = new List<Ped>();
      this.Stage = 0;
    }

    public override void End()
    {
      if (this._cleanup != null)
      {
        foreach (Entity entity in this._cleanup)
          entity.Delete();
      }
      if (this._enemies != null)
      {
        foreach (Entity enemy in this._enemies)
          enemy.Delete();
      }
      if (this._destBlip != null)
        this._destBlip.Remove();
      UI.ShowSubtitle(" ");
    }

    public override void Update()
    {
      if (this.Stage == 0)
      {
        UI.ShowSubtitle("Go to the ~y~ambush point.", 120000);
        this._destBlip = World.CreateBlip(this._calafiaBridge);
        this._destBlip.Color = BlipColor.Yellow;
        this._destBlip.ShowRoute = true;
        ++this.Stage;
      }
      if (this.Stage == 1)
      {
        Util.DrawEntryMarker(this._calafiaBridge, 2f);
        if (Game.Player.Character.IsInRangeOf(this._calafiaBridge, 3f))
        {
          ++this.Stage;
          this._destBlip.Remove();
          this.SpawnConvoy();
          UI.ShowSubtitle("Wait for the convoy.", 120000);
        }
      }
      if (this.Stage == 2 && (Entity) this._barracks != (Entity) null && Game.Player.Character.IsInRangeOf(this._barracks.Position, 150f))
      {
        ++this.Stage;
        UI.ShowSubtitle("Steal the ~b~Barracks.", 120000);
        this._enemies.ForEach((Action<Ped>) (p => p.CurrentBlip.Alpha = (int) byte.MaxValue));
        this._barracks.AddBlip();
        this._barracks.CurrentBlip.Color = BlipColor.Blue;
      }
      if (this.Stage == 3 && (Entity) this._barracks != (Entity) null && Game.Player.Character.IsInVehicle(this._barracks))
      {
        UI.ShowSubtitle("Deliver the ~b~Barracks~w~ to the ~y~hideout.", 120000);
        this._barracks.CurrentBlip.Alpha = 0;
        this._destBlip = World.CreateBlip(this._barnPos);
        this._destBlip.Color = BlipColor.Yellow;
        this._destBlip.ShowRoute = true;
        if (((IEnumerable<Ped>) EntryPoint.Team).Count<Ped>((Func<Ped, bool>) (p => p.IsAlive)) >= 3 && this._insurgent.IsAlive && Game.Player.Character.IsInRangeOf(this._insurgent.Position, 20f))
        {
          Ped firstTeammember = ((IEnumerable<Ped>) EntryPoint.Team).FirstOrDefault<Ped>((Func<Ped, bool>) (t => t.IsAlive && (Entity) t != (Entity) Game.Player.Character));
          Ped ped = ((IEnumerable<Ped>) EntryPoint.Team).FirstOrDefault<Ped>((Func<Ped, bool>) (t => t.IsAlive && (Entity) t != (Entity) Game.Player.Character && (Entity) t != (Entity) firstTeammember));
          if ((Entity) firstTeammember != (Entity) null && (Entity) ped != (Entity) null)
          {
            firstTeammember.BlockPermanentEvents = true;
            ped.RelationshipGroup = Game.Player.Character.RelationshipGroup;
            Function.Call(Hash.REMOVE_PED_FROM_GROUP,  ped.Handle);
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES,  ped.Handle,  3,  false);
            TaskSequence sequence1 = new TaskSequence();
            sequence1.AddTask.RunTo(this._insurgent.Position + Vector3.RandomXY() * 3f);
            sequence1.AddTask.EnterVehicle(this._insurgent, VehicleSeat.Driver);
            sequence1.AddTask.Wait(3000);
            Function.Call(Hash.TASK_VEHICLE_ESCORT,  0,  this._insurgent.Handle,  this._barracks.Handle,  -1,  20f,  1074528293,  5f,  20,  2f);
            sequence1.Close();
            firstTeammember.Task.PerformSequence(sequence1);
            firstTeammember.AlwaysKeepTask = true;
            TaskSequence sequence2 = new TaskSequence();
            sequence2.AddTask.RunTo(this._insurgent.Position + Vector3.RandomXY() * 3f);
            sequence2.AddTask.EnterVehicle(this._insurgent, (VehicleSeat) 7);
            sequence2.Close();
            ped.Task.PerformSequence(sequence2);
            ped.FiringPattern = FiringPattern.FullAuto;
            ped.BlockPermanentEvents = false;
            this._insurgentShooter = ped;
            this._insurgetDriver = firstTeammember;
          }
        }
        int num = World.AddRelationshipGroup("PACIFIC_HACK_ENEMIES");
        World.SetRelationshipBetweenGroups(Relationship.Hate, num, Game.Player.Character.RelationshipGroup);
        World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, num);
        this._stealStart = Game.GameTime;
        ++this.Stage;
      }
      if (this.Stage == 4)
        Util.DrawEntryMarker(this._barnPos, 2f);
      if (this.Stage != 4 || !Game.Player.Character.IsInRangeOf(this._barnPos, 3f) || !this._enemies.Where<Ped>((Func<Ped, bool>) (en => en.IsAlive)).All<Ped>((Func<Ped, bool>) (e => !e.IsInRangeOf(Game.Player.Character.Position, 100f))) || !Game.Player.Character.IsInVehicle(this._barracks))
        return;
      this.HasWon = true;
      this.HasFinished = true;
    }

    public override void BackgroundThreadUpdate()
    {
      Game.MaxWantedLevel = 0;
      Game.Player.WantedLevel = 0;
      if ((Entity) this._barracks != (Entity) null && this._barracks.IsDead)
      {
        this.FailureReason = "The Barracks has been destroyed.";
        this.HasFinished = true;
      }
      foreach (Ped ped in this._enemies.Where<Ped>((Func<Ped, bool>) (p => (uint) p.CurrentBlip.Alpha > 0U)))
      {
        if (ped.IsDead)
        {
          Function.Call(Hash.SET_PED_TO_INFORM_RESPECTED_FRIENDS,  ped.Handle,  200f,  16);
          ped.CurrentBlip.Alpha = 0;
        }
      }
      if (this.Stage == 4 && Game.GameTime - this._stealStart > 20000)
      {
        if ((Entity) this._insurgentShooter != (Entity) null && (Entity) this._barracks != (Entity) null && (Entity) this._insurgetDriver != (Entity) null && this._insurgent.IsAlive && ((Entity) this._insurgent.GetPedOnSeat((VehicleSeat) 7) != (Entity) this._insurgentShooter || (Entity) this._insurgent.GetPedOnSeat(VehicleSeat.Driver) != (Entity) this._insurgetDriver))
        {
          this._insurgetDriver.SetIntoVehicle(this._insurgent, VehicleSeat.Driver);
          this._insurgentShooter.SetIntoVehicle(this._insurgent, (VehicleSeat) 7);
          Function.Call(Hash.TASK_VEHICLE_ESCORT,  this._insurgetDriver.Handle,  this._insurgent.Handle,  this._barracks.Handle,  -1,  20f,  1074528293,  5f,  20,  2f);
          this._insurgentShooter.FiringPattern = FiringPattern.FullAuto;
        }
        else if ((Entity) this._insurgentShooter != (Entity) null && (Entity) this._barracks != (Entity) null && (Entity) this._insurgetDriver != (Entity) null && this._insurgent.IsAlive && (Entity) this._insurgent.GetPedOnSeat((VehicleSeat) 7) == (Entity) this._insurgentShooter && !this._shootingModeSet)
        {
          this._insurgentShooter.FiringPattern = FiringPattern.FullAuto;
          this._shootingModeSet = true;
        }
        this.SpawnChasingDudes();
        this._stealStart = Game.GameTime;
      }
      if (this.Stage >= 3)
      {
        Function.Call(Hash.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME,  0.0f);
        Function.Call(Hash.SET_RANDOM_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME,  0.0f);
        Function.Call(Hash.SET_PARKED_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME,  0.0f);
        Function.Call(Hash.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME,  0.0f);
        Function.Call(Hash.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME,  0.0f,  0.0f);
        Function.Call(Hash._0x2F9A292AD0A3BD89);
        Function.Call(Hash._0x5F3B7749C112D552);
      }
      if (this.Stage != 4 || !((Entity) this._barracks != (Entity) null))
        return;
      bool flag1 = Game.Player.Character.IsInVehicle(this._barracks);
      bool flag2 = this._enemies.Where<Ped>((Func<Ped, bool>) (en => en.IsAlive)).Any<Ped>((Func<Ped, bool>) (e => e.IsInRangeOf(Game.Player.Character.Position, 100f)));
      if (!this._oldEnemies & flag2)
        UI.ShowSubtitle("Escape ~r~Merryweather.", 120000);
      if (this._oldEnemies && !flag2)
        UI.ShowSubtitle("Deliver the ~b~Barracks~w~ to the ~y~hideout.", 120000);
      if (this._oldInCar && !flag1)
      {
        UI.ShowSubtitle("Get back in the ~b~Barracks", 120000);
        this._barracks.CurrentBlip.Alpha = (int) byte.MaxValue;
        this._destBlip.Alpha = 0;
        this._destBlip.ShowRoute = false;
      }
      if (!this._oldInCar & flag1)
      {
        UI.ShowSubtitle("Deliver the ~b~Barracks~w~ to the ~y~hideout.", 120000);
        this._barracks.CurrentBlip.Alpha = 0;
        this._destBlip.Alpha = (int) byte.MaxValue;
        this._destBlip.ShowRoute = true;
      }
      this._oldInCar = flag1;
      this._oldEnemies = flag2;
    }

    private void SpawnConvoy()
    {
      ScriptHandler.Log("SPAWNING CONVOY " + (object) DateTime.Now);
      ScriptHandler.Log("Game version: " + (object) Game.Version);
      ScriptHandler.Log("Loading models...");
      Model model1 = new Model(VehicleHash.Mesa3);
      model1.Request(10000);
      model1.RequestCollisions(10000);
      Model model2 = new Model(VehicleHash.Barracks);
      model2.Request(10000);
      model2.RequestCollisions(10000);
      Model model3 = new Model(PedHash.Blackops01SMY);
      model3.Request(10000);
      model3.RequestCollisions(10000);
      Model m = new Model(PedHash.Blackops02SMY);
      m.Request(10000);
      m.RequestCollisions(10000);
      Model model4 = new Model(VehicleHash.Insurgent);
      model4.Request(10000);
      model4.RequestCollisions(10000);
      Model model5 = new Model(VehicleHash.Savage);
      model5.Request(10000);
      model5.RequestCollisions(10000);
      ScriptHandler.Log("Mesa loaded: " + model1.IsLoaded.ToString());
      ScriptHandler.Log("Barracks loaded: " + model2.IsLoaded.ToString());
      ScriptHandler.Log("Blackops1 loaded: " + model3.IsLoaded.ToString());
      ScriptHandler.Log("Blackops 2 loaded: " + m.IsLoaded.ToString());
      ScriptHandler.Log("Insurgent loaded: " + model4.IsLoaded.ToString());
      ScriptHandler.Log("Savage loaded: " + model5.IsLoaded.ToString());
      Vehicle vehicle1 = World.CreateVehicle(model1, this._convoy1Pos, this._convoyHeading);
      this._cleanup.Add((Entity) vehicle1);
      Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG,  vehicle1,  true);
      DateTime now1 = DateTime.Now;
      while (DateTime.Now.Subtract(now1).TotalMilliseconds < 10000.0)
      {
        if (!Function.Call<bool>(Hash._0xE9676F61BC0B3321,  vehicle1))
          Script.Yield();
        else
          break;
      }
      for (int index = 0; index < 3; ++index)
      {
        Ped ped = World.CreatePed(Util.SharedRandom.Next(2) == 0 ? model3 : m, this._convoy1Pos);
        ped.SetIntoVehicle(vehicle1, VehicleSeat.Any);
        this._enemies.Add(ped);
        this._cleanup.Add((Entity) ped);
        if (index == 0)
          ped.Task.DriveTo(vehicle1, this._convoyDestination, 2f, 7f, 786603);
      }
      this._barracks = World.CreateVehicle(model2, this._convoy2Pos, this._convoyHeading);
      this._cleanup.Add((Entity) this._barracks);
      Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG,  this._barracks,  true);
      DateTime now2 = DateTime.Now;
      while (DateTime.Now.Subtract(now2).TotalMilliseconds < 10000.0)
      {
        if (!Function.Call<bool>(Hash._0xE9676F61BC0B3321,  this._barracks))
          Script.Yield();
        else
          break;
      }
      for (int index = 0; index < 2; ++index)
      {
        Ped ped = World.CreatePed(Util.SharedRandom.Next(2) == 0 ? model3 : m, this._convoy2Pos);
        ped.SetIntoVehicle(this._barracks, VehicleSeat.Any);
        this._enemies.Add(ped);
        this._cleanup.Add((Entity) ped);
        if (index == 0)
          Function.Call(Hash.TASK_VEHICLE_ESCORT,  ped.Handle,  this._barracks.Handle,  vehicle1,  -1,  8f,  1074528293,  5f,  20,  2f);
      }
      this._insurgent = World.CreateVehicle(model4, this._convoy3Pos, this._convoyHeading);
      this._cleanup.Add((Entity) this._insurgent);
      Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG,  this._insurgent,  true);
      DateTime now3 = DateTime.Now;
      while (DateTime.Now.Subtract(now3).TotalMilliseconds < 10000.0)
      {
        if (!Function.Call<bool>(Hash._0xE9676F61BC0B3321,  this._insurgent))
          Script.Yield();
        else
          break;
      }
      for (int index = 0; index < 8; ++index)
      {
        Ped ped = World.CreatePed(Util.SharedRandom.Next(2) == 0 ? model3 : m, this._convoy3Pos);
        ped.SetIntoVehicle(this._insurgent, VehicleSeat.Any);
        this._enemies.Add(ped);
        this._cleanup.Add((Entity) ped);
        if (index == 7)
          Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES,  ped.Handle,  3,  false);
        if (index == 0)
          Function.Call(Hash.TASK_VEHICLE_ESCORT,  ped.Handle,  this._insurgent.Handle,  this._barracks,  -1,  9f,  1074528293,  5f,  20,  2f);
      }
      Vehicle vehicle2 = World.CreateVehicle(model5, this._savagePos, 90f);
      this._cleanup.Add((Entity) vehicle2);
      Function.Call(Hash.SET_ENTITY_LOAD_COLLISION_FLAG,  vehicle2,  true);
      DateTime now4 = DateTime.Now;
      while (DateTime.Now.Subtract(now4).TotalMilliseconds < 10000.0)
      {
        if (!Function.Call<bool>(Hash._0xE9676F61BC0B3321,  vehicle2))
          Script.Yield();
        else
          break;
      }
      Ped ped1 = World.CreatePed(model3, this._savagePos);
      this._enemies.Add(ped1);
      Ped ped2 = World.CreatePed(model3, this._savagePos);
      this._enemies.Add(ped2);
      Ped ped3 = World.CreatePed(model3, this._savagePos);
      this._enemies.Add(ped3);
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped2,  -2084633992,  1000,  true,  true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped3,  -2084633992,  1000,  true,  true);
      ped1.SetIntoVehicle(vehicle2, VehicleSeat.Driver);
      ped2.SetIntoVehicle(vehicle2, VehicleSeat.LeftRear);
      ped3.SetIntoVehicle(vehicle2, VehicleSeat.RightRear);
      Function.Call(Hash.TASK_VEHICLE_HELI_PROTECT,  ped1.Handle,  vehicle2.Handle,  this._barracks.Handle,  15f,  20,  8f,  20,  20);
      int num = World.AddRelationshipGroup("PACIFIC_HACK_ENEMIES");
      World.SetRelationshipBetweenGroups(Relationship.Neutral, num, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Neutral, Game.Player.Character.RelationshipGroup, num);
      ScriptHandler.Log("Null entities: " + (object) this._cleanup.Count<Entity>((Func<Entity, bool>) (e => e == (Entity) null)));
      ScriptHandler.Log("Preparing enemies...");
      foreach (Ped enemy in this._enemies)
      {
        if (!((Entity) enemy == (Entity) null))
        {
          enemy.RelationshipGroup = num;
          Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  enemy.Handle,  1);
          enemy.BlockPermanentEvents = false;
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  enemy,  453432689,  1000,  true,  true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  enemy,  324215364,  1000,  true,  true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  enemy,  -2084633992,  1000,  true,  true);
          enemy.AddBlip();
          enemy.CurrentBlip.Sprite = BlipSprite.Enemy;
          enemy.CurrentBlip.Color = BlipColor.Red;
          enemy.CurrentBlip.Scale = 0.7f;
          enemy.CurrentBlip.Alpha = 0;
          enemy.CurrentBlip.IsShortRange = true;
          enemy.FiringPattern = FiringPattern.FullAuto;
        }
      }
      ScriptHandler.Log("Convoy complete!");
      ped2.FiringPattern = FiringPattern.FullAuto;
      ped3.FiringPattern = FiringPattern.FullAuto;
      m.MarkAsNoLongerNeeded();
      model3.MarkAsNoLongerNeeded();
      model1.MarkAsNoLongerNeeded();
      model2.MarkAsNoLongerNeeded();
      model4.MarkAsNoLongerNeeded();
      model5.MarkAsNoLongerNeeded();
    }

    private bool HasPlayerAttackedAVehicle(Vehicle veh)
    {
      Ped pedOnSeat = veh.GetPedOnSeat(VehicleSeat.Driver);
      return (Entity) pedOnSeat == (Entity) null || !pedOnSeat.IsAlive || Util.HasPedDamagedEntity(Game.Player.Character, (Entity) veh);
    }

    private void SpawnChasingDudes()
    {
      Vector3 positionOnStreet = World.GetNextPositionOnStreet(Game.Player.Character.Position + Game.Player.Character.ForwardVector * 180f);
      Model model1 = new Model(VehicleHash.Mesa3);
      model1.Request(10000);
      Vehicle vehicle = World.CreateVehicle(model1, positionOnStreet);
      this._cleanup.Add((Entity) vehicle);
      model1.MarkAsNoLongerNeeded();
      Model model2 = new Model(PedHash.Blackops01SMY);
      model2.Request(10000);
      Ped ped1 = World.CreatePed(model2, positionOnStreet);
      Ped ped2 = World.CreatePed(model2, positionOnStreet);
      model2.MarkAsNoLongerNeeded();
      int num = World.AddRelationshipGroup("PACIFIC_HACK_ENEMIES");
      ped1.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  ped1.Handle,  3);
      ped1.BlockPermanentEvents = false;
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped1,  -2084633992,  1000,  true,  true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped1,  324215364,  1000,  true,  true);
      ped2.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  ped2.Handle,  3);
      ped2.BlockPermanentEvents = false;
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped2,  -2084633992,  1000,  true,  true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped2,  324215364,  1000,  true,  true);
      ped1.Task.WarpIntoVehicle(vehicle, VehicleSeat.Driver);
      ped2.Task.WarpIntoVehicle(vehicle, VehicleSeat.Passenger);
      Script.Wait(1000);
      Function.Call(Hash.TASK_VEHICLE_CHASE,  ped1.Handle,  Game.Player.Character.Handle);
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
      this._cleanup.Add((Entity) ped1);
      this._cleanup.Add((Entity) ped2);
      this._enemies.Add(ped1);
      this._enemies.Add(ped2);
    }
  }
}
