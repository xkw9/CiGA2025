using Assets.Scripts;
using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Audio
{
    public class AudioManager : MonoBehaviour
    {

        private SFXPlayer SFXPlayer;
        private SFXPlayer SystemSFXPlayer;
        private SFXPlayer TextBeepPlayer;
        private BGMPlayer BGMPlayer;

        private void Awake()
        {
            InitComponent();   
        }

        private void InitComponent()
        {

            SFXPlayer = new SFXPlayer(new AudioSource[] { 
                UnityUtils.MakeObject<AudioSource>("SFXChannel1", transform),
                UnityUtils.MakeObject<AudioSource>("SFXChannel1", transform),
                UnityUtils.MakeObject<AudioSource>("SFXChannel1", transform)
            });

            SystemSFXPlayer = new SFXPlayer(new AudioSource[] { 
                UnityUtils.MakeObject<AudioSource>("SystemSFXChannel", transform) 
            });

            BGMPlayer = new BGMPlayer(
                UnityUtils.MakeObject<AudioSource>("BGMChannel1", transform),
                UnityUtils.MakeObject<AudioSource>("BGMChannel1", transform)
            );

            TextBeepPlayer = new SFXPlayer(new AudioSource[]
            {
                UnityUtils.MakeObject<AudioSource>("TextBeepChannel", transform)
            });

        }

        public void PlayBGM(string clip_name, double transitionTime = 1f, float volume = 1f)
        {
            AudioClip clip = LoadClip(clip_name);

            if (clip == null)
            {
                Debug.LogWarning("BGM Clip not found: " + clip_name);
                return;
            }

            BGMPlayer.Play(clip, transitionTime, volume);
        }

        public void Mute(double transitionTime = 1)
        {
            SFXPlayer.Stop();
            SystemSFXPlayer.Stop();
            TextBeepPlayer.Stop();
            StopBGM(transitionTime);
        }

        public void StopSFX()
        {
            SFXPlayer.Stop();
        }

        public void PlaySFX(string clip_name)
        {
            AudioClip clip = LoadClip(clip_name);

            if (clip == null)
            {
                Debug.LogWarning("SFX Clip not found: " + clip_name);
                return;
            }

            SFXPlayer.Play(clip);

        }

        public void PlaySystemSFX(string clip_name)
        {
            AudioClip clip = LoadClip(clip_name);

            if (clip == null)
            {
                Debug.LogWarning("SystemSFX Clip not found: " + clip_name);
                return;
            }

            SystemSFXPlayer.Play(clip);
        }

        public void StopBGM(double transitionTime)
        {
            BGMPlayer.Stop(transitionTime);
        }

        /// <summary>
        /// update volume when settings changed.
        /// </summary>
        public void UpdateVolume()
        {
            BGMPlayer.UpdateVolume();
        }

        public void ChangeBGMVolume(double volume, double transitionTime)
        {
            BGMPlayer.ChangeVolume(transitionTime, (float)volume);
        }


        public AudioClip LoadClip(string name)
        {
            AudioClip clip = Resources.Load(Config.AUDIO_PATH_HEADER + name) as AudioClip;
            if (clip == null)
            {
                Debug.LogWarning("Audio clip not found: " + name);
                return null;
            } else
            {
                return clip;
            }
        }
    }
}
