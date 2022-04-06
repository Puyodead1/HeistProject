// Decompiled with JetBrains decompiler
// Type: InstaScript.PacificFinale
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject;
using HeistProject.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace InstaScript
{
  public class PacificFinale : ScriptedLogic
  {
    private Vector3 _getawayCarSave = new Vector3(3.75f, 109.76f, 77.76f);
    private float _getawayCarHeading = 156.81f;
    private int _thermiteSceneId;
    private BarTimerBar _hostageControl;
    private List<Hostage> Hostages;
    private Vector3 _bankVehDestination = new Vector3(233.88f, 197.78f, 104.21f);
    private PosInfo[] _poses = new PosInfo[10]
    {
      new PosInfo(252.29f, 223.26f, 106.29f, 161.04f),
      new PosInfo(249.23f, 224.3f, 106.29f, 164.15f),
      new PosInfo(254.34f, 215.14f, 106.29f, 337.38f),
      new PosInfo(248.28f, 214.7f, 106.29f, 260.87f),
      new PosInfo(248.37f, 209.75f, 106.29f, 341.5f),
      new PosInfo(251.46f, 220.97f, 106.29f, 345.9f),
      new PosInfo(248.42f, 215.99f, 106.29f, 350.05f),
      new PosInfo(256.21f, 215.02f, 106.29f, 79.89f),
      new PosInfo(238.38f, 223.56f, 106.29f, 304.87f),
      new PosInfo(242.53f, 212.94f, 106.29f, 162.38f)
    };
    private PosInfo _crowdControlPos1 = new PosInfo(241.42f, 219.77f, 106.29f, 265.58f);
    private PosInfo _crowdControlPos2 = new PosInfo(256.15f, 212.35f, 106.29f, 57.64f);
    private int _soundid = -1;
    private int _alarmSoundid = -1;
    private int _crowdControlStart;
    private int _thermalChargeStart;
    private bool _hasHostagesBeenSpooked;
    private float _hostageIntimidation;
    private int _thermalFxHandle;
    private Prop _thermiteProp;
    private Prop _bagProp;
    private Prop _card;
    private bool _thermiteStarted;
    private bool _hasThermiteSceneStarted;
    private bool _vaultDoorOpen;
    private int _lesterMsgStart;
    private int _lesterMsgCount;
    private bool _frontDoorsOpen;
    private bool _firstGateOpen;
    private bool _secondGateOpen;
    private Vehicle _titan;
    private Ped _titanPilot;
    private int _take;
    private PosInfo _thermalCharge1Pos = new PosInfo(256.92f, 220.41f, 106.29f, 345.25f);
    private PosInfo _thermalCharge2Pos = new PosInfo(261.99f, 221.53f, 106.28f, 251.91f);
    private PosInfo _currentThermalPos;
    private bool _soundAlarm;
    private bool _firstAlarmSound;
    private PosInfo _hackInfo = new PosInfo(253.59f, 228.3f, 100.68f, 73.15f);
    private PosInfo _guard1Pos = new PosInfo(264.28f, 219.85f, 100.68f, 314.01f);
    private PosInfo _guard2Pos = new PosInfo(258.14f, 223.55f, 100.87f, 315.41f);
    private PosInfo _cashGrabPos1 = new PosInfo(263.01f, 215.64f, 100.68f, 346.97f);
    private PosInfo _cashGrabPos2 = new PosInfo(262.51f, 213.79f, 100.68f, 102.85f);
    private Vector3 _vaultPos = new Vector3(255.2283f, 223.976f, 102.3932f);
    private Vector3 _vaultRot = new Vector3(0.0f, 0.0f, 160f);
    private Vector3 _bankEntrance = new Vector3(234.94f, 217.11f, 106.29f);
    private int _alleywayIndex;
    private Vector3[] _alleywayCheckpoints = new Vector3[5]
    {
      new Vector3(269.56f, 149.55f, 103.49f),
      new Vector3(232.84f, 45.51f, 82.97f),
      new Vector3(182.66f, 64.46f, 82.67f),
      new Vector3(65.93f, 113.29f, 78.11f),
      new Vector3(19.14f, 118.51f, 78.08f)
    };
    private int _vaultHash = 961976194;
    private Ped _guard1;
    private Ped _guard2;
    private Blip _destBlip;
    private Vehicle _vehTarget;
    private List<Entity> _cleanupBag;
    private TextTimerBar _takeBar;
    private Prop _cashGrabTray1;
    private Prop _cashGrabTray2;
    private bool _isGrabbingMoney;
    private int _lastMoneyGrab;
    private bool _nooseCalled;
    private bool _lastcar;

    private int Stage { get; set; }

    public override void Start()
    {
      this.Stage = -2;
      ScriptHandler.Log("Starting Pac Stand finale " + (object) DateTime.Now);
      ScriptHandler.Log("Game version: " + (object) Game.Version);
      this._cleanupBag = new List<Entity>();
      this._frontDoorsOpen = true;
      Function.Call(Hash.REQUEST_MISSION_AUDIO_BANK, (InputArgument) "Vault_Door", (InputArgument) 0);
      Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS");
      this._destBlip = World.CreateBlip(this._bankVehDestination);
      this._destBlip.Color = BlipColor.Yellow;
      this._destBlip.ShowRoute = true;
      Util.SetPedAccessory(EntryPoint.Team[3], Util.HeistAccessory.FullDuffelBag);
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullDuffelBag);
      this.Hostages = new List<Hostage>();
      for (int index = 0; index < this._poses.Length; ++index)
        this.Hostages.Add(new Hostage(this._poses[index], index));
      Model model1 = new Model(PedHash.Armoured01SMM);
      model1.Request(10000);
      Model model2 = new Model(269934519);
      model2.Request(10000);
      this._cleanupBag.Add((Entity) (this._cashGrabTray1 = World.CreateProp(model2, new Vector3(262.0326f, 213.2618f, 101.16f), new Vector3(0.0f, 0.0f, -39.99998f), false, false)));
      this._cashGrabTray1.FreezePosition = true;
      this._cashGrabTray1.Position = new Vector3(262.0326f, 213.2618f, 101.16f);
      this._cleanupBag.Add((Entity) (this._cashGrabTray2 = World.CreateProp(model2, new Vector3(262.8111f, 216.412f, 101.16f), new Vector3(0.0f, 0.0f, -154.9996f), false, false)));
      this._cashGrabTray2.FreezePosition = true;
      this._cashGrabTray2.Position = new Vector3(262.8111f, 216.412f, 101.16f);
      this._guard1 = World.CreatePed(model1, this._guard1Pos.Position, this._guard1Pos.Heading);
      this._guard2 = World.CreatePed(model1, this._guard2Pos.Position, this._guard2Pos.Heading);
      this._cleanupBag.Add((Entity) this._guard1);
      this._cleanupBag.Add((Entity) this._guard2);
      int num = World.AddRelationshipGroup("PACIFIC_STANDARD_FINALE_GUARDS");
      World.SetRelationshipBetweenGroups(Relationship.Hate, num, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, num);
      this._guard1.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, (InputArgument) this._guard1.Handle, (InputArgument) 1);
      this._guard1.BlockPermanentEvents = true;
      Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) this._guard1, (InputArgument) 453432689, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) this._guard1, (InputArgument) -1074790547, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
      this._guard2.RelationshipGroup = num;
      Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, (InputArgument) this._guard2.Handle, (InputArgument) 1);
      this._guard2.BlockPermanentEvents = true;
      Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) this._guard2, (InputArgument) 453432689, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) this._guard2, (InputArgument) -1074790547, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
      model1.MarkAsNoLongerNeeded();
      model2.MarkAsNoLongerNeeded();
    }

    public override void End()
    {
      TimerBarPool.Remove((TimerBarBase) this._hostageControl);
      this._soundAlarm = false;
      if (this._takeBar != null)
        TimerBarPool.Remove((TimerBarBase) this._takeBar);
      if (this._thermalFxHandle != 0)
      {
        if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, (InputArgument) this._thermalFxHandle))
          Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, (InputArgument) this._thermalFxHandle, (InputArgument) 0);
      }
      if ((Entity) this._titan != (Entity) null)
        this._titan.Delete();
      if ((Entity) this._titanPilot != (Entity) null)
        this._titanPilot.Delete();
      if ((Entity) this._card != (Entity) null)
        this._card.Delete();
      if ((Entity) this._bagProp != (Entity) null)
        this._bagProp.Delete();
      if ((Entity) this._thermiteProp != (Entity) null)
        this._thermiteProp.Delete();
      if ((Entity) this._vehTarget != (Entity) null)
        this._vehTarget.Delete();
      if (Blip.op_Inequality(this._destBlip, (Blip) null))
        this._destBlip.Remove();
      if (this._cleanupBag != null)
      {
        foreach (Entity entity in this._cleanupBag)
          entity.Delete();
      }
      foreach (Hostage hostage in this.Hostages)
        hostage.Clean();
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
      UI.ShowSubtitle(" ");
      Game.Player.Character.FreezePosition = false;
      World.RenderingCamera = (Camera) null;
      Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, (InputArgument) "ALARMS_SOUNDSET");
      Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, (InputArgument) "HEIST_FLEECA_DRILL");
      Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, (InputArgument) "HEIST_FLEECA_DRILL_2");
      Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_HEIST_FLEECA_SOUNDSET");
      if (this._soundid != -1)
      {
        Function.Call(Hash.STOP_SOUND, (InputArgument) this._soundid);
        Function.Call(Hash.RELEASE_SOUND_ID, (InputArgument) this._soundid);
      }
      if (this._alarmSoundid != -1)
      {
        Function.Call(Hash.STOP_SOUND, (InputArgument) this._alarmSoundid);
        Function.Call(Hash.RELEASE_SOUND_ID, (InputArgument) this._alarmSoundid);
      }
      Game.Player.CanControlCharacter = true;
      if ((Entity) this._cashGrabTray1 != (Entity) null)
        this._cashGrabTray1.Delete();
      if (!((Entity) this._cashGrabTray2 != (Entity) null))
        return;
      this._cashGrabTray2.Delete();
    }

    public override void Update()
    {
      if (!this._vaultDoorOpen)
        Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) 961976194, (InputArgument) this._vaultPos.X, (InputArgument) this._vaultPos.Y, (InputArgument) this._vaultPos.Z, (InputArgument) true, (InputArgument) 0, (InputArgument) 0);
      Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) 110411286, (InputArgument) 232.6054f, (InputArgument) 214.1584f, (InputArgument) 106.4049f, (InputArgument) !this._frontDoorsOpen, (InputArgument) 0, (InputArgument) 0);
      Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) 110411286, (InputArgument) 231.5123f, (InputArgument) 216.5177f, (InputArgument) 106.4049f, (InputArgument) !this._frontDoorsOpen, (InputArgument) 0, (InputArgument) 0);
      Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) 110411286, (InputArgument) 260.6432f, (InputArgument) 203.2052f, (InputArgument) 106.4049f, (InputArgument) !this._frontDoorsOpen, (InputArgument) 0, (InputArgument) 0);
      Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) 110411286, (InputArgument) 258.2022f, (InputArgument) 204.1005f, (InputArgument) 106.4049f, (InputArgument) !this._frontDoorsOpen, (InputArgument) 0, (InputArgument) 0);
      Model model1 = new Model("hei_v_ilev_bk_gate_pris");
      Model model2 = new Model("hei_v_ilev_bk_gate2_pris");
      Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) model1.Hash, (InputArgument) 256.9158f, (InputArgument) 220.5128f, (InputArgument) 105.2852f, (InputArgument) !this._firstGateOpen, (InputArgument) 0, (InputArgument) 0);
      Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) model1.Hash, (InputArgument) 261.9825f, (InputArgument) 221.8922f, (InputArgument) 105.2847f, (InputArgument) !this._secondGateOpen, (InputArgument) 0, (InputArgument) 0);
      Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) model2.Hash, (InputArgument) 256.9158f, (InputArgument) 220.5128f, (InputArgument) 105.2852f, (InputArgument) !this._firstGateOpen, (InputArgument) 0, (InputArgument) 0);
      Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) model2.Hash, (InputArgument) 261.9825f, (InputArgument) 221.8922f, (InputArgument) 105.2847f, (InputArgument) !this._secondGateOpen, (InputArgument) 0, (InputArgument) 0);
      if (this._soundAlarm && this._alarmSoundid == -1)
      {
        this._alarmSoundid = Function.Call<int>(Hash.GET_SOUND_ID);
        Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_HEIST_FLEECA_SOUNDSET", (InputArgument) 1);
        Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "ALARMS_SOUNDSET", (InputArgument) 1);
        for (int index = 0; index < 200; ++index)
        {
          if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "ALARM_BELL_01", (InputArgument) 1))
          {
            Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_HEIST_FLEECA_SOUNDSET", (InputArgument) 1);
            Script.Yield();
          }
          else
            break;
        }
      }
      if (this._soundAlarm)
      {
        if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, (InputArgument) this._alarmSoundid) || !this._firstAlarmSound)
        {
          Function.Call(Hash.PLAY_SOUND_FROM_COORD, (InputArgument) this._alarmSoundid, (InputArgument) "Bell_01", (InputArgument) this._vaultPos.X, (InputArgument) this._vaultPos.Y, (InputArgument) this._vaultPos.Z, (InputArgument) "ALARMS_SOUNDSET", (InputArgument) 0, (InputArgument) 0, (InputArgument) 0);
          this._firstAlarmSound = true;
        }
      }
      if (this._take != 0)
      {
        if (this._takeBar == null)
        {
          this._takeBar = new TextTimerBar("TAKE", "$0");
          TimerBarPool.Add((TimerBarBase) this._takeBar);
        }
        this._takeBar.Text = this._take.ToString("C0");
      }
      if (this.Stage == -1)
      {
        if (Game.GameTime - this._lesterMsgStart > 5000 && this._lesterMsgCount == 0)
        {
          Util.SendLesterMessage("Woo, here it is, keep it simple. You go in heavy, you go in well-armed.");
          ++this._lesterMsgCount;
        }
        if (Game.GameTime - this._lesterMsgStart > 15000 && this._lesterMsgCount == 1)
        {
          Util.SendLesterMessage("Some of you will do crowd control. The vault team, you will go downstairs.");
          ++this._lesterMsgCount;
        }
        if (Game.GameTime - this._lesterMsgStart > 25000 && this._lesterMsgCount == 2)
        {
          Util.SendLesterMessage("Blow the first two doors with the thermal charges, then hack the vault door with the equipment.");
          ++this._lesterMsgCount;
        }
        if (Game.GameTime - this._lesterMsgStart > 40000 && this._lesterMsgCount == 3)
        {
          Util.SendLesterMessage("Then you'll have about two minutes before every cop in town is waiting for you outside the bank.");
          ++this._lesterMsgCount;
        }
        if (Game.GameTime - this._lesterMsgStart > 50000 && this._lesterMsgCount == 4)
        {
          Util.SendLesterMessage("Move fast, get in the van and meet at the rendezvous point where a pilot will be waiting for you.");
          ++this._lesterMsgCount;
        }
      }
      if (this.Stage == -2)
      {
        ++this.Stage;
        UI.ShowSubtitle("Go to the ~y~Pacific Standard Bank.", 120000);
        this._lesterMsgStart = Game.GameTime;
      }
      if (this.Stage >= 7)
      {
        Game.MaxWantedLevel = 5;
        Game.Player.WantedLevel = 5;
        Function.Call(Hash.SET_PLAYER_WANTED_CENTRE_POSITION, (InputArgument) Game.Player, (InputArgument) Game.Player.Character.Position.X, (InputArgument) Game.Player.Character.Position.Y, (InputArgument) Game.Player.Character.Position.Z);
      }
      if ((Entity) this._titan != (Entity) null)
        Function.Call(Hash.SET_VEHICLE_DOOR_CONTROL, (InputArgument) this._titan.Handle, (InputArgument) 5, (InputArgument) 0, (InputArgument) 2f);
      if (this.Stage == 7 && Game.Player.Character.IsInRangeOf(this._bankEntrance, 2f))
      {
        if (Blip.op_Inequality(this._destBlip, (Blip) null))
          this._destBlip.Remove();
        foreach (Entity allVehicle in World.GetAllVehicles())
          allVehicle.Delete();
        this._frontDoorsOpen = true;
        ++this.Stage;
        Model model3 = new Model(VehicleHash.GBurrito2);
        model3.Request(10000);
        this._vehTarget = World.CreateVehicle(model3, this._getawayCarSave, 340f);
        model3.MarkAsNoLongerNeeded();
        this.SpawnCops();
        Model model4 = new Model(VehicleHash.Titan);
        model4.Request(10000);
        this._titan = World.CreateVehicle(model4, new Vector3(1706.87f, 3249.65f, 40.85f), 104.04f);
        this._titan.IsInvincible = true;
        model4.MarkAsNoLongerNeeded();
        this._cleanupBag.Add((Entity) this._titan);
        this._titan.OpenDoor(VehicleDoor.Trunk, true, true);
        this._titanPilot = World.CreateRandomPed(new Vector3(1706.87f, 3249.65f, 40.85f));
        this._cleanupBag.Add((Entity) this._titanPilot);
        this._titanPilot.BlockPermanentEvents = true;
        this._titanPilot.SetIntoVehicle(this._titan, VehicleSeat.Driver);
        UI.ShowSubtitle("Go to the ~y~alley.", 120000);
        this._destBlip = World.CreateBlip(this._alleywayCheckpoints[0]);
        this._destBlip.Color = BlipColor.Yellow;
        EntryPoint.Team[1].BlockPermanentEvents = false;
        EntryPoint.Team[2].BlockPermanentEvents = false;
        EntryPoint.Team[3].BlockPermanentEvents = false;
      }
      if (this.Stage == 8 && Game.Player.Character.IsInRangeOf(this._alleywayCheckpoints[this._alleywayIndex], 3f))
      {
        ++this._alleywayIndex;
        UI.ShowSubtitle("Continue through the ~y~alley.", 120000);
        if (this._alleywayIndex >= this._alleywayCheckpoints.Length)
        {
          ++this.Stage;
          UI.ShowSubtitle("Get in the ~b~Burrito.", 120000);
          this._vehTarget.AddBlip();
          this._vehTarget.CurrentBlip.Color = BlipColor.Blue;
          this._destBlip.Remove();
          this._destBlip = (Blip) null;
        }
        else
          this._destBlip.Position = this._alleywayCheckpoints[this._alleywayIndex];
      }
      if (this.Stage == 8)
        Util.DrawEntryMarker(this._alleywayCheckpoints[this._alleywayIndex], 2f);
      if (this.Stage == 9 && Game.Player.Character.IsInVehicle(this._vehTarget))
      {
        this._destBlip = World.CreateBlip(new Vector3(1811.2f, 3269.72f, 43.18f));
        this._destBlip.Color = BlipColor.Yellow;
        this._destBlip.ShowRoute = true;
        UI.ShowSubtitle("Go to the ~y~Sandy Shores Airfield.", 120000);
        ++this.Stage;
      }
      if (this.Stage == 11 && (Entity) this._titan != (Entity) null)
      {
        if (Game.Player.Character.IsInRangeOf(this._titan.Position, 10f))
        {
          this.MissionChallenges.Add("NOOSE Not called", new Tuple<string, bool>("", !this._nooseCalled));
          this.MissionChallenges.Add("No hostages killed", new Tuple<string, bool>("", this.Hostages.All<Hostage>((Func<Hostage, bool>) (h => h.Ped.IsAlive))));
          this.HasWon = true;
          this.HasFinished = true;
        }
        else
        {
          this.FailureReason = "The Titan left without you.";
          this.HasFinished = true;
        }
      }
      if (this.Stage >= 1 && this.Stage < 7 && Game.GameTime - this._crowdControlStart > 10000)
      {
        this._crowdControlStart = Game.GameTime;
        Hostage hostage1 = this.Hostages[Util.SharedRandom.Next(this.Hostages.Count)];
        Hostage hostage2 = this.Hostages[Util.SharedRandom.Next(this.Hostages.Count)];
        EntryPoint.Team[1].Task.AimAt((Entity) hostage1.Ped, -1);
        EntryPoint.Team[2].Task.AimAt((Entity) hostage2.Ped, -1);
        hostage1.Scream();
        hostage2.Scream();
      }
      if (this.Stage == -1)
        Util.DrawEntryMarker(this._bankVehDestination, 2f);
      if (this.Stage == -1 && Game.Player.Character.IsInRangeOf(this._bankVehDestination, 3f))
      {
        this._destBlip.Remove();
        UI.ShowSubtitle("Enter the bank.", 120000);
        Util.SendLesterMessage("Good luck people, nothing can go wrong.");
        ++this.Stage;
      }
      if (this.Stage == 0 && (Game.Player.Character.IsShooting || this.Hostages.Count<Hostage>((Func<Hostage, bool>) (h => Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, (InputArgument) h.Ped.Handle, (InputArgument) Game.Player.Character, (InputArgument) 17))) > 3) && !this._hasHostagesBeenSpooked)
      {
        EntryPoint.Team[1].Position = this._bankEntrance + new Vector3(0.0f, -2f, 0.0f);
        EntryPoint.Team[2].Position = this._bankEntrance + new Vector3(0.0f, 2f, 0.0f);
        EntryPoint.Team[3].Position = this._bankEntrance + new Vector3(2f, 0.0f, 0.0f);
        this.Hostages.ForEach((Action<Hostage>) (h => h.Spook()));
        this._hasHostagesBeenSpooked = true;
        this._hostageControl = new BarTimerBar("INTIMIDATION");
        this._hostageControl.ForegroundColor = Color.Orange;
        this._hostageControl.BackgroundColor = Color.FromArgb(112, 88, 28);
        this._hostageControl.Percentage = 1f;
        TimerBarPool.Add((TimerBarBase) this._hostageControl);
        this._hostageIntimidation = 1f;
        EntryPoint.Team[1].Task.RunTo(this._crowdControlPos1.Position);
        EntryPoint.Team[2].Task.RunTo(this._crowdControlPos2.Position);
        EntryPoint.Team[1].BlockPermanentEvents = true;
        EntryPoint.Team[2].BlockPermanentEvents = true;
        this.StartDoorMelting(this._thermalCharge1Pos, new Vector3(0.0f, 1f, 0.0f));
        this._crowdControlStart = Game.GameTime;
        this._frontDoorsOpen = false;
        UI.ShowSubtitle("Control the crowd.", 120000);
        Util.SendLesterMessage("Keep the crowd in check. Get in their faces, shoot near them if you have to.");
        ++this.Stage;
      }
      if (this.Stage >= 1)
        this.Hostages.ForEach((Action<Hostage>) (p => p.Scream()));
      if (this.Stage >= 1 && this.Stage <= 3)
      {
        this._hostageIntimidation -= 1f / 500f;
        if (Game.IsControlPressed(0, Control.Aim))
          this._hostageIntimidation += 1f / 1000f;
        if (Game.Player.Character.IsShooting)
        {
          float num = 0.0f;
          switch (Game.Player.Character.Weapons.Current.Hash)
          {
            case WeaponHash.MicroSMG:
              num = 0.035f;
              goto case WeaponHash.Unarmed;
            case WeaponHash.PumpShotgun:
              num = 0.05f;
              goto case WeaponHash.Unarmed;
            case WeaponHash.Unarmed:
              this._hostageIntimidation += num;
              if ((double) num > 0.0)
              {
                this.Hostages.ForEach((Action<Hostage>) (p => p.Scream()));
                break;
              }
              break;
            default:
              num = 0.03f;
              goto case WeaponHash.Unarmed;
          }
        }
        if ((double) this._hostageIntimidation > 1.0)
          this._hostageIntimidation = 1f;
        if ((double) this._hostageIntimidation < 0.0)
          this._hostageIntimidation = 0.0f;
        this._hostageControl.Percentage = this._hostageIntimidation;
      }
      if (this.Stage == 2)
      {
        this._firstGateOpen = true;
        this.StartDoorMelting(this._thermalCharge2Pos, new Vector3(1f, -0.5f, 0.0f));
        ++this.Stage;
      }
      if (this.Stage == 4)
      {
        this._guard1.BlockPermanentEvents = false;
        this._guard2.BlockPermanentEvents = false;
        EntryPoint.Team[1].BlockPermanentEvents = true;
        EntryPoint.Team[2].BlockPermanentEvents = true;
        TimerBarPool.Remove((TimerBarBase) this._hostageControl);
        this._secondGateOpen = true;
        this._destBlip = World.CreateBlip(this._hackInfo.Position);
        this._destBlip.Color = BlipColor.Green;
        this._destBlip.ShowRoute = false;
        this._destBlip.Scale = 0.7f;
        UI.ShowSubtitle("Hack the ~g~control panel~w~ for the bank vault.", 120000);
        Util.SendLesterMessage("Hacker, you're up. We've repackaged that other crew's hardware.");
        ++this.Stage;
      }
      if (this._hasThermiteSceneStarted && (Entity) this._bagProp != (Entity) null && Game.GameTime - this._thermalChargeStart > 7000 && !this._thermiteStarted)
      {
        while (true)
        {
          if (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, (InputArgument) "pat_heist"))
          {
            Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, (InputArgument) "pat_heist");
            Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, (InputArgument) "scr_ornate_heist");
            Script.Yield();
          }
          else
            break;
        }
        Function.Call(Hash._SET_PTFX_ASSET_NEXT_CALL, (InputArgument) "pat_heist");
        Function.Call(Hash._0x6F60E89A7B64EE1D, (InputArgument) "scr_heist_ornate_thermal_burn_patch", (InputArgument) this._thermiteProp, (InputArgument) 0, (InputArgument) 1f, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 1065353216, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0);
        this._thermiteStarted = true;
      }
      if (Game.GameTime - this._thermalChargeStart > 30000 && this._thermalChargeStart != 0 && this._thermiteStarted)
      {
        Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS");
        Function.Call(Hash.PLAY_SOUND_FROM_COORD, (InputArgument) -1, (InputArgument) "Gate_Lock_Break", (InputArgument) this._thermiteProp.Position.X, (InputArgument) this._thermiteProp.Position.Y, (InputArgument) this._thermiteProp.Position.Z, (InputArgument) "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", (InputArgument) 1, (InputArgument) 30, (InputArgument) 0);
        Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, (InputArgument) this._thermalFxHandle, (InputArgument) 0);
        this._thermiteProp.Delete();
        this._bagProp.Delete();
        Util.SetPedAccessory(EntryPoint.Team[3], Util.HeistAccessory.FullDuffelBag);
        EntryPoint.Team[3].Task.ClearAll();
        this._thermiteStarted = false;
        this._thermiteProp = (Prop) null;
        this._bagProp = (Prop) null;
        this._hasThermiteSceneStarted = false;
        this._thermalChargeStart = 0;
        Function.Call(Hash._0xCD9CC7E200A52A6F, (InputArgument) this._thermiteSceneId);
        ++this.Stage;
      }
      if (this.Stage >= 1 && this.Stage < 7)
      {
        Game.MaxWantedLevel = 0;
        Game.Player.WantedLevel = 0;
      }
      if (this.Stage < 7)
        return;
      Function.Call(Hash.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, (InputArgument) 0.0f);
      Function.Call(Hash.SET_RANDOM_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, (InputArgument) 0.0f);
      Function.Call(Hash.SET_PARKED_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, (InputArgument) 0.0f);
      Function.Call(Hash.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME, (InputArgument) 0.0f);
      Function.Call(Hash.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME, (InputArgument) 0.0f, (InputArgument) 0.0f);
      Function.Call(Hash._0x2F9A292AD0A3BD89);
      Function.Call(Hash._0x5F3B7749C112D552);
    }

    private void StartDoorMelting(PosInfo door, Vector3 offset)
    {
      EntryPoint.Team[3].Weapons.Select(EntryPoint.Team[3].Weapons[WeaponHash.Unarmed]);
      this._currentThermalPos = door;
      using (TaskSequence sequence = new TaskSequence())
      {
        sequence.AddTask.RunTo(door.Position - offset);
        sequence.AddTask.SlideTo(door.Position - offset, door.Heading);
        sequence.AddTask.Wait(1000);
        sequence.Close();
        EntryPoint.Team[3].Task.PerformSequence(sequence);
      }
    }

    public override void BackgroundThreadUpdate()
    {
      if (this.Stage == 10 && Game.Player.Character.IsInRangeOf(new Vector3(1811.2f, 3269.72f, 43.18f), 100f))
      {
        if (Blip.op_Inequality(this._destBlip, (Blip) null))
          this._destBlip.Remove();
        this._titan.AddBlip();
        this._titan.CurrentBlip.Color = BlipColor.Blue;
        UI.ShowSubtitle("Get inside the ~b~Titan.", 120000);
        Vector3 vector3 = new Vector3(631.5f, 2958.58f, 90f);
        Function.Call(Hash.TASK_PLANE_LAND, (InputArgument) this._titanPilot, (InputArgument) this._titan, (InputArgument) 1260.24f, (InputArgument) 3130.67f, (InputArgument) 39.76f, (InputArgument) vector3.X, (InputArgument) vector3.Y, (InputArgument) vector3.Z);
        Script.Wait(30000);
        Function.Call(Hash.TASK_PLANE_MISSION, (InputArgument) this._titanPilot.Handle, (InputArgument) this._titan.Handle, (InputArgument) 0, (InputArgument) 0, (InputArgument) vector3.X, (InputArgument) vector3.Y, (InputArgument) vector3.Z, (InputArgument) 4, (InputArgument) 10f, (InputArgument) 0.0f, (InputArgument) 90f, (InputArgument) 0, (InputArgument) -1000f);
        Script.Wait(20000);
        ++this.Stage;
      }
      if (this.Stage >= 1 && this.Stage <= 3 && (double) this._hostageIntimidation <= 0.0 && this.Hostages.All<Hostage>((Func<Hostage, bool>) (h => !h.HasRebeled)))
      {
        Hostage hostage = this.Hostages.OrderBy<Hostage, int>((Func<Hostage, int>) (k => Util.SharedRandom.Next(this.Hostages.Count))).FirstOrDefault<Hostage>((Func<Hostage, bool>) (h => h.CanRebel()));
        if (hostage != null)
        {
          hostage.Rebel();
          if (!hostage.HasGunForRebelling())
          {
            Script.Wait(6000);
            this._soundAlarm = true;
            this._nooseCalled = true;
            Util.SendLesterMessage("Shit! One of the hostages pressed the alarm button! The NOOSE will be waiting for you!");
          }
          else
          {
            EntryPoint.Team[1].BlockPermanentEvents = false;
            EntryPoint.Team[2].BlockPermanentEvents = false;
          }
        }
      }
      if (this.Stage == 5)
        Util.DrawEntryMarker(this._hackInfo.Position);
      if (this.Stage <= 1 && Game.Player.WantedLevel > 0)
      {
        this.FailureReason = "You attracted police attention before entering the bank.";
        this.HasFinished = true;
      }
      if (EntryPoint.Team[3].TaskSequenceProgress == 2 && (Entity) this._bagProp == (Entity) null)
      {
        Util.SetPedAccessory(EntryPoint.Team[3], Util.HeistAccessory.None);
        Model model1 = new Model("hei_p_m_bag_var22_arm_s");
        model1.Request(10000);
        this._bagProp = World.CreateProp(model1, EntryPoint.Team[3].Position, false, false);
        this._bagProp.FreezePosition = true;
        this._bagProp.HasCollision = false;
        this._bagProp.IsInvincible = true;
        model1.MarkAsNoLongerNeeded();
        Model model2 = new Model("hei_prop_heist_thermite_flash");
        model2.Request(10000);
        this._thermiteProp = World.CreateProp(model2, EntryPoint.Team[3].Position, false, false);
        this._thermiteProp.FreezePosition = true;
        this._thermiteProp.HasCollision = false;
        this._thermiteProp.IsInvincible = true;
        model2.MarkAsNoLongerNeeded();
        Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, (InputArgument) this._thermiteProp, (InputArgument) EntryPoint.Team[3], (InputArgument) EntryPoint.Team[3].GetBoneIndex(Bone.PH_R_Hand), (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) -90f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 2, (InputArgument) 1);
        Function.Call(Hash.PLAY_ENTITY_ANIM, (InputArgument) this._thermiteProp, (InputArgument) "bomb_thermal_charge", (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@thermal_charge"), (InputArgument) 1000.0, (InputArgument) 1, (InputArgument) 1, (InputArgument) 1, (InputArgument) 0, (InputArgument) 0);
        this._thermiteSceneId = this.CreateDoorMeltingScene(EntryPoint.Team[3], this._currentThermalPos);
        this._thermalChargeStart = Game.GameTime;
        this._hasThermiteSceneStarted = true;
      }
      if (this.Stage == 5 && Game.Player.Character.IsInRangeOf(this._hackInfo.Position, 1.3f))
      {
        if (Blip.op_Inequality(this._destBlip, (Blip) null))
          this._destBlip.Remove();
        Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
        Game.Player.Character.Weapons.Select(Game.Player.Character.Weapons[WeaponHash.Unarmed]);
        Model model3 = new Model("hei_p_m_bag_var22_arm_s");
        model3.Request(10000);
        this._bagProp = World.CreateProp(model3, Game.Player.Character.Position, false, false);
        this._bagProp.FreezePosition = true;
        this._bagProp.HasCollision = false;
        this._bagProp.IsInvincible = true;
        model3.MarkAsNoLongerNeeded();
        Model model4 = new Model("hei_prop_hst_laptop");
        model4.Request(10000);
        this._thermiteProp = World.CreateProp(model4, Game.Player.Character.Position, false, false);
        this._thermiteProp.FreezePosition = true;
        this._thermiteProp.HasCollision = false;
        this._thermiteProp.IsInvincible = true;
        model4.MarkAsNoLongerNeeded();
        Model model5 = new Model("hei_prop_heist_card_hack_02");
        model5.Request(10000);
        this._card = World.CreateProp(model5, Game.Player.Character.Position, false, false);
        this._card.FreezePosition = true;
        this._card.HasCollision = false;
        this._card.IsInvincible = true;
        model5.MarkAsNoLongerNeeded();
        this.CreateBlockingHackingScene("enter");
        this.CreateBlockingHackingScene("loop");
        this.CreateBlockingHackingScene("loop");
        this.CreateBlockingHackingScene("loop");
        this.CreateBlockingHackingScene("exit");
        Function.Call(Hash.REQUEST_MISSION_AUDIO_BANK, (InputArgument) "Vault_Door", (InputArgument) 0);
        this._bagProp.Delete();
        this._thermiteProp.Delete();
        this._bagProp = (Prop) null;
        this._thermiteProp = (Prop) null;
        this._card.Delete();
        this._card = (Prop) null;
        Game.Player.Character.Task.ClearAll();
        Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullDuffelBag);
        this._vaultDoorOpen = true;
        Function.Call(Hash.PLAY_SOUND_FROM_COORD, (InputArgument) -1, (InputArgument) "vault_unlock", (InputArgument) this._vaultPos.X, (InputArgument) this._vaultPos.Y, (InputArgument) this._vaultPos.Z, (InputArgument) "dlc_heist_fleeca_bank_door_sounds", (InputArgument) 0, (InputArgument) 0, (InputArgument) 0);
        UI.ShowSubtitle("Grab the ~g~cash.", 120000);
        Util.SendLesterMessage("Boom! You're in. Get all the cash you can carry.");
        this._destBlip = World.CreateBlip(this._cashGrabPos1.Position);
        this._destBlip.Color = BlipColor.Green;
        this._destBlip.Scale = 0.7f;
        int gameTime = Game.GameTime;
        while (Game.GameTime - gameTime < 5000)
        {
          Function.Call(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, (InputArgument) 961976194, (InputArgument) this._vaultPos.X, (InputArgument) this._vaultPos.Y, (InputArgument) this._vaultPos.Z, (InputArgument) true, (InputArgument) Util.LinearFloatLerp(0.0f, -1f, Game.GameTime - gameTime, 5000), (InputArgument) 0);
          Script.Yield();
        }
        this.Stage = 6;
      }
      Model model6;
      if (this.Stage == 6 && (Entity) this._cashGrabTray1 != (Entity) null && Game.Player.Character.IsInRangeOf(this._cashGrabTray1.Position, 1.5f))
      {
        model6 = this._cashGrabTray1.Model;
        if (model6.Hash == 269934519)
        {
          Model model7 = new Model("hei_p_m_bag_var22_arm_s");
          model7.Request(10000);
          this._bagProp = World.CreateProp(model7, Game.Player.Character.Position, false, false);
          this._bagProp.FreezePosition = true;
          this._bagProp.HasCollision = false;
          this._bagProp.IsInvincible = true;
          model7.MarkAsNoLongerNeeded();
          Model model8 = new Model("hei_prop_hei_cash_trolly_03");
          model8.Request(10000);
          Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
          this._cashGrabPos2 = new PosInfo(this._cashGrabTray1.Position.X, this._cashGrabTray1.Position.Y, this._cashGrabTray1.Position.Z, this._cashGrabTray1.Heading);
          this.CreateCashBlockingGrabbingScene(Game.Player.Character, this._cashGrabPos2, "intro");
          this._lastMoneyGrab = Game.GameTime;
          this._isGrabbingMoney = true;
          this.CreateCashBlockingGrabbingScene(Game.Player.Character, this._cashGrabPos2, "grab", this._cashGrabTray1);
          this._isGrabbingMoney = false;
          this._cashGrabTray1.Delete();
          this._cashGrabTray1 = World.CreateProp(model8, new Vector3(262.0326f, 213.2618f, 101.16f), new Vector3(0.0f, 0.0f, -39.99998f), false, false);
          this._cashGrabTray1.FreezePosition = true;
          this._cashGrabTray1.Position = new Vector3(262.0326f, 213.2618f, 101.16f);
          model8.MarkAsNoLongerNeeded();
          this._take = this._take >= 600000 ? 1150000 : 575000;
          this.CreateCashBlockingGrabbingScene(Game.Player.Character, this._cashGrabPos2, "exit");
          Game.Player.Character.Task.ClearAll();
          this._bagProp.Delete();
          this._thermiteProp = (Prop) null;
          this._bagProp = (Prop) null;
          Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullDuffelBag);
        }
      }
      if (this.Stage == 6 && (Entity) this._cashGrabTray2 != (Entity) null && Game.Player.Character.IsInRangeOf(this._cashGrabTray2.Position, 1.5f))
      {
        model6 = this._cashGrabTray2.Model;
        if (model6.Hash == 269934519)
        {
          Model model9 = new Model("hei_p_m_bag_var22_arm_s");
          model9.Request(10000);
          this._bagProp = World.CreateProp(model9, Game.Player.Character.Position, false, false);
          this._bagProp.FreezePosition = true;
          this._bagProp.HasCollision = false;
          this._bagProp.IsInvincible = true;
          model9.MarkAsNoLongerNeeded();
          this._cashGrabPos1 = new PosInfo(this._cashGrabTray2.Position.X, this._cashGrabTray2.Position.Y, this._cashGrabTray2.Position.Z, this._cashGrabTray2.Heading);
          Model model10 = new Model("hei_prop_hei_cash_trolly_03");
          model10.Request(10000);
          Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
          this.CreateCashBlockingGrabbingScene(Game.Player.Character, this._cashGrabPos1, "intro");
          this._lastMoneyGrab = Game.GameTime;
          this._isGrabbingMoney = true;
          this.CreateCashBlockingGrabbingScene(Game.Player.Character, this._cashGrabPos1, "grab", this._cashGrabTray2);
          this._isGrabbingMoney = false;
          this._cashGrabTray2.Delete();
          this._cashGrabTray2 = World.CreateProp(model10, new Vector3(262.8111f, 216.412f, 101.16f), new Vector3(0.0f, 0.0f, -154.9996f), false, false);
          this._cashGrabTray2.FreezePosition = true;
          this._cashGrabTray2.Position = new Vector3(262.8111f, 216.412f, 101.16f);
          this._take = this._take >= 600000 ? 1150000 : 575000;
          model10.MarkAsNoLongerNeeded();
          this.CreateCashBlockingGrabbingScene(Game.Player.Character, this._cashGrabPos1, "exit");
          Game.Player.Character.Task.ClearAll();
          Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullDuffelBag);
          this._bagProp.Delete();
          this._bagProp = (Prop) null;
          this._thermiteProp = (Prop) null;
        }
      }
      if (this.Stage != 6 || !((Entity) this._cashGrabTray2 != (Entity) null) || !((Entity) this._cashGrabTray1 != (Entity) null))
        return;
      model6 = this._cashGrabTray1.Model;
      if (model6.Hash == 269934519)
        return;
      model6 = this._cashGrabTray2.Model;
      if (model6.Hash == 269934519)
        return;
      if (Blip.op_Inequality(this._destBlip, (Blip) null))
        this._destBlip.Remove();
      this._destBlip = World.CreateBlip(this._bankEntrance);
      this._destBlip.Color = BlipColor.Yellow;
      this._destBlip.Scale = 0.7f;
      UI.ShowSubtitle("Regroup with your team at the ~y~bank entrance.", 120000);
      EntryPoint.Team[1].BlockPermanentEvents = false;
      EntryPoint.Team[2].BlockPermanentEvents = false;
      EntryPoint.Team[1].Task.RunTo(this._bankEntrance + new Vector3(0.0f, -2f, 0.0f));
      EntryPoint.Team[2].Task.RunTo(this._bankEntrance + new Vector3(0.0f, 2f, 0.0f));
      EntryPoint.Team[3].Task.RunTo(this._bankEntrance + new Vector3(2f, 0.0f, 0.0f));
      this._soundAlarm = true;
      Game.MaxWantedLevel = 5;
      Game.Player.WantedLevel = 5;
      ++this.Stage;
    }

    public static string LoadDict(string dict)
    {
      while (true)
      {
        if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, (InputArgument) dict))
        {
          Function.Call(Hash.REQUEST_ANIM_DICT, (InputArgument) dict);
          Script.Yield();
        }
        else
          break;
      }
      return dict;
    }

    private void CreateBlockingHackingScene(string name)
    {
      Vector3 vector3 = new Vector3(252.8668f, 228.5071f, 102.0883f);
      Model model = new Model("hei_prop_hei_securitypanel");
      int num1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, (InputArgument) vector3.X, (InputArgument) vector3.Y, (InputArgument) vector3.Z, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) this._hackInfo.Heading, (InputArgument) 2);
      Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, (InputArgument) Game.Player.Character, (InputArgument) num1, (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@hack"), (InputArgument) ("hack_" + name), (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 3341, (InputArgument) 16, (InputArgument) 1148846080, (InputArgument) 0);
      Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, (InputArgument) this._bagProp, (InputArgument) num1, (InputArgument) ("hack_" + name + "_bag"), (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@hack"), (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 0, (InputArgument) 1148846080);
      Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, (InputArgument) this._thermiteProp, (InputArgument) num1, (InputArgument) ("hack_" + name + "_laptop"), (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@hack"), (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 0, (InputArgument) 1148846080);
      Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, (InputArgument) this._card, (InputArgument) num1, (InputArgument) ("hack_" + name + "_card"), (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@hack"), (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 0, (InputArgument) 1148846080);
      Function.Call(Hash._0x40FDEDB72F8293B2, (InputArgument) this._bagProp);
      int num2 = 0;
      bool flag = false;
      InputArgument[] inputArgumentArray;
      do
      {
        do
        {
          if ((double) Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, (InputArgument) num1) < 1.0)
          {
            Script.Yield();
            ++num2;
          }
          else
            goto label_5;
        }
        while (num2 <= 20);
        inputArgumentArray = new InputArgument[1]
        {
          (InputArgument) num1
        };
      }
      while ((double) Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, inputArgumentArray) != 0.0);
      flag = true;
label_5:
      if (flag)
        this.CreateBlockingHackingScene(name);
      Function.Call(Hash._0xCD9CC7E200A52A6F, (InputArgument) num1);
    }

    private int CreateDoorMeltingScene(Ped ped, PosInfo pos)
    {
      int doorMeltingScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, (InputArgument) pos.Position.X, (InputArgument) pos.Position.Y, (InputArgument) pos.Position.Z, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) pos.Heading, (InputArgument) 2);
      Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, (InputArgument) ped, (InputArgument) doorMeltingScene, (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@thermal_charge"), (InputArgument) "thermal_charge", (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 3341, (InputArgument) 16, (InputArgument) 1148846080, (InputArgument) 0);
      Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, (InputArgument) this._bagProp, (InputArgument) doorMeltingScene, (InputArgument) "bag_thermal_charge", (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@thermal_charge"), (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 0, (InputArgument) 1148846080);
      Function.Call(Hash._0x40FDEDB72F8293B2, (InputArgument) this._bagProp);
      return doorMeltingScene;
    }

    private void CreateCashBlockingGrabbingScene(Ped ped, PosInfo pos, string type, Prop cart = null)
    {
      int num1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, (InputArgument) pos.Position.X, (InputArgument) pos.Position.Y, (InputArgument) pos.Position.Z, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) pos.Heading, (InputArgument) 2);
      Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, (InputArgument) ped, (InputArgument) num1, (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@grab_cash"), (InputArgument) type, (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 3341, (InputArgument) 16, (InputArgument) 1148846080, (InputArgument) 0);
      Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, (InputArgument) this._bagProp, (InputArgument) num1, (InputArgument) ("bag_" + type), (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@grab_cash"), (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 0, (InputArgument) 1148846080);
      if ((Entity) cart != (Entity) null)
      {
        Model model = new Model("hei_prop_heist_cash_pile");
        model.Request(10000);
        this._thermiteProp = World.CreateProp(model, Game.Player.Character.Position, false, false);
        this._thermiteProp.FreezePosition = true;
        this._thermiteProp.HasCollision = false;
        this._thermiteProp.IsInvincible = true;
        this._thermiteProp.IsVisible = false;
        model.MarkAsNoLongerNeeded();
        Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, (InputArgument) this._thermiteProp, (InputArgument) Game.Player.Character, (InputArgument) Game.Player.Character.GetBoneIndex(Bone.PH_L_Hand), (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 2, (InputArgument) 1);
        Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, (InputArgument) cart, (InputArgument) num1, (InputArgument) "cart_cash_dissapear", (InputArgument) PacificFinale.LoadDict("anim@heists@ornate_bank@grab_cash"), (InputArgument) 8f, (InputArgument) -8f, (InputArgument) 0, (InputArgument) 1148846080);
      }
      Function.Call(Hash._0x40FDEDB72F8293B2, (InputArgument) this._bagProp);
      int num2 = 0;
      bool flag = false;
      InputArgument[] inputArgumentArray;
      do
      {
        do
        {
          if ((double) Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, (InputArgument) num1) < 1.0)
          {
            Script.Yield();
            if ((Entity) cart != (Entity) null)
            {
              if (Function.Call<bool>(Hash._0xEAF4CD9EA3E7E922, (InputArgument) Game.Player.Character, (InputArgument) Function.Call<int>(Hash.GET_HASH_KEY, (InputArgument) "CASH_APPEAR")) && !this._thermiteProp.IsVisible)
                this._thermiteProp.IsVisible = true;
              if (Function.Call<bool>(Hash._0xEAF4CD9EA3E7E922, (InputArgument) Game.Player.Character, (InputArgument) Function.Call<int>(Hash.GET_HASH_KEY, (InputArgument) "RELEASE_CASH_DESTROY")) && this._thermiteProp.IsVisible)
              {
                this._thermiteProp.IsVisible = false;
                this._take += 12777;
                Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "HUD_FRONTEND_CUSTOM_SOUNDSET", (InputArgument) 1);
                Function.Call(Hash.PLAY_SOUND_FRONTEND, (InputArgument) -1, (InputArgument) "ROBBERY_MONEY_TOTAL", (InputArgument) "HUD_FRONTEND_CUSTOM_SOUNDSET", (InputArgument) 1);
              }
            }
            ++num2;
          }
          else
            goto label_14;
        }
        while (num2 <= 20);
        inputArgumentArray = new InputArgument[1]
        {
          (InputArgument) num1
        };
      }
      while ((double) Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, inputArgumentArray) != 0.0);
      if ((Entity) this._thermiteProp != (Entity) null)
        this._thermiteProp.Delete();
      flag = true;
