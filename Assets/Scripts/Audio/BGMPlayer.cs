using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Audio
{
    public class BGMPlayer
    {

        private AudioSource curChannel;
        private AudioSource altChannel;

        private AudioClip curClip = null;

        /// <summary>
        /// current volume, not considering the global volume setting.
        /// </summary>
        private float volume = 1f;

        public BGMPlayer(AudioSource channel1, AudioSource channel2)
        {
            this.curChannel = channel1;
            this.curChannel.loop = true;
            this.curChannel.playOnAwake = false;
            this.altChannel = channel2;
            this.altChannel.loop = true;
            this.altChannel.playOnAwake = false;
        }

        /// <summary>
        /// dynamically switch the BGM with a transition time.
        /// If the BGM is transitioning, the call will be queued.
        /// </summary>
        public void Play(AudioClip nextClip, double trasitionTime = 1d, float volume = 1f)
        {            

            if (curClip == nextClip && volume == curChannel.volume)
            {
                // nothing to do
                return;
            }

            curClip = nextClip;
            curChannel.clip = nextClip;
            curChannel.volume = volume;
            curChannel.Play();

        }

        /// <summary>
        /// set the volume of the current channel immediately.
        /// If the channel is transitioning, it will be queued.
        /// </summary>
        /// <param name="volume"></param>

        public void Stop(double transitionTime)
        {
            Play(null, transitionTime, 0);
        }

        /// <summary>
        /// Update the volume of the current channel. Should only be called when settings changed.
        /// </summary>
        /// <param name="transitionTime"></param>
        public void UpdateVolume()
        {
            ChangeVolume(0.5, volume);
        }

        public void ChangeVolume(double transitionTime, float volume)
        {
            Play(curClip, transitionTime, volume);

        }

    }
}
