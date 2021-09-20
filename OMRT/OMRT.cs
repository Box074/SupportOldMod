using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Modding;

namespace HKLab
{
    public class OMRT : Modding.Mod
    {
        public const int CompileVersion = 1;
        public static readonly int CurrentVersion = CompileVersion;
        public override string GetVersion()
        {
            return CompileVersion.ToString();
        }
        public OMRT() : base()
        {
            new Thread(ReflectionHelper.PreloadCommonTypes).Start();
        }
    }
}
