using Assets.Audio;
using Assets.Utils;
using Assets.Scripts.API;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Threading.Tasks;
using System.Linq;

namespace Assets.Scripts
{
    public static class GameManager
    {

        public static Player player;

        public static AudioManager AudioManager;

        public static Light2D globalLight;

        static Dictionary< int , ILevel > levelMap = new();
        static Dictionary< int , int > finishObjNeedMap = new Dictionary< int , int >();
        public static int currentLevel = 0;

        public static GameObject levelObj;
        public static ILevel level; 
        public static ILevelProgressUI levelProgressUI;

        public static AstarPath aStarPath;

        public static bool isTransitioning = false;
        public static bool lightOn = true;

        public static bool ready = false;

        public static GameObject welcomeText;
        private static List<GameObject> _tmpdst;
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
            levelProgressUI = GameObject.FindObjectOfType<LevelProgressUI>();
            AudioManager = UnityUtils.MakeObject<AudioManager>("AudioManager");
            globalLight = GameObject.Find("Global Light").GetComponent<Light2D>();
            aStarPath = GameObject.FindObjectOfType<AstarPath>();

            welcomeText = GameObject.Find("welcomeText");

        }

        public static async void ShowFirstScreen()
        {

            LoadNextLevel();

            player.GetComponentInChildren<Light2D>().intensity = 0f;
            player.gameObject.SetActive(false);

            var dstLst = GameObject.FindGameObjectsWithTag("OriginLocation").ToList();
            _tmpdst = dstLst;

            for (int i = 0; i < dstLst.Count; i++)
            {
                var dst = dstLst[i];
                if (dst.TryGetComponent<OriginLocation>(out OriginLocation originLocation))
                {
                    originLocation.gameObject.SetActive(false);
                }
            }

            await Task.Delay(1000);

            ready = true;

        }

        public static async void StartGame() {

            welcomeText.SetActive(false);
            GameManager.LightOff();

            await Task.Delay(6000);

            AudioManager.PlayBGM("bgm");
            player.GetComponentInChildren<Light2D>().intensity = 0f;
            player.gameObject.SetActive(true);

            await Task.Delay(500);

            AudioManager.PlaySFX("flashlight");
            player.GetComponentInChildren<Light2D>().intensity = Config.PLAYER_LIGHT_INTENSITY;

            await Task.Delay(2000);

            var dstLst = _tmpdst;

            for (int i = 0; i < dstLst.Count; i++)
            {
                var dst = dstLst[i];
                if (dst.TryGetComponent<OriginLocation>(out OriginLocation originLocation))
                {
                    originLocation.gameObject.SetActive(true);
                }
            }

        }

        public static void LoadNextLevel()
        {

            currentLevel++;

            if (levelObj != null)
            {
                GameObject.DestroyImmediate(levelObj);
            }

            var levelObjPrefab = Resources.Load<GameObject>("Prefabs/Level" + currentLevel);

            if (levelObjPrefab == null)
            {
                Debug.LogError("Level prefab not found for level: " + currentLevel);
                return;
            }

            levelObj = GameObject.Instantiate(levelObjPrefab);

            aStarPath.Scan();

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
            finishObjNeedMap[currentLevel] = originLocationList.Count;
            ILevel newLevel = new Level();
            newLevel.LoadLevel(1, movingObjList, originLocationList);
            levelMap[currentLevel] = newLevel;
            level = newLevel;

            isTransitioning = false;
        }

        public static void LightOn()
        {
            if (lightOn) return;

            lightOn = true;
            //AudioManager.PlaySFX("switch");
            globalLight.intensity = 0.5f;
            player.GetComponentInChildren<Light2D>().intensity = 0f;

            level.GetMovingObjects().ForEach(movingobj => movingobj.OnSpot());
        }

        public static void LightOff()
        {
            if (!lightOn) return;

            lightOn = false;
            AudioManager.PlaySFX("switch");
            globalLight.intensity = 0f;
            player.GetComponentInChildren<Light2D>().intensity = Config.PLAYER_LIGHT_INTENSITY;

            level.GetMovingObjects().ForEach(movingobj => movingobj.OnUnspot());
        }
        
        public static void Win(){

            if(levelMap.ContainsKey(currentLevel))
            {
                isTransitioning = true;
                levelMap[currentLevel].Win();
            }
        }
    }
}
