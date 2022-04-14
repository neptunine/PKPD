using UnityEngine;

namespace Game
{
    public class AudioController : MonoBehaviour
    {
        private AudioSource
            audioSource;

        public AudioClip[]
            wordPass,
            wordFail,
            levelStart,
            levelVictory;

        private void Start()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        public void PlayWordPass()
        {
            audioSource.PlayOneShot(wordPass[Random.Range(0, wordPass.Length)]);
        }

        public void PlayWordFail()
        {
            audioSource.PlayOneShot(wordFail[Random.Range(0, wordFail.Length)]);
        }

        public void PlayLevelStart()
        {
            audioSource.PlayOneShot(levelStart[Random.Range(0, levelStart.Length)]);
        }

        public void PlayVictory()
        {
            audioSource.PlayOneShot(levelVictory[Random.Range(0, levelVictory.Length)]);
        }
    }
}
