using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;    
using System.Threading.Tasks;
using Mono.Cecil;

namespace OMConvertS
{
    public static class Converter
    {
        public delegate IMetadataScope
            GetTypeHandler(ModuleDefinition module, string @namespace, string name, string fullname);
        public static void ConvertOldMod(ModuleDefinition mainModule,GetTypeHandler gt = null)
        {
            var rl = HKLab.RTData.ReplaceRef;
            List<AssemblyNameReference> ams = new List<AssemblyNameReference>();
            foreach(var v in mainModule.GetTypeReferences())
            {
                (string, string,string) t = rl.FirstOrDefault(x => x.Item1 == v.FullName);
                if (t != default)
                {
                    v.Namespace = t.Item2;
                    v.Name = t.Item3;
                }
                IMetadataScope scope = gt?.Invoke(mainModule, v.Namespace, v.Name, v.FullName);
                if (scope != null)
                {
                    v.Scope = scope;
                }
                AssemblyNameReference ar = (AssemblyNameReference)v.Scope;
                if (!ams.Contains(ar)) ams.Add(ar);
            }
            mainModule.AssemblyReferences.Clear();
            foreach (var v in ams) mainModule.AssemblyReferences.Add(v);
            ams.Clear();
        }
        public static void ConvertOM(ModuleDefinition mainModule)
        {
            foreach(var v in mainModule.GetTypeReferences())
            {
                if (v.Namespace == "Modding" && HKLab.RTData.NDelegates.Contains(v.Name))
                {
                    v.Namespace = "Modding.Delegates";
                }
            }
        }

        public static void TryConvert(ModuleDefinition module, GetTypeHandler gt = null)
        {
            if (module.Runtime == TargetRuntime.Net_4_0) ConvertOM(module);
            else
            {
                ConvertOldMod(module, gt);
            }
        }

        public static GetTypeHandler GFromAssembly(params Assembly[] assemblies)
        {
            IMetadataScope GetT(ModuleDefinition module, string @namespace, string name, string fullname)
            {
                Type t = null;
                foreach(var v in assemblies)
                {
                    t = v.GetType(fullname, false, true);
                    if (t != null) break;
                }
                if (t != null)
                {
                    AssemblyNameReference im = module.AssemblyReferences.
                        FirstOrDefault(x => x.FullName == t.Assembly.GetName().FullName);
                    if (im == null)
                    {
                        im = AssemblyNameReference.Parse(t.Assembly.GetName().FullName);
                        module.AssemblyReferences.Add(im);
                    }
                    return im;
                }
                return null;
            }
            return new GetTypeHandler(GetT);
        }
        public static GetTypeHandler GFrom(params AssemblyDefinition[] assemblies)
        {
            IMetadataScope GetT(ModuleDefinition module, string @namespace, string name, string fullname)
            {
                TypeReference t = null;
                foreach (var v in assemblies)
                {
                    t = v.MainModule.GetType(@namespace, name);
                    if (t != null) break;
                }
                if (t != null)
                {
                    AssemblyNameReference im = module.AssemblyReferences.
                        FirstOrDefault(x => x.FullName == t.Module.Assembly.FullName);
                    if (im == null)
                    {
                        im = AssemblyNameReference.Parse(t.Module.Assembly.FullName);
                        module.AssemblyReferences.Add(im);
                    }
                    return im;
                }
                return null;
            }
            return new GetTypeHandler(GetT);
        }
    }
}
