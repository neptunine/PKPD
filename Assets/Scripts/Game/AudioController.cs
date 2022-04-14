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
                _volume = Mathf.Clamp01(value);
                _audioSource.volume = Mathf.Clamp01(3.1623e-3f * Mathf.Exp(_volume * 5.757f) - 3.1623e-3f);
                // https://www.dr-lex.be/info-stuff/volumecontrols.html#ideal 50dB dynamic range constants
            }
        }

        public bool Mute
        {
            get { return _audioSource.mute; }
            set { _audioSource.mute = value; }
        }

        public AudioClip[]
            start,
            characterCorrect,
            characterIncorrect,
            wordCorrect,
            wordFail,
            nextWord,
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

            _audioSource.PlayOneShot(start[Random.Range(0, start.Length)]);
        }

        public void PlayCharacterCorrect()
        {
            _audioSource.PlayOneShot(characterCorrect[Random.Range(0, characterCorrect.Length)]);
        }

        public void PlayCharacterIncorrect()
        {
            _audioSource.PlayOneShot(characterIncorrect[Random.Range(0, characterIncorrect.Length)]);
        }

        public void PlayWordCorrect()
        {
            _audioSource.PlayOneShot(wordCorrect[Random.Range(0, wordCorrect.Length)]);
        }

        public void PlayWordFail()
        {
            _audioSource.PlayOneShot(wordFail[Random.Range(0, wordFail.Length)]);
        }

        public void PlayNextWord()
        {
            _audioSource.PlayOneShot(nextWord[Random.Range(0, nextWord.Length)]);
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
