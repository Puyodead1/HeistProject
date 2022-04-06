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
  public class VansSetupLogic : ScriptedLogic
  {
    private int _heistStart;
    private int _msgsShown;
    private bool _oldInCar;
    private bool _oldWanted;
    private int _relgroup;
    private bool _isTailingVan;
    private Blip _destBlip;
    private Vehicle[] _vans;
    private Ped[] _drivers;
    private Vehicle _targetVan;

    private int Stage { get; set; }

    public override void Start()
    {
      this.Stage = 0;
      this._vans = new Vehicle[4];
      this._drivers = new Ped[4];
      this._relgroup = World.AddRelationshipGroup("HEIST_DRIVERS");
      World.SetRelationshipBetweenGroups(Relationship.Neutral, this._relgroup, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Neutral, Game.Player.Character.RelationshipGroup, this._relgroup);
      Model model1 = new Model(VehicleHash.Boxville4);
      model1.Request(10000);
      Model model2 = new Model(PedHash.Postal01SMM);
      model2.Request(10000);
      for (int index = 0; index < 4; ++index)
      {
        Vector3 positionOnStreet = World.GetNextPositionOnStreet(this.RandomXY(1000f), true);
        this._vans[index] = World.CreateVehicle(model1, positionOnStreet);
        this._vans[index].AddBlip();
        this._vans[index].CurrentBlip.Sprite = BlipSprite.SonicWave;
        this._vans[index].CurrentBlip.Color = BlipColor.White;
        this._vans[index].CurrentBlip.Scale = 3f;
        Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, (InputArgument) this._vans[index].Handle, (InputArgument) 1);
        this._drivers[index] = World.CreatePed(model2, positionOnStreet + new Vector3(5f, 0.0f, 0.0f));
        this._drivers[index].SetIntoVehicle(this._vans[index], VehicleSeat.Driver);
        this._drivers[index].BlockPermanentEvents = false;
        this._drivers[index].RelationshipGroup = this._relgroup;
        this._drivers[index].Armor = 200;
        this._drivers[index].Task.CruiseWithVehicle(this._vans[index], 10f, 786603);
      }
      model2.MarkAsNoLongerNeeded();
      model1.MarkAsNoLongerNeeded();
    }

    public override void End()
    {
      for (int index = 0; index < this._vans.Length; ++index)
      {
        this._vans[index].Delete();
        this._drivers[index].Delete();
      }
      if (this._destBlip != null)
        this._destBlip.Remove();
      UI.ShowSubtitle(" ");
    }

    public override void Update()
    {
      if (this.Stage == 0)
      {
        ++this.Stage;
        Util.SendLesterMessage("There are 4 Post Op vans driving in the city. I need you to get me each of those van's plates.");
        UI.ShowSubtitle("Catch up to one of the vans.", 120000);
      }
      if (this.Stage == 1 && ((IEnumerable<Vehicle>) this._vans).Any<Vehicle>((Func<Vehicle, bool>) (v => Game.Player.Character.IsInRangeOf(v.Position, 50f) && v.CurrentBlip.Alpha > 0)))
      {
        if (!this._isTailingVan)
        {
          this._isTailingVan = true;
          Util.SendLesterMessage("Alright, get his license plate and message me.");
          UI.ShowSubtitle("Send Lester the van's license plate.", 120000);
          Util.DisplayHelpText("Press ~INPUT_CONTEXT~ to send Lester a message.");
        }
        if (Game.IsControlJustPressed(0, Control.Context))
        {
          string input = Game.GetUserInput(10);
          Vehicle vehicle;
          if ((Entity) (vehicle = ((IEnumerable<Vehicle>) this._vans).FirstOrDefault<Vehicle>((Func<Vehicle, bool>) (v => v.NumberPlate == input))) != (Entity) null)
          {
            vehicle.CurrentBlip.Alpha = 0;
            if (((IEnumerable<Vehicle>) this._vans).Count<Vehicle>((Func<Vehicle, bool>) (v => v.CurrentBlip.Alpha == 0)) == this._vans.Length)
            {
              Util.SendLesterMessage("Alright, checking the database, give me a sec.");
              Script.Wait(10000);
              this._targetVan = this._vans[Util.SharedRandom.Next(this._vans.Length)];
              this._targetVan.CurrentBlip.Remove();
              this._targetVan.AddBlip();
              this._targetVan.CurrentBlip.Color = BlipColor.Blue;
              Util.SendLesterMessage("I've got the van, it's on your map.");
              UI.ShowSubtitle("Hijack the ~r~van~w~ with the transmitter.", 120000);
              this._targetVan.CurrentBlip.Color = BlipColor.Blue;
              ++this.Stage;
            }
            else
              Util.SendLesterMessage("Alright, got that one. Move onto the next.");
          }
          else
            Util.SendLesterMessage("No results found, check it again.");
        }
      }
      else if (this.Stage == 1)
      {
        if (this._isTailingVan)
          UI.ShowSubtitle("Catch up to one of the vans.", 120000);
        this._isTailingVan = false;
      }
      if (this.Stage == 2 && (Entity) this._targetVan != (Entity) null && Game.Player.Character.IsInVehicle(this._targetVan))
      {
        UI.ShowSubtitle("Deliver the van to the ~y~warehouse.", 120000);
        this._targetVan.CurrentBlip.Alpha = 0;
        this._destBlip = World.CreateBlip(Util.Warehouse);
        this._destBlip.Color = BlipColor.Yellow;
        this._destBlip.ShowRoute = true;
        ++this.Stage;
      }
      if (this.Stage == 3 && (Entity) this._targetVan != (Entity) null && Game.Player.Character.IsInVehicle(this._targetVan) && Game.Player.WantedLevel == 0 && Game.Player.Character.IsInRangeOf(Util.Warehouse, 3f))
      {
        this.HasWon = true;
        this.HasFinished = true;
      }
      if (this.Stage != 3)
        return;
      Util.DrawEntryMarker(Util.Warehouse, 2f);
    }

    public Vector3 RandomXY(float range) => new Vector3()
    {
      X = (float) (Util.SharedRandom.NextDouble() * 2.0 - 1.0) * range,
      Y = (float) (Util.SharedRandom.NextDouble() * 2.0 - 1.0) * range,
      Z = 50f
    };

    public override void BackgroundThreadUpdate()
    {
      if (Game.Player.WantedLevel > 0 && this.Stage == 1)
      {
        this.HasFinished = true;
        this.FailureReason = "You attracted police attention.";
      }
      if (this.Stage == 1 && ((IEnumerable<Vehicle>) this._vans).Any<Vehicle>(new Func<Vehicle, bool>(this.HasPlayerAttackedAVehicle)))
      {
        this.HasFinished = true;
        this.FailureReason = "You alerted the van driver.";
      }
      if (this.Stage != 3)
        return;
      bool flag1 = Game.Player.WantedLevel > 0;
      if (flag1 && !this._oldWanted)
      {
        UI.ShowSubtitle("Lose the cops.");
        this._destBlip.Alpha = 0;
        this._destBlip.ShowRoute = false;
      }
      if (!flag1 && this._oldWanted)
      {
        UI.ShowSubtitle("Deliver the van to the ~y~warehouse.", 120000);
        this._destBlip.Alpha = (int) byte.MaxValue;
        this._destBlip.ShowRoute = true;
      }
      this._oldWanted = flag1;
      bool flag2 = Game.Player.Character.IsInVehicle(this._targetVan);
      if (this._oldInCar && !flag2)
      {
        UI.ShowSubtitle("Get back in the ~r~van.", 120000);
        this._targetVan.CurrentBlip.Alpha = (int) byte.MaxValue;
        this._destBlip.Alpha = 0;
        this._destBlip.ShowRoute = false;
      }
      if (!this._oldInCar & flag2)
      {
        UI.ShowSubtitle("Deliver the van to the ~y~warehouse.", 120000);
        this._targetVan.CurrentBlip.Alpha = 0;
        this._destBlip.Alpha = (int) byte.MaxValue;
        this._destBlip.ShowRoute = true;
      }
      this._oldInCar = flag2;
    }

    private bool HasPlayerAttackedAVehicle(Vehicle veh)
    {
      Ped pedOnSeat = veh.GetPedOnSeat(VehicleSeat.Driver);
      return (Entity) pedOnSeat == (Entity) null || !pedOnSeat.IsAlive || Util.HasPedDamagedEntity(Game.Player.Character, (Entity) veh);
    }
  }
}
