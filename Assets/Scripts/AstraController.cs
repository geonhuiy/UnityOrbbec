#if UNITY_ANDROID && !UNITY_EDITOR
#define ASTRA_UNITY_ANDROID_NATIVE
#endif

using Astra;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class NewDepthFrameEvent : UnityEvent<DepthFrame> { }

[System.Serializable]
public class NewColorFrameEvent : UnityEvent<ColorFrame> { }

[System.Serializable]
public class NewNV21ColorFrameEvent : UnityEvent<ColorFrame> { }

[System.Serializable]
public class NewBodyFrameEvent : UnityEvent<BodyStream, BodyFrame> { }

[System.Serializable]
public class NewMaskedColorFrameEvent : UnityEvent<MaskedColorFrame> { }

[System.Serializable]
public class NewColorizedBodyFrameEvent : UnityEvent<ColorizedBodyFrame> { }

[System.Serializable]
public class NewBodyMaskEvent : UnityEvent<BodyMask> { }

public class AstraController : MonoBehaviour
{
    public bool AutoRequestAndroidUsbPermission = true;

    private Astra.StreamSet _streamSet;
    private Astra.StreamReader _readerDepth;
    private Astra.StreamReader _readerColor;
    private Astra.StreamReader _readerNV21Color;
    private Astra.StreamReader _readerBody;
    private Astra.StreamReader _readerMaskedColor;
    private Astra.StreamReader _readerColorizedBody;

    private DepthStream _depthStream;
    private ColorStream _colorStream;
    private ColorStream _nv21ColorStream;
    private BodyStream _bodyStream;
    private MaskedColorStream _maskedColorStream;
    private ColorizedBodyStream _colorizedBodyStream;

    bool _isDepthOn = false;
    bool _isColorOn = false;
    bool _isNV21ColorOn = false;
    bool _isBodyOn = false;
    bool _isMaskedColorOn = false;
    bool _isColorizedBodyOn = false;

    private long _lastBodyFrameIndex = -1;
    private long _lastDepthFrameIndex = -1;
    private long _lastColorFrameIndex = -1;
    private long _lastNV21ColorFrameIndex = -1;
    private long _lastMaskedColorFrameIndex = -1;
    private long _lastColorizedBodyFrameIndex = -1;

    private int _lastWidth = 0;
    private int _lastHeight = 0;
    private short[] _buffer;
    private int _frameCount = 0;
    private bool _areStreamsInitialized = false;

    private TimerHistory updateFramesTime = new TimerHistory();
    private TimerHistory astraUpdateTime = new TimerHistory();
    private TimerHistory totalFrameTime = new TimerHistory();

    public Text TimeText = null;
    public Toggle ToggleDebugText = null;
    private bool debugTextEnabled = false;

    public NewDepthFrameEvent NewDepthFrameEvent = new NewDepthFrameEvent();
    public NewColorFrameEvent NewColorFrameEvent = new NewColorFrameEvent();
    public NewNV21ColorFrameEvent NewNV21ColorFrameEvent = new NewNV21ColorFrameEvent();
    public NewBodyFrameEvent NewBodyFrameEvent = new NewBodyFrameEvent();
    public NewMaskedColorFrameEvent NewMaskedColorFrameEvent = new NewMaskedColorFrameEvent();
    public NewColorizedBodyFrameEvent NewColorizedBodyFrameEvent = new NewColorizedBodyFrameEvent();
    public NewBodyMaskEvent NewBodyMaskEvent = new NewBodyMaskEvent();

    public Toggle ToggleDepth = null;
    public Toggle ToggleColor = null;
    public Toggle ToggleNV21Color = null;
    public Toggle ToggleBody = null;
    public Toggle ToggleMaskedColor = null;
    public Toggle ToggleColorizedBody = null;

    private void Awake()
    {
        Debug.Log("AstraUnityContext.Awake");
        AstraUnityContext.Instance.Initializing += OnAstraInitializing;
        AstraUnityContext.Instance.Terminating += OnAstraTerminating;
        AstraUnityContext.Instance.Initialize();
    }

