// Decompiled with JetBrains decompiler

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject;
using System.Drawing;

namespace ExternalScript
{
  public class HeatHeistLogic : ScriptedLogic
  {
    private Vehicle _mainTarget;
    private Ped[] _guards;
    private System.Random _rand;
    private int _guardRelGroup;
    private Pickup _moneyPickup;
    private bool copsAfterYou;
    private bool silentApproach;
    private Blip _moneyBlip;
    private int Stage;

    public override void Start()
    {
      this._rand = new System.Random();
      Function.Call(Hash.GIVE_WEAPON_TO_PED,  Game.Player.Character,  741814745,  5,  true,  true);
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.EmptyDuffelBag);
      this._guardRelGroup = World.AddRelationshipGroup("HEIST_GUARDS");
      World.SetRelationshipBetweenGroups(Relationship.Neutral, this._guardRelGroup, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Neutral, Game.Player.Character.RelationshipGroup, this._guardRelGroup);
      Model model1 = new Model(VehicleHash.Stockade);
      model1.Request(10000);
      Vector3 positionOnStreet = World.GetNextPositionOnStreet(Vector3.RandomXY() * 600f, true);
      this._mainTarget = World.CreateVehicle(model1, positionOnStreet);
      this._mainTarget.AddBlip();
      this._mainTarget.CurrentBlip.Sprite = BlipSprite.PoliceOfficer | BlipSprite.Helicopter;
      this._mainTarget.CurrentBlip.Color = BlipColor.Red;
      Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED,  this._mainTarget.Handle,  1);
      model1.MarkAsNoLongerNeeded();
      this._guards = new Ped[2];
      for (int index = 0; index < 2; ++index)
      {
        Model model2 = new Model(this._rand.Next(0, 2) == 1 ? PedHash.Armoured01SMM : PedHash.Armoured02SMM);
        model2.Request(10000);
        this._guards[index] = World.CreatePed(model2, positionOnStreet + new Vector3(5f, 0.0f, 0.0f));
        Ped guard = this._guards[index];
        Vehicle mainTarget = this._mainTarget;
        int seat;
        switch (index)
        {
          case 0:
            seat = -1;
            break;
          case 1:
            seat = 0;
            break;
          default:
            seat = 2;
            break;
        }
        guard.SetIntoVehicle(mainTarget, (VehicleSeat) seat);
        Function.Call(Hash.GIVE_WEAPON_TO_PED,  this._guards[index],  -1074790547,  1000,  true,  true);
        this._guards[index].BlockPermanentEvents = false;
        this._guards[index].RelationshipGroup = this._guardRelGroup;
        this._guards[index].Armor = 200;
        Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  this._guards[index].Handle,  1);
        model2.MarkAsNoLongerNeeded();
      }
      this._guards[0].Task.CruiseWithVehicle(this._mainTarget, 10f, 786603);
    }

    public override void End()
    {
      this.HasFinished = true;
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
      this._mainTarget.Delete();
      for (int index = 0; index < this._guards.Length; ++index)
        this._guards[index].Delete();
      if (this._moneyBlip != null)
        this._moneyBlip.Remove();
      if (this._moneyPickup != null)
        this._moneyPickup.Delete();
      UI.ShowSubtitle("");
    }

    public override void Update()
    {
      if (this.Stage == 0)
      {
        UI.ShowSubtitle("Stop the ~r~Stockade~w~.", 60000);
        ++this.Stage;
      }
      if (this.Stage == 1 && (double) this._mainTarget.Speed < 1.0 && (Util.IsVehicleDoorOpen(this._mainTarget, VehicleDoor.BackLeftDoor) || Util.IsVehicleDoorOpen(this._mainTarget, VehicleDoor.BackRightDoor)))
      {
        this.Stage = 3;
        this._mainTarget.CurrentBlip.Remove();
        Vector3 offsetInWorldCoords = this._mainTarget.GetOffsetInWorldCoords(new Vector3(0.0f, -5f, 0.0f));
        this._moneyBlip = World.CreateBlip(offsetInWorldCoords);
        this._moneyBlip.Color = BlipColor.Green;
        this._moneyBlip.Scale = 0.5f;
        this._moneyPickup = new Pickup(Function.Call<int>(Hash.CREATE_PICKUP_ROTATE,  -562499202,  offsetInWorldCoords.X,  offsetInWorldCoords.Y,  offsetInWorldCoords.Z,  0,  0,  0,  1,  10000,  2,  0,  0));
        UI.ShowSubtitle("Pick up the ~g~money.", 60000);
        Util.SendLesterMessage("They don't even know what hit'em! Nice job.");
        this.silentApproach = true;
        if (this._guards[0].IsAlive && this._guards[1].IsAlive)
        {
          this._guards[0].Weapons.RemoveAll();
          this._guards[1].Weapons.RemoveAll();
          this._guards[0].BlockPermanentEvents = true;
          this._guards[1].BlockPermanentEvents = true;
          string[] strArray = "missfbi1 idle_inside_cuboard_malehostage01".Split();
          int num = 49;
          while (true)
          {
            if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED,  strArray[0]))
            {
              Function.Call(Hash.REQUEST_ANIM_DICT,  strArray[0]);
              Script.Yield();
            }
            else
              break;
          }
          TaskSequence sequence1 = new TaskSequence();
          sequence1.AddTask.Wait(1000);
          Function.Call(Hash.TASK_PLAY_ANIM,  new Ped(0),  strArray[0],  strArray[1],  8f,  1f,  -1,  num,  -8f,  false,  false,  false);
          sequence1.AddTask.LeaveVehicle(this._mainTarget, false);
          sequence1.AddTask.GoTo(this._mainTarget.GetOffsetInWorldCoords(new Vector3(-1f, 0.0f, 0.0f)), false);
          sequence1.Close();
          TaskSequence sequence2 = new TaskSequence();
          sequence2.AddTask.Wait(1000);
          Function.Call(Hash.TASK_PLAY_ANIM,  new Ped(0),  strArray[0],  strArray[1],  8f,  1f,  -1,  num,  -8f,  false,  false,  false);
          sequence2.AddTask.LeaveVehicle(this._mainTarget, false);
          sequence2.AddTask.GoTo(this._mainTarget.GetOffsetInWorldCoords(new Vector3(0.0f, -6f, 0.0f)), false);
          sequence2.Close();
          this._guards[0].Task.PerformSequence(sequence1);
          this._guards[1].Task.PerformSequence(sequence2);
        }
      }
      if (this.Stage == 1 && this.HasPlayerAttackedTheStockade())
      {
        UI.ShowSubtitle("Blow the back doors.", 60000);
        ++this.Stage;
      }
      if (this.Stage <= 2 && (Util.IsVehicleDoorOpen(this._mainTarget, VehicleDoor.BackLeftDoor) || Util.IsVehicleDoorOpen(this._mainTarget, VehicleDoor.BackRightDoor)))
      {
        this._mainTarget.CurrentBlip.Remove();
        Vector3 offsetInWorldCoords = this._mainTarget.GetOffsetInWorldCoords(new Vector3(0.0f, -5f, 0.0f));
        this._moneyBlip = World.CreateBlip(offsetInWorldCoords);
        this._moneyBlip.Color = BlipColor.Green;
        this._moneyBlip.Scale = 0.5f;
        this._moneyPickup = new Pickup(Function.Call<int>(Hash.CREATE_PICKUP_ROTATE,  -562499202,  offsetInWorldCoords.X,  offsetInWorldCoords.Y,  offsetInWorldCoords.Z,  0,  0,  0,  1,  10000,  2,  0,  0));
        UI.ShowSubtitle("Pick up the ~g~money.", 60000);
        this.Stage = 3;
      }
      if (this.Stage == 3 && this._moneyPickup != null && Game.Player.Character.IsInRangeOf(this._moneyPickup.Position, 1.2f) && !Game.Player.Character.IsInVehicle())
      {
        this._moneyBlip.Remove();
        Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullDuffelBag);
        if (Game.MaxWantedLevel == 0)
          UI.ShowSubtitle("Leave the area.", 120000);
        else
          UI.ShowSubtitle("Lose the cops.", 120000);
        ++this.Stage;
      }
      if (this.Stage == 4 && Game.Player.WantedLevel == 0 && !Game.Player.Character.IsInRangeOf(this._mainTarget.Position, 300f))
      {
        Util.SendLesterMessage("Nice work indeed. I'll text you next time I need you.");
        this._moneyBlip = World.CreateBlip(new Vector3(453.53f, -3074.97f, 5.1f));
        this._moneyBlip.ShowRoute = true;
        UI.ShowSubtitle("Return to the ~y~warehouse.", 120000);
        ++this.Stage;
      }
      if (this.Stage == 5)
      {
        World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(453.53f, -3074.97f, 5.1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), Color.Yellow);
        if (Game.Player.Character.IsInRangeOf(new Vector3(453.53f, -3074.97f, 5.1f), 3f))
        {
          this.HasFinished = true;
          this.HasWon = true;
        }
      }
      if (!this.copsAfterYou)
      {
        Game.MaxWantedLevel = 0;
        Game.Player.WantedLevel = 0;
      }
      else
        Game.MaxWantedLevel = 5;
    }

    public override void BackgroundThreadUpdate()
    {
      if (this.copsAfterYou || !this.HasPlayerAttackedTheStockade() && !this.silentApproach)
        return;
      World.SetRelationshipBetweenGroups(Relationship.Hate, this._guardRelGroup, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, this._guardRelGroup);
      Script.Wait(5000);
      if (this.silentApproach)
        return;
      Util.SendLesterMessage("I've intercepted the emergency call, you've got about 90 seconds before the cops take notice.");
      int gameTime = Game.GameTime;
      while (Game.GameTime - gameTime < 90000 && !this.HasFinished)
        Script.Yield();
      if (!this.copsAfterYou && Game.Player.Character.IsInRangeOf(this._mainTarget.Position, 300f) && !this.HasFinished)
      {
        Game.MaxWantedLevel = 5;
        if (Game.Player.WantedLevel < 3)
          Game.Player.WantedLevel = 3;
        Util.SendLesterMessage("Shit, the cops are onto you! Get out of there, pronto!");
        UI.ShowSubtitle("Lose the cops.", 120000);
      }
      this.copsAfterYou = true;
    }

    private bool HasPlayerAttackedTheStockade() => !this._guards[0].IsInVehicle(this._mainTarget) || !this._guards[1].IsInVehicle(this._mainTarget) || !this._guards[1].IsAlive || !this._guards[0].IsAlive || Util.HasPedDamagedEntity(Game.Player.Character, (Entity) this._mainTarget);
  }
}
