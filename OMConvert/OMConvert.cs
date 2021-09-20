using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using Modding;
using Mono.Cecil;
using MonoMod.RuntimeDetour.HookGen;
using UnityEngine;

namespace OMConvert
{
    public class OMConvert : Mod
    {
        public static Type modInstanceT = typeof(Mod).Assembly.GetType("Modding.ModLoader")
            .GetNestedType("ModInstance");
        public static Type modErrorStateT = typeof(Mod).Assembly.GetType("Modding.ModLoader")
            .GetNestedType("ModErrorState");
        public OMConvert()
        {
            Log("SupportOldMods: Version " + HKLab.OMRT.CurrentVersion);
            HookEndpointManager.Add(typeof(Mod).Assembly.GetType("Modding.ModLoader", true, true).GetMethod("AddModInstance",
                BindingFlags.Static | BindingFlags.NonPublic)
                , new Action<Action<Type, object>, Type, object>(MAddModInstance));
        }
        public void MAddModInstance(Action<Type, object> orig, Type t, object mi)
        {
            orig(t, mi);
            List<string> m = new List<string>();
            Assembly Resolve(object sender, ResolveEventArgs args)
            {
                var asm_name = new AssemblyName(args.Name);

                if (m.FirstOrDefault(x => x.EndsWith($"{asm_name.Name}.dll")) is string path)
                    return Assembly.LoadFrom(path);

                return null;
            }

            AppDomain.CurrentDomain.AssemblyResolve += Resolve;
            RemoveCache();
            
            foreach(var v in FindOldMods())
            {
                try
                {
                    m.Add(ConvertMod(v));
                    Log("Convert Done: " + v);
                    
                }catch(Exception e)
                {
                    LogError(e.ToString());
                    LogError("Failed: " + v);
                }
            }
            foreach (var v in m)
            {
                Assembly ass = Assembly.LoadFrom(v);
                foreach (var v2 in ass.GetTypes().Where(x => x.IsSubclassOf(typeof(IMod)) && !x.IsAbstract))
                {
                    try
                    {
                        IMod mod = v2.GetConstructor(Type.EmptyTypes)?.Invoke(null) as IMod;
                        if (mod != null)
                        {
                            orig(v2, CreateModInstance(mod, mod.GetName(), null, false));
                        }
                        Log("Done: " + v);
                    }
                    catch (Exception e)
                    {
                        orig(v2, CreateModInstance(null, v2.Name, "Construct", false));
                        LogError(e.ToString());
                        LogError("Failed: " + v);
                    }
                }
            }
            AppDomain.CurrentDomain.AssemblyResolve -= Resolve;
        }
        public string[] FindOldMods()
        {
            return Directory.GetFiles(OldModsDir, "*.dll").Where(x => !Path.GetFileName(x).StartsWith("cache_")).ToArray();
        }
        public void RemoveCache()
        {
            foreach (var v in Directory.GetFiles(OldModsDir, "*.dll").Where(x => Path.GetFileName(x).StartsWith("cahce_")))
                File.Delete(v);
        }
        public string ConvertMod(string p)
        {
            using (AssemblyDefinition ass = AssemblyDefinition.ReadAssembly(p))
            {
                try
                {
                    Log("Try to convert: " + p);
                    string s = Path.Combine(OldModsDir, "cache_" + Path.GetFileName(p));
                    OMConvertS.Converter.TryConvert(
                        ass.MainModule, OMConvertS.Converter.GFromAssembly(AppDomain.CurrentDomain.GetAssemblies()));
                    ass.Write(s);
                    File.SetAttributes(s, FileAttributes.Normal | FileAttributes.Normal | FileAttributes.Hidden);
                    return s;
                }catch(Exception e)
                {
                    LogError(e);
                }
            }
            return string.Empty;
        }

        public static string OldModsDir
        {
            get
            {
                string p = SystemInfo.operatingSystemFamily switch
                {
                    OperatingSystemFamily.Windows => Path.Combine(Application.dataPath, "Managed", "oldMods"),
                    OperatingSystemFamily.MacOSX => Path.Combine(Application.dataPath, "Resources", "Data", "Managed", "oldMods"),
                    OperatingSystemFamily.Linux => Path.Combine(Application.dataPath, "Managed", "oldMods"),

                    OperatingSystemFamily.Other => null,

                    _ => throw new ArgumentOutOfRangeException()
                };
                if (!Directory.Exists(p)) Directory.CreateDirectory(p);

                return p;
            }
        }
        
        public static object CreateModInstance(IMod mod,string name,string err,bool enable = false)
        {
            Type enumT = typeof(Nullable<>).MakeGenericType(modErrorStateT);
            object mi = Activator.CreateInstance(modInstanceT);
            modInstanceT.GetField("Mod").SetValue(mi, mod);
            modInstanceT.GetField("Name").SetValue(mi, name);
            modInstanceT.GetField("Enable").SetValue(mi, enable);
            if (string.IsNullOrEmpty(err))
            {
                modInstanceT.GetField("Error").SetValue(mi, Activator.CreateInstance(enumT));
            }
            else {

                object m = Enum.Parse(modErrorStateT, err, true);
                modInstanceT.GetField("Error").SetValue(mi, Activator.CreateInstance(enumT, m));
            }
            return mi;
        }
    }
}
