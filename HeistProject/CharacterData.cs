// Decompiled with JetBrains decompiler
// Type: HeistProject.CharacterData
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Native;
using System.Collections.Generic;

namespace HeistProject
{
  public class CharacterData
  {
    public int HairColor;
    public int BeardColor;

    public CharacterData()
    {
      this.DrawableCollection = new Dictionary<int, int>();
      this.TextureCollection = new Dictionary<int, int>();
      this.PaletteCollection = new Dictionary<int, int>();
      this.PropDrawableCollection = new Dictionary<int, int>();
      this.PropTextureCollection = new Dictionary<int, int>();
      this.HeadOverlay = new Dictionary<int, int>();
    }

    public static CharacterData GetRandomCharacter(bool male = true)
    {
      CharacterData randomCharacter = new CharacterData();
      randomCharacter.Model = male ? 1885233650 : -1667301416;
      randomCharacter.BeardColor = Util.SharedRandom.Next(64);
      randomCharacter.HairColor = Util.SharedRandom.Next(64);
      randomCharacter.HasHead = true;
      randomCharacter.HeadLayout = new HeadLayout()
      {
        isParent = false,
        shapeFirstId = Util.SharedRandom.Next(3),
        shapeSecondId = Util.SharedRandom.Next(3),
        shapeThirdId = Util.SharedRandom.Next(3),
        skinFirstId = Util.SharedRandom.Next(3),
        skinSecondId = Util.SharedRandom.Next(3),
        skinThirdId = Util.SharedRandom.Next(3),
        shapeMix = (float) Util.SharedRandom.NextDouble(),
        skinMix = (float) Util.SharedRandom.NextDouble(),
        thirdMix = (float) Util.SharedRandom.NextDouble()
      };
      for (int key = 0; key < 15; ++key)
      {
        randomCharacter.DrawableCollection[key] = Util.SharedRandom.Next(50);
        randomCharacter.PaletteCollection[key] = 0;
        randomCharacter.TextureCollection[key] = 0;
        randomCharacter.PropTextureCollection[key] = 0;
        randomCharacter.PropDrawableCollection[key] = 0;
      }
      return randomCharacter;
    }

    public static CharacterData GetPedCharacterData(Ped ped)
    {
      CharacterData pedCharacterData = new CharacterData();
      pedCharacterData.Model = ped.Model.Hash;
      for (int key = 0; key < 15; ++key)
      {
        pedCharacterData.PaletteCollection[key] = Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, (InputArgument) ped.Handle, (InputArgument) key);
        pedCharacterData.DrawableCollection[key] = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, (InputArgument) ped.Handle, (InputArgument) key);
        pedCharacterData.TextureCollection[key] = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, (InputArgument) ped.Handle, (InputArgument) key);
        pedCharacterData.PropDrawableCollection[key] = Function.Call<int>(Hash.GET_PED_PROP_INDEX, (InputArgument) ped.Handle, (InputArgument) key);
        pedCharacterData.PropTextureCollection[key] = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, (InputArgument) ped.Handle, (InputArgument) key);
        pedCharacterData.HeadOverlay[key] = Function.Call<int>((Hash) 11965769224155712835, (InputArgument) ped.Handle, (InputArgument) key);
      }
      pedCharacterData.HasHead = false;
      pedCharacterData.HairColor = 0;
      for (int index = 0; index < Function.Call<int>((Hash) 16555460409682481488); ++index)
      {
        if (Function.Call<bool>((Hash) 17589755200512041942, (InputArgument) index))
          pedCharacterData.HairColor = index;
      }
      pedCharacterData.BeardColor = 0;
      for (int index = 0; index < Function.Call<int>((Hash) 15129783665620559896); ++index)
      {
        if (!Function.Call<bool>((Hash) 4503651381657368180, (InputArgument) index))
          pedCharacterData.BeardColor = index;
      }
      return pedCharacterData;
    }

    public static void SetPedCharacterData(Ped ped, CharacterData data, bool hairOnly = false)
    {
      if (ped.Model.Hash != data.Model && (Entity) ped == (Entity) Game.Player.Character)
      {
        new GTA.Model(data.Model).Request(10000);
        Function.Call(Hash.SET_PLAYER_MODEL, new InputArgument(Game.Player), (InputArgument) data.Model);
        Script.Wait(2000);
        CharacterData.SetPedCharacterData(ped, data);
      }
      else
      {
        if (!hairOnly)
        {
          for (int key = 0; key < 15; ++key)
          {
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, (InputArgument) ped.Handle, (InputArgument) key, (InputArgument) data.DrawableCollection[key], (InputArgument) data.TextureCollection[key], (InputArgument) data.PaletteCollection[key]);
            Function.Call(Hash.SET_PED_PROP_INDEX, (InputArgument) ped.Handle, (InputArgument) key, (InputArgument) data.PropDrawableCollection[key], (InputArgument) data.PropTextureCollection[key], (InputArgument) true);
            Function.Call(Hash.SET_PED_HEAD_OVERLAY, (InputArgument) ped.Handle, (InputArgument) key, (InputArgument) data.HeadOverlay[key], (InputArgument) 1f);
          }
          if (data.HasHead)
            Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, (InputArgument) ped.Handle, (InputArgument) data.HeadLayout.shapeFirstId, (InputArgument) data.HeadLayout.shapeSecondId, (InputArgument) data.HeadLayout.shapeThirdId, (InputArgument) data.HeadLayout.skinFirstId, (InputArgument) data.HeadLayout.skinSecondId, (InputArgument) data.HeadLayout.skinThirdId, (InputArgument) data.HeadLayout.shapeMix, (InputArgument) data.HeadLayout.skinMix, (InputArgument) data.HeadLayout.thirdMix, (InputArgument) data.HeadLayout.isParent);
        }
        else
          Function.Call(Hash.SET_PED_COMPONENT_VARIATION, (InputArgument) ped.Handle, (InputArgument) 2, (InputArgument) data.DrawableCollection[2], (InputArgument) data.TextureCollection[2], (InputArgument) data.PaletteCollection[2]);
        Function.Call((Hash) 5548371331445766729, (InputArgument) ped.Handle, (InputArgument) data.HairColor, (InputArgument) data.HairColor);
        Function.Call((Hash) 5295097686177659218, (InputArgument) ped.Handle, (InputArgument) 1, (InputArgument) 1, (InputArgument) data.BeardColor, (InputArgument) data.BeardColor);
      }
    }

    public int Model { get; set; }

    public Dictionary<int, int> DrawableCollection { get; set; }

    public Dictionary<int, int> TextureCollection { get; set; }

    public Dictionary<int, int> PaletteCollection { get; set; }

    public Dictionary<int, int> PropDrawableCollection { get; set; }

    public Dictionary<int, int> PropTextureCollection { get; set; }

    public Dictionary<int, int> HeadOverlay { get; set; }

    public HeadLayout HeadLayout { get; set; }

    public bool HasHead { get; set; }
  }
}
