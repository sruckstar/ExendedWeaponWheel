using GTA;
using GTA.Native;
using GTA.Math;
using GTA.UI;
using System;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace ExendedWeaponWheel
{
    public class WeaponWheel : Script
    {
        static string path = @".\scripts\ExendedWeaponWheel.ini";
        static string path2 = @".\scripts\ShopList.ini";
        ScriptSettings config = ScriptSettings.Load(path);
        ScriptSettings shops = ScriptSettings.Load(path2);
        int temp;
        int temp1;
        int temp2;
        int temp3;
        int snack;
        int snack1;
        int snack2;
        int snack3;
        int armor;
        int fix;
        int fix2;
        int fix3;
        int fix4;
        int fix5;
        int fix6;
        int fix7;
        int fix8;
        int fix9;
        int clears;
        int blocked;
        int blocked2;
        int finish;
        int snackmode;
        int armormode;
        int propcreated;
        int propcreated2;
        int propcreated3;
        int propcreated4;
        int snackID;
        int armorID;
        string snackNAME;
        string armorNAME;

        Vector3 snackpos = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 armorpos = new Vector3(0.0f, 0.0f, 0.0f);
        Prop[] props = new Prop[3];
        Blip[] blips = new Blip[1000];

        public WeaponWheel()
        {
            Tick += OnTick;
            //KeyDown += OnKeyDown;

            ShopCreateBlips();
            GetControls();
        }

        void OnTick(object sender, EventArgs e)
        {
            WeaponWheelNotifs();
            WeaponWheelHealth();
            WeaponWheelArmor();
            CreatePropMichel();
            CreatePropFranklin();
            CreatePropFranklin2();
            CreatePropTrevor();
            AddSnackMichel();
            AddSnackFranklin();
            AddSnackFranklin2();
            AddSnackTrevor();
            AddArmorMichel();
            AddArmorFranklin();
            AddArmorFranklin2();
            AddArmorTrevor();
            AddSnackButton();
            AddArmorkButton();
            WeaponWheelDrop();
            PlayerNearShop();
            EndPlayerShop();
            AddSnackButtonShop();
        }

        /*/void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.K)
            {
                for (int i = 1; i < 9999; i++)
                {
                    string b = shops.GetValue<string>("SHOPS", $"SHOP{i}", "nope");
                    if (b != "nope")
                    {
                        string[] subs = b.Split(',');
                        float x = float.Parse(subs[0], CultureInfo.InvariantCulture.NumberFormat);
                        float y = float.Parse(subs[1], CultureInfo.InvariantCulture.NumberFormat);
                        float z = float.Parse(subs[2], CultureInfo.InvariantCulture.NumberFormat);
                        GTA.UI.Screen.ShowSubtitle($"X {x}, Y {y}, Z {z}");
                        Wait(1000);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }/*/

        void ShopCreateBlips()
        {
            temp = config.GetValue<int>("BLIPS", "enabled", 0);
            if(temp == 1)
            {
                for (int i = 1; i < 1000; i++)
                {
                    string b = shops.GetValue<string>("SHOPS", $"SHOP{i}", "nope");
                    if (b != "nope")
                    {
                        string[] subs = b.Split(',');
                        float x = float.Parse(subs[0], CultureInfo.InvariantCulture.NumberFormat);
                        float y = float.Parse(subs[1], CultureInfo.InvariantCulture.NumberFormat);
                        float z = float.Parse(subs[2], CultureInfo.InvariantCulture.NumberFormat);
                        Vector3 ShopCoords = new Vector3(x, y, z);
                        blips[i] = World.CreateBlip(ShopCoords);
                        Function.Call(Hash.SET_BLIP_SPRITE, blips[i], 59);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        void GetControls()
        {
            temp = config.GetValue<int>("CONTROLS", "oldversion", 0);
            if(temp == 0)
            {
                snackID = 361;
                snackNAME = "~INPUT_EAT_SNACK~";
                armorID = 362;
                armorNAME = "~INPUT_USE_ARMOR~";
            }
            else
            {
                temp = config.GetValue<int>("CONTROLS", "altsnackID", 324);
                string temp2 = config.GetValue<string>("CONTROLS", "altsnackNAME", "~INPUT_REPLAY_TIMELINE_DUPLICATE_CLIP~");
                snackID = temp;
                snackNAME = temp2;

                temp = config.GetValue<int>("CONTROLS", "altarmorID", 325);
                temp2 = config.GetValue<string>("CONTROLS", "altarmorNAME", "~INPUT_REPLAY_TIMELINE_PLACE_CLIP~");
                snackID = temp;
                snackNAME = temp2;
            }
        }

        void PlayerNearShop()
        {
            for (int i = 1; i < 1000; i++)
            {
                string b = shops.GetValue<string>("SHOPS", $"SHOP{i}", "nope");
                if (b != "nope")
                {
                    string[] subs = b.Split(',');
                    float x = float.Parse(subs[0], CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(subs[1], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(subs[2], CultureInfo.InvariantCulture.NumberFormat);


                    var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                    if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, x, y, z, 0) < 2.0 && blocked != 1)
                    {
                        DisplayInShop();
                        blocked = 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        void EndPlayerShop()
        {
            if (blocked == 1)
            {
                finish = 0;
                for (int i = 1; i < 1000; i++)
                {
                    string b = shops.GetValue<string>("SHOPS", $"SHOP{i}", "nope");
                    if (b != "nope")
                    {
                        string[] subs = b.Split(',');
                        float x = float.Parse(subs[0], CultureInfo.InvariantCulture.NumberFormat);
                        float y = float.Parse(subs[1], CultureInfo.InvariantCulture.NumberFormat);
                        float z = float.Parse(subs[2], CultureInfo.InvariantCulture.NumberFormat);
                        var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                        if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, x, y, z, 0) < 2.0)
                        {
                            finish = 1;

                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if(finish == 0)
                {
                    Function.Call(Hash.THEFEED_REMOVE_ITEM, clears);
                    blocked = 0;
                    finish = 0;

                }
            }
        }

        void CreatePropMichel()
        {
            LoadUnloadPropsM();
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) < 40.0 && propcreated == 0)
            {
                Model model1 = new Model(-1877813643);
                Model model2 = new Model(1111175276);
                model1.Request(10000);
                model2.Request(10000);
                SetPropMichel();
                props[2] = GTA.World.CreateProp(model2, new GTA.Math.Vector3(-812.7941f, 173.4821f, 76.02f), new GTA.Math.Vector3(0f, 0f, 0f), true, true);
                //props[2].FreezePosition = true;
                propcreated = 1;
            }
            else
            {
                position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) > 40.0 && propcreated == 1)
                {
                    props[0].Delete();
                    props[1].Delete();
                    props[2].Delete();
                    propcreated = 0;
                }
            }
        }

        void CreatePropFranklin()
        {
            LoadUnloadPropsF();
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) < 40.0 && propcreated2 == 0)
            {
                Model model1 = new Model(-1877813643);
                Model model2 = new Model(1111175276);
                model1.Request(10000);
                model2.Request(10000);
                SetPropFrnklin();
                props[2] = GTA.World.CreateProp(model2, new GTA.Math.Vector3(1.570109f, 530.7902f, 173.94f), new GTA.Math.Vector3(0f, 0f, 0f), true, true);
                //props[2].FreezePosition = true;
                propcreated2 = 1;
            }
            else
            {
                position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) > 40.0 && propcreated2 == 1)
                {
                    props[0].Delete();
                    props[1].Delete();
                    props[2].Delete();
                    propcreated2 = 0;
                }
            }
        }

        void CreatePropFranklin2()
        {
            LoadUnloadPropsF2();
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) < 40.0 && propcreated4 == 0)
            {
                Model model1 = new Model(-1877813643);
                Model model2 = new Model(1111175276);
                model1.Request(10000);
                model2.Request(10000);
                SetPropFrnklin2();     
                props[1] = GTA.World.CreateProp(model2, new GTA.Math.Vector3(-17.68428f, -1438.333f, 30.42f), new GTA.Math.Vector3(0f, 0f, 0f), true, true);
               // props[1].FreezePosition = true;
                propcreated4 = 1;
            }
            else
            {
                position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) > 40.0 && propcreated4 == 1)
                {
                    props[0].Delete();
                    props[1].Delete();
                    propcreated4 = 0;
                }
            }
        }

        void LoadUnloadPropsM()
        {
            if(temp == 1)
            {
                snackpos.X = -795.9562f;
                snackpos.Y = 185.489f;
                snackpos.Z = 72.6f;
            }
            else
            {
                snackpos.X = -812.381f;
                snackpos.Y = 179.592f;
                snackpos.Z = 76.745f;
            }
        }

        void LoadUnloadPropsF()
        {
            if (temp == 1)
            {
                snackpos.X = -11.8087f;
                snackpos.Y = 518.0471f;
                snackpos.Z = 174.59f;
            }
            else
            {
                snackpos.X = 3.075f;
                snackpos.Y = 528.747f;
                snackpos.Z = 174.628f;
            }
        }

        void LoadUnloadPropsF2()
        {
            if (temp == 1)
            {
                snackpos.X = -11.73833f;
                snackpos.Y = -1427.607f;
                snackpos.Z = 31.07f;
            }
            else
            {
                snackpos.X = -19.16891f;
                snackpos.Y = -1439.75f;
                snackpos.Z = 30.9482f;
            }
        }

        void SetPropMichel()
        {
            temp = config.GetValue<int>("PROPS", "newposition", 1);
            if(temp == 1)
            {
                props[0] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(-795.9562f, 185.489f, 72.6f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
                props[1] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(-795.8687f, 185.2973f, 72.59f), new GTA.Math.Vector3(0f, 0f, 49.9999f), false, false);
            }
            else
            {
                props[0] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(-812.0465f, 177.4039f, 76.74417f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
                props[1] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(-812.2594f, 177.4324f, 76.74417f), new GTA.Math.Vector3(0f, 0f, 49.9999f), false, false);
            }
        }

        void SetPropFrnklin()
        {
            temp = config.GetValue<int>("PROPS", "newposition", 1);
            if (temp == 1)
            {
                props[0] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(-11.8087f, 518.0471f, 174.59f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
                props[1] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(-11.64f, 518.05f, 174.59f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
            }
            else
            {
                props[0] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(2.653496f, 528.372f, 174.13f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
                props[1] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(2.84f, 528.3309f, 174.14f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
            }
        }

        void SetPropFrnklin2()
        {
            temp = config.GetValue<int>("PROPS", "newposition", 1);
            if (temp == 1)
            {
                props[0] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(-11.73833f, -1427.607f, 31.07f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
            }
            else
            {
                props[0] = GTA.World.CreateProp(-1877813643, new GTA.Math.Vector3(-19.16891f, -1439.75f, 30.9482f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
            }
        }

        void CreatePropTrevor()
        {
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, 1975.462f, 3818.728f, 33.436f, 0) < 40.0 && propcreated3 == 0)
            {
                Model model1 = new Model(-1877813643);
                Model model2 = new Model(1111175276);
                model1.Request(10000);
                model2.Request(10000);
                props[0] = GTA.World.CreateProp(model1, new GTA.Math.Vector3(1975.838f, 3819.072f, 33.42604f), new GTA.Math.Vector3(0f, 0f, 0f), false, false);
                props[1] = GTA.World.CreateProp(model1, new GTA.Math.Vector3(1976.027f, 3819.059f, 33.42604f), new GTA.Math.Vector3(0f, 0f, -81.99979f), false, false);
                props[2] = GTA.World.CreateProp(model2, new GTA.Math.Vector3(1974.735f, 3821.268f, 33.17f), new GTA.Math.Vector3(0f, 0f, 0f), true, true);
                //props[2].FreezePosition = true;
                propcreated3 = 1;
            }
            else
            {
                position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, 1975.462f, 3818.728f, 33.436f, 0) > 40.0 && propcreated3 == 1)
                {
                    props[0].Delete();
                    props[1].Delete();
                    props[2].Delete();
                    propcreated3 = 0;
                }
            }
        }

        void WeaponWheelNotifs() //tick
        {
            if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_HUD_COMPONENT_ACTIVE, 19) == true)
            {
                ShowMainNofif();
                fix7 = 1;
            }
            else
            {
                if (fix7 == 1 && (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_HUD_COMPONENT_ACTIVE, 19) == false))
                {
                    blocked2 = 0;
                    //GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix7 = 0;
                }
            }
        }

        void AddSnackMichel() //tick
        {
            LoadUnloadPropsM();
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            //float test = Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0);
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) < 1.0)
            {
                ShowAddSnack();
                fix = 1;
                snackmode = 1;
            }
            else
            {
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) > 1.0 && fix == 1)
                {
                    GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix = 0;
                    snackmode = 0;
                    armormode = 0;
                }
            }
        }

        void AddSnackFranklin() //tick
        {
            LoadUnloadPropsF();
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            //float test = Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0);
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) < 1.0)
            {
                ShowAddSnack();
                fix2 = 1;
                snackmode = 1;
            }
            else
            {
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) > 1.0 && fix2 == 1)
                {
                    GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix2 = 0;
                    snackmode = 0;
                    armormode = 0;
                }
            }
        }

        void AddSnackFranklin2() //tick
        {
            LoadUnloadPropsF2();
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            //float test = Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0);
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) < 1.0)
            {
                ShowAddSnack();
                fix8 = 1;
                snackmode = 1;
            }
            else
            {
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, snackpos.X, snackpos.Y, snackpos.Z, 0) > 1.0 && fix8 == 1)
                {
                    GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix8 = 0;
                    snackmode = 0;
                    armormode = 0;
                }
            }
        }

        void AddSnackTrevor() //tick
        {
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            //float test = Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0);
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, 1975.462f, 3818.728f, 33.436f, 0) < 1.0)
            {
                ShowAddSnack();
                fix3 = 1;
                snackmode = 1;
            }
            else
            {
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, 1975.462f, 3818.728f, 33.436f, 0) > 1.0 && fix3 == 1)
                {
                    GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix3 = 0;
                    snackmode = 0;
                    armormode = 0;
                }
            }
        }

        void AddArmorMichel() //tick
        {
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            //float test = Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0);
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -813.022f, 174.007f, 76.741f, 0) < 1.0)
            {
                ShowAddArmor();
                fix4 = 1;
                armormode = 1;
            }
            else
            {
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0) > 1.0 && fix4 == 1)
                {
                    GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix4 = 0;
                    snackmode = 0;
                    armormode = 0;
                }
            }
        }

        void AddArmorFranklin() //tick
        {
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            //float test = Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0);
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, 1.923f, 530.518f, 174.628f, 0) < 1.0)
            {
                ShowAddArmor();
                fix5 = 1;
                armormode = 1;
            }
            else
            {
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, 1.923f, 530.518f, 174.628f, 0) > 1.0 && fix5 == 1)
                {
                    GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix5 = 0;
                    snackmode = 0;
                    armormode = 0;
                }
            }
        }

        void AddArmorFranklin2() //tick
        {
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            //float test = Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0);
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -17.68428f, -1438.333f, 30.42f, 0) < 1.0)
            {
                ShowAddArmor();
                fix9 = 1;
                armormode = 1;
            }
            else
            {
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -17.68428f, -1438.333f, 30.42f, 0) > 1.0 && fix9 == 1)
                {
                    GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix9 = 0;
                    snackmode = 0;
                    armormode = 0;
                }
            }
        }

        void AddArmorTrevor() //tick
        {
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            //float test = Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, -812.325f, 177.918f, 76.741f, 0);
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, 1974.867f, 3820.693f, 33.437f, 0) < 1.0)
            {
                ShowAddArmor();
                fix6 = 1;
                armormode = 1;
            }
            else
            {
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, position.X, position.Y, position.Z, 1974.867f, 3820.693f, 33.437f, 0) > 1.0 && fix6 == 1)
                {
                    GTA.Native.Function.Call(GTA.Native.Hash.CLEAR_ALL_HELP_MESSAGES);
                    fix6 = 0;
                    snackmode = 0;
                    armormode = 0;
                }
            }
        }

        void AddSnackButton()
        {
            if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_CONTROL_PRESSED, 0, 54) && snackmode == 1) //~INPUT_PICKUP~
            {
                SetAllSnacks();
                SetAllSnacksNotif();
            }
        }

        void AddSnackButtonShop()
        {
            if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_CONTROL_PRESSED, 0, snackID) && blocked == 1) //~INPUT_PICKUP~
            {
                SetAllSnacks();
                SetAllSnacksNotif();
            }
        }

        void AddArmorkButton()
        {
            if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_CONTROL_PRESSED, 0, armorID) && armormode == 1) //~INPUT_PICKUP~
            {
                SetAllArmor();
                Wait(500);
            }
        }

        void WeaponWheelHealth() //tick
        {
            if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_CONTROL_PRESSED, 0, snackID) && GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_HUD_COMPONENT_ACTIVE, 19) == true) //INPUT_CREATOR_RT
            {
                SetHealthScript();
                Wait(500);
            }
        }

        void WeaponWheelArmor() //tick
        {
            if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_CONTROL_PRESSED, 0, armorID) && GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_HUD_COMPONENT_ACTIVE, 19) == true) //INPUT_REPLAY_TIMELINE_PLACE_CLIP
            {
                SetArmorScript();
                Wait(500);
            }
        }

        void WeaponWheelDrop() //tick
        {
            if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_CONTROL_PRESSED, 0, 57)) //INPUT_REPLAY_TIMELINE_PLACE_CLIP
            {

                GTA.Native.Function.Call(GTA.Native.Hash.REQUEST_ANIM_DICT, "mp_weapon_drop");
                while (GTA.Native.Function.Call<bool>(GTA.Native.Hash.HAS_ANIM_DICT_LOADED, "mp_weapon_drop") == false) Script.Wait(100);

                Ped ped = Game.Player.Character;
                GTA.Native.Function.Call(GTA.Native.Hash.SET_PED_DROPS_WEAPONS_WHEN_DEAD, ped, true);
                Hash weap = GTA.Native.Function.Call<Hash>(GTA.Native.Hash.GET_SELECTED_PED_WEAPON, ped);
                GTA.Native.Function.Call(GTA.Native.Hash.SET_PED_DROPS_INVENTORY_WEAPON, ped, weap, 0, 2.0, 0, -1);
                GTA.Native.Function.Call(GTA.Native.Hash.TASK_PLAY_ANIM, ped, "mp_weapon_drop", "drop_bh", 8.0, 2.0, -1, 0, 2.0, 0, 0, 0);
                Wait(500);
            }
        }

        void ShowMainNofif()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayTextBox("Нажмите ~INPUT_DROP_AMMO~, чтобы выбросить оружие.~n~Нажмите ~INPUT_EAT_SNACK~, чтобы съесть закуску.~n~Нажмите ~INPUT_USE_ARMOR~, чтобы использовать броню");
            }
            else
            {
                DisplayTextBox("Press ~INPUT_DROP_AMMO~ to drop weapon.~n~Press ~INPUT_EAT_SNACK~ to eat a snack.~n~Press ~INPUT_USE_ARMOR~ to use armor.");
            }
        }

        void SetAllSnacksNotif()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayNotify("Вы пополнили запас закусок до максимума.");
                snackmode = 0;
                armormode = 0;
            }
            else
            {
                DisplayNotify("Now you have a maximum of snacks.");
                snackmode = 0;
                armormode = 0;
            }
        }

        void ShowAddSnack()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayTextBox("Нажмите ~INPUT_PICKUP~, чтобы взять закуски.");
            }
            else
            {
                DisplayTextBox("Press ~INPUT_PICKUP~ to take snacks.");
            }
        }

        void ShowAddArmor()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayTextBox("Нажмите ~INPUT_PICKUP~, чтобы взять бронежилет.");
            }
            else
            {
                DisplayTextBox("Press ~INPUT_PICKUP~ to take armor.");
            }
        }

        void ShowNotifNotEat()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayTextBox("У вас нет закусок");
            }
            else
            {
                DisplayTextBox("You have no snacks.");           
            }
        }

        void ShowNotifNotArmor()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayTextBox("У вас нет бронежилета.");
            }
            else
            {
                DisplayTextBox("You have no armor.");
            }
        }

        void ShowNotifAllArmor()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayNotify("Вы взяли бронежилет.");
                snackmode = 0;
                armormode = 0;
            }
            else
            {
                DisplayNotify("You took the armor.");
                snackmode = 0;
                armormode = 0;
            }
        }

        void ShowNotArmorNotif()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayNotify("Вы не можете взять больше одного бронежилета.");
                snackmode = 0;
                armormode = 0;
            }
            else
            {
                DisplayNotify("You cannot take more than one armor.");
                snackmode = 0;
                armormode = 0;
            }
        }

        void DisplayInShop()
        {
            if (GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CURRENT_LANGUAGE) == 7) //_GET_CURRENT_LANGUAGE_ID
            {
                DisplayNotifyWithButton("взять закуски с собой.");
            }
            else
            {
                DisplayNotifyWithButton("to take snacks with you");
            }
        }


        void SetHealthScript()
        {
            GetSnacksMeteorite();
            if (snack > 0)
            {
                Game.Player.Character.Health += 50;
                snack--;
                SetSnacksMeteorite();
            }
            else
            {
                GetSnacksEgochaser();
                if (snack > 0)
                {
                    Game.Player.Character.Health += 20;
                    snack--;
                    SetSnacksEgochaser();
                }
                else
                {
                    GetSnacksPsandqs();
                    if (snack > 0)
                    {
                        Game.Player.Character.Health += 10;
                        snack--;
                        SetSnacksPsandqs();
                    }
                    else
                    {
                        ShowNotifNotEat();
                    }
                }
            }
        }

        void SetArmorScript()
        {
            GetArmor();
            if (armor > 0)
            {
                Game.Player.Character.Armor += 100;
                armor--;
                SetArmor();
            }
            else
            {
                ShowNotifNotArmor();
            }
        }

        void GetArmor()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                armor = config.GetValue<int>("ZERO", "ARMOR", 0);
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    armor = config.GetValue<int>("ONE", "ARMOR", 0);
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        armor = config.GetValue<int>("TWO", "ARMOR", 0);
                    }
                }
            }
        }

        void SetArmor()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                config.SetValue<int>("ZERO", "ARMOR", armor);
                config.Save();
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    config.SetValue<int>("ONE", "ARMOR", armor);
                    config.Save();
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        config.SetValue<int>("TWO", "ARMOR", armor);
                        config.Save();
                    }
                }
            }
        }

        void SetAllSnacks()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                config.SetValue<int>("ZERO", "Meteorite", 5);
                config.SetValue<int>("ZERO", "EGOCHASER", 15);
                config.SetValue<int>("ZERO", "PSANDQS", 30);
                config.Save();
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    config.SetValue<int>("ONE", "Meteorite", 5);
                    config.SetValue<int>("ONE", "EGOCHASER", 15);
                    config.SetValue<int>("ONE", "PSANDQS", 30);
                    config.Save();
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        config.SetValue<int>("TWO", "Meteorite", 5);
                        config.SetValue<int>("TWO", "EGOCHASER", 15);
                        config.SetValue<int>("TWO", "PSANDQS", 30);
                        config.Save();
                    }
                }
            }
        }

        void SetAllArmor()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                armor = config.GetValue<int>("ZERO", "ARMOR", 0);
                if (armor == 0)
                {
                    config.SetValue<int>("ZERO", "ARMOR", 1);
                    config.Save();
                    ShowNotifAllArmor();
                }
                else
                {
                    ShowNotArmorNotif();
                }
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    armor = config.GetValue<int>("ONE", "ARMOR", 0);
                    if (armor == 0)
                    {
                        config.SetValue<int>("ONE", "ARMOR", 1);
                        config.Save();
                        ShowNotifAllArmor();
                    }
                    else
                    {
                        ShowNotArmorNotif();
                    }
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        armor = config.GetValue<int>("TWO", "ARMOR", 0);
                        if (armor == 0)
                        {
                            config.SetValue<int>("TWO", "ARMOR", 1);
                            config.Save();
                            ShowNotifAllArmor();
                        }
                        else
                        {
                            ShowNotArmorNotif();
                        }
                    }
                }
            }
        }

        void GetSnacksMeteorite()
            {
                if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
                    {
                    snack = config.GetValue<int>("ZERO", "Meteorite", 0);
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                    {
                    snack = config.GetValue<int>("ONE", "Meteorite", 0);
                    }
                    else
                    {
                        if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                        {
                            snack = config.GetValue<int>("TWO", "Meteorite", 0);
                        }
                    }
                }
            }

        void SetSnacksMeteorite()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                config.SetValue<int>("ZERO", "Meteorite", snack);
                config.Save();
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    config.SetValue<int>("ONE", "Meteorite", snack);
                    config.Save();
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        config.SetValue<int>("TWO", "Meteorite", snack);
                        config.Save();
                    }
                }
            }
        }

        void GetSnacksEgochaser()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                snack = config.GetValue<int>("ZERO", "Egochaser", 0);
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    snack = config.GetValue<int>("ONE", "Egochaser", 0);
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        snack = config.GetValue<int>("TWO", "Egochaser", 0);
                    }
                }
            }
        }

        void SetSnacksEgochaser()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                config.SetValue<int>("ZERO", "Egochaser", snack);
                config.Save();
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    config.SetValue<int>("ONE", "Egochaser", snack);
                    config.Save();
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        config.SetValue<int>("TWO", "Egochaser", snack);
                        config.Save();
                    }
                }
            }
        }

        void GetSnacksPsandqs()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                snack = config.GetValue<int>("ZERO", "Psandqs", 0);
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    snack = config.GetValue<int>("ONE", "Psandqs", 0);
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        snack = config.GetValue<int>("TWO", "Psandqs", 0);
                    }
                }
            }
        }

        void SetSnacksPsandqs()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                config.SetValue<int>("ZERO", "Psandqs", snack);
                config.Save();
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    config.SetValue<int>("ONE", "Psandqs", snack);
                    config.Save();
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        config.SetValue<int>("TWO", "Psandqs", snack);
                        config.Save();
                    }
                }
            }
        }

        void GetSnacksOnDisplay()
        {
            if (Game.Player.Character.Model.Hash == new Model("player_zero").Hash)
            {
                snack1 = config.GetValue<int>("ZERO", "Meteorite", 0);
                snack2 = config.GetValue<int>("ZERO", "Egochaser", 0);
                snack3 = config.GetValue<int>("ZERO", "Psandqs", 0);
                DisplayNotify($"Meteorite: {snack1}~n~Egochaser: {snack2}~n~P's & Q's: {snack3}");
            }
            else
            {
                if (Game.Player.Character.Model.Hash == new Model("player_one").Hash)
                {
                    snack1 = config.GetValue<int>("ONE", "Meteorite", 0);
                    snack2 = config.GetValue<int>("ONE", "Egochaser", 0);
                    snack3 = config.GetValue<int>("ONE", "Psandqs", 0);
                    DisplayNotify($"Meteorite: {snack1}~n~Egochaser: {snack2}~n~P's & Q's: {snack3}");
                }
                else
                {
                    if (Game.Player.Character.Model.Hash == new Model("player_two").Hash)
                    {
                        snack1 = config.GetValue<int>("TWO", "Meteorite", 0);
                        snack2 = config.GetValue<int>("TWO", "Egochaser", 0);
                        snack3 = config.GetValue<int>("TWO", "Psandqs", 0);
                        DisplayNotify($"Meteorite: {snack1}~n~Egochaser: {snack2}~n~P's & Q's: {snack3}");
                    }
                }
            }
        }

        void ShowSnack()
        {
            if(temp1 != snack1 || temp2 != snack2 || temp3 != snack3)
            {
                Function.Call(Hash.THEFEED_REMOVE_ITEM, clears);
                DisplayNotify($"Meteorite: {snack1}~n~Egochaser: {snack2}~n~P's & Q's: {snack3}");
                temp1 = snack1;
                temp2 = snack2;
                temp3 = snack3;
            }
            else
            {
                if (blocked2 != 1)
                {
                    DisplayNotify($"Meteorite: {snack1}~n~Egochaser: {snack2}~n~P's & Q's: {snack3}");
                    blocked2 = 1;
                }
            }
        }

        void DisplayNotify(string text)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            clears = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, true, false);
        }

        void DisplayNotifyWithButton(string text)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            clears = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_REPLAY_INPUT, 1, "~INPUT_EAT_SNACK~", text);
        }

        void DisplayTextBox(string text)
        {
                var pos = new Point(250, 300);
                var Text4Screen = new GTA.UI.TextElement(text, pos, 0.45f, Color.White, GTA.UI.Font.ChaletLondon, GTA.UI.Alignment.Left, true, false, 400); //SIZE IS THIRD PARAMETER, LAST IS THE WIDTH BEFORE IT WRAPS
                Text4Screen.Enabled = true;

                GTA.UI.Screen.ShowHelpText(text, -1, true, false); //top left
        }
    }
}