
using UnityEngine;
using UnityEngine.UI;
using Game;

namespace UI
{
    public class VolumeController : MonoBehaviour
    {
        private AudioController _audioController;

        public Slider slider;

        public Image Icon;

        public Sprite muteIcon;
        public Sprite zeroVolumeIcon;
        public Sprite lowVolumeIcon;
        public Sprite HighVolumeIcon;

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
                if (_audioController) _audioController.Mute = _muted;
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
            SetMuted(false);

            _sliderValueChanging = true;
            if (_audioController) _audioController.Volume = slider.value;
            _sliderValueChanging = false;

            UpdateVolumeIcon();
        }

        public void OnMutePressed()
        {
            _muted = !_muted;
            if (_audioController) _audioController.Mute = _muted;

            UpdateVolumeIcon();
        }

        void UpdateVolumeIcon()
        {
            if (_muted)
            {
                Icon.sprite = muteIcon;
            }
            else if (slider.value > 0.5f)
            {
                Icon.sprite = HighVolumeIcon;
            }
            else if (slider.value > 0f)
            {
                Icon.sprite = lowVolumeIcon;
            }
            else
            {
                Icon.sprite = zeroVolumeIcon;
            }
        }
    }
}
