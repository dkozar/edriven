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

using UnityEngine;

namespace eDriven.Audio
{
    /// <summary>
    /// Maps string to audio resource
    /// Holds configuration values for playing the audio clip
    /// </summary>
    [AddComponentMenu("eDriven/Audio/AudioToken")]

    public class AudioToken : MonoBehaviour
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Id;

        /// <summary>
        /// Audio clip
        /// </summary>
        public AudioClip AudioClip;

        /// <summary>
        /// Volume
        /// </summary>
        public float Volume = 1.0f;

        /// <summary>
        /// Looping
        /// </summary>
        public bool Loop;

        /// <summary>
        /// Pitch
        /// </summary>
        public float Pitch = 1f;

        /// <summary>
        /// Pitch randomness
        /// </summary>
        public float PitchRandomness;

        /// <summary>
        /// Min distance
        /// </summary>
        public float MinDistance;

        /// <summary>
        /// Max distance
        /// </summary>
        public float MaxDistance = 1f;

        /// <summary>
        /// Rolloff mode
        /// </summary>
        public AudioRolloffMode RolloffMode = AudioRolloffMode.Linear;
        
        public override string ToString()
        {
            return string.Format(@"AudioToken [Id:{0}]
=====================
AudioClip:{1}; 
Volume:{2}; 
Loop:{3};
Pitch:{4};
PitchRandomness:{5};
MinDistance:{6};
MaxDistance:{7};
RolloffMode:{8};
=====================
", Id, AudioClip, Volume, Loop, Pitch, PitchRandomness, MinDistance, MaxDistance, RolloffMode);
        }

        /// <summary>
        /// Applies options
        /// </summary>
        /// <param name="options"></param>
        public void ApplyOptions(AudioOption[] options)
        {
            if (options != null)
            {
                int len = options.Length;
                for (int i = 0; i < len; i++)
                {
                    AudioOption option = options[i];
                    switch (option.Type)
                    {
                        case AudioOptionType.AudioClip:
                            AudioClip = (AudioClip)option.Value;
                            break;

                        case AudioOptionType.Loop:
                            Loop = (bool)option.Value;
                            break;

                        case AudioOptionType.MaxVolume:
                            MaxDistance = (float)option.Value;
                            break;

                        case AudioOptionType.MinVolume:
                            MinDistance = (float)option.Value;
                            break;

                        case AudioOptionType.Pitch:
                            Pitch = (float)option.Value;
                            break;

                        case AudioOptionType.PitchRandomness:
                            PitchRandomness = (float)option.Value;
                            break;

                        case AudioOptionType.RolloffFactor:
                            RolloffMode = (AudioRolloffMode) option.Value;
                            break;

                        case AudioOptionType.Volume:
                            Volume = (float)option.Value;
                            break;
                    }
                }
            }
        }
    }
}