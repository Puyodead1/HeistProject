// Decompiled with JetBrains decompiler
// Type: InstaScript.FinaleLogic
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
  public class FinaleLogic : ScriptedLogic
  {
    private Vector3 _fleecaScopePos = new Vector3(1175.03f, 2695.7f, 36.45f);
    private Vector3 _fleecaEntrance = new Vector3(1174.86f, 2705.32f, 38.1f);
    private Vector3 _vehSpawnPoint = new Vector3(455.87f, -3063.9f, 5.72f);
    private Vector3 _drillingPos = new Vector3(1173.13f, 2716.18f, 37.07f);
    private Vector3 _cam1Pos = new Vector3(1171.619f, 2706.988f, 39.45744f);
    private Vector3 _cam2Pos = new Vector3(1181.445f, 2708.697f, 39.95f);
    private Vector3 _cam3Pos = new Vector3(1181.515f, 2710.006f, 40.01706f);
    private Vector3 _crowdControlPos = new Vector3(1178.58f, 2705.6f, 38.09f);
    private Vector3 _escapeHelPos = new Vector3(1601.17f, 3578.9f, 41.86f);
    private float _escapeHelHead = 33.08f;
    private Blip _destBlip;
    private Vehicle _vehTarget;
    private int _openDoorStart;
    private int _heistStart;
    private List<Entity> _cleanupBag;
    private Ped _counterLady;
    private Ped _linePed;
    private Ped _sittingPed;
    private Ped _pilot;
    private Vehicle _escapeChopper;
    private Prop _doors;
    private Prop _bag;
    private Prop _drill;
    private bool _firstSound;
    private Camera _drillCam;
    private bool _oldIsInVeh;
    private bool _alarmFirstSound;
    private int _alarmSoundId;
    private int _lastAim;
    private int _aimtarget;
    private int soundid;
    private int _drillingCompleteTime;
    private TextTimerBar _mainBar;
    private int _txtStart;
    private int _txtNum;

    private int Stage { get; set; }

    public override void Start()
    {
      this.Stage = 0;
      ScriptHandler.Log("STARTING FLEECA FINALE " + (object) DateTime.Now);
      ScriptHandler.Log("Game version: " + (object) Game.Version);
      this._heistStart = Game.GameTime;
      this.MissionChallenges = new Dictionary<string, Tuple<string, bool>>();
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullDuffelBag);
      this._cleanupBag = new List<Entity>();
      Model[] source = new Model[4]
      {
        new Model(VehicleHash.Kuruma2),
        new Model(664399832),
        new Model(-1280051738),
        new Model(-1697435671)
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
      this._vehTarget = World.CreateVehicle(source[0], this._vehSpawnPoint, 2.6f);
      int num2 = 0;
      do
      {
        this._counterLady = World.CreatePed(source[1], new Vector3(1176.476f, 2708.119f, 38.08789f), -164.6922f);
        ++num2;
        if (num2 > 200)
        {
          UI.Notify("Error while creating ped 01.");
          this.End();
          return;
        }
      }
      while ((Entity) this._counterLady == (Entity) null);
      int num3 = 0;
      do
      {
        this._linePed = World.CreatePed(source[2], new Vector3(1176.552f, 2706.912f, 38.10142f), 6.08192f);
        ++num3;
        if (num3 > 200)
        {
          UI.Notify("Error while creating ped 02.");
          this.End();
          return;
        }
      }
      while ((Entity) this._linePed == (Entity) null);
      int num4 = 0;
      do
      {
        this._sittingPed = World.CreatePed(source[3], new Vector3(1171.977f, 2705.079f, 38.08856f), -81.43325f);
        ++num4;
        if (num4 > 200)
        {
          UI.Notify("Error while creating ped 03.");
          this.End();
          return;
        }
      }
      while ((Entity) this._sittingPed == (Entity) null);
      this._counterLady.BlockPermanentEvents = true;
      this._linePed.BlockPermanentEvents = true;
      this._sittingPed.BlockPermanentEvents = true;
      this._counterLady.CanRagdoll = false;
      this._linePed.CanRagdoll = false;
      this._sittingPed.CanRagdoll = false;
      Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) this._sittingPed.Handle, (InputArgument) this.LoadDict("switch@michael@sitting"), (InputArgument) "idle", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 1, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
      Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) this._counterLady.Handle, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@scope_out@cashier_loop"), (InputArgument) "cashier_loop", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 1, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
      Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) this._linePed.Handle, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@hostages@ped_e@intro"), (InputArgument) "intro_loop", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 1, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
      for (int index = 0; index < source.Length; ++index)
        source[index].MarkAsNoLongerNeeded();
      ScriptHandler.Log("Fleeca finale started.");
    }

    public override void End()
    {
      if ((Entity) this._vehTarget != (Entity) null)
        this._vehTarget.Delete();
      if (Blip.op_Inequality(this._destBlip, (Blip) null))
        this._destBlip.Remove();
      if ((Entity) this._doors != (Entity) null)
        this._doors.Delete();
      if ((Entity) this._drill != (Entity) null)
        this._drill.Delete();
      if ((Entity) this._bag != (Entity) null)
        this._bag.Delete();
      if ((Entity) this._pilot != (Entity) null)
        this._pilot.Delete();
      if ((Entity) this._escapeChopper != (Entity) null)
        this._escapeChopper.Delete();
      for (int index = 0; index < this._cleanupBag.Count; ++index)
      {
        if (this._cleanupBag[index] != (Entity) null)
          this._cleanupBag[index].Delete();
      }
      if ((Entity) this._counterLady != (Entity) null)
        this._counterLady.Delete();
      if ((Entity) this._linePed != (Entity) null)
        this._linePed.Delete();
      if ((Entity) this._sittingPed != (Entity) null)
        this._sittingPed.Delete();
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
      UI.ShowSubtitle(" ");
      Game.Player.Character.FreezePosition = false;
      World.RenderingCamera = (Camera) null;
      Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, (InputArgument) "ALARMS_SOUNDSET");
      Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, (InputArgument) "HEIST_FLEECA_DRILL");
      Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, (InputArgument) "HEIST_FLEECA_DRILL_2");
      Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_HEIST_FLEECA_SOUNDSET");
      TimerBarPool.Remove((TimerBarBase) this._mainBar);
      if (this.soundid != 0)
      {
        Function.Call(Hash.STOP_SOUND, (InputArgument) this.soundid);
        Function.Call(Hash.RELEASE_SOUND_ID, (InputArgument) this.soundid);
      }
      if (this._alarmSoundId == 0)
        return;
      Function.Call(Hash.STOP_SOUND, (InputArgument) this._alarmSoundId);
      Function.Call(Hash.RELEASE_SOUND_ID, (InputArgument) this._alarmSoundId);
    }

    public override void Update()
    {
      if (this.Stage == 0)
      {
        UI.ShowSubtitle("Get in the ~b~Kuruma.", 120000);
        this._vehTarget.AddBlip();
        this._vehTarget.CurrentBlip.Color = BlipColor.Blue;
        ++this.Stage;
      }
      if (this.Stage == 1 && Game.Player.Character.IsInVehicle(this._vehTarget))
      {
        UI.ShowSubtitle("Drive to the ~y~Fleeca Bank franchise.", 120000);
        this._vehTarget.CurrentBlip.Alpha = 0;
        this._destBlip = World.CreateBlip(this._fleecaScopePos);
        this._destBlip.ShowRoute = true;
        this._txtStart = Game.GameTime;
        ++this.Stage;
      }
      if (this.Stage == 2 && Game.Player.Character.IsInVehicle(this._vehTarget))
        World.DrawMarker(MarkerType.VerticalCylinder, this._fleecaScopePos, new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), Color.Yellow);
      if (this.Stage == 2 && Game.Player.Character.IsInVehicle(this._vehTarget) && Game.Player.Character.IsInRangeOf(this._fleecaScopePos, 3f))
      {
        ++this.Stage;
        this._destBlip.Remove();
        this._vehTarget.CurrentBlip.Remove();
        UI.ShowSubtitle("Enter the bank with a weapon out.", 120000);
      }
      if (this.Stage == 3 && Game.Player.Character.IsInRangeOf(this._fleecaEntrance, 2f))
      {
        UI.ShowSubtitle("");
        TaskSequence sequence = new TaskSequence();
        Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) 0, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@hostages@intro"), (InputArgument) "intro_ped_d", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 0, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
        Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) 0, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@hostages@ped_d@"), (InputArgument) "idle", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 1, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
        sequence.Close();
        this._counterLady.Task.PerformSequence(sequence);
        sequence.Dispose();
        Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) this._sittingPed.Handle, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@hostages@ped_a@intro"), (InputArgument) "intro", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 2, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
        Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) this._linePed.Handle, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@hostages@intro"), (InputArgument) "intro_ped_e", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 2, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
        string[] strArray = new string[4]
        {
          "GENERIC_FRIGHTENED_HIGH",
          "GENERIC_CURSE_HIGH",
          "GENERIC_SHOCKED_HIGH",
          "GENERIC_SHOCKED_MED"
        };
        Function.Call(Hash._PLAY_AMBIENT_SPEECH1, (InputArgument) this._counterLady.Handle, (InputArgument) strArray[Util.SharedRandom.Next(strArray.Length)], (InputArgument) "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL", (InputArgument) 1);
        Function.Call(Hash._PLAY_AMBIENT_SPEECH1, (InputArgument) this._sittingPed.Handle, (InputArgument) strArray[Util.SharedRandom.Next(strArray.Length)], (InputArgument) "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL", (InputArgument) 1);
        Function.Call(Hash._PLAY_AMBIENT_SPEECH1, (InputArgument) this._linePed.Handle, (InputArgument) strArray[Util.SharedRandom.Next(strArray.Length)], (InputArgument) "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL", (InputArgument) 1);
        ++this.Stage;
      }
      if (this.Stage == 5 && Geometry.RaycastEntity(new Vector2(0.0f, 0.0f), GameplayCamera.Position, GameplayCamera.Rotation) == (Entity) this._counterLady && Game.Player.Character.IsShooting)
      {
        string[] strArray = new string[4]
        {
          "GENERIC_FRIGHTENED_HIGH",
          "GENERIC_CURSE_HIGH",
          "GENERIC_SHOCKED_HIGH",
          "GENERIC_SHOCKED_MED"
        };
        Function.Call(Hash._PLAY_AMBIENT_SPEECH1, (InputArgument) this._counterLady.Handle, (InputArgument) strArray[Util.SharedRandom.Next(strArray.Length)], (InputArgument) "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL", (InputArgument) 1);
        Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) this._counterLady.Handle, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@hostages@ped_d@door"), (InputArgument) "open", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 2, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
        this._openDoorStart = Game.GameTime;
        ++this.Stage;
      }
      if (this.Stage >= 4 && this.Stage <= 7)
        Game.MaxWantedLevel = 0;
      if (this.Stage >= 4 && (Entity) this._doors == (Entity) null)
      {
        int handle = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, (InputArgument) 1175.542f, (InputArgument) 2710.861f, (InputArgument) 38.22689f, (InputArgument) 1f, (InputArgument) 2121050683, (InputArgument) 0);
        if (handle != 0)
        {
          new Prop(handle).Delete();
          Model model = new Model(2121050683);
          model.Request(10000);
          this._doors = World.CreateProp(model, new Vector3(1175.542f, 2710.861f, 38.22689f), new Vector3(0.0f, 0.0f, 90f), false, false);
          this._doors.FreezePosition = true;
          this._doors.Position = new Vector3(1175.542f, 2710.861f, 38.22689f);
          model.MarkAsNoLongerNeeded();
        }
      }
      if (this.Stage == 7)
        Util.DrawEntryMarker(this._drillingPos, 0.5f);
      if (this.Stage == 7 && Game.Player.Character.IsInRangeOf(this._drillingPos + new Vector3(0.0f, 0.0f, 1f), 1.3f))
      {
        ++this.Stage;
        UI.ShowSubtitle("");
        this._destBlip.Remove();
        Game.Player.Character.Weapons.Select(Game.Player.Character.Weapons[WeaponHash.Unarmed]);
        Game.Player.Character.FreezePosition = true;
        Game.Player.Character.Position = new Vector3(1173.29f, 2716.36f, 37.07f);
        Game.Player.Character.Heading = 0.0f;
        Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
        Model model1 = new Model("hei_prop_heist_drill");
        model1.Request(10000);
        this._drill = World.CreateProp(model1, Game.Player.Character.Position, false, false);
        this._drill.FreezePosition = true;
        this._drill.HasCollision = false;
        this._drill.IsInvincible = true;
        Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, (InputArgument) this._drill, (InputArgument) Game.Player.Character, (InputArgument) Game.Player.Character.GetBoneIndex(Bone.PH_R_Hand), (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 2, (InputArgument) 1);
        model1.MarkAsNoLongerNeeded();
        Model model2 = new Model("hei_p_m_bag_var22_arm_s");
        model2.Request(10000);
        this._bag = World.CreateProp(model2, Game.Player.Character.Position, false, false);
        this._bag.FreezePosition = true;
        this._bag.HasCollision = false;
        this._bag.IsInvincible = true;
        Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, (InputArgument) this._bag.Handle, (InputArgument) Game.Player.Character, (InputArgument) Game.Player.Character.GetBoneIndex(Bone.PH_L_Hand), (InputArgument) 0.0, (InputArgument) 0.0, (InputArgument) 0.0, (InputArgument) 0.0, (InputArgument) 0.0, (InputArgument) 0.0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0, (InputArgument) 2, (InputArgument) 1);
        Function.Call(Hash.PLAY_ENTITY_ANIM, (InputArgument) this._bag, (InputArgument) "bag_intro", (InputArgument) "anim@heists@fleeca_bank@drilling", (InputArgument) 1000.0, (InputArgument) 1, (InputArgument) 1, (InputArgument) 1, (InputArgument) 0, (InputArgument) 0);
        this._drillCam = World.CreateCamera(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.9193f, -0.5807f, 0.0869f)), new Vector3(), 60f);
        this._drillCam.PointAt(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0.1376f, 0.4819f, 0.4162f)));
        this._drillCam.Shake(CameraShake.Hand, 0.1f);
        World.RenderingCamera = this._drillCam;
        model2.MarkAsNoLongerNeeded();
        TaskSequence sequence = new TaskSequence();
        Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) 0, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@drilling"), (InputArgument) "intro", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 0, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
        Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) 0, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@drilling"), (InputArgument) "drill_right_end", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 1, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
        sequence.Close();
        Game.Player.Character.Task.PerformSequence(sequence);
        DrillingMinigame.StartNew();
        DrillingMinigame.Visible = true;
        Function.Call(Hash.STOP_ENTITY_ANIM, (InputArgument) "bag_intro", (InputArgument) "anim@heists@fleeca_bank@drilling");
        Script.Yield();
        Function.Call(Hash.PLAY_ENTITY_ANIM, (InputArgument) this._bag, (InputArgument) "bag_drill_straight_idle", (InputArgument) "anim@heists@fleeca_bank@drilling", (InputArgument) 1000.0, (InputArgument) 1, (InputArgument) 1, (InputArgument) 1, (InputArgument) 0, (InputArgument) 0);
        Script.Wait(7000);
        DrillingMinigame.OnDrillingComplete = (Action) (() =>
        {
          if (Game.GameTime - this._openDoorStart < 90000)
            this.MissionChallenges.Add("Complete drilling before alarm sounds", new Tuple<string, bool>("", true));
          else
            this.MissionChallenges.Add("Complete drilling before alarm sounds", new Tuple<string, bool>("", false));
          EntryPoint.Team[1].BlockPermanentEvents = false;
          EntryPoint.Team[1].Position = this._crowdControlPos;
          this.Stage = 9;
          Function.Call(Hash.STOP_SOUND, (InputArgument) this.soundid);
          DrillingMinigame.StartNew();
          World.RenderingCamera = (Camera) null;
          DrillingMinigame.Visible = false;
          Game.Player.Character.Task.ClearAll();
          Function.Call(Hash.TASK_PLAY_ANIM, (InputArgument) Game.Player.Character.Handle, (InputArgument) this.LoadDict("anim@heists@fleeca_bank@drilling"), (InputArgument) "outro", (InputArgument) 8f, (InputArgument) 1f, (InputArgument) -1, (InputArgument) 0, (InputArgument) -8f, (InputArgument) false, (InputArgument) false, (InputArgument) false);
          Script.Wait(5000);
          Game.Player.Character.FreezePosition = false;
          if ((Entity) this._drill != (Entity) null)
            this._drill.Delete();
          this._bag.Delete();
          Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullDuffelBag);
          Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
          UI.ShowSubtitle("Get in the ~b~Kuruma.", 120000);
          Util.SendLesterMessage("Alright, you got the bonds. Now get the hell out of there.");
          this._drillingCompleteTime = Game.GameTime;
        });
      }
      if (this.Stage == 8)
      {
        Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, (InputArgument) "FMMC", (InputArgument) 1);
        Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, (InputArgument) "MC_DRILL_1", (InputArgument) 1);
        if (this.soundid == 0)
          this.soundid = Function.Call<int>(Hash.GET_SOUND_ID);
        Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_MPHEIST\\HEIST_FLEECA_DRILL", (InputArgument) 1);
        Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", (InputArgument) 1);
        Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "HEIST_FLEECA_DRILL", (InputArgument) 1);
        Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "HEIST_FLEECA_DRILL_2", (InputArgument) 1);
        Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "DLC_HEIST_FLEECA_SOUNDSET", (InputArgument) 1);
        if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, (InputArgument) this.soundid) || !this._firstSound)
        {
          Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, (InputArgument) this.soundid, (InputArgument) "Drill", (InputArgument) this._drill, (InputArgument) "DLC_HEIST_FLEECA_SOUNDSET", (InputArgument) 1, (InputArgument) 0);
          this._firstSound = true;
        }
        if ((double) DrillingMinigame.f7 < (double) DrillingMinigame.fD)
          Function.Call(Hash.SET_VARIABLE_ON_SOUND, (InputArgument) this.soundid, (InputArgument) "DrillState", (InputArgument) 0.0);
        else if ((double) DrillingMinigame.f7 <= (double) DrillingMinigame.fD)
          Function.Call(Hash.SET_VARIABLE_ON_SOUND, (InputArgument) this.soundid, (InputArgument) "DrillState", (InputArgument) 0.5);
        else
          Function.Call(Hash.SET_VARIABLE_ON_SOUND, (InputArgument) this.soundid, (InputArgument) "DrillState", (InputArgument) 1.0);
      }
      if (this.Stage >= 7 && Game.GameTime - this._openDoorStart > 90000)
      {
        if (this._alarmSoundId == 0)
          this._alarmSoundId = Function.Call<int>(Hash.GET_SOUND_ID);
        for (int index = 0; index < 200; ++index)
        {
          if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, (InputArgument) "ALARM_BELL_01", (InputArgument) 0))
            Script.Yield();
          else
            break;
        }
        if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, (InputArgument) this._alarmSoundId) || !this._alarmFirstSound)
        {
          Function.Call(Hash.PLAY_SOUND_FROM_COORD, (InputArgument) this._alarmSoundId, (InputArgument) "Bell_01", (InputArgument) this._crowdControlPos.X, (InputArgument) this._crowdControlPos.Y, (InputArgument) this._crowdControlPos.Z, (InputArgument) "ALARMS_SOUNDSET", (InputArgument) 0, (InputArgument) 0, (InputArgument) 0);
          this._alarmFirstSound = true;
        }
        EntryPoint.Team[1].BlockPermanentEvents = false;
      }
      if (this.Stage == 9 && Game.Player.Character.IsInVehicle(this._vehTarget))
      {
        Util.SendLesterMessage("I've set up a pilot to meet you on the roof of the abandoned motel in Sandy Shores.");
        UI.ShowSubtitle("Meet up with the ~b~Pilot.", 120000);
        Model model3 = new Model(VehicleHash.Maverick);
        model3.Request(10000);
        this._escapeChopper = World.CreateVehicle(model3, this._escapeHelPos, this._escapeHelHead);
        this._escapeChopper.FreezePosition = true;
        this._escapeChopper.IsInvincible = true;
        model3.MarkAsNoLongerNeeded();
        this._escapeChopper.AddBlip();
        this._escapeChopper.CurrentBlip.Color = BlipColor.Blue;
        this._escapeChopper.CurrentBlip.ShowRoute = true;
        Model model4 = new Model(PedHash.Pilot01SMM);
        model4.Request(10000);
        this._pilot = World.CreatePed(model4, this._escapeHelPos);
        this._pilot.Task.WarpIntoVehicle(this._escapeChopper, VehicleSeat.Driver);
        this._pilot.BlockPermanentEvents = true;
        model4.MarkAsNoLongerNeeded();
        this.Stage = 10;
      }
      if (this.Stage == 10 && Game.Player.Character.IsInRangeOf(this._pilot.Position, 100f))
        this._escapeChopper.FreezePosition = false;
      if (this.Stage == 10 && Game.Player.Character.IsInRangeOf(this._pilot.Position, 5f))
      {
        Game.DisableControl(0, Control.Enter);
        if (Game.IsControlJustPressed(0, Control.Enter))
          Game.Player.Character.Task.EnterVehicle(this._escapeChopper, VehicleSeat.LeftRear);
      }
      if (this.Stage == 10 && Game.Player.Character.IsInVehicle(this._escapeChopper) && (EntryPoint.Team[1].IsDead || EntryPoint.Team[1].IsInVehicle(this._escapeChopper)))
      {
        ++this.Stage;
        UI.ShowSubtitle("");
        Function.Call(Hash.TASK_HELI_MISSION, (InputArgument) this._pilot.Handle, (InputArgument) this._escapeChopper.Handle, (InputArgument) 0, (InputArgument) 0, (InputArgument) 0.0f, (InputArgument) 0.0f, (InputArgument) 80f, (InputArgument) 6, (InputArgument) 40f, (InputArgument) 1f, (InputArgument) 36f, (InputArgument) 15, (InputArgument) 15, (InputArgument) -1f, (InputArgument) 1);
        this._drillingCompleteTime = Game.GameTime;
      }
      if (this.Stage != 11 || Game.GameTime - this._drillingCompleteTime <= 30000 || this.HasFinished)
        return;
      if (Game.GameTime - this._heistStart < 420000)
        this.MissionChallenges.Add("Completed in under 7:00", new Tuple<string, bool>(Util.FormatMilliseconds(Game.GameTime - this._heistStart), true));
      else
        this.MissionChallenges.Add("Completed in under 7:00", new Tuple<string, bool>(Util.FormatMilliseconds(Game.GameTime - this._heistStart), false));
      float num = (float) (1.0 - (double) this._vehTarget.Health / (double) this._vehTarget.MaxHealth);
      if ((double) num <= 0.0599999986588955)
        this.MissionChallenges.Add("Kuruma damage under 6%", new Tuple<string, bool>(num.ToString("P"), true));
      else
        this.MissionChallenges.Add("Kuruma damage under 6%", new Tuple<string, bool>(num.ToString("P"), false));
      if (((IEnumerable<Ped>) EntryPoint.Team).Any<Ped>((Func<Ped, bool>) (p => p.IsDead)))
        this.MissionChallenges.Add("All team members survive", new Tuple<string, bool>("", false));
      else
        this.MissionChallenges.Add("All team members survive", new Tuple<string, bool>("", true));
      this.HasWon = true;
      this.HasFinished = true;
    }

    public override void BackgroundThreadUpdate()
    {
      if (this.Stage <= 3 && Game.Player.WantedLevel > 0)
      {
        this.FailureReason = "You attracted police attention before entering the bank.";
        this.HasFinished = true;
      }
      if (this.Stage == 8)
      {
        if (Game.CurrentInputMode == 1)
          Util.DisplayHelpTextThisFrame("Use ~INPUT_MOVE_DOWN_ONLY~ to push the drill towards the bite point. Don't push too hard.");
        else
          Util.DisplayHelpTextThisFrame("Use ~INPUT_MOVE_DOWN_ONLY~ to control the drill power. Use ~INPUT_LOOK_DOWN_ONLY~ push the drill.");
      }
      if ((Entity) this._counterLady != (Entity) null && (Entity) this._linePed != (Entity) null && (Entity) this._sittingPed != (Entity) null && (this._counterLady.IsDead || this._linePed.IsDead || this._sittingPed.IsDead))
      {
        this.FailureReason = "A witness has died.";
        this.HasFinished = true;
      }
      if (this.Stage == 2)
      {
        if (Game.GameTime - this._txtStart > 10000 && this._txtNum == 0)
        {
          Util.SendLesterMessage("Alright, armed robbery, here we go. Once you walk through those doors there's no turning back.");
          ++this._txtNum;
        }
        if (Game.GameTime - this._txtStart > 25000 && this._txtNum == 1)
        {
          Util.SendLesterMessage("There are 4 security cameras in the foyer, make sure you take them out as soon as you enter.");
          ++this._txtNum;
        }
        if (Game.GameTime - this._txtStart > 40000 && this._txtNum == 2)
        {
          Util.SendLesterMessage("The teller has a button to open the vault, so wave your gun in their face to intimidate them.");
          ++this._txtNum;
        }
        if (Game.GameTime - this._txtStart > 50000 && this._txtNum == 3)
        {
          Util.SendLesterMessage("Maybe even shoot the glass so they don't think you're fucking around.");
          ++this._txtNum;
        }
        if (Game.GameTime - this._txtStart > 50000 && this._txtNum == 4)
        {
          Util.SendLesterMessage("Once the vault door opens, you will have 90 seconds before the automatic alarm goes off.");
          ++this._txtNum;
        }
      }
      if ((this.Stage == 2 || this.Stage == 3) && (Entity) this._counterLady != (Entity) null && (Entity) this._linePed != (Entity) null && (Entity) this._sittingPed != (Entity) null)
      {
        Function.Call(Hash.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME, (InputArgument) 0.0f);
        Function.Call(Hash.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME, (InputArgument) 0.0f, (InputArgument) 0.0f);
        Ped[] allPeds = World.GetAllPeds();
        List<int> intList = new List<int>();
        intList.Add(EntryPoint.Team[0].Handle);
        intList.Add(EntryPoint.Team[1].Handle);
        intList.Add(this._counterLady.Handle);
        intList.Add(this._linePed.Handle);
        intList.Add(this._sittingPed.Handle);
        foreach (Ped ped in allPeds)
        {
          if (!((Entity) ped == (Entity) null) && !ped.IsInVehicle() && !intList.Contains(ped.Handle))
            ped.Delete();
        }
      }
      if (this.Stage == 6 && Game.GameTime - this._openDoorStart >= 4000)
      {
        this._openDoorStart = Game.GameTime;
        UI.ShowSubtitle("Go to the ~g~safety deposit box.", 120000);
        this._destBlip = World.CreateBlip(this._drillingPos);
        this._destBlip.ShowRoute = false;
        this._destBlip.Color = BlipColor.Green;
        this._destBlip.Scale = 0.7f;
        this._mainBar = new TextTimerBar("ALARM", "1:30");
        TimerBarPool.Add((TimerBarBase) this._mainBar);
        Game.MaxWantedLevel = 5;
        ++this.Stage;
      }
      if (this.Stage >= 7)
      {
        int time = 90000 - (Game.GameTime - this._openDoorStart);
        if (time > 0)
        {
          this._mainBar.Text = Util.FormatMilliseconds(time);
        }
        else
        {
          TimerBarPool.Remove((TimerBarBase) this._mainBar);
          Game.MaxWantedLevel = 5;
          Game.Player.WantedLevel = 3;
        }
      }
      if (this.Stage == 7)
      {
        int currentTime = Game.GameTime - this._openDoorStart;
        if (currentTime < 5000 && (Entity) this._doors != (Entity) null)
          this._doors.Rotation = new Vector3(0.0f, 0.0f, Util.LinearFloatLerp(90f, 0.0f, currentTime, 5000));
      }
      if (this.Stage == 4)
      {
        Ped ped = EntryPoint.Team[1];
        int num = 1200;
        ped.Weapons.Select(ped.Weapons[WeaponHash.PumpShotgun]);
        ped.BlockPermanentEvents = true;
        ped.AlwaysKeepTask = true;
        ped.Task.RunTo(this._fleecaEntrance);
        while (!ped.IsInRangeOf(this._fleecaEntrance, 2f))
          Script.Yield();
        ped.Task.ShootAt(this._cam1Pos, num);
        Script.Wait(num);
        ped.Task.RunTo(this._crowdControlPos, true);
        while (!ped.IsInRangeOf(this._crowdControlPos, 1f))
          Script.Yield();
        ped.Task.ShootAt(this._cam3Pos, num);
        Script.Wait(num);
        ped.Task.ShootAt(this._cam2Pos, num);
        Script.Wait(num);
        this._lastAim = Game.GameTime - 5000;
        ++this.Stage;
      }
      if (this.Stage >= 5 && this.Stage < 9 && Game.GameTime - this._lastAim >= 5000 && (Game.GameTime - this._openDoorStart < 90000 || this._openDoorStart == 0))
      {
        Entity target = (Entity) null;
        switch (this._aimtarget % (this.Stage == 3 ? 3 : 2))
        {
          case 0:
            target = (Entity) this._sittingPed;
            break;
          case 1:
            target = (Entity) this._linePed;
            break;
          case 2:
            target = (Entity) this._counterLady;
            break;
        }
        ++this._aimtarget;
        EntryPoint.Team[1].Task.AimAt(target, 5000);
        string[] strArray = new string[4]
        {
          "GENERIC_FRIGHTENED_HIGH",
          "GENERIC_CURSE_HIGH",
          "GENERIC_SHOCKED_HIGH",
          "GENERIC_SHOCKED_MED"
        };
        if (target != (Entity) null)
          Function.Call(Hash._PLAY_AMBIENT_SPEECH1, (InputArgument) target.Handle, (InputArgument) strArray[Util.SharedRandom.Next(strArray.Length)], (InputArgument) "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL", (InputArgument) 1);
        this._lastAim = Game.GameTime;
      }
      if (this.Stage != 2 || !((Entity) this._vehTarget != (Entity) null))
        return;
      bool flag = Game.Player.Character.IsInVehicle(this._vehTarget);
      if (this._oldIsInVeh && !flag)
      {
        UI.ShowSubtitle("Get back in the ~b~Kuruma", 120000);
        if (Blip.op_Inequality(this._destBlip, (Blip) null) && this._destBlip.Exists())
        {
          this._destBlip.ShowRoute = false;
          this._destBlip.Alpha = 0;
        }
        this._vehTarget.CurrentBlip.Alpha = (int) byte.MaxValue;
      }
      if (!this._oldIsInVeh & flag)
      {
        if (this.Stage == 2)
        {
          UI.ShowSubtitle("Drive to the ~y~Fleeca Bank franchise.", 120000);
          this._destBlip.ShowRoute = true;
          this._destBlip.Alpha = (int) byte.MaxValue;
        }
        this._vehTarget.CurrentBlip.Alpha = 0;
      }
      this._oldIsInVeh = flag;
    }

    public string LoadDict(string dict)
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
  }
}
