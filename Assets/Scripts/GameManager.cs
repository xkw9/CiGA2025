using Assets.Audio;
using Assets.Utils;
using Assets.Scripts.API;
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

        static Dictionary< int , ILevel > levelMap = new();
        static Dictionary< int , int > finishObjNeedMap = new Dictionary< int , int >();
        public static int currentLevel = 1;
                
        public static void addFinishObject(string tag, MovingObject obj){
            // Debug.Log("addFinishObject: " + tag);

            if(levelMap.ContainsKey(currentLevel))
            {
                levelMap[currentLevel].addFinishObject(tag,obj);
            }
        }

        public static void removeFinishObject(string tag, MovingObject obj){
            // Debug.Log("removeFinishObject: " + tag);
            if(levelMap.ContainsKey(currentLevel))
            {
                levelMap[currentLevel].removeFinishObject(tag,obj);
            }
        }

        public static void Init()
        {

            Debug.Log("loading new game!");

            player = GameObject.FindObjectOfType<Player>();
            List<MovingObject> movingObjList = new();
            GameObject[] movingObjects = GameObject.FindGameObjectsWithTag("MovingObject");
            for (int i = 0; i < movingObjects.Length; i++)
            {
                movingObjList.Add(movingObjects[i].GetComponent<MovingObject>());
            }
            List<OriginLocation> originLocationList = new();
            GameObject[] originLocationObjects = GameObject.FindGameObjectsWithTag("OriginLocation");
            for (int i = 0; i < originLocationObjects.Length; i++)
            {
                originLocationList.Add(originLocationObjects[i].GetComponent<OriginLocation>());
            }
            // levelMap[1] = new();
            finishObjNeedMap[1] = 2;
            ILevel newLevel = new Level();
            newLevel.LoadLevel(1, movingObjList, originLocationList);
            levelMap[1] = newLevel;
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
