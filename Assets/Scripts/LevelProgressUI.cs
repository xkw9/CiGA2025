using Assets.Scripts.API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelProgressUI : MonoBehaviour, ILevelProgressUI
    {
        TextMeshProUGUI countdownText;
        TextMeshProUGUI objPlacedText;

        int level = 0;

        int amountNeed = 0;

        int curAmount = 0;

        bool isCounting = false;

        float holdingTime = 0f;

        Coroutine WinChecker;

        void Awake()
        {
            countdownText = GameObject.Find("countdownText").GetComponent<TextMeshProUGUI>();
            objPlacedText = GameObject.Find("objPlacedText").GetComponent<TextMeshProUGUI>();
            
            //Debug.Log("test countdownText: " + countdownText);
            //Debug.Log("test objPlacedText: " + objPlacedText);

            countdownText.gameObject.SetActive(false);
            objPlacedText.gameObject.SetActive(false);
        }

        public void LoadLevelInfo(int level, int amountNeed)
        {
            Debug.Log("test LoadLevelInfo: " + "level: "+ level + "amountNeed: " + amountNeed);
            this.level = level;
            this.amountNeed = amountNeed;

            objPlacedText.gameObject.SetActive(true);
            RefreshCurAmount(0);
        }

        public void RefreshCurAmount(int amount)
        {
            curAmount = amount;
            objPlacedText.text = $"{amount}/{this.amountNeed}";

            if (curAmount < amountNeed && isCounting)
            {
                isCounting = false;
                countdownText.gameObject.SetActive(false);
                if (WinChecker != null)
                {
                    StopCoroutine(WinChecker);
                    GameManager.AudioManager.Mute();
                }
            }

            holdingTime = 0;
        }

        public void startCountDown(int seconds)
        {
            if (! isCounting)
            {
                isCounting = true;
                countdownText.gameObject.SetActive(true);
                WinChecker = StartCoroutine(WinCheckerCoroutine());
                GameManager.AudioManager.PlaySFX("tick_tock");
            }
        }

        private IEnumerator WinCheckerCoroutine()
        { 
            while (holdingTime < Config.WIN_DURARION)
            {
                holdingTime += Time.deltaTime;
                countdownText.text = $"{Config.WIN_DURARION - holdingTime:F1}";

                yield return null;
            }

            // Win condition met, trigger win logic
            holdingTime = 0;
            isCounting = false;
            countdownText.gameObject.SetActive(false);
            WinChecker = null;
            GameManager.Win();
        }

    }
}
