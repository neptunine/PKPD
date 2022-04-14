using UnityEngine;
using UI;

namespace Game
{
    public class AudioController : MonoBehaviour
    {
        private GameController
            _controller;

        [SerializeField]
        private VolumeController
            volumeController;

        private AudioSource
            _audioSource;

        private float _volume;
        public float Volume
        {
            get { return _volume; }
            set
            {
                _volume = Mathf.Clamp01(Volume);
                _audioSource.volume = Mathf.Clamp01(3.1623e-3f * Mathf.Exp(_volume * 5.757f) - 3.1623e-3f);
                // https://www.dr-lex.be/info-stuff/volumecontrols.html#ideal 50dB dynamic range constants
            }
        }

        public bool Mute
        {
            get { return _audioSource.mute; }
            set { _audioSource.mute = Mute; }
        }

        public AudioClip[]
            wordPass,
            wordFail,
            levelStart,
            levelVictory,
            levelFail;

        public void SetController(GameController controller)
        {
            _controller = controller;
        }

        private void Start()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            volumeController.SetController(this);
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

        public void PlayFailed()
        {
            _audioSource.PlayOneShot(levelFail[Random.Range(0, levelFail.Length)]);
        }
    }
}
