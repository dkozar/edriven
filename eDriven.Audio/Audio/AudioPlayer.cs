#region License

/*
 
Copyright (c) 2010-2014 Danko Kozar

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 
*/

#endregion License

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace eDriven.Audio
{
    /// <summary>
    /// Plays an audio token attached to this object
    /// The poliphony depends of number of audio sources attached
    /// </summary>
    [AddComponentMenu("eDriven/Audio/AudioPlayer")]

    //[RequireComponent(typeof(AudioSource))]
    //[RequireComponent(typeof(AudioToken))]

    public class AudioPlayer : MonoBehaviour
    {
        /// <summary>
        /// True to play at current camera
        /// </summary>
        public bool PlayAtMainCamera;

        /// <summary>
        /// True for sound enabled
        /// </summary>
        public bool SoundEnabled = true;

        /// <summary>
        /// Volume
        /// </summary>
        public float Volume = 0.5f;

        /// <summary>
        /// Pitch
        /// </summary>
        public float Pitch = 1f;

        /// <summary>
        /// Randomness
        /// </summary>
        public float PitchRandomness;

//// ReSharper disable InconsistentNaming
//        void OnGUI()
//// ReSharper restore InconsistentNaming
//        {
//            if (GUI.changed)
//                Volume = Mathf.Clamp(Volume, 0f, 1f);
//        }

// ReSharper disable UnusedMember.Local
        void Start()
// ReSharper restore UnusedMember.Local
        {
            AudioSource source = (AudioSource) FindObjectOfType(typeof(AudioSource));
            if (null == source)
            {
                throw new Exception("No audio source found for AudioPlayerMapper at: " + gameObject.transform);
            }
        }

        #region Public methods

        /// <summary>
        /// Searches the token with the specified ID attached to the same object as this script
        /// Plays it with an audio source attached to the same object as this script
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="options"></param>
        public void PlaySound(string tokenId, params AudioOption[] options)
        {
            if (!SoundEnabled)
                return;

            if (PlayAtMainCamera)
                ChangePosition();

            AudioToken token = PickToken(tokenId);

            if (null == token)
            {
                Debug.LogWarning(string.Format(AudioPlayerException.TokenNotFound, tokenId));
                return;
            }

            if (null == token.AudioClip)
            {
                Debug.LogWarning(string.Format(AudioPlayerException.TokenAudioClipNotFound, tokenId));
                return;
            }

            token.ApplyOptions(options);

            //Debug.Log("Token found: " + token);

            AudioSource audioSource = PickSource();
            audioSource.clip = token.AudioClip;
            audioSource.volume = token.Volume * Volume;
            audioSource.pitch = Random.Range(token.Pitch * Pitch - token.PitchRandomness - PitchRandomness, token.Pitch + token.PitchRandomness + PitchRandomness);
            audioSource.loop = token.Loop;
            //audioSource.minVolume = token.MinDistance;
            //audioSource.maxVolume = token.MaxDistance;
            //audioSource.rolloffFactor = token.RolloffMode;
            audioSource.minDistance = token.MinDistance;
            audioSource.maxDistance = token.MaxDistance;
            audioSource.rolloffMode = token.RolloffMode;
            audioSource.Play();
        }

        private void ChangePosition()
        {
            //Debug.Log("Camera.main: " + Camera.main);

            var cam = Camera.main.transform;
            //Debug.Log("transform.position: " + transform.position);

            var cameraRelativePos = cam.InverseTransformPoint(transform.position);
            //Debug.Log("cameraRelative: " + cameraRelative);
            
            // change position on this transform
            var p = transform.position;
            transform.position = new Vector3(p.x - cameraRelativePos.x, p.y - cameraRelativePos.y, p.z - cameraRelativePos.z);
        }

        /// <summary>
        /// Stops all sounds
        /// </summary>
        public void StopAllSounds()
        {
            TraverseAllSources(GetComponents(typeof(AudioSource)), StopHandler);
        }

        ///// <summary>
        ///// Pauses all sounds
        ///// </summary>
        //public void PauseAllSounds()
        //{
        //    TraverseAllSources(GetComponents(typeof(AudioSource)), PauseHandler);
        //}

        ///// <summary>
        ///// Plays all sounds
        ///// </summary>
        //public void PlayAllSounds()
        //{
        //    TraverseAllSources(GetComponents(typeof(AudioSource)), PlayHandler);
        //}

        #endregion

        #region Private methods

        /// <summary>
        /// Looks for a token
        /// </summary>
        /// <param name="id">Token ID</param>
        /// <returns>Audio token</returns>
        private AudioToken PickToken(string id)
        {
            Component[] tokens = GetComponents(typeof(AudioToken));

            if (tokens.Length == 0)
            {
                //throw new AudioPlayerException(AudioPlayerException.NoTokensFound);
                Debug.LogWarning(AudioPlayerException.NoTokensFound);
                return null;
            }

            AudioToken token = null;

            foreach (Component t in tokens)
            {
                AudioToken tok = (AudioToken)t;

                if (tok.Id == id)
                {
                    token = tok;
                    break;
                }
            }

            return token;
        }

        /// <summary>
        /// Gets a free (non playing) audio source
        /// If no free sources, gets the first one
        /// </summary>
        /// <returns>Audio source</returns>
        private AudioSource PickSource()
        {
            Component[] audioSources = GetComponents(typeof(AudioSource));

            if (audioSources.Length == 0)
                throw new AudioPlayerException(AudioPlayerException.NoAudioSourcesFound);

            //Debug.Log("audioSources count: " + audioSources.Length);

            AudioSource src = null;

            foreach (Component source in audioSources)
            {
                AudioSource audioSource  = (AudioSource) source;

                if (!audioSource.isPlaying)
                {
                    src = audioSource;
                    break;
                }
            }

            // else get first one
            if (null == src)
                src = (AudioSource)audioSources[0];

            return src;
        }

        private delegate void AudioActionHandler(AudioSource source);

// ReSharper disable UnusedMember.Local
        private static void PlayHandler(AudioSource source)
// ReSharper restore UnusedMember.Local
        {
            source.Play();
        }

// ReSharper disable UnusedMember.Local
        private static void PauseHandler(AudioSource source)
// ReSharper restore UnusedMember.Local
        {
            source.Pause();
        }

        private static void StopHandler(AudioSource source)
        {
            source.Stop();
        }

        private static void TraverseAllSources(IEnumerable<Component> sources, AudioActionHandler handler)
        {
            if (null == sources)
                throw new AudioPlayerException(AudioPlayerException.NoAudioSourcesFound);

            foreach (Component source in sources)
            {
                handler((AudioSource)source);
            }
        }

        #endregion

    }

    /// <summary>
    /// The exception that can be thrown by AudioPlayer
    /// </summary>
    public class AudioPlayerException : Exception
    {
        public static string NoAudioSourcesFound = "No AudioSource found";
        public static string NoTokensFound = "No AudioToken found";
        public static string TokenNotFound = "AudioToken with id [{0}] not found";
        public static string TokenAudioClipNotFound = "AudioToken with id [{0}] has no Audio Clip attached";
        
// ReSharper disable UnusedMember.Global
        public AudioPlayerException()
// ReSharper restore UnusedMember.Global
        {

        }

        /// <summary>
        /// Constructor
        ///</summary>
        ///<param name="message"></param>
        public AudioPlayerException(string message)
            : base(message)
        {

        }
    }
}