using HarmonyLib;
using NCMS;
using UnityEngine;

namespace Easy_Localization{
    [ModEntry]
    class Main : MonoBehaviour{
        void Awake()
        {
            LocalizeManager.Init();
            
            Harmony.CreateAndPatchAll(typeof(LocalizeManager));
        }
    }
}