using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKLab
{
    public static class RTData
    {
        public static readonly List<(string, string, string)> ReplaceRef = new()
        {
            ("Modding.Mod", "HKLab", "Mod"),
            ("Modding.ModHooks", "HKLab", "ModHooks"),
            ("Modding.ReflectionHelper", "HKLab", "ReflectionHelper")
        };
        public static readonly List<string> NDelegates = new();

        static RTData()
        {
            foreach(var v in typeof(RTData).Assembly.GetTypes().Where(x => x.FullName.StartsWith("HKLab.Delegates")))
            {
                ReplaceRef.Add(
                    ("ModHook." + v.Name, "HKLab.Delegates", v.Name)
                    );
            }
            foreach(var v in typeof(Modding.Mod).Assembly.GetTypes().Where(x=>x.FullName.StartsWith("Modding.Delegates")
            && x.BaseType == typeof(Delegate) && !x.IsNested))
            {
                NDelegates.Add(v.Name);
            }
        }
    }
}
