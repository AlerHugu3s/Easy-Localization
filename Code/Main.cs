using HarmonyLib;
using NCMS;
using UnityEngine;

namespace EasyLocalization{
    [ModEntry]
    class Main : MonoBehaviour{
        void Awake()
        {
            LocalizeManager.Init();
            
            Harmony.CreateAndPatchAll(typeof(LocalizeManager));
        }
    }
}