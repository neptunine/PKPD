using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasPP : MonoBehaviour
    {
        private PixelPerfectCamera
            _pixelPerfectCamera;

        private CanvasScaler
            _canvasScaler;

        private void Start()
        {
            _pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();
            _canvasScaler = GetComponent<CanvasScaler>();
        }
        private void LateUpdate()
        {
            if (_pixelPerfectCamera && _pixelPerfectCamera.enabled)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    _canvasScaler.scaleFactor = _pixelPerfectCamera.pixelRatio;
                else
                    if (_pixelPerfectCamera.runInEditMode)
                        _canvasScaler.scaleFactor = _pixelPerfectCamera.pixelRatio;
#else
                _canvasScaler.scaleFactor = _pixelPerfectCamera.pixelRatio;
#endif
            }
        }
    }
}
