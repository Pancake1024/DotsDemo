using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace PFF.Common
{
    public static class Utils
    {
        #region Platform

        public static PlatfromType GetPlatformType()
        {
            PlatfromType platfromType = PlatfromType.Editor;
#if UNITY_EDITOR
            platfromType = PlatfromType.Editor;
#elif UNITY_STANDALONE || UNITY_STANDALONE_WIN
            platfromType = PlatfromType.PC;
#elif UNITY_ANDROID || UNITY_IOS
            platfromType = PlatfromType.Mobile;
#endif
            return platfromType;
        }

        #endregion
    }
}