using Assets.Audio;
using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;
using UnityEngine.Rendering.Universal;


namespace Assets.Scripts
{
    public static class GameManager
    {

        public static Player player;

        public static AudioManager AudioManager;

        public static Light2D globalLight;

        static Dictionary< int , ConcurrentDictionary< string , int > > finishObjMap = new();
        static Dictionary< int , int > finishObjNeedMap = new Dictionary< int , int >();
        static List<MovingObject> movingObjList = new();
        public static int currentLevel = 1;
                
        public static void addFinishObject(string tag, MovingObject obj){
            // Debug.Log("addFinishObject: " + tag);
            if(finishObjMap.ContainsKey(currentLevel))
            {
                if(!finishObjMap[currentLevel].TryGetValue(tag, out int value)){
                    finishObjMap[currentLevel].TryAdd(tag,0);
                }
                Debug.Log("addFinishObject: tag " + tag + " level: "+ currentLevel+ " has "+ finishObjMap[currentLevel].Count+" done");
                if (finishObjMap[currentLevel].Count == finishObjNeedMap[currentLevel]){
                    Debug.Log("win!");
                    for (int i = 0; i < movingObjList.Count; i++)
                    {
                        // movingObjList[i].Done();
                    }
                }
            }
        }

        public static void removeFinishObject(string tag, MovingObject obj){
            // Debug.Log("removeFinishObject: " + tag);
            if(finishObjMap.ContainsKey(currentLevel))
            {
                if(finishObjMap[currentLevel].ContainsKey(tag)){
                    finishObjMap[currentLevel].TryRemove(tag, out _);
                }
                Debug.Log("removeFinishObject: tag " + tag + " level: "+ currentLevel+ " has "+ finishObjMap[currentLevel].Count+" done");
            }
        }

        public static void Init()
        {

            Debug.Log("loading new game!");

            player = GameObject.FindObjectOfType<Player>();
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("MovingObject");
            // 转换为 MovingObject 类型的数组
            List<MovingObject> movingObjects = new();

            for (int i = 0; i < taggedObjects.Length; i++)
            {
                movingObjects.Add(taggedObjects[i].GetComponent<MovingObject>());
            }
            finishObjMap[1] = new();
            finishObjNeedMap[1] = 2;
            AudioManager = UnityUtils.MakeObject<AudioManager>("AudioManager");

            globalLight = GameObject.Find("Global Light").GetComponent<Light2D>();

        }

        public static void LightOn()
        {
            AudioManager.PlaySFX("switch");
            globalLight.intensity = 0.5f;
            player.GetComponentInChildren<Light2D>().intensity = 0f;
        }

        public static void LightOff()
        {
            AudioManager.PlaySFX("switch");
            globalLight.intensity = 0f;
            player.GetComponentInChildren<Light2D>().intensity = 1f;
        }

    }
}
