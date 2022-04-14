
using UnityEngine;
using UnityEngine.UI;
using Game;

namespace UI
{
    public class VolumeController : MonoBehaviour
    {
        private AudioController _audioController;

        public Slider slider;

        public GameObject muteIcon;
        public GameObject zeroVolumeIcon;
        public GameObject lowVolumeIcon;
        public GameObject HighVolumeIcon;

        bool _muted = false;

        private void Start()
        {
            UpdateVolumeIcon();
        }

        public void SetController(AudioController controller)
        {
            _audioController = controller;
        }

        public void SetMuted(bool muted)
        {
            if (muted != _muted)
            {
                _muted = muted;
                UpdateVolumeIcon();
            }
        }

        public void SetVolume(float volume)
        {
            if (!_sliderValueChanging)
            {
                slider.value = volume;
                UpdateVolumeIcon();
            }
        }

        bool _sliderValueChanging = false;

        public void OnSliderValueChanged()
        {
            _sliderValueChanging = true;
            if (_audioController) _audioController.Volume = slider.value;
            _sliderValueChanging = false;

            UpdateVolumeIcon();
        }

        public void OnMutePressed()
        {
            if (_audioController) _audioController.Mute = _muted;
        }

        void UpdateVolumeIcon()
        {
            if (_muted)
            {
                muteIcon.SetActive(true);
                zeroVolumeIcon.SetActive(false);
                lowVolumeIcon.SetActive(false);
                HighVolumeIcon.SetActive(false);
            }
            else if (slider.value > 0.5f)
            {
                muteIcon.SetActive(false);
                zeroVolumeIcon.SetActive(false);
                lowVolumeIcon.SetActive(false);
                HighVolumeIcon.SetActive(true);
            }
            else if (slider.value > 0f)
            {
                muteIcon.SetActive(false);
                zeroVolumeIcon.SetActive(false);
                lowVolumeIcon.SetActive(true);
                HighVolumeIcon.SetActive(false);
            }
            else
            {
                muteIcon.SetActive(false);
                zeroVolumeIcon.SetActive(true);
                lowVolumeIcon.SetActive(false);
                HighVolumeIcon.SetActive(false);
            }
        }
    }
}
