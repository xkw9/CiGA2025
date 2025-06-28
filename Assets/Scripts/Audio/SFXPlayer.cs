using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Audio
{
    public class SFXPlayer
    {
        private AudioSource[] _channels;
        private int _currentChannel = 0;

        public SFXPlayer(AudioSource[] channels) { 
        
            _channels = channels;
        }

        public void Play(AudioClip clip)
        {
            _channels[_currentChannel].Stop();
            _channels[_currentChannel].volume = 1;
            _channels[_currentChannel].clip = clip;
            _channels[_currentChannel].Play();
            _currentChannel = (_currentChannel + 1) % _channels.Length;
        }

        public void Stop()
        {
            foreach (var channel in _channels)
            {
                channel.Stop();
            }
        }

        public bool isPlaying() => _channels[_currentChannel].isPlaying;
    }
}