    void Start()
    {
        if (TimeText != null)
        {
            TimeText.text = "";
        }

        if (ToggleDebugText != null)
        {
            debugTextEnabled = ToggleDebugText.isOn;
        }
    }

    private void OnAstraInitializing(object sender, AstraInitializingEventArgs e)
    {
        Debug.Log("AstraController is initializing");

#if ASTRA_UNITY_ANDROID_NATIVE
        if (AutoRequestAndroidUsbPermission)
        {
            Debug.Log("Auto-requesting usb device access.");
            AstraUnityContext.Instance.RequestUsbDeviceAccessFromAndroid(this);
        }
#else
        try
        {
            GameObject.Find("NV21ColorMapViewer").SetActive(false);
            GameObject.Find("NV21ColorMapViewerBG").SetActive(false);
            GameObject.Find("ToggleNV21Color").SetActive(false);
        }
        catch (System.Exception)
        {
        }
        InitializeStreams();
#endif
    }

    public void InitializeStreams()
    {
        try
        {
            AstraUnityContext.Instance.WaitForUpdate(AstraBackgroundUpdater.WaitIndefinitely);

            _streamSet = Astra.StreamSet.Open();

            _readerDepth = _streamSet.CreateReader();
            _readerColor = _streamSet.CreateReader();
            _readerNV21Color = _streamSet.CreateReader();
            _readerBody = _streamSet.CreateReader();
            _readerMaskedColor = _streamSet.CreateReader();
            _readerColorizedBody = _streamSet.CreateReader();

            _depthStream = _readerDepth.GetStream<DepthStream>();
            
            var depthModes = _depthStream.AvailableModes;
            ImageMode selectedDepthMode = depthModes[0];

            int targetDepthWidth, targetDepthHeight, targetDepthFps;
#if ASTRA_UNITY_ANDROID_NATIVE
            //Deeyea and Dabai doesn't support qqvga and qvga resolution.
            //use 640*400.
            if (_depthStream.usbInfo.Pid == 0x60b ||
                _depthStream.usbInfo.Pid == 0x60e ||
                _depthStream.usbInfo.Pid == 0x608 ||
                _depthStream.usbInfo.Pid == 0x617)
            {
                targetDepthWidth = 640;
                targetDepthHeight = 400;
                targetDepthFps = 30;
            }
            else
            {
                targetDepthWidth = 160;
                targetDepthHeight = 120;
                targetDepthFps = 30;
            }
#else
            if (_depthStream.usbInfo.Pid == 0x60b ||
                _depthStream.usbInfo.Pid == 0x60e ||
                _depthStream.usbInfo.Pid == 0x608 ||
                _depthStream.usbInfo.Pid == 0x617)
            {
                targetDepthWidth = 640;
                targetDepthHeight = 400;
                targetDepthFps = 30;
            }
            else
            {
                targetDepthWidth = 320;
                targetDepthHeight = 240;
                targetDepthFps = 30;
            }
    #endif

            foreach (var m in depthModes)
            {
                if (m.Width == targetDepthWidth &&
                    m.Height == targetDepthHeight &&
                    m.FramesPerSecond == targetDepthFps)
                {
                    selectedDepthMode = m;
                    break;
                }
            }

            _depthStream.SetMode(selectedDepthMode);

            _colorStream = _readerColor.GetStream<ColorStream>();

            var colorModes = _colorStream.AvailableModes;
            ImageMode selectedColorMode = colorModes[0];

            if (_depthStream.usbInfo.Pid == 0x60b ||
                _depthStream.usbInfo.Pid == 0x617)
            {
                //for deeyea, set mirror to false to match depth.
                _colorStream.IsMirroring = false;
            }
#if ASTRA_UNITY_ANDROID_NATIVE
            int targetColorWidth, targetColorHeight, targetColorFps;
            if (_depthStream.usbInfo.Pid == 0x608 ||
                _depthStream.usbInfo.Pid == 0x60f ||
                _depthStream.usbInfo.Pid == 0x617)
            {
                targetColorWidth = 640;
                targetColorHeight = 480;
                targetColorFps = 30;
            }
            else
            {
                targetColorWidth = 320;
                targetColorHeight = 240;
                targetColorFps = 30;
            }
#else
            int targetColorWidth = 640;
            int targetColorHeight = 480;
            int targetColorFps = 30;
#endif

            foreach (var m in colorModes)
            {
                if (m.Width == targetColorWidth &&
                    m.Height == targetColorHeight &&
                    m.FramesPerSecond == targetColorFps)
                {
                    selectedColorMode = m;
                    break;
                }
            }

            _colorStream.SetMode(selectedColorMode);

#if ASTRA_UNITY_ANDROID_NATIVE
            _nv21ColorStream = _readerNV21Color.GetStream<ColorStream>(Astra.Core.StreamSubType.COLOR_NV21_SUBTYPE);
            if (_nv21ColorStream.IsAvailable)
            {
                //COLOR_NV21_SUBTYPE is only available when using astra pro and astra pro plus. 
                colorModes = _nv21ColorStream.AvailableModes;
                selectedColorMode = colorModes[0];

                foreach (var m in colorModes)
                {
                    if (m.Width == targetColorWidth &&
                        m.Height == targetColorHeight &&
                        m.FramesPerSecond == targetColorFps)
                    {
                        selectedColorMode = m;
                        break;
                    }
                }

                _nv21ColorStream.SetMode(selectedColorMode);
                if (_depthStream.usbInfo.Pid == 0x60b ||
                    _depthStream.usbInfo.Pid == 0x617)
                {
                    //for deeyea, set mirror to false to match depth.
                    _nv21ColorStream.IsMirroring = false;
                }
            }
            else
            {
                _readerNV21Color.Dispose();
                _readerNV21Color = null;
                _nv21ColorStream = null;
            }
#endif

            _bodyStream = _readerBody.GetStream<BodyStream>();

            _maskedColorStream = _readerMaskedColor.GetStream<MaskedColorStream>();

            _colorizedBodyStream = _readerColorizedBody.GetStream<ColorizedBodyStream>();

            _areStreamsInitialized = true;
        }
        catch (AstraException e)
        {
            Debug.Log("AstraController: Couldn't initialize streams: " + e.ToString());
            UninitializeStreams();
        }
    }

    private void OnAstraTerminating(object sender, AstraTerminatingEventArgs e)
    {
        Debug.Log("AstraController is tearing down");
        UninitializeStreams();
    }

    private void UninitializeStreams()
    {
        AstraUnityContext.Instance.WaitForUpdate(AstraBackgroundUpdater.WaitIndefinitely);

        Debug.Log("AstraController: Uninitializing streams");
        if (_readerDepth != null)
        {
            _readerDepth.Dispose();
            _readerColor.Dispose();
            if (_readerNV21Color != null)
            {
                _readerNV21Color.Dispose();
                _readerNV21Color = null;
            }
            _readerBody.Dispose();
            _readerMaskedColor.Dispose();
            _readerColorizedBody.Dispose();
            _readerDepth = null;
            _readerColor = null;
            _readerBody = null;
            _readerMaskedColor = null;
            _readerColorizedBody = null;
        }

        if (_streamSet != null)
        {
            _streamSet.Dispose();
            _streamSet = null;
        }
    }

    private void CheckDepthReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerDepth.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                DepthFrame depthFrame = frame.GetFrame<DepthFrame>();

                if (depthFrame != null)
                {
                    if(_lastDepthFrameIndex != depthFrame.FrameIndex)
                    {
                        _lastDepthFrameIndex = depthFrame.FrameIndex;

                        NewDepthFrameEvent.Invoke(depthFrame);
                    }
                }
            }
        }
    }

    private void CheckColorReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerColor.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                ColorFrame colorFrame = frame.GetFrame<ColorFrame>();

                if (colorFrame != null)
                {
                    if(_lastColorFrameIndex != colorFrame.FrameIndex)
                    {
                        _lastColorFrameIndex = colorFrame.FrameIndex;

                        NewColorFrameEvent.Invoke(colorFrame);
                    }
                }
            }
        }
    }

    private void CheckNV21ColorReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerNV21Color == null)
            return;
        if (_readerNV21Color.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                ColorFrame colorFrame = frame.GetFrame<ColorFrame>(Astra.Core.StreamSubType.COLOR_NV21_SUBTYPE);

                if (colorFrame != null)
                {
                    if (_lastNV21ColorFrameIndex != colorFrame.FrameIndex)
                    {
                        _lastNV21ColorFrameIndex = colorFrame.FrameIndex;

                        NewNV21ColorFrameEvent.Invoke(colorFrame);
                    }
                }
            }
        }
    }

    private void CheckBodyReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerBody.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                BodyFrame bodyFrame = frame.GetFrame<BodyFrame>();

                if (bodyFrame != null)
                {
                    if(_lastBodyFrameIndex != bodyFrame.FrameIndex)
                    {
                        _lastBodyFrameIndex = bodyFrame.FrameIndex;

                        NewBodyFrameEvent.Invoke(_bodyStream, bodyFrame);
                        NewBodyMaskEvent.Invoke(bodyFrame.BodyMask);
                    }
                }
            }
        }
    }

    private void CheckMaskedColorReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerMaskedColor.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                MaskedColorFrame maskedColorFrame = frame.GetFrame<MaskedColorFrame>();

                if (maskedColorFrame != null)
                {
                    if(_lastMaskedColorFrameIndex != maskedColorFrame.FrameIndex)
                    {
                        _lastMaskedColorFrameIndex = maskedColorFrame.FrameIndex;

                        NewMaskedColorFrameEvent.Invoke(maskedColorFrame);
                    }
                }
            }
        }
    }

    private void CheckColorizedBodyReader()
    {
        // Assumes AstraUnityContext.Instance.IsUpdateAsyncComplete is already true

        ReaderFrame frame;
        if (_readerColorizedBody.TryOpenFrame(0, out frame))
        {
            using (frame)
            {
                ColorizedBodyFrame colorizedBodyFrame = frame.GetFrame<ColorizedBodyFrame>();

                if (colorizedBodyFrame != null)
                {
                    if(_lastColorizedBodyFrameIndex != colorizedBodyFrame.FrameIndex)
                    {
                        _lastColorizedBodyFrameIndex = colorizedBodyFrame.FrameIndex;

                        NewColorizedBodyFrameEvent.Invoke(colorizedBodyFrame);
                    }
                }
            }
        }
    }

    private bool UpdateUntilDelegate()
    {
        return true;
        // Check if any readers have new frames.
        // StreamReader.HasNewFrame() is thread-safe and can be called
        // from any thread.
        bool hasNewFrameDepth = _readerDepth != null && _readerDepth.HasNewFrame();
        bool hasNewFrameColor = _readerColor != null && _readerColor.HasNewFrame();
        bool hasNewFrameNV21Color = _readerNV21Color != null && _readerNV21Color.HasNewFrame();
        bool hasNewFrameBody = _readerBody != null && _readerBody.HasNewFrame();
        bool hasNewFrameMaskedColor = _readerMaskedColor != null && _readerMaskedColor.HasNewFrame();
        bool hasNewFrameColorizedBody = _readerColorizedBody != null && _readerColorizedBody.HasNewFrame();

        Debug.Log("ND: " + hasNewFrameDepth +
                  " NC: " + hasNewFrameColor +
                  " NB: " + hasNewFrameBody +
                  " NMC: " + hasNewFrameMaskedColor +
                  " NCB: " + hasNewFrameColorizedBody);
        Debug.Log("DO: " + _isDepthOn +
                  " CO: " + _isColorOn +
                  " NVCO: " + _isNV21ColorOn +
                  " BO: " + _isBodyOn +
                  " MCO: " + _isMaskedColorOn +
                  " CBO: " + _isColorizedBodyOn);
        bool hasNewFrame = true;
        if (_isColorizedBodyOn)
        {
            hasNewFrame = hasNewFrameColorizedBody;
        }
        else if (_isMaskedColorOn)
        {
            hasNewFrame = hasNewFrameMaskedColor;
        }
        else if (_isBodyOn)
        {
            hasNewFrame = hasNewFrameBody;
        }
        else if (_isDepthOn)
        {
            hasNewFrame = hasNewFrameDepth;
        }

        if (_isColorOn)
        {
            hasNewFrame = hasNewFrame && hasNewFrameColor;
        }

#if ASTRA_UNITY_ANDROID_NATIVE
        if (_isNV21ColorOn)
        {
            hasNewFrame = hasNewFrame && hasNewFrameNV21Color;
        }
#endif

        // If no streams are started (during start up or shutdown)
        // then allow updateUntil to be complete
        bool noStreamsStarted = !_isDepthOn &&
                                !_isColorOn &&
                                !_isBodyOn &&
                                !_isMaskedColorOn &&
                                !_isColorizedBodyOn;

#if ASTRA_UNITY_ANDROID_NATIVE
        noStreamsStarted = noStreamsStarted && !_isNV21ColorOn;
#endif
        return hasNewFrame || noStreamsStarted;
    }

    private void CheckForNewFrames()
    {
        if (AstraUnityContext.Instance.WaitForUpdate(5) && AstraUnityContext.Instance.IsUpdateAsyncComplete)
        {
            // Inside this block until UpdateAsync() call below, we can use the Astra API safely
            updateFramesTime.Start();

            CheckDepthReader();
            CheckColorReader();
#if ASTRA_UNITY_ANDROID_NATIVE
            CheckNV21ColorReader();
#endif
            CheckBodyReader();
            CheckMaskedColorReader();
            CheckColorizedBodyReader();

            _frameCount++;

            updateFramesTime.Stop();
        }

        if (!AstraUnityContext.Instance.IsUpdateRequested)
        {
            UpdateStreamStartStop();
            // After calling UpdateAsync() the Astra API will be called from a background thread
            AstraUnityContext.Instance.UpdateAsync(UpdateUntilDelegate);
        }
    }

    void PrintBody(Astra.BodyFrame bodyFrame)
    {
        if (bodyFrame != null)
        {
            Body[] bodies = { };
            bodyFrame.CopyBodyData(ref bodies);
            foreach (Body body in bodies)
            {
                Astra.Joint headJoint = body.Joints[(int)JointType.Head];

                Debug.Log("Body " + body.Id + " COM " + body.CenterOfMass +
                    " Head Depth: " + headJoint.DepthPosition.X + "," + headJoint.DepthPosition.Y +
                    " World: " + headJoint.WorldPosition.X + "," + headJoint.WorldPosition.Y + "," + headJoint.WorldPosition.Z +
                    " Status: " + headJoint.Status.ToString());
            }
        }
    }

    void PrintDepth(Astra.DepthFrame depthFrame,
                    Astra.CoordinateMapper mapper)
    {
        if (depthFrame != null)
        {
            int width = depthFrame.Width;
            int height = depthFrame.Height;
            long frameIndex = depthFrame.FrameIndex;

            //determine if buffer needs to be reallocated
            if (width != _lastWidth || height != _lastHeight)
            {
                _buffer = new short[width * height];
                _lastWidth = width;
                _lastHeight = height;
            }
            depthFrame.CopyData(ref _buffer);

            int index = (int)((width * (height / 2.0f)) + (width / 2.0f));
            short middleDepth = _buffer[index];

            Vector3D worldPoint = mapper.MapDepthPointToWorldSpace(new Vector3D(width / 2.0f, height / 2.0f, middleDepth));
            Vector3D depthPoint = mapper.MapWorldPointToDepthSpace(worldPoint);

            Debug.Log("depth frameIndex: " + frameIndex
                      + " width: " + width
                      + " height: " + height
                      + " middleDepth: " + middleDepth
                      + " wX: " + worldPoint.X
                      + " wY: " + worldPoint.Y
                      + " wZ: " + worldPoint.Z
                      + " dX: " + depthPoint.X
                      + " dY: " + depthPoint.Y
                      + " dZ: " + depthPoint.Z + " frameCount: " + _frameCount);
        }
    }

    private void UpdateStreamStartStop()
    {
        // This methods assumes it is called from a safe location to call Astra API
        _isDepthOn = ToggleDepth == null || ToggleDepth.isOn;
        _isColorOn = ToggleColor == null || ToggleColor.isOn;
        _isNV21ColorOn = ToggleNV21Color == null || ToggleNV21Color.isOn;
        _isBodyOn = ToggleBody == null || ToggleBody.isOn;
        _isMaskedColorOn = ToggleMaskedColor == null || ToggleMaskedColor.isOn;
        _isColorizedBodyOn = ToggleColorizedBody == null || ToggleColorizedBody.isOn;

        if (_isDepthOn)
        {
            _depthStream.Start();
        }
        else
        {
            _depthStream.Stop();
        }

        if (_isColorOn)
        {
            _colorStream.Start();
        }
        else
        {
            _colorStream.Stop();
        }

#if ASTRA_UNITY_ANDROID_NATIVE
        if (_readerNV21Color != null)
        {
            if (_isNV21ColorOn)
            {
                _nv21ColorStream.Start();
            }
            else
            {
                _nv21ColorStream.Stop();
            }
        }
#endif

        if (_isBodyOn)
        {
            _bodyStream.Start();
        }
        else
        {
            _bodyStream.Stop();
        }

        if (_isMaskedColorOn)
        {
            _maskedColorStream.Start();
        }
        else
        {
            _maskedColorStream.Stop();
        }

        if (_isColorizedBodyOn)
        {
            _colorizedBodyStream.Start();
        }
        else
        {
            _colorizedBodyStream.Stop();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_areStreamsInitialized)
        {
#if ASTRA_UNITY_ANDROID_NATIVE
            return;
#else
            InitializeStreams();
#endif
        }

        totalFrameTime.Stop();
        totalFrameTime.Start();

        if (_areStreamsInitialized)
        {
            CheckForNewFrames();
        }

        astraUpdateTime.Start();
        astraUpdateTime.Stop();

        if (ToggleDebugText != null)
        {
            bool newDebugTextEnabled = ToggleDebugText.isOn;

            if (debugTextEnabled && !newDebugTextEnabled)
            {
                // Clear TimeText once if ToggleDebugText was just turned off
                TimeText.text = "";
            }

            debugTextEnabled = newDebugTextEnabled;
        }

        if (TimeText != null && debugTextEnabled)
        {
            BackgroundUpdaterTimings backgroundTimings = AstraUnityContext.Instance.BackgroundTimings;
            float totalFrameMs = totalFrameTime.AverageMilliseconds;
            float astraUpdateMs = backgroundTimings.updateAvgMillis;
            float lockWaitMs = backgroundTimings.lockWaitAvgMillis;
            float updateUntilMs = backgroundTimings.updateUntilAvgMillis;
            float updateFrameMs = updateFramesTime.AverageMilliseconds;
            TimeText.text = "Tot: " + totalFrameMs.ToString("0.0") + " ms\n" +
                            "AU: " + astraUpdateMs.ToString("0.0") + " ms\n" +
                            "LockWait: " + lockWaitMs.ToString("0.0") + " ms\n" +
                            "UpdateUntil: " + updateUntilMs.ToString("0.0") + " ms\n" +
                            "UpdateFr: " + updateFrameMs.ToString("0.0") + " ms\n";
        }
    }

    void OnDestroy()
    {
        Debug.Log("AstraController.OnDestroy");

        AstraUnityContext.Instance.WaitForUpdate(AstraBackgroundUpdater.WaitIndefinitely);

        if (_depthStream != null)
        {
            _depthStream.Stop();
        }

        if (_colorStream != null)
        {
            _colorStream.Stop();
        }

#if ASTRA_UNITY_ANDROID_NATIVE
        if (_nv21ColorStream != null)
        {
            _nv21ColorStream.Stop();
        }
#endif

        if (_bodyStream != null)
        {
            _bodyStream.Stop();
        }

        if (_maskedColorStream != null)
        {
            _maskedColorStream.Stop();
        }

        if (_colorizedBodyStream != null)
        {
            _colorizedBodyStream.Stop();
        }

        UninitializeStreams();

        AstraUnityContext.Instance.Initializing -= OnAstraInitializing;
        AstraUnityContext.Instance.Terminating -= OnAstraTerminating;

        AstraUnityContext.Instance.Terminate();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("AstraController handling OnApplicationQuit");
        AstraUnityContext.Instance.Terminate();
    }
}
