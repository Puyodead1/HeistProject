// Decompiled with JetBrains decompiler
// Type: ExternalScript.SetupHeistLogic
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject;
using HeistProject.GUI;
using System.Drawing;

namespace ExternalScript
{
  public class SetupHeistLogic : ScriptedLogic
  {
    private Vehicle _mainTarget;
    private Ped[] _guards;
    private int _guardRelGroup;
    private BarTimerBar _bar;
    private TextTimerBar _timeBar;
    private int _tailSpan;
    private int _timeLeft;
    private Blip _destination;
    private int Stage;

    public override void Start()
    {
      this._guardRelGroup = World.AddRelationshipGroup("HEIST_GUARDS");
      World.SetRelationshipBetweenGroups(Relationship.Neutral, this._guardRelGroup, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Neutral, Game.Player.Character.RelationshipGroup, this._guardRelGroup);
      Model model1 = new Model(VehicleHash.Stockade);
      model1.Request(10000);
      Vector3 positionOnStreet = World.GetNextPositionOnStreet(Vector3.RandomXY() * 600f, true);
      this._mainTarget = World.CreateVehicle(model1, positionOnStreet);
      this._mainTarget.AddBlip();
      this._mainTarget.CurrentBlip.Sprite = BlipSprite.SonicWave;
      this._mainTarget.CurrentBlip.Color = BlipColor.Blue;
      Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, (InputArgument) this._mainTarget.Handle, (InputArgument) 1);
      model1.MarkAsNoLongerNeeded();
      this._guards = new Ped[2];
      for (int index = 0; index < 2; ++index)
      {
        Model model2 = new Model(Util.SharedRandom.Next(0, 2) == 1 ? PedHash.Armoured01SMM : PedHash.Armoured02SMM);
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
        Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) this._guards[index], (InputArgument) -1074790547, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
        this._guards[index].BlockPermanentEvents = false;
        this._guards[index].RelationshipGroup = this._guardRelGroup;
        this._guards[index].Armor = 200;
        Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, (InputArgument) this._guards[index].Handle, (InputArgument) 3);
        model2.MarkAsNoLongerNeeded();
      }
      this._guards[0].Task.CruiseWithVehicle(this._mainTarget, 10f, 786603);
    }

    public override void End()
    {
      this.HasFinished = true;
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
      this._mainTarget.Delete();
      if (Blip.op_Inequality(this._destination, (Blip) null))
        this._destination.Remove();
      for (int index = 0; index < this._guards.Length; ++index)
        this._guards[index].Delete();
      TimerBarPool.Remove((TimerBarBase) this._bar);
      TimerBarPool.Remove((TimerBarBase) this._timeBar);
      UI.ShowSubtitle("");
    }

    public override void Update()
    {
      if (this.Stage == 0)
      {
        UI.ShowSubtitle("Catch up to the ~b~Stockade~w~.", 120000);
        ++this.Stage;
      }
      if (this.Stage == 1 && Game.Player.Character.IsInRangeOf(this._mainTarget.Position, 100f))
      {
        UI.ShowSubtitle("Tail the ~b~Stockade~w~ without being noticed.", 120000);
        ++this.Stage;
        this._bar = new BarTimerBar("DISTANCE");
        this._bar.Percentage = 0.0f;
        TimerBarPool.Add((TimerBarBase) this._bar);
        this._timeBar = new TextTimerBar("TIME LEFT", "3:00");
        TimerBarPool.Add((TimerBarBase) this._timeBar);
        Util.SendLesterMessage("Alright I'm receiving the signal. Keep at it but don't get too close or they'll notice you.");
        this._tailSpan = Game.GameTime;
        this._timeLeft = 180000;
      }
      if (this.Stage == 2)
      {
        float num1 = (this._mainTarget.Position - Game.Player.Character.Position).Length();
        float num2 = (float) ((Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, (InputArgument) this._mainTarget.Handle, (InputArgument) Game.Player.Character.Handle, (InputArgument) 17) ? 10.0 : 0.0) + ((double) num1 < 100.0 ? 100.0 - (double) num1 : 0.0));
        if ((double) num2 > 100.0)
        {
          World.SetRelationshipBetweenGroups(Relationship.Hate, this._guardRelGroup, Game.Player.Character.RelationshipGroup);
          World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, this._guardRelGroup);
          UI.ShowSubtitle("");
          Script.Wait(5000);
          this.FailureReason = "They have noticed you.";
          this.HasFinished = true;
          return;
        }
        if ((double) num2 > 0.0)
        {
          this._timeLeft -= Game.GameTime - this._tailSpan;
          this._tailSpan = Game.GameTime;
          if (this._timeLeft <= 0)
          {
            ++this.Stage;
            Util.SendLesterMessage("Alright, I've got the routes. Nice work!");
            UI.ShowSubtitle("Return to the ~y~hideout.", 60000);
            TimerBarPool.Remove((TimerBarBase) this._bar);
            TimerBarPool.Remove((TimerBarBase) this._timeBar);
            this._destination = World.CreateBlip(new Vector3(453.53f, -3074.97f, 5.1f));
            this._destination.ShowRoute = true;
            this._mainTarget.CurrentBlip.Remove();
            return;
          }
          int num3 = this._timeLeft / 60000;
          int num4 = this._timeLeft / 1000 % 60;
          int num5 = this._timeLeft % 1000;
          this._timeBar.Text = string.Format("{0}:{1:00}", (object) num3, (object) num4);
        }
        else
          this._tailSpan = Game.GameTime;
        this._bar.Percentage = num2 / 100f;
      }
      if (this.Stage != 3)
        return;
      World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(453.53f, -3074.97f, 5.1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), Color.Yellow);
      if (!Game.Player.Character.IsInRangeOf(new Vector3(453.53f, -3074.97f, 5.1f), 3f))
        return;
      this.HasFinished = true;
      this.HasWon = true;
    }

    public override void BackgroundThreadUpdate()
    {
      if (!this.HasPlayerAttackedTheStockade())
        return;
      UI.ShowSubtitle("");
      World.SetRelationshipBetweenGroups(Relationship.Hate, this._guardRelGroup, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, this._guardRelGroup);
      Script.Wait(5000);
      this.FailureReason = "They have noticed you.";
      this.HasFinished = true;
    }

    private bool HasPlayerAttackedTheStockade() => !this._guards[0].IsInVehicle(this._mainTarget) || !this._guards[1].IsInVehicle(this._mainTarget) || !this._guards[1].IsAlive || !this._guards[0].IsAlive || Util.HasPedDamagedEntity(Game.Player.Character, (Entity) this._mainTarget);
  }
}