label_14:
      if (flag)
        this.CreateCashBlockingGrabbingScene(ped, pos, type);
      if ((Entity) this._thermiteProp != (Entity) null)
        this._thermiteProp.Delete();
      Function.Call(Hash._0xCD9CC7E200A52A6F, (InputArgument) num1);
    }

    private void SpawnCops()
    {
      List<Entity> entityList1 = new List<Entity>();
      List<Entity> entityList2 = new List<Entity>();
      Model model1 = new Model(2046537925);
      model1.Request(10000);
      Model model2 = new Model(1581098148);
      model2.Request(10000);
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(209.847f, 224.2409f, 105.2116f), -40.13285f));
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(207.9398f, 209.9086f, 105.1664f), -6.93387f));
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(216.6283f, 190.7955f, 105.2281f), 36.81634f));
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(206.8993f, 198.1261f, 105.1723f), -34.97464f));
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(285.4244f, 155.8807f, 103.8543f), -44.7131f));
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(289.2029f, 161.489f, 103.8673f), 156.9361f));
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(232.7252f, 41.11103f, 83.36666f), -91.69824f));
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(166.7619f, 82.96301f, 85.24829f), 130.2665f));
      entityList1.Add((Entity) World.CreateVehicle(model1, new Vector3(53.78642f, 118.4461f, 78.70818f), -15.649f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(207.153f, 223.1033f, 105.6054f), -93.79714f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(209.4577f, 226.769f, 105.605f), -95.12871f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(206.4377f, 211.6941f, 105.5926f), -44.61947f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(206.5428f, 208.0937f, 105.5414f), -94.5715f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(205.4871f, 199.6029f, 105.5668f), -63.99994f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(204.8215f, 195.7986f, 105.5644f), -69.99993f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(216.6255f, 188.0737f, 105.6627f), 98.58871f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(214.1611f, 190.7622f, 105.6435f), 138.2545f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(291.405f, 162.7149f, 104.2321f), 60f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(290.1209f, 158.6022f, 104.2283f), 54.99997f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(286.4288f, 154.0663f, 104.1818f), 51.45631f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(234.3599f, 39.53797f, 83.65764f), -0.9999998f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(230.6921f, 39.39994f, 83.52926f), 0.0f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(167.0022f, 85.04649f, 86.18564f), -129.9998f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(164.1218f, 83.18378f, 85.45215f), -132.9627f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(52.04792f, 116.6036f, 79.11424f), -84.99989f));
      entityList1.Add((Entity) World.CreatePed(model2, new Vector3(52.6731f, 121.1831f, 79.26024f), -129.9997f));
      model1.MarkAsNoLongerNeeded();
      model2.MarkAsNoLongerNeeded();
      int num = World.AddRelationshipGroup("COP");
      if (this._nooseCalled)
      {
        Model model3 = new Model(-1205689942);
        model3.Request(10000);
        Model model4 = new Model(-1920001264);
        model4.Request(10000);
        entityList2.Add((Entity) World.CreateVehicle(model3, new Vector3(198.6722f, 211.65f, 105.1559f), 13.33959f));
        entityList2.Add((Entity) World.CreateVehicle(model3, new Vector3(223.3751f, 91.49889f, 92.7678f), 70.44042f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(196.4423f, 208.0565f, 105.5176f), -100.7551f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(195.7256f, 214.2792f, 105.4206f), -60.65371f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(204.2193f, 197.8195f, 105.5659f), -44.23095f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(206.2503f, 209.1179f, 105.5271f), -50.44782f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(215.194f, 189.1221f, 105.6525f), -35.24888f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(228.424f, 88.24722f, 92.71055f), -84.69496f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(229.319f, 90.68475f, 93.15582f), -129.9802f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(234.5972f, 100.5806f, 93.85809f), -92.80697f));
        entityList2.Add((Entity) World.CreatePed(model4, new Vector3(231.1614f, 80.08932f, 92.34354f), -74.92347f));
        foreach (Ped ped in entityList2.Where<Entity>((Func<Entity, bool>) (e => e.Model.IsPed)))
        {
          ped.BlockPermanentEvents = false;
          Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) ped, (InputArgument) -270015777, (InputArgument) 9999, (InputArgument) true, (InputArgument) true);
          ped.RelationshipGroup = num;
          Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, (InputArgument) ped.Handle, (InputArgument) 1);
        }
        model4.MarkAsNoLongerNeeded();
        model3.MarkAsNoLongerNeeded();
      }
      foreach (Ped ped in entityList1.Where<Entity>((Func<Entity, bool>) (e => e.Model.IsPed)))
      {
        ped.BlockPermanentEvents = false;
        Function.Call(Hash.GIVE_WEAPON_TO_PED, (InputArgument) ped, (InputArgument) 453432689, (InputArgument) 1000, (InputArgument) true, (InputArgument) true);
        ped.RelationshipGroup = num;
        Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, (InputArgument) ped.Handle, (InputArgument) 1);
      }
      this._cleanupBag.AddRange((IEnumerable<Entity>) entityList1);
      this._cleanupBag.AddRange((IEnumerable<Entity>) entityList2);
    }
  }
}
