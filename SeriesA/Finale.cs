// Decompiled with JetBrains decompiler
// Type: SeriesA.Finale
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

namespace SeriesA
{
  public class Finale : ScriptedLogic
  {
    private Vector3 _getawayCarSave = new Vector3(3.75f, 109.76f, 77.76f);
    private float _getawayCarHeading = 156.81f;
    private int _thermiteSceneId;
    private BarTimerBar _hostageControl;
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
    }

    public override void End()
    {
    }

    public override void Update()
    {
    }

    public override void BackgroundThreadUpdate()
    {
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
  }
}
