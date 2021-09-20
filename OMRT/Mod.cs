using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using JetBrains.Annotations;
using Modding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MonoMod;
using UnityEngine;

namespace HKLab
{

    [PublicAPI]
    public abstract class Mod : Modding.Mod, IGlobalSettings<ModSettings>, ILocalSettings<ModSettings>
    {
        public Mod() : this(null)
        {
            Modding.ReflectionHelper.SetField(this, "globalSettingsType", GlobalSettingsType);
            Modding.ReflectionHelper.SetField(this, "saveSettingsType", LocalSettingsType);
        }
        public Mod(string name) : base(name)
        {
            Modding.ReflectionHelper.SetField(this, "globalSettingsType", GlobalSettingsType);
            Modding.ReflectionHelper.SetField(this, "saveSettingsType", LocalSettingsType);
        }
        internal virtual List<(string, string)> preloadNames() => null;
        public readonly string _globalSettingsPath = null;
        public override List<(string, string)> GetPreloadNames() => preloadNames();

        public void OnLoadGlobal(ModSettings s)
        {
            try
            {
                GlobalSettings = s;
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        public ModSettings OnSaveGlobal()
        {
            return GlobalSettings;
        }

        public void OnLoadLocal(ModSettings s)
        {
            try
            {
                SaveSettings = s;
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        public ModSettings OnSaveLocal()
        {
            return SaveSettings;
        }

        public virtual ModSettings GlobalSettings { get; set; } = null;
        public virtual ModSettings SaveSettings { get; set; } = null;


        internal virtual Type GlobalSettingsType
        {
            get
            {
                if (_gsettingsType == null)
                {
                    ModSettings settings = GlobalSettings;
                    if (settings != null)
                    {
                        _gsettingsType = settings.GetType();
                    }
                }
                return _gsettingsType;
            }
        }
        private Type _gsettingsType = null;

        internal virtual Type LocalSettingsType
        {
            get
            {
                if (_lsettingsType == null)
                {
                    ModSettings settings = SaveSettings;
                    if (settings != null)
                    {
                        _lsettingsType = settings.GetType();
                    }
                }
                return _lsettingsType;
            }
        }
        private Type _lsettingsType = null;

    }
}