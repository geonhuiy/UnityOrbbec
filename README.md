# UnityOrbbec
> Project repository for a Unity-based 3-D motion controlled game/platform designed for the elderly. This project is designed to run on the Orbbec Persee (check under prerequisites).  
## Table of Contents  
 * [Getting Started](#getting-started)   
   * [Prerequisites](#prerequisites)  
   * [Installing](#installing)
 * [Running Tests](#running-tests)
   * [Setting up GitLab for Continuous Integration](#setting-up-gitlab-for-continuous-integration)  
   * [Testing Game Environment With a Mouse](#testing-game-environment-with-a-mouse)  
 * [Deployment](#deployment)  
   * [Exporting Project From Unity](#exporting-project-from-unity)
   * [Importing & Configuring Project in Android Studio](#importing-&-configuring-project-in-android-studio)
   * [Deploying & Installing via Cloud Storage](#deploying-&-installing-via-cloud-storage)  
 * [Licensing](#licensing)  
 
## Getting Started  
### Prerequisites   
  - Orbbec Astra (for debugging)
  >https://orbbec3d.com/product-astra-pro/
  
  - Orbbec Persee (for deployment)
  >https://orbbec3d.com/product-persee/  
  
  - Unity  
  
  Download Unity Hub from  
  https://unity3d.com/get-unity/download  
  >Unity 2019.2.12f1 was used to create this project.  
  After the installation is complete, open Unity Hub and add Unity 2019.2.12f1 under Installs - Add.  
  Add "Android Build Support" module.
  
  - Android Studio (for building a working apk)
  >https://developer.android.com/studio
  
  
  
### Installing  
 - Clone the repo to your local machine.  
 `git clone https://github.com/geonhuiy/UnityOrbbec.git`

## Running Tests
### Setting up GitLab for Continuous Integration
1. Setting up GitLab Runner
  - Install GitLab Runner on GNU/Linux, macOS, FreeBSD, Windows. Detailed instructions can be found under:  
    > https://docs.gitlab.com/runner/install/  
  - Register installed Runner  
    > https://docs.gitlab.com/runner/register/  
    > Runner tags should include 'Unity' and/or 'Orbbec'  
    > Choose 'shell' for executor when prompt during Runner registration  
    
### Testing Game Environment With a Mouse
  - After opening the project on Unity, navigate to `HandCanvas` on the editor located on the left side.  
  - Expand the `HandCanvas` and select either `LeftHand` or `RightHand` and enable the `Attach` script on the right side of the editor to be able to control the hand with the mouse.  
  
## Deployment  
### 1. Exporting Project From Unity.
  - Select: `File - Build Settings`.
  - Ensure that `Export project` option is checked.
  - Click on `Player Settings` and check that `Target API Level` under `Other settings - Identification` is set to `Android 5.1 'Lollipop' (API Level 22)`.
  - Export the project.
  
### 2. Importing & Configuring Project in Android Studio.
  - Ensure that corresponding Android SDK is installed on Android Studio. 
  - Navigate and open the exported project.
  - Remove/edit following lines from files :  
      #### AndroidManifest.xml  
      ##### Remove :  
      `<uses-sdk android:minSdkVersion="19" />`        
      `<android:uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />`     
      
      #### build.gradle  
      ##### Remove :  
      ```
      packagingOptions {
        doNotStrip '*/armeabi-v7a/*.so'
      }
      (Under defaultConfig) ndk {
        abiFilters 'armeabi-v7a'
      }
      ```  
      
      ##### Edit :  
      `targetSdkVersion targetVersionHere -> 26`  
      
  - Sync Gradle files and build the APK under `Build - Build Bundle(s) / APK(s)`.  
  
### 3. Deploying & Installing via Cloud Storage
   - Locate the built APK and upload it to a cloud storage of choice ([Google Drive](https://www.google.com/drive/) recommended).    
   - Download from the Orbbec Persee and install.  
      

## Licensing  
Placeholder
  
