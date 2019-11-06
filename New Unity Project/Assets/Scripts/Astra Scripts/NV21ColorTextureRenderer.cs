using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    class NV21ColorTextureRenderer : MonoBehaviour
    {
        private Material mShareMaterial;
        private Texture2D _YImageTex;
        private Texture2D _UVImageTex;
        private const TextureFormat Format = TextureFormat.YUY2;

        private long _lastFrameIndex = -1;

        void Start()
        {
            int width = 320, height = 240;
            _YImageTex = new Texture2D(width, height, TextureFormat.Alpha8, false);
            _UVImageTex = new Texture2D(width / 2, height / 2, TextureFormat.RGBA4444, false);
        }

        public void OnNewFrame(Astra.ColorFrame frame)
        {
            if (frame.Width == 0 ||
                frame.Height == 0)
            {
                return;
            }

            if (_lastFrameIndex == frame.FrameIndex)
            {
                return;
            }

            _lastFrameIndex = frame.FrameIndex;

            EnsureTexture(frame.Width, frame.Height);
            _YImageTex.LoadRawTextureData(frame.DataPtr, (int)frame.Width * frame.Height);
            _YImageTex.Apply();
            var ptr = new IntPtr(frame.DataPtr.ToInt64() + frame.Width * frame.Height);
            _UVImageTex.LoadRawTextureData(ptr, (int)frame.Width * frame.Height / 2);
            _UVImageTex.Apply();
            GetComponent<Renderer>().material.SetTexture("_MainTex", _YImageTex);
            GetComponent<Renderer>().material.SetTexture("_UVTex", _UVImageTex);
        }

        private void EnsureTexture(int width, int height)
        {
            if (_YImageTex == null)
            {
                _YImageTex = new Texture2D(width, height, TextureFormat.Alpha8, false);
                _UVImageTex = new Texture2D(width / 2, height / 2, TextureFormat.RGBA4444, false);
                return;
            }

            if (_YImageTex.width != width ||
                _YImageTex.height != height)
            {
                _YImageTex = new Texture2D(width, height, TextureFormat.Alpha8, false);
                _UVImageTex = new Texture2D(width / 2, height / 2, TextureFormat.RGBA4444, false);
            }
        }
    }
}
