// Decompiled with JetBrains decompiler

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstaScript
{
  public class TitanSetupLogic : ScriptedLogic
  {
    private int _heistStart;
    private int _msgsShown;
    private bool _oldInCar;
    private List<Entity> _cleanup;
    private List<Ped> _enemies;
    private Blip _destBlip;
    private Vehicle _titan;
    private Vector3 _titanSpawnPos = new Vector3(-1274.48f, -3385.61f, 14.78f);
    private float _titanSpawnHeading = 330.92f;
    private Vector3 _sandyShoresHangar = new Vector3(1757.61f, 3266.59f, 40.22f);
    private int _titanEnterTime;

    private int Stage { get; set; }

    public override void Start()
    {
      this.Stage = 0;
      this._cleanup = new List<Entity>();
      this._enemies = new List<Ped>();
      ScriptHandler.Log("STARTING TITAN SETUP " + (object) DateTime.Now);
      ScriptHandler.Log("Game version: " + (object) Game.Version);
      Model[] source = new Model[5]
      {
        new Model(1981688531),
        new Model(-2064372143),
        new Model(-2137348917),
        new Model(-877478386),
        new Model(788443093)
      };
      ScriptHandler.Log("Starting model load...");
      DateTime now = DateTime.Now;
      int num1;
      for (num1 = 0; !((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)) && num1 <= 200; ++num1)
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
      ScriptHandler.Log("Spawning entities...");
      this._cleanup.Add((Entity) (this._titan = World.CreateVehicle(source[0], new Vector3(-1273.841f, -3384.967f, 14.78024f), -29.08082f)));
      this._cleanup.Add((Entity) World.CreateVehicle(source[1], new Vector3(-1248.865f, -3387.322f, 13.70972f), 18.80969f));
      this._cleanup.Add((Entity) World.CreateVehicle(source[1], new Vector3(-1285.003f, -3363.349f, 13.71278f), -96.20724f));
      this._cleanup.Add((Entity) World.CreateVehicle(source[1], new Vector3(-1294.6f, -3397.292f, 13.7096f), -152.1115f));
      Vehicle vehicle1;
      this._cleanup.Add((Entity) (vehicle1 = World.CreateVehicle(source[2], new Vector3(-1269.239f, -3356.507f, 14.01297f), 40.73475f)));
      Vehicle vehicle2;
      this._cleanup.Add((Entity) (vehicle2 = World.CreateVehicle(source[3], new Vector3(-1263.535f, -3361.517f, 15.76355f), 72.23425f)));
      Function.Call(Hash.ATTACH_VEHICLE_TO_TRAILER, vehicle1, vehicle2, 5f);
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1251.501f, -3387.389f, 13.94017f), 26.15033f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1252.367f, -3386.354f, 13.94385f), -117.3318f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1284.409f, -3360.724f, 13.94355f), -77.4324f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1282.451f, -3361.513f, 13.94028f), 40.19783f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1284.924f, -3365.566f, 13.9434f), -73.07863f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1296.155f, -3399.993f, 13.94187f), 0.7785163f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1296.91f, -3396.917f, 13.94373f), -161.7712f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1275.82f, -3379.8f, 13.94344f), 1.505941f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1276.403f, -3377.611f, 13.9421f), -149.5538f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1249.583f, -3407.903f, 24.26862f), 27.67922f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1284.859f, -3419.814f, 24.26875f), -19.35975f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1307.903f, -3387.296f, 24.27754f), -62.62064f));
      this._cleanup.Add((Entity) World.CreatePed(source[4], new Vector3(-1299.921f, -3358.847f, 24.26973f), -82.91515f));
      int num2 = World.AddRelationshipGroup("PACIFIC_SETUP_TITAN");
      World.SetRelationshipBetweenGroups(Relationship.Hate, num2, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, num2);
      string[] strArray = new string[4]
      {
        "WORLD_HUMAN_AA_SMOKE",
        "WORLD_HUMAN_DRINKING",
        "WORLD_HUMAN_DRUG_DEALER",
        "WORLD_HUMAN_PARTYING"
      };
      int num3 = 0;
      foreach (Entity entity in this._cleanup)
      {
        if (!(entity == (Entity) null) && entity.Model.IsPed)
        {
          Ped ped = (Ped) entity;
          ped.RelationshipGroup = num2;
          Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, ped.Handle, 1);
          ped.BlockPermanentEvents = false;
          Function.Call(Hash.SET_PED_HEARING_RANGE, ped, 100f);
          Function.Call(Hash.SET_PED_SEEING_RANGE, ped, 100f);
          Function.Call(Hash.SET_PED_COMBAT_RANGE, ped, 1);
          ped.AddBlip();
          ped.CurrentBlip.Sprite = BlipSprite.Enemy;
          ped.CurrentBlip.Color = BlipColor.Red;
          ped.CurrentBlip.Scale = 0.7f;
          ped.CurrentBlip.Alpha = 0;
          Function.Call(Hash.GIVE_WEAPON_TO_PED, ped, 324215364, 1000, true, true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED, ped, 453432689, 1000, true, true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED, ped, -1074790547, 1000, true, true);
          if (num3 >= 9)
            Function.Call(Hash.GIVE_WEAPON_TO_PED, ped, 100416529, 1000, true, true);
          Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped, strArray[Util.SharedRandom.Next(strArray.Length)], 0, false);
          this._enemies.Add(ped);
          ++num3;
        }
      }
      for (int index = 0; index < source.Length; ++index)
        source[index].MarkAsNoLongerNeeded();
      ScriptHandler.Log("Titan start complete.");
    }

    public override void End()
    {
      foreach (Entity entity in this._cleanup)
        entity.Delete();
      if (this._destBlip != null)
        this._destBlip.Remove();
      UI.ShowSubtitle("");
    }

    public override void Update()
    {
      if (this.Stage == 0)
      {
        UI.ShowSubtitle("Steal the ~b~Titan.", 120000);
        this._titan.AddBlip();
        this._titan.CurrentBlip.Color = BlipColor.Blue;
        ++this.Stage;
      }
      if (this.Stage == 1 && this._enemies.Any<Ped>((Func<Ped, bool>) (p => p.IsInCombat)) && this._enemies.All<Ped>((Func<Ped, bool>) (p => p.CurrentBlip.Alpha == 0)) && this._enemies.Count<Ped>((Func<Ped, bool>) (p => p.IsAlive)) > 4)
      {
        this._enemies.ForEach((Action<Ped>) (p => p.CurrentBlip.Alpha = (int) byte.MaxValue));
        this._titanEnterTime = Game.GameTime;
      }
      if (this.Stage == 1 && Game.Player.Character.IsInVehicle(this._titan))
      {
        if (EntryPoint.Team[1].IsAlive)
          EntryPoint.Team[1].SetIntoVehicle(this._titan, VehicleSeat.Passenger);
        if (EntryPoint.Team[2].IsAlive)
          EntryPoint.Team[2].SetIntoVehicle(this._titan, VehicleSeat.LeftRear);
        if (EntryPoint.Team[3].IsAlive)
          EntryPoint.Team[3].SetIntoVehicle(this._titan, VehicleSeat.RightRear);
        UI.ShowSubtitle("Deliver the ~b~Titan~w~ to the ~y~Sandy Shores airfield.", 120000);
        this._titan.CurrentBlip.Alpha = 0;
        this._destBlip = World.CreateBlip(this._sandyShoresHangar);
        this._destBlip.Color = BlipColor.Yellow;
        ++this.Stage;
      }
      if (this.Stage == 2 && Game.Player.Character.IsInVehicle(this._titan))
        Util.DrawEntryMarker(this._sandyShoresHangar, 8f);
      if (this.Stage != 2 || !Game.Player.Character.IsInVehicle(this._titan) || !Game.Player.Character.IsInRangeOf(this._sandyShoresHangar, 10f) || (double) Game.Player.Character.CurrentVehicle.Speed >= 1.0)
        return;
      this.HasWon = true;
      this.HasFinished = true;
    }

    public override void BackgroundThreadUpdate()
    {
      Game.MaxWantedLevel = 0;
      Game.Player.WantedLevel = 0;
      if (this._titan.IsDead)
      {
        this.FailureReason = "The Titan has been destroyed.";
        this.HasFinished = true;
      }
      if (this.Stage >= 1 && Game.GameTime - this._titanEnterTime > 15000 && this._titanEnterTime != 0 && Game.Player.Character.IsInRangeOf(this._titanSpawnPos, 500f) && this._enemies.Any<Ped>((Func<Ped, bool>) (e => e.IsDead)))
      {
        this.SpawnChasingDudes();
        this.SpawnChasingDudes();
        this.SpawnChasingDudes();
        this._titanEnterTime = Game.GameTime;
      }
      if (this.Stage == 2)
      {
        bool flag = Game.Player.Character.IsInVehicle(this._titan);
        if (this._oldInCar && !flag)
        {
          UI.ShowSubtitle("Get back in the ~b~Titan.", 120000);
          this._destBlip.Alpha = 0;
          this._titan.CurrentBlip.Alpha = (int) byte.MaxValue;
        }
        else if (!this._oldInCar & flag)
        {
          UI.ShowSubtitle("Deliver the ~b~Titan~w~ to the ~y~Sandy Shores airfield.", 120000);
          this._destBlip.Alpha = (int) byte.MaxValue;
          this._titan.CurrentBlip.Alpha = 0;
        }
        this._oldInCar = flag;
      }
      foreach (Ped ped in this._enemies.Where<Ped>((Func<Ped, bool>) (p => (uint) p.CurrentBlip.Alpha > 0U)))
      {
        if (ped.IsDead)
          ped.CurrentBlip.Alpha = 0;
      }
    }

    private void SpawnChasingDudes()
    {
      Vector3 position = World.GetNextPositionOnStreet(Game.Player.Character.Position + Game.Player.Character.ForwardVector * 180f) + Vector3.RandomXY() * 5f;
      Model model1 = new Model(VehicleHash.Mesa3);
      model1.Request(10000);
      Model model2 = new Model(PedHash.Blackops01SMY);
      model2.Request(10000);
      Vehicle vehicle = World.CreateVehicle(model1, position);
      this._cleanup.Add((Entity) vehicle);
      Ped ped1 = World.CreatePed(model2, position);
      Ped ped2 = World.CreatePed(model2, position);
      int num = World.AddRelationshipGroup("PACIFIC_HACK_ENEMIES");
      ped1.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, ped1.Handle, 3);
      ped1.BlockPermanentEvents = false;
      Function.Call(Hash.GIVE_WEAPON_TO_PED, ped1, -2084633992, 1000, true, true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED, ped1, 324215364, 1000, true, true);
      ped2.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, ped2.Handle, 3);
      ped2.BlockPermanentEvents = false;
      Function.Call(Hash.GIVE_WEAPON_TO_PED, ped2, -2084633992, 1000, true, true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED, ped2, 324215364, 1000, true, true);
      ped1.Task.WarpIntoVehicle(vehicle, VehicleSeat.Driver);
      ped2.Task.WarpIntoVehicle(vehicle, VehicleSeat.Passenger);
      Script.Wait(1000);
      Function.Call(Hash.TASK_VEHICLE_CHASE, ped1.Handle, Game.Player.Character.Handle);
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
      model2.MarkAsNoLongerNeeded();
      model1.MarkAsNoLongerNeeded();
    }
  }
}
