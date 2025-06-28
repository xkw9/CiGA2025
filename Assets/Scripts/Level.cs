using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Assets.Scripts.API;
using Assets.Scripts;
using UnityEngine;

namespace Assets
{
    public class Level : ILevel
    {
        ConcurrentDictionary< string , int >  finishObjMap = new();
        int finishObjNeed = 0;
        List<MovingObject> movingObjList = new();
        List<OriginLocation> originLocationList = new();
        public  int currentLevel = 1;
        public Vector2 GetPlayerOriginPos(){
            return new Vector2(0.0f,0.0f);
        }

        public OriginLocation GetDestination(MovingObject obj){
            return new OriginLocation();
        }

        public void LoadLevel(int level, List<MovingObject> movingObjList, List<OriginLocation> originLocationList){
            this.currentLevel = level;
            this.movingObjList = movingObjList;
            this.originLocationList = originLocationList;
            this.finishObjNeed = originLocationList.Count;
            GameManager.levelProgressUI.LoadLevelInfo(this.currentLevel, this.finishObjNeed);
        }   
        public void addFinishObject(string tag, MovingObject obj){
            // Debug.Log("addFinishObject: " + tag);
            if(!finishObjMap.TryGetValue(tag, out int value)){
                finishObjMap.TryAdd(tag,0);
                GameManager.levelProgressUI.RefreshCurAmount(finishObjMap.Count);
            }
            Debug.Log("addFinishObject: tag " + tag + " level: "+ currentLevel+ " has "+ finishObjMap.Count+" done");
            if (checkIfWin()){
                GameManager.levelProgressUI.startCountDown(5);
            }
        }

        public bool checkIfWin(){
            // Debug.Log("addFinishObject: " + tag);
            if (finishObjMap.Count == finishObjNeed){
                return true;
            }else{
                return false;
            }
        }

        public void removeFinishObject(string tag, MovingObject obj){
            // Debug.Log("removeFinishObject: " + tag);
            if(finishObjMap.ContainsKey(tag)){
                finishObjMap.TryRemove(tag, out _);
                GameManager.levelProgressUI.RefreshCurAmount(finishObjMap.Count);
            }
            Debug.Log("removeFinishObject: tag " + tag + " level: "+ currentLevel+ " has "+ finishObjMap.Count+" done");
        }

        public void Win(){
            Debug.Log("win!");
            GameManager.AudioManager.PlaySFX("drop_right_short");
            for (int i = 0; i < movingObjList.Count; i++)
            {
                movingObjList[i].Done();
                Debug.Log(movingObjList[i].objName+" done!");
            }

            foreach (var originLocation in originLocationList)
            {
                originLocation.gameObject.SetActive(false);
            }

            GameManager.LightOn();
            GoToNextLevel();
        }

        public async void GoToNextLevel()
        {
            await Task.Delay(3000);
            GameManager.LightOff();
            GameManager.LoadNextLevel();
        }
    }
}