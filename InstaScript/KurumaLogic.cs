// Decompiled with JetBrains decompiler

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace InstaScript
{
  public class KurumaLogic : ScriptedLogic
  {
    private Vector3 _kurumaPos = new Vector3(-1244.39f, -648.14f, 39.82f);
    private float _kurumaHead = 128.19f;
    private Blip _destBlip;
    private Vehicle _vehTarget;
    private int _lastGoonSpawn;
    private List<Entity> _cleanupBag;
    private bool _oldIsInVeh;
    private bool _oldWanted;

    private int Stage { get; set; }

    public override void Start()
    {
      this.Stage = 0;
      this._cleanupBag = new List<Entity>();
      ScriptHandler.Log("==== KURUMA HEIST START ==== " + (object) DateTime.Now);
      ScriptHandler.Log("Game version: " + (object) Game.Version);
      Model[] source = new Model[14]
      {
        new Model(-304802106),
        new Model(75131841),
        new Model(80636076),
        new Model(108773431),
        new Model(142944341),
        new Model(349605904),
        new Model(384071873),
        new Model(408192225),
        new Model(464687292),
        new Model(933092024),
        new Model(70821038),
        new Model(1371553700),
        new Model(2119136831),
        new Model(-9308122)
      };
      int num1 = 0;
      ScriptHandler.Log("Starting model load...");
      DateTime now = DateTime.Now;
      for (; !((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)) && num1 <= 200; ++num1)
      {
        foreach (Model model in ((IEnumerable<Model>) source).Where<Model>((Func<Model, bool>) (m => !m.IsLoaded)))
        {
          model.Request();
          Script.Yield();
        }
      }
      ScriptHandler.Log("Time elapsed: " + (object) DateTime.Now.Subtract(now).TotalMilliseconds + "ms");
      ScriptHandler.Log("Iterations: " + (object) num1);
      if (!((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)))
      {
        ScriptHandler.Log("All models loaded: false. Unloaded models:");
        foreach (Model model in ((IEnumerable<Model>) source).Where<Model>((Func<Model, bool>) (m => !m.IsLoaded)))
          ScriptHandler.Log(model.Hash.ToString());
      }
      else
        ScriptHandler.Log("All models loaded: true");
      ScriptHandler.Log("Creating vehicles...");
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[0], new Vector3(-1231.509f, -640.9623f, 39.97462f), -2.956406f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[0], new Vector3(-1239.18f, -654.0615f, 39.97463f), -46.01252f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[1], new Vector3(-1207.566f, -657.8344f, 40.05859f), -52.0477f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[2], new Vector3(-1233.01f, -661.8339f, 39.64507f), -49.58582f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[3], new Vector3(-1246.585f, -645.2906f, 39.6528f), -62.48361f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[4], new Vector3(-1223.157f, -653.3138f, 40.26498f), 128.0181f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[5], new Vector3(-1224.585f, -672.549f, 39.86357f), -42.9401f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[6], new Vector3(-1217.729f, -679.9518f, 39.97824f), -51.10104f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[7], new Vector3(-1201.167f, -666.2057f, 39.52089f), -46.52832f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[8], new Vector3(-1206.655f, -670.6198f, 40.05848f), -49.34415f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[9], new Vector3(-1228.97f, -643.6428f, 40.35754f), 125.7952f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[10], new Vector3(-1241.286f, -651.549f, 40.35754f), -18.24997f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[11], new Vector3(-1234.679f, -660.3904f, 40.35754f), 1.55016f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[12], new Vector3(-1233.606f, -659.291f, 40.35754f), 104.9205f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[12], new Vector3(-1230.636f, -644.8346f, 40.35754f), -30.08508f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[13], new Vector3(-1239.274f, -651.3271f, 40.35754f), 84.66802f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[13], new Vector3(-1233.625f, -641.3906f, 40.35754f), -0.04267382f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[13], new Vector3(-1233.925f, -640.1281f, 40.35754f), -163.0356f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[13], new Vector3(-1243.734f, -645.9791f, 40.35754f), -0.0001087287f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[13], new Vector3(-1244.095f, -644.413f, 40.35754f), -153.6445f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[13], new Vector3(-1247.714f, -644.267f, 40.35754f), 0.2582828f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[13], new Vector3(-1225.096f, -650.9821f, 40.35754f), 1.093226f));
      ScriptHandler.Log("Entities created. Null vehicles: " + (object) this._cleanupBag.Count<Entity>((Func<Entity, bool>) (e => e == (Entity) null)));
      for (int index = 0; index < source.Length; ++index)
        source[index].MarkAsNoLongerNeeded();
      int num2 = World.AddRelationshipGroup("KURUMA_SETUP_ENEMIES");
      World.SetRelationshipBetweenGroups(Relationship.Neutral, num2, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Neutral, Game.Player.Character.RelationshipGroup, num2);
      string[] strArray = new string[4]
      {
        "WORLD_HUMAN_AA_SMOKE",
        "WORLD_HUMAN_DRINKING",
        "WORLD_HUMAN_DRUG_DEALER",
        "WORLD_HUMAN_PARTYING"
      };
      ScriptHandler.Log("Preparing enemies...");
      foreach (Entity entity in this._cleanupBag)
      {
        if (!(entity == (Entity) null) && entity.Model.IsPed)
        {
          Ped ped = (Ped) entity;
          ped.RelationshipGroup = num2;
          Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  ped.Handle,  1);
          ped.BlockPermanentEvents = false;
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  453432689,  9999,  false,  true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  324215364,  9999,  false,  true);
          if (Util.SharedRandom.Next(10) > 6)
            Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  -1074790547,  9999,  false,  true);
          if (Util.SharedRandom.Next(100) > 30)
            Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE,  ped,  strArray[Util.SharedRandom.Next(strArray.Length)],  0,  false);
        }
      }
      ScriptHandler.Log("Kuruma startup complete!\n===================");
    }

    public override void End()
    {
      if ((Entity) this._vehTarget != (Entity) null)
        this._vehTarget.Delete();
      if (this._destBlip != null)
        this._destBlip.Remove();
      for (int index = 0; index < this._cleanupBag.Count; ++index)
      {
        if (this._cleanupBag[index] != (Entity) null)
          this._cleanupBag[index].Delete();
      }
      UI.ShowSubtitle(" ");
    }

    public override void Update()
    {
      if (this.Stage == 0)
      {
        UI.ShowSubtitle("Steal the ~b~Kuruma.", 120000);
        this._vehTarget = World.CreateVehicle(new Model(VehicleHash.Kuruma2), this._kurumaPos, this._kurumaHead);
        this._vehTarget.AddBlip();
        this._vehTarget.CurrentBlip.Color = BlipColor.Blue;
        this._vehTarget.CurrentBlip.ShowRoute = true;
        ++this.Stage;
      }
      if (this.Stage == 1 && Game.Player.Character.IsInRangeOf(this._vehTarget.Position, 5f))
      {
        int num = World.AddRelationshipGroup("KURUMA_SETUP_ENEMIES");
        World.SetRelationshipBetweenGroups(Relationship.Hate, num, Game.Player.Character.RelationshipGroup);
        World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, num);
      }
      if (this.Stage == 1 && Game.Player.Character.IsInVehicle(this._vehTarget))
      {
        UI.ShowSubtitle("Deliver the ~b~Kuruma~w~ to the ~y~warehouse.", 120000);
        this._vehTarget.CurrentBlip.Alpha = 0;
        this._destBlip = World.CreateBlip(new Vector3(453.53f, -3074.97f, 5.1f));
        this._destBlip.Color = BlipColor.Yellow;
        this._destBlip.ShowRoute = true;
        this.SecondWave();
        this._lastGoonSpawn = Game.GameTime;
        ++this.Stage;
      }
      if (this.Stage == 2)
        World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(453.53f, -3074.97f, 5.1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), Color.Yellow);
      if (this.Stage != 2 || !Game.Player.Character.IsInRangeOf(new Vector3(453.53f, -3074.97f, 5.1f), 2f) || Game.Player.WantedLevel != 0)
        return;
      float num1 = (float) (1.0 - (double) this._vehTarget.Health / (double) this._vehTarget.MaxHealth);
      if ((double) num1 <= 0.0599999986588955)
        this.MissionChallenges.AddSafe<string, Tuple<string, bool>>("Kuruma damage under 6%", new Tuple<string, bool>(num1.ToString("P"), true));
      else
        this.MissionChallenges.AddSafe<string, Tuple<string, bool>>("Kuruma damage under 6%", new Tuple<string, bool>(num1.ToString("P"), false));
      this.HasWon = true;
      this.HasFinished = true;
    }

    public override void BackgroundThreadUpdate()
    {
      if ((Entity) this._vehTarget != (Entity) null && this._vehTarget.IsDead)
      {
        this.FailureReason = "The kuruma has been destroyed.";
        this.HasFinished = true;
      }
      if (this.Stage == 2 && (Entity) this._vehTarget != (Entity) null)
      {
        bool flag1 = Game.Player.Character.IsInVehicle(this._vehTarget);
        bool flag2 = Game.Player.WantedLevel > 0;
        if (this._oldIsInVeh && !flag1)
        {
          UI.ShowSubtitle("Steal the ~b~Kuruma.", 120000);
          if (this._destBlip != null)
            this._destBlip.Alpha = 0;
          if (this._vehTarget.CurrentBlip != null)
            this._vehTarget.CurrentBlip.Alpha = (int) byte.MaxValue;
        }
        if (!this._oldIsInVeh & flag1)
        {
          UI.ShowSubtitle("Deliver the ~b~Kuruma~w~ to the ~y~warehouse.", 120000);
          if (this._destBlip != null)
            this._destBlip.Alpha = (int) byte.MaxValue;
          if (this._vehTarget.CurrentBlip != null)
            this._vehTarget.CurrentBlip.Alpha = 0;
        }
        if (flag2 && !this._oldWanted)
        {
          UI.ShowSubtitle("Lose the cops.", 120000);
          if (this._destBlip != null)
            this._destBlip.Alpha = 0;
        }
        if (!flag2 && this._oldWanted)
        {
          UI.ShowSubtitle("Deliver the ~b~Kuruma~w~ to the ~y~warehouse.", 120000);
          if (this._destBlip != null)
            this._destBlip.Alpha = (int) byte.MaxValue;
        }
        this._oldIsInVeh = flag1;
        this._oldWanted = flag2;
      }
      if (this.Stage != 2 || Game.GameTime - this._lastGoonSpawn <= 25000)
        return;
      this._lastGoonSpawn = Game.GameTime;
      this.SpawnChasingDudes();
    }

    private void SpawnChasingDudes()
    {
      Vector3 positionOnStreet = World.GetNextPositionOnStreet(Game.Player.Character.Position + Game.Player.Character.ForwardVector * 100f + Vector3.RandomXY() * 50f);
      Vehicle vehicle = World.CreateVehicle(new Model(VehicleHash.Dominator), positionOnStreet);
      this._cleanupBag.Add((Entity) vehicle);
      Ped ped1 = World.CreatePed(new Model(-9308122), positionOnStreet);
      Ped ped2 = World.CreatePed(new Model(2119136831), positionOnStreet);
      int num = World.AddRelationshipGroup("KURUMA_SETUP_ENEMIES");
      ped1.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  ped1.Handle,  3);
      ped1.BlockPermanentEvents = false;
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped1,  324215364,  9999,  false,  true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped1,  453432689,  9999,  false,  true);
      ped2.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  ped2.Handle,  3);
      ped2.BlockPermanentEvents = false;
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped2,  324215364,  9999,  false,  true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped2,  453432689,  9999,  false,  true);
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
      this._cleanupBag.Add((Entity) ped1);
      this._cleanupBag.Add((Entity) ped2);
    }

    private void SecondWave()
    {
      ScriptHandler.Log("KURUMA SECOND WAVE " + (object) DateTime.Now);
      Model[] source = new Model[5]
      {
        new Model(-1450650718),
        new Model(464687292),
        new Model(-1255452397),
        new Model(-9308122),
        new Model(2119136831)
      };
      int num = 0;
      ScriptHandler.Log("Loading models into memory...");
      DateTime now = DateTime.Now;
      for (; !((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)) && num < 200; ++num)
      {
        foreach (Model model in ((IEnumerable<Model>) source).Where<Model>((Func<Model, bool>) (m => !m.IsLoaded)))
        {
          model.Request();
          Script.Yield();
        }
      }
      ScriptHandler.Log("Time elapsed: " + (object) DateTime.Now.Subtract(now).TotalMilliseconds + "ms");
      ScriptHandler.Log("Iterations: " + (object) num);
      if (!((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)))
      {
        ScriptHandler.Log("All models loaded: false. Unloaded models:");
        foreach (Model model in ((IEnumerable<Model>) source).Where<Model>((Func<Model, bool>) (m => !m.IsLoaded)))
          ScriptHandler.Log(model.Hash.ToString());
      }
      else
        ScriptHandler.Log("All models loaded: true");
      ScriptHandler.Log("Adding entities...");
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[0], new Vector3(-1207.631f, -670.7169f, 35.10327f), 107.0522f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[1], new Vector3(-1222.549f, -657.5234f, 35.23972f), 87.97679f));
      this._cleanupBag.Add((Entity) World.CreateVehicle(source[2], new Vector3(-1225.162f, -659.6329f, 8656f * (float) System.Math.E / 923f), -116.579f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[3], new Vector3(-1220.045f, -659.4192f, 35.53879f), -0.0003503989f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[3], new Vector3(-1223.735f, -659.145f, 35.53879f), -0.1142341f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[3], new Vector3(-1212.329f, -668.6511f, 35.53879f), -128.7379f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[3], new Vector3(-1206.559f, -669.078f, 35.53879f), 168.485f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[4], new Vector3(-1223.991f, -662.0961f, 25.90129f), 0.0f));
      this._cleanupBag.Add((Entity) World.CreatePed(source[4], new Vector3(-1227.18f, -660.605f, 25.90129f), -109.5013f));
      ScriptHandler.Log("Entities added. Null entities: " + (object) this._cleanupBag.Count<Entity>((Func<Entity, bool>) (e => e == (Entity) null)));
      for (int index = 0; index < source.Length; ++index)
        source[index].MarkAsNoLongerNeeded();
      ScriptHandler.Log("Preparing enemies...");
      foreach (Entity entity in this._cleanupBag)
      {
        if (!(entity == (Entity) null) && entity.Model.IsPed)
        {
          Ped ped = (Ped) entity;
          Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  ped.Handle,  1);
          ped.BlockPermanentEvents = false;
          ped.RelationshipGroup = World.AddRelationshipGroup("KURUMA_SETUP_ENEMIES");
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  324215364,  9999,  false,  true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  453432689,  9999,  false,  true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  -1074790547,  9999,  false,  true);
          Function.Call(Hash.SET_PED_HEARING_RANGE,  ped,  100f);
          Function.Call(Hash.SET_PED_SEEING_RANGE,  ped,  100f);
          Function.Call(Hash.SET_PED_COMBAT_RANGE,  ped,  1);
        }
      }
      ScriptHandler.Log("Second wave complete!\n===========");
    }
  }
}
