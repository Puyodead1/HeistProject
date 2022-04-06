// Decompiled with JetBrains decompiler

using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace HeistProject
{
  public static class Util
  {
    public static System.Random SharedRandom = new System.Random();
    public static Vector3 Warehouse = new Vector3(453.52f, -3077.9f, 5.07f);

    public static void AddSafe<TKey, TValue>(
      this Dictionary<TKey, TValue> dict,
      TKey key,
      TValue value)
    {
      if (dict.ContainsKey(key))
        dict[key] = value;
      else
        dict.Add(key, value);
    }

    public static void RequestCollisions(this Model m, int timeout)
    {
      Function.Call(Hash.REQUEST_COLLISION_FOR_MODEL, m.Hash);
      DateTime now = DateTime.Now;
      do
      {
        if (!Function.Call<bool>(Hash.HAS_COLLISION_FOR_MODEL_LOADED, m.Hash))
          Script.Yield();
        else
          goto label_4;
      }
      while (DateTime.Now.Subtract(now).TotalMilliseconds <= (double) timeout);
      return;
label_4:;
    }

    public static void BringVehicleToHalt(Vehicle veh) => Function.Call(Hash._0x260BE8F09E326A20, 10f, 1, 0);

    public static VehicleSeat GetPedSeat(Ped ped, Vehicle veh)
    {
      for (int seat = -1; seat < veh.PassengerSeats; ++seat)
      {
        if ((Entity) veh.GetPedOnSeat((VehicleSeat) seat) == (Entity) ped)
          return (VehicleSeat) seat;
      }
      return VehicleSeat.Any;
    }

    public static Point AddPoints(this Point left, Point right) => new Point(left.X + right.X, left.Y + right.Y);

    public static Point SubtractPoints(this Point left, Point right) => new Point(left.X - right.X, left.Y - right.Y);

    public static string TranslateAssetAddress(HeistDefinition target, string origin) => target.AssetTranslation[origin];

    public static string FormatMilliseconds(int time)
    {
      int num1 = time / 60000;
      int num2 = time / 1000 % 60;
      int num3 = time % 1000;
      return string.Format("{0}:{1:00}", (object) num1, (object) num2);
    }

    public static float Clamp(this float val, float min, float max)
    {
      if ((double) val > (double) max)
        return max;
      return (double) val < (double) min ? min : val;
    }

    public static void DrawEntryMarker(Vector3 pos, float size = 1f) => World.DrawMarker(MarkerType.VerticalCylinder, pos, new Vector3(), new Vector3(), new Vector3(size, size, size), Color.FromArgb(100, Color.Yellow));

    public static Vector3 LinearVectorLerp(
      Vector3 start,
      Vector3 end,
      int currentTime,
      int duration)
    {
      return new Vector3()
      {
        X = Util.LinearFloatLerp(start.X, end.X, currentTime, duration),
        Y = Util.LinearFloatLerp(start.Y, end.Y, currentTime, duration),
        Z = Util.LinearFloatLerp(start.Z, end.Z, currentTime, duration)
      };
    }

    public static Vector3 VectorLerp(
      Vector3 start,
      Vector3 end,
      int currentTime,
      int duration,
      Func<float, float, int, int, float> easingFunc)
    {
      return new Vector3()
      {
        X = easingFunc(start.X, end.X, currentTime, duration),
        Y = easingFunc(start.Y, end.Y, currentTime, duration),
        Z = easingFunc(start.Z, end.Z, currentTime, duration)
      };
    }

    public static float LinearFloatLerp(float start, float end, int currentTime, int duration) => (end - start) * (float) currentTime / (float) duration + start;

    public static float QuadraticEasingLerp(float start, float end, int currentTime, int duration)
    {
      float num1 = (float) currentTime;
      float num2 = (float) duration;
      float num3 = end - start;
      float num4 = num1 / (num2 / 2f);
      if ((double) num4 < 1.0)
        return num3 / 2f * num4 * num4 + start;
      float num5 = num4 - 1f;
      return (float) (-(double) num3 / 2.0 * ((double) num5 * ((double) num5 - 2.0) - 1.0)) + start;
    }

    public static IEnumerable<string> SplitInParts(this string s, int partLength)
    {
      if (s == null)
        throw new ArgumentNullException(nameof (s));
      if (partLength <= 0)
        throw new ArgumentException("Part length has to be positive.", nameof (partLength));
      for (int i = 0; i < s.Length; i += partLength)
        yield return s.Substring(i, System.Math.Min(partLength, s.Length - i));
    }

    public static void SetPlayerModel(Model model)
    {
      if (Game.Player.Character.Model.Hash == model.Hash)
        return;
      model.Request(10000);
      Function.Call(Hash.SET_PLAYER_MODEL, new InputArgument(Game.Player), model.Hash);
    }

    public static void DisplayHelpTextThisFrame(string text)
    {
      Function.Call(Hash._0x8509B634FBE7DA11, "STRING");
      Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
      Function.Call(Hash._0x238FFE5C7B0498A6, 0, 1, 0, -1);
    }

    public static void DisplayHelpText(string text)
    {
      Function.Call(Hash._0x8509B634FBE7DA11, "STRING");
      Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, text);
      Function.Call(Hash._0x238FFE5C7B0498A6, 0, 0, 1, -1);
    }

    public static void PlaySoundFrontend(string soundDict, string soundName) => Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, soundName, soundDict);

    public static bool HasPedDamagedEntity(Ped ped, Entity target, bool useLastVehicle = true)
    {
      bool flag = false;
      if (ped.IsInVehicle() | useLastVehicle)
        flag = Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, Function.Call<int>(Hash.GET_VEHICLE_PED_IS_IN, ped.Handle, useLastVehicle), target.Handle, true);
      return Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, ped.Handle, target.Handle, true) | flag;
    }

    public static void SendPictureNotification(
      string body,
      string pic,
      int flash,
      int iconType,
      string sender,
      string subject)
    {
      Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");
      Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, body);
      Function.Call(Hash._0x1CCD9A37359072CF, pic, pic, flash, iconType, sender, subject);
      Function.Call(Hash._0xF020C96915705B3A, false, true);
    }

    public static void SendPictureNotification(
      string body,
      string picdict,
      string picname,
      int flash,
      int iconType,
      string sender,
      string subject)
    {
      Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, picdict);
      Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");
      Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, body);
      Function.Call(Hash._0x1CCD9A37359072CF, picdict, picname, flash, iconType, sender, subject);
      Function.Call(Hash._0xF020C96915705B3A, false, true);
    }

    public static void SendLesterMessage(string message)
    {
      Util.SendPictureNotification(message, "CHAR_LESTER", 0, 1, "Lester", "");
      Util.PlaySoundFrontend("Phone_SoundSet_Default", "Text_Arrive_Tone");
    }

    public static void SendFleecaMessage(string message)
    {
      Util.SendPictureNotification(message, "CHAR_BANK_FLEECA", 0, 9, "Fleeca Bank", "");
      Util.PlaySoundFrontend("Phone_SoundSet_Default", "Text_Arrive_Tone");
    }

    public static bool IsVehicleDoorOpen(Vehicle veh, VehicleDoor door) => (double) Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, veh, (int) door) > 0.100000001490116 || veh.IsDoorBroken(door);

    public static void SetPedComponentVariation(Ped ped, int componentId, int id, int texture) => Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped.Handle, componentId, id, texture, 2);

    public static int GetPedDrawableVariation(Ped ped, int componentId) => Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, ped.Handle, componentId);

    public static void SetPedAccessory(Ped ped, Util.HeistAccessory acc) => Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped.Handle, 5, (int) acc, 0, 0);

    public static void SetPedMask(Ped ped, int id, int texture) => Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped.Handle, 1, id, texture, 2);

    public static int GetComponentTextureVariations(Ped ped, int componentid, int drawableId) => Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, ped.Handle, componentid, drawableId);

    public static int GetMaskTextureVariations(Ped ped, int drawableId) => Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, ped.Handle, 1, drawableId);

    public enum HeistAccessory
    {
      None = 0,
      Parachute = 32, // 0x00000020
      EmptyGreenDuffelBag = 40, // 0x00000028
      FullGreenDuffelBag = 41, // 0x00000029
      EmptyDuffelBag = 44, // 0x0000002C
      FullDuffelBag = 45, // 0x0000002D
    }
  }
}
