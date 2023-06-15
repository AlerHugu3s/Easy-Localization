using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HarmonyLib;
using SimpleJSON;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EasyLocalization
{
    public class LocalizeManager
    {
        public static Dictionary<string,string> ModDicts = new Dictionary<string, string>();
        public static void InitAllModsDir()
        {
            string modsDir = Application.dataPath + "/../Mods";
            if (Directory.Exists(modsDir))
            {
                foreach (string directory in Directory.GetDirectories(modsDir))
                {
                    string jsonPath = directory + "/mod.json";
                    if (File.Exists(jsonPath))
                    {
                        string JSONString = File.ReadAllText(jsonPath, Encoding.UTF8);
                        JSONNode node = JSON.Parse(JSONString);
                        string key = node.GetValueOrDefault("name", "");
                        ModDicts.Add(key,directory);
                    }
                }
            }
        }
        
        public static void Init()
        {
            InitAllModsDir();
            string pLanguage = Traverse.Create(LocalizedTextManager.instance).Field("language").GetValue() as string;
            LoadLocalizedTextFromFile(pLanguage);
        }

        public static void LoadLocalizedTextFromFile(string language)
        {
            foreach (var kv in ModDicts)
            {
                if (kv.Key == null || kv.Value == null) continue;
                string key = kv.Key;
                string embadedPath = kv.Value + "\\LocalizedText\\";
                
                if (!File.Exists(embadedPath + language + ".txt")) language = "en";
                if (!File.Exists(embadedPath + language + ".txt")) continue;
                
                Debug.Log(("LOAD MOD <"+key+"> LANGUAGE " + language));
                
                string oriStr = File.ReadAllText(embadedPath + language + ".txt",Encoding.UTF8);
                string midStr = oriStr.Replace("\n", "");
                string[] strs = midStr.Split(';');
                int index = 0;
                foreach (string str in strs)
                {
                    index += 1;
                    string[] text = str.Split(',');
                    if (text.Length != 2)
                    {
                        Debug.LogWarning("index "+ index +  " in LocalizedFile " + language + " of the MOD <"+ key +"> has problem !");
                        continue;
                    }
                    NCMS.Utils.Localization.AddOrSet(text[0],text[1]);
                }
            }
        }
        
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalizedTextManager),"setLanguage")]
        public static void setModLanguage(string pLanguage)
        {
            Debug.Log(("LOAD MOD LANGUAGE " + pLanguage));
            LoadLocalizedTextFromFile(pLanguage);
        }
    }
}