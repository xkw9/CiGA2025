using Assets.Audio;
using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts
{
    public static class GameManager
    {

        public static Player player;

        public static AudioManager AudioManager;

        public static Light2D globalLight;

        
        public static void Init()
        {

            Debug.Log("loading new game!");

            player = GameObject.FindObjectOfType<Player>();

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
