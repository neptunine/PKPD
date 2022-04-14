using UnityEngine;

namespace Game
{
    public class AudioController : MonoBehaviour
    {
        private AudioSource
            _audioSource;

        private float _effectVolume;
        public float effectVolume
        {
            get { return _effectVolume; }
            set
            {
                // https://www.dr-lex.be/info-stuff/volumecontrols.html#ideal 50dB dynamic range constants
                _effectVolume = Mathf.Clamp01(3.1623e-3f * Mathf.Exp(Mathf.Clamp01(effectVolume) * 5.757f) - 3.1623e-3f);
            }
        }

        private float _voiceVolume;
        public float voiceVolume
        {
            get { return _voiceVolume; }
            set
            {
                // https://www.dr-lex.be/info-stuff/volumecontrols.html#ideal 50dB dynamic range constants
                _voiceVolume = Mathf.Clamp01(3.1623e-3f * Mathf.Exp(Mathf.Clamp01(voiceVolume) * 5.757f) - 3.1623e-3f);
            }
        }

        public AudioClip[]
            wordPass,
            wordFail,
            levelStart,
            levelVictory,
            levelFail,
            levelVictoryClip,
            levelFailClip;

        private void Start()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        public void PlayWordPass()
        {
            _audioSource.PlayOneShot(wordPass[Random.Range(0, wordPass.Length)]);
        }

        public void PlayWordFail()
        {
            _audioSource.PlayOneShot(wordFail[Random.Range(0, wordFail.Length)]);
        }

        public void PlayLevelStart()
        {
            _audioSource.PlayOneShot(levelStart[Random.Range(0, levelStart.Length)]);
        }

        public void PlayVictory()
        {
            _audioSource.PlayOneShot(levelVictory[Random.Range(0, levelVictory.Length)]);
        }

        public void PlayVictoryClip()
        {
            _audioSource.PlayOneShot(levelVictoryClip[Random.Range(0, levelVictoryClip.Length)]);
        }

        public void PlayFailed()
        {
            _audioSource.PlayOneShot(levelFail[Random.Range(0, levelFail.Length)]);
        }

        public void PlayFailedClip()
        {
            _audioSource.PlayOneShot(levelFailClip[Random.Range(0, levelFailClip.Length)]);
        }
    }
}
