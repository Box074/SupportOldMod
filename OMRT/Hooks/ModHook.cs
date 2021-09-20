using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKLab.Delegates;
using UnityEngine;

namespace HKLab
{
    public class ModHooks
    {
        private static ModHooks _instance = null;
        public string ModVersion = Modding.ModHooks.ModVersion;
        public readonly bool IsCurrent = true;
        public readonly List<string> LoadedMods = Modding.ModHooks.GetAllMods().Select(x => x.GetName()).ToList();
        public static readonly char PathSeperator = SystemInfo.operatingSystem.Contains("Windows") ? '\\' : '/';
        public GameVersionData version;
        public ModHooks()
        {
            version = Modding.ModHooks.version;
        }
        public static ModHooks Instance
        {
            get
            {
                if (_instance == null) _instance = new ModHooks();
                return _instance;
            }
        }
        public event LanguageGetHandler LanguageGetHook
        {
            add => Modding.ModHooks.LanguageGetHook += HooksD.Add(value,
                new Modding.Delegates.LanguageGetProxy(
                    (a, b, c) =>
                    {
                        return value(a, b);
                    }
                    )
                );

            remove => Modding.ModHooks.LanguageGetHook -= HooksD.Remove<Modding.Delegates.LanguageGetProxy>(value);
        }
        public event CursorHandler CursorHook
        {
            add => Modding.ModHooks.CursorHook += new Action(value);
            remove => Modding.ModHooks.CursorHook -= new Action(value);
        }
        public event ColliderCreateHandler ColliderCreateHook
        {
            add => Modding.ModHooks.ColliderCreateHook += new Action<GameObject>(value);
            remove => Modding.ModHooks.ColliderCreateHook -= new Action<GameObject>(value);
        }
        public event GameObjectHandler ObjectPoolSpawnHook
        {
            add => Modding.ModHooks.ObjectPoolSpawnHook += new Func<GameObject, GameObject>(value);
            remove => Modding.ModHooks.ObjectPoolSpawnHook -= new Func<GameObject, GameObject>(value);
        }
        /** public event HKLab.Hooks.GameObjectFsmHandler OnGetEventSenderHook {
         add => Modding.ModHooks.OnGetEventSenderHook += new Modding.GameObjectFsmHandler(value);
         remove => Modding.ModHooks.OnGetEventSenderHook -= new Modding.GameObjectFsmHandler(value); 
}
         **/
        public event ApplicationQuitHandler ApplicationQuitHook
        {
            add => Modding.ModHooks.ApplicationQuitHook += new Action(value);
            remove => Modding.ModHooks.ApplicationQuitHook -= new Action(value);
        }
        /** public event HKLab.Hooks.SetFontHandler SetFontHook {
         add => Modding.ModHooks.SetFontHook += new Modding.SetFontHandler(value);
         remove => Modding.ModHooks.SetFontHook -= new Modding.SetFontHandler(value); 
}
         **//** public event HKLab.Hooks.TextDirectionProxy TextDirectionHook {
         add => Modding.ModHooks.TextDirectionHook += new Modding.TextDirectionProxy(value);
         remove => Modding.ModHooks.TextDirectionHook -= new Modding.TextDirectionProxy(value); 
}
         **/
        public event HitInstanceHandler HitInstanceHook
        {
            add => Modding.ModHooks.HitInstanceHook += new Modding.Delegates.HitInstanceHandler(value);
            remove => Modding.ModHooks.HitInstanceHook -= new Modding.Delegates.HitInstanceHandler(value);
        }
        public event DrawBlackBordersHandler DrawBlackBordersHook
        {
            add => Modding.ModHooks.DrawBlackBordersHook += new Action<List<GameObject>>(value);
            remove => Modding.ModHooks.DrawBlackBordersHook -= new Action<List<GameObject>>(value);
        }
        public event OnEnableEnemyHandler OnEnableEnemyHook
        {
            add => Modding.ModHooks.OnEnableEnemyHook += new Modding.Delegates.OnEnableEnemyHandler(value);
            remove => Modding.ModHooks.OnEnableEnemyHook -= new Modding.Delegates.OnEnableEnemyHandler(value);
        }
        /** public event HKLab.Hooks.OnRecieveDeathEventHandler OnRecieveDeathEventHook {
         add => Modding.ModHooks.OnRecieveDeathEventHook += new Modding.OnRecieveDeathEventHandler(value);
         remove => Modding.ModHooks.OnRecieveDeathEventHook -= new Modding.OnRecieveDeathEventHandler(value); 
}
         **/
        public event OnReceiveDeathEventHandler OnReceiveDeathEventHook
        {
            add => Modding.ModHooks.OnReceiveDeathEventHook += new Modding.Delegates.OnReceiveDeathEventHandler(value);
            remove => Modding.ModHooks.OnReceiveDeathEventHook -= new Modding.Delegates.OnReceiveDeathEventHandler(value);
        }
        /** public event HKLab.Hooks.OnRecordKillForJournalHandler OnRecordKillForJournalHook {
         add => Modding.ModHooks.OnRecordKillForJournalHook += new Modding.OnRecordKillForJournalHandler(value);
         remove => Modding.ModHooks.OnRecordKillForJournalHook -= new Modding.OnRecordKillForJournalHandler(value); 
}
         **/
        public event RecordKillForJournalHandler RecordKillForJournalHook
        {
            add => Modding.ModHooks.RecordKillForJournalHook += new Modding.Delegates.RecordKillForJournalHandler(value);
            remove => Modding.ModHooks.RecordKillForJournalHook -= new Modding.Delegates.RecordKillForJournalHandler(value);
        }
        public event SetBoolProxy SetPlayerBoolHook
        {
            add => Modding.ModHooks.SetPlayerBoolHook += HooksD.Add(value,
                new Modding.Delegates.SetBoolProxy((a, b) =>
                {
                    value(a, b);
                    return PlayerData.instance.GetBoolInternal(a);
                }));
            remove => Modding.ModHooks.SetPlayerBoolHook -= HooksD.Remove<Modding.Delegates.SetBoolProxy>(value);
        }
        public event GetBoolProxy GetPlayerBoolHook
        {
            add => Modding.ModHooks.GetPlayerBoolHook += HooksD.Add(value,
                new Modding.Delegates.GetBoolProxy(
                    (a, b) =>
                    {
                        return value(a);
                    }
                    )
                );

            remove => Modding.ModHooks.GetPlayerBoolHook -= HooksD.Remove<Modding.Delegates.GetBoolProxy>(value);
        }
        public event SetIntProxy SetPlayerIntHook
        {
            add => Modding.ModHooks.SetPlayerIntHook += HooksD.Add(value,
                new Modding.Delegates.SetIntProxy((a, b) =>
                {
                    value(a, b);
                    return PlayerData.instance.GetIntInternal(a);
                }));
            remove => Modding.ModHooks.SetPlayerIntHook -= HooksD.Remove<Modding.Delegates.SetIntProxy>(value);
        }
        public event GetIntProxy GetPlayerIntHook
        {
            add => Modding.ModHooks.GetPlayerIntHook += HooksD.Add(value,
                new Modding.Delegates.GetIntProxy(
                    (a, b) =>
                    {
                        return value(a);
                    }
                    )
                );

            remove => Modding.ModHooks.GetPlayerIntHook -= HooksD.Remove<Modding.Delegates.GetIntProxy>(value);
        }
        public event SetFloatProxy SetPlayerFloatHook
        {
            add => Modding.ModHooks.SetPlayerFloatHook += HooksD.Add(value,
                new Modding.Delegates.SetFloatProxy((a, b) =>
                {
                    value(a, b);
                    return PlayerData.instance.GetFloatInternal(a);
                }));
            remove => Modding.ModHooks.SetPlayerFloatHook -= HooksD.Remove<Modding.Delegates.SetFloatProxy>(value);
        }
        public event GetFloatProxy GetPlayerFloatHook
        {
            add => Modding.ModHooks.GetPlayerFloatHook += HooksD.Add(value,
                new Modding.Delegates.GetFloatProxy(
                    (a, b) =>
                    {
                        return value(a);
                    }
                    )
                );

            remove => Modding.ModHooks.GetPlayerFloatHook -= HooksD.Remove<Modding.Delegates.GetFloatProxy>(value);
        }
        public event SetStringProxy SetPlayerStringHook
        {
            add => Modding.ModHooks.SetPlayerStringHook += HooksD.Add(value,
                new Modding.Delegates.SetStringProxy((a, b) =>
                {
                    value(a, b);
                    return PlayerData.instance.GetStringInternal(a);
                }));
            remove => Modding.ModHooks.SetPlayerStringHook -= HooksD.Remove<Modding.Delegates.SetStringProxy>(value);
        }
        public event GetStringProxy GetPlayerStringHook
        {
            add => Modding.ModHooks.GetPlayerStringHook += HooksD.Add(value,
                new Modding.Delegates.GetStringProxy(
                    (a, b) =>
                    {
                        return value(a);
                    }
                    )
                );

            remove => Modding.ModHooks.GetPlayerStringHook -= HooksD.Remove<Modding.Delegates.GetStringProxy>(value);
        }
        public event SetVector3Proxy SetPlayerVector3Hook
        {
            add => Modding.ModHooks.SetPlayerVector3Hook += HooksD.Add(value,
                new Modding.Delegates.SetVector3Proxy((a, b) =>
                {
                    value(a, b);
                    return PlayerData.instance.GetVector3Internal(a);
                }));
            remove => Modding.ModHooks.SetPlayerVector3Hook -= HooksD.Remove<Modding.Delegates.SetVector3Proxy>(value);
        }
        public event GetVector3Proxy GetPlayerVector3Hook
        {
            add => Modding.ModHooks.GetPlayerVector3Hook += HooksD.Add(value,
                new Modding.Delegates.GetVector3Proxy(
                    (a, b) =>
                    {
                        return value(a);
                    }
                    )
                );

            remove => Modding.ModHooks.GetPlayerVector3Hook -= HooksD.Remove<Modding.Delegates.GetVector3Proxy>(value);
        }
        public event SetVariableProxy SetPlayerVariableHook
        {
            add => Modding.ModHooks.SetPlayerVariableHook += new Modding.Delegates.SetVariableProxy(value);
            remove => Modding.ModHooks.SetPlayerVariableHook -= new Modding.Delegates.SetVariableProxy(value);
        }
        public event GetVariableProxy GetPlayerVariableHook
        {
            add => Modding.ModHooks.GetPlayerVariableHook += new Modding.Delegates.GetVariableProxy(value);
            remove => Modding.ModHooks.GetPlayerVariableHook -= new Modding.Delegates.GetVariableProxy(value);
        }
        /** public event HKLab.Hooks.NewPlayerDataHandler NewPlayerDataHook {
         add => Modding.ModHooks.NewPlayerDataHook += new Modding.NewPlayerDataHandler(value);
         remove => Modding.ModHooks.NewPlayerDataHook -= new Modding.NewPlayerDataHandler(value); 
}
         **/
        public event BlueHealthHandler BlueHealthHook
        {
            add => Modding.ModHooks.BlueHealthHook += new Func<int>(value);
            remove => Modding.ModHooks.BlueHealthHook -= new Func<int>(value);
        }
        public event TakeHealthProxy TakeHealthHook
        {
            add => Modding.ModHooks.TakeHealthHook += new Modding.Delegates.TakeHealthProxy(value);
            remove => Modding.ModHooks.TakeHealthHook -= new Modding.Delegates.TakeHealthProxy(value);
        }
        public event TakeDamageProxy TakeDamageHook
        {
            add => Modding.ModHooks.TakeDamageHook += new Modding.Delegates.TakeDamageProxy(value);
            remove => Modding.ModHooks.TakeDamageHook -= new Modding.Delegates.TakeDamageProxy(value);
        }
        public event AfterTakeDamageHandler AfterTakeDamageHook
        {
            add => Modding.ModHooks.AfterTakeDamageHook += new Modding.Delegates.AfterTakeDamageHandler(value);
            remove => Modding.ModHooks.AfterTakeDamageHook -= new Modding.Delegates.AfterTakeDamageHandler(value);
        }
        public event VoidHandler BeforePlayerDeadHook
        {
            add => Modding.ModHooks.BeforePlayerDeadHook += new Action(value);
            remove => Modding.ModHooks.BeforePlayerDeadHook -= new Action(value);
        }
        public event VoidHandler AfterPlayerDeadHook
        {
            add => Modding.ModHooks.AfterPlayerDeadHook += new Action(value);
            remove => Modding.ModHooks.AfterPlayerDeadHook -= new Action(value);
        }
        public event AttackHandler AttackHook
        {
            add => Modding.ModHooks.AttackHook += new Action<GlobalEnums.AttackDirection>(value);
            remove => Modding.ModHooks.AttackHook -= new Action<GlobalEnums.AttackDirection>(value);
        }
        public event DoAttackHandler DoAttackHook
        {
            add => Modding.ModHooks.DoAttackHook += new Action(value);
            remove => Modding.ModHooks.DoAttackHook -= new Action(value);
        }
        public event AfterAttackHandler AfterAttackHook
        {
            add => Modding.ModHooks.AfterAttackHook += new Action<GlobalEnums.AttackDirection>(value);
            remove => Modding.ModHooks.AfterAttackHook -= new Action<GlobalEnums.AttackDirection>(value);
        }
        public event SlashHitHandler SlashHitHook
        {
            add => Modding.ModHooks.SlashHitHook += new Modding.Delegates.SlashHitHandler(value);
            remove => Modding.ModHooks.SlashHitHook -= new Modding.Delegates.SlashHitHandler(value);
        }
        public event CharmUpdateHandler CharmUpdateHook
        {
            add => Modding.ModHooks.CharmUpdateHook += new Modding.Delegates.CharmUpdateHandler(value);
            remove => Modding.ModHooks.CharmUpdateHook -= new Modding.Delegates.CharmUpdateHandler(value);
        }
        public event HeroUpdateHandler HeroUpdateHook
        {
            add => Modding.ModHooks.HeroUpdateHook += new Action(value);
            remove => Modding.ModHooks.HeroUpdateHook -= new Action(value);
        }
        public event BeforeAddHealthHandler BeforeAddHealthHook
        {
            add => Modding.ModHooks.BeforeAddHealthHook += new Func<int, int>(value);
            remove => Modding.ModHooks.BeforeAddHealthHook -= new Func<int, int>(value);
        }
        /** public event HKLab.Hooks.BeforeAddHealthHandler _BeforeAddHealthHook {
         add => Modding.ModHooks._BeforeAddHealthHook += new Modding.BeforeAddHealthHandler(value);
         remove => Modding.ModHooks._BeforeAddHealthHook -= new Modding.BeforeAddHealthHandler(value); 
}
         **/
        public event FocusCostHandler FocusCostHook
        {
            add => Modding.ModHooks.FocusCostHook += new Func<float>(value);
            remove => Modding.ModHooks.FocusCostHook -= new Func<float>(value);
        }
        public event SoulGainHandler SoulGainHook
        {
            add => Modding.ModHooks.SoulGainHook += new Func<int, int>(value);
            remove => Modding.ModHooks.SoulGainHook -= new Func<int, int>(value);
        }
        public event DashVelocityHandler DashVectorHook
        {
            add => Modding.ModHooks.DashVectorHook += new Func<Vector2, Vector2>(value);
            remove => Modding.ModHooks.DashVectorHook -= new Func<Vector2, Vector2>(value);
        }
        public event DashPressedHandler DashPressedHook
        {
            add => Modding.ModHooks.DashPressedHook += new Func<bool>(value);
            remove => Modding.ModHooks.DashPressedHook -= new Func<bool>(value);
        }
        public event SavegameLoadHandler SavegameLoadHook
        {
            add => Modding.ModHooks.SavegameLoadHook += new Action<int>(value);
            remove => Modding.ModHooks.SavegameLoadHook -= new Action<int>(value);
        }
        public event SavegameSaveHandler SavegameSaveHook
        {
            add => Modding.ModHooks.SavegameSaveHook += new Action<int>(value);
            remove => Modding.ModHooks.SavegameSaveHook -= new Action<int>(value);
        }
        public event NewGameHandler NewGameHook
        {
            add => Modding.ModHooks.NewGameHook += new Action(value);
            remove => Modding.ModHooks.NewGameHook -= new Action(value);
        }
        public event ClearSaveGameHandler SavegameClearHook
        {
            add => Modding.ModHooks.SavegameClearHook += new Action<int>(value);
            remove => Modding.ModHooks.SavegameClearHook -= new Action<int>(value);
        }
        public event AfterSavegameLoadHandler AfterSavegameLoadHook
        {
            add => Modding.ModHooks.AfterSavegameLoadHook += new Action<SaveGameData>(value);
            remove => Modding.ModHooks.AfterSavegameLoadHook -= new Action<SaveGameData>(value);
        }
        public event BeforeSavegameSaveHandler BeforeSavegameSaveHook
        {
            add => Modding.ModHooks.BeforeSavegameSaveHook += new Action<SaveGameData>(value);
            remove => Modding.ModHooks.BeforeSavegameSaveHook -= new Action<SaveGameData>(value);
        }
        public event GetSaveFileNameHandler GetSaveFileNameHook
        {
            add => Modding.ModHooks.GetSaveFileNameHook += new Func<int, string>(value);
            remove => Modding.ModHooks.GetSaveFileNameHook -= new Func<int, string>(value);
        }
        public event AfterClearSaveGameHandler AfterSaveGameClearHook
        {
            add => Modding.ModHooks.AfterSaveGameClearHook += new Action<int>(value);
            remove => Modding.ModHooks.AfterSaveGameClearHook -= new Action<int>(value);
        }
        public event SceneChangedHandler SceneChanged
        {
            add => Modding.ModHooks.SceneChanged += new Action<string>(value);
            remove => Modding.ModHooks.SceneChanged -= new Action<string>(value);
        }
        public event BeforeSceneLoadHandler BeforeSceneLoadHook
        {
            add => Modding.ModHooks.BeforeSceneLoadHook += new Func<string, string>(value);
            remove => Modding.ModHooks.BeforeSceneLoadHook -= new Func<string, string>(value);
        }

    }
}