#if UNITY_ANDROID && !UNITY_EDITOR
#define ASTRA_UNITY_ANDROID_NATIVE
#endif

using Astra;
using System;
using System.IO;
using UnityEngine;
#if UNITY_ANDROID

#endif

public sealed class AstraUnityContext
{
    private bool _initialized = false;
    private bool _initializing = false;

#if ASTRA_UNITY_ANDROID_NATIVE
	private static AndroidJavaClass javaClass;
	private static AndroidJavaObject javaActivity;

    public class AstraDeviceManagerListener : AndroidJavaProxy
    {
        private AstraController obj_;
        public AstraDeviceManagerListener(AstraController obj) : base("com.orbbec.astra.android.AstraDeviceManagerListener")
        {
            obj_ = obj;
        }

        void onOpenAllDevicesCompleted(AndroidJavaObject availableDevices)
        {
            Debug.Log("AstraUnityContext onOpenAllDevicesCompleted");
            System.Object[] args = new System.Object[0];
            int num = javaActivity.Call<int>("getAvailableDevicesSize", args);
            if (num > 0)
            {
                Debug.Log("AstraUnityContext getAvailableDevicesSize:" + num);
                obj_.InitializeStreams();
            }
        }

        void onOpenDeviceCompleted(AndroidJavaObject device, AndroidJavaObject opened)
        { }

        void onNoDevice()
        {
            Debug.Log("AstraUnityContext onNoDevice");
            //GameObject.Find("AstraController").SendMessage("InitializeStreams");
        }

        void onPermissionDenied(AndroidJavaObject device)
        {
            Debug.Log("AstraUnityContext onPermissionDenied");
            //GameObject.Find("AstraController").SendMessage("InitializeStreams");
        }
    }
#endif

    private AstraBackgroundUpdater _backgroundUpdater = new AstraBackgroundUpdater();

    public BackgroundUpdaterTimings BackgroundTimings { get { return _backgroundUpdater.Timings; } }

    public bool IsUpdateAsyncComplete { get { return _backgroundUpdater.IsUpdateAsyncComplete; } }
    public bool IsUpdateRequested { get { return _backgroundUpdater.IsUpdateRequested; } }

    //private AstraUnityContext()
    //{
    //    Initialize();
    //}


    public event EventHandler<AstraInitializingEventArgs> Initializing;

    public event EventHandler<AstraTerminatingEventArgs> Terminating;

    public event EventHandler<PermissionRequestCompletedEventArgs> PermissionRequestCompleted;

    public static AstraUnityContext Instance { get { return Nested.Context; } }
    private class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested() { }
        internal static readonly AstraUnityContext Context = new AstraUnityContext();
    }

    public void Initialize()
    {
        if (_initialized)
        {
            Debug.Log("Astra SDK previously initialized");
            return;
        }

        if (_initializing)
        {
            return;
        }

        _initializing = true;
#if UNITY_STANDALONE_WIN
        Debug.Log("copy necessary file");
        string srcName = Application.streamingAssetsPath + "/orbbec.ini";
        string dstName = Application.streamingAssetsPath + "/../Plugins/orbbec.ini";
        try
        {
            var srcDate = File.GetLastWriteTime(srcName);
            //if dstName doesn't exist, dstDate will be "1/1/1601 8:00:00 AM"
            var dstData = File.GetLastWriteTime(dstName);
            if (srcDate > dstData)
            {
                Debug.Log("remove old file");
                File.Delete(dstName);
                File.Copy(srcName, dstName);
            }
        }
        catch (Exception)
        {
            throw;
        }
#endif
        Debug.Log("Astra SDK initializing.");
        Context.Initialize();
        RaiseInitializing();

        // After Start()ing _backgroundUpdater, we just use Lock()/Unlock() around
        // any Astra SDK API calls
        _backgroundUpdater.Start();

        _initialized = true;
        _initializing = false;
    }

    public void Terminate()
    {
        if (!_initialized)
        {
            return;
        }

        Debug.Log("Astra SDK terminating.");
        RaiseTerminating();

        _backgroundUpdater.Stop();
        Context.Terminate();
        _initialized = false;
    }

    public void UpdateAsync(Func<bool> updateUntilCondition)
    {
        // Request the background thread perform an update
        _backgroundUpdater.UpdateAsync(updateUntilCondition);
    }

    public bool WaitForUpdate(int timeoutMilliseconds)
    {
        // Wait until the background thread completes an update
        return _backgroundUpdater.Wait(timeoutMilliseconds);
    }

    public void RequestUsbDeviceAccessFromAndroid(AstraController controller)
    {
        Initialize();

        // Make sure we aren't updating Astra in the background
        _backgroundUpdater.Wait(-1);

#if ASTRA_UNITY_ANDROID_NATIVE
        EnsureJavaActivity();

        Debug.Log("AstraUnityContext.RequestUsbDeviceAccessFromAndroid() calling openAllDevices");

        javaActivity.Call("openAllDevices", new AstraDeviceManagerListener(controller));

        Debug.Log("AstraUnityContext.RequestUsbDeviceAccessFromAndroid() called openAllDevices");

        //TODO: only call this in the callback with the success/fail results
        RaisePermissionRequestCompleted(true);
#else
        RaisePermissionRequestCompleted(true);
#endif
    }

    private void RaisePermissionRequestCompleted(bool granted)
    {
        var eventArgs = new PermissionRequestCompletedEventArgs(granted);
        var handler = PermissionRequestCompleted;
        if (handler != null) handler(this, eventArgs);
    }

    ~AstraUnityContext()
    {
        Debug.Log("Finalizer of AstraUnityContext");
        Terminate();
    }

#if ASTRA_UNITY_ANDROID_NATIVE
    private void EnsureJavaActivity()
    {
        if (javaActivity == null)
        {
            Debug.Log("AstraUnityContext.EnsureJavaActivity() Getting Java activity");
            javaClass = new AndroidJavaClass("com.orbbec.astra.android.unity3d.AstraUnityPlayerActivity");
            javaActivity = javaClass.GetStatic<AndroidJavaObject>("Instance");
            Debug.Log("AstraUnityContext.EnsureJavaActivity() Got Java activity");
        }
    }
#endif

    private void RaiseInitializing()
    {
        var handler = Initializing;
        if (handler != null) handler(this, new AstraInitializingEventArgs());
    }

    private void RaiseTerminating()
    {
        var handler = Terminating;
        if (handler != null) handler(this, new AstraTerminatingEventArgs());
    }
}
