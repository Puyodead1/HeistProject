// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.SetupMenu
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;

namespace HeistProject.GUI
{
  public class SetupMenu : IDisposable
  {
    public List<MenuSetup> Setups;
    private int _index;
    private Prop _board;
    private DateTime _lastUpdate;
    private DateTime _startup;
    private Vector3 _scaleformPosition;
    private Vector3 _scaleformRotation;
    private Vector3 _scaleformScale;
    private Vector3 _mapPosition;
    private Vector3 _mapRotation;
    private Vector3 _mapScale;
    private Vector3 _spotlightPos;
    private Vector3 _camPos;
    private Camera _mainCamera;
    private Model _prevModel;

    public string HeistName { get; set; }

    public bool Visible { get; set; }

    public void Dispose()
    {
      this._board?.Delete();
      World.RenderingCamera = (Camera) null;
      this.Visible = false;
      Game.Player.CanControlCharacter = true;
    }

    public SetupMenu(string heistName, params MenuSetup[] setups)
    {
      this._prevModel = Game.Player.Character.Model;
      Model model1 = new Model(PedHash.FreemodeMale01);
      model1.Request(10000);
      Util.SetPlayerModel(model1);
      model1.MarkAsNoLongerNeeded();
      this.Visible = true;
      this._startup = DateTime.Now;
      Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_PLANNING_BOARD_SOUNDS", 0);
      Game.Player.CanControlCharacter = false;
      Model model2 = new Model("hei_prop_dlc_heist_board");
      model2.Request(10000);
      this._board = World.CreateProp(model2, new Vector3(576.2565f, -3125.518f, 19.90923f), new Vector3(0.0f, 0.0f, 2889f / 32f), false, false);
      this._board.FreezePosition = true;
      this._board.Position = new Vector3(576.2565f, -3125.518f, 19.90923f);
      model2.MarkAsNoLongerNeeded();
      this._scaleformPosition = new Vector3(576.25f, -3127.282f, 20.01f);
      this._scaleformRotation = new Vector3(0.0f, 0.0f, 89.8211f);
      this._scaleformScale = new Vector3(3.5399f, 2.02f, 1f);
      this._mapPosition = new Vector3(576.23f, -3125.15f, 19.93f);
      this._mapRotation = new Vector3(0.0f, 0.0f, 89.8211f);
      this._mapScale = new Vector3(3.6899f, 2.24f, 1f);
      this._camPos = new Vector3(573.7408f, -3125.56f, 19.9f);
      this._spotlightPos = new Vector3(573.7408f, -3126.56f, 19.9f);
      World.DestroyAllCameras();
      this._mainCamera = World.CreateCamera(this._camPos, new Vector3(), 60f);
      this._mainCamera.PointAt((Entity) this._board);
      World.RenderingCamera = this._mainCamera;
      HeistMap.Cleanup();
      HeistBoard.Init();
      HeistBoard.Clear();
      this.Setups = new List<MenuSetup>((IEnumerable<MenuSetup>) setups);
      if (this.Setups.Count > 0)
      {
        this._index = (1000 - 1000 % this.Setups.Count + this._index) % this.Setups.Count;
        this.Setups[this._index].Highlighted = true;
      }
      this.HeistName = heistName;
      HeistBoard.InitializePlanningSlots(this.HeistName, false, this.Setups.ToArray());
    }

    public void Update()
    {
      if (!this.Visible)
        return;
      Game.MaxWantedLevel = 0;
      Game.Player.WantedLevel = 0;
      if (DateTime.Now.Subtract(this._startup).TotalSeconds > 1.0)
      {
        HeistBoard.Refresh(1);
        HeistMap.Cleanup();
        for (int index = 0; index < this.Setups.Count; ++index)
        {
          Vector3? position = this.Setups[index].Position;
          if (position.HasValue)
          {
            int number = index + 1;
            position = this.Setups[index].Position;
            int posX = (int) position.Value.X - 100;
            position = this.Setups[index].Position;
            int posY = (int) position.Value.Y + 400;
            HeistMap.AddPostit(number, posX, posY);
            position = this.Setups[index].Position;
            int xPos = (int) position.Value.X - 100;
            position = this.Setups[index].Position;
            int yPos = (int) position.Value.Y + 100;
            HeistMap.AddHighlight(xPos, yPos);
          }
        }
        this._startup = DateTime.MaxValue;
      }
      this.ProcessControls();
      HeistBoard.Draw(this._scaleformPosition, this._scaleformRotation, this._scaleformScale);
      HeistMap.Draw3D(this._mapPosition, this._mapRotation, this._mapScale);
      Vector3 vector3 = this._board.Position + new Vector3(0.0f, 1f, 0.0f) - this._spotlightPos;
      Function.Call(Hash._0x5BCA583A583194DB, this._spotlightPos.X, this._spotlightPos.Y, this._spotlightPos.Z, vector3.X, vector3.Y, vector3.Z, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, vector3.Length(), 10f, 10f, 100f, 1f, 1f);
      Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
      if (DateTime.Now.Subtract(this._lastUpdate).TotalMilliseconds > 5000.0)
      {
        this._lastUpdate = DateTime.MaxValue;
        HeistBoard.InitializePlanningSlots(this.HeistName, true, this.Setups.ToArray());
      }
      else
        HeistBoard.InitializePlanningSlots(this.HeistName, false, this.Setups.ToArray());
    }

    private void ProcessControls()
    {
      if (Game.IsControlJustPressed(0, Control.FrontendPauseAlternate))
      {
        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Highlight_Cancel", "DLC_HEIST_PLANNING_BOARD_SOUNDS", 1);
        this._board.Delete();
        World.RenderingCamera = (Camera) null;
        this.Visible = false;
        Game.Player.CanControlCharacter = true;
        Game.Player.Character.Position = new Vector3(453.52f, -3077.9f, 5.07f);
        Util.SetPlayerModel(this._prevModel);
        DateTime now = DateTime.Now;
        while (DateTime.Now.Subtract(now).TotalMilliseconds < 1000.0)
        {
          Game.DisableControl(0, Control.FrontendPauseAlternate);
          Script.Yield();
        }
      }
      if (this.Setups.Count == 0)
        return;
      if (Game.IsControlJustPressed(0, Control.PhoneDown))
      {
        this.Setups[this._index].Highlighted = false;
        this._index = (1000 - 1000 % this.Setups.Count + this._index + 1) % this.Setups.Count;
        this.Setups[this._index].Highlighted = true;
        HeistBoard.HighlightItem(this._index);
        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Highlight_Move", "DLC_HEIST_PLANNING_BOARD_SOUNDS", 1);
      }
      if (Game.IsControlJustPressed(0, Control.PhoneUp))
      {
        this.Setups[this._index].Highlighted = false;
        this._index = (1000 - 1000 % this.Setups.Count + this._index - 1) % this.Setups.Count;
        this.Setups[this._index].Highlighted = true;
        HeistBoard.HighlightItem(this._index);
        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Highlight_Move", "DLC_HEIST_PLANNING_BOARD_SOUNDS", 1);
      }
      if (!Game.IsControlJustPressed(0, Control.PhoneSelect))
        return;
      if (!this.Setups[this._index].Available || this.Setups[this._index].Complete)
      {
        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Highlight_Error", "DLC_HEIST_PLANNING_BOARD_SOUNDS", 1);
      }
      else
      {
        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Highlight_Accept", "DLC_HEIST_PLANNING_BOARD_SOUNDS", 1);
        this.Setups[this._index].OnActivated();
      }
    }
  }
}
