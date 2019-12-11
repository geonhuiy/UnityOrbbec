# Kitchen Cards, A 3D Motion Controlled Game For The Elderly
> Project repository for a Unity-based 3-D motion controlled game/platform designed for the elderly. This project is designed to run on the Orbbec Persee (check under prerequisites).  

This project was developed by students of [Metropolia](https://www.metropolia.fi/) for [Physilect](https://physilect.com/), in collaboration with [Hippa Metropolia](https://hippa.metropolia.fi/).  

![Banner Here](https://raw.githubusercontent.com/geonhuiy/UnityOrbbec/master/LogoBanner.png)  

![GitHub last commit](https://img.shields.io/github/last-commit/geonhuiy/UnityOrbbec) ![GitHub contributors](https://img.shields.io/github/contributors/geonhuiy/UnityOrbbec) ![GitHub commit activity](https://img.shields.io/github/commit-activity/m/geonhuiy/UnityOrbbec) ![GitHub closed pull requests](https://img.shields.io/github/issues-pr-closed-raw/geonhuiy/UnityOrbbec) ![GitHub All Releases](https://img.shields.io/github/downloads/geonhuiy/UnityOrbbec/total) ![Gitlab pipeline status](https://img.shields.io/gitlab/pipeline/geonhuiy/UnityOrbbec?label=GitLab%20build) [![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=data%3Aimage%2Fpng%3Bbase64%2CiVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAMAAAAolt3jAAABklBMVEUIJCYRLjARLzEWICcbIyYcLDQdJS4dKjMdLTQeKTMeKTUeKjMeKzMeKzQeNDceNTkeNzkeODkfIy8fJi8fJjAfMDQgJzEgKDIgKTIgMTUgMjkhJjAhKDMhKTIhKTQhKzYhLDYhLDchLjUhLjYiKTAiLDciLTgjKjIjLTcjLjkkLTgnKDYnKTYnLjb%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F9oVHO%2FAAAAhXRSTlMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQUGCAkMDhATFBcZGh0hIyYtNT1IS05RVFZXW1xeYWNnbG9wcXN2eHt9goaKkpWXo6usrbCztLW2ubq7vL2%2Bv8HDxsjKzNfY5OXn6%2Bzt8fP09vj5%2FP3%2BxDGH3QAAAMlJREFUeAFjUFTiZ5AWEFQ1dgwvDuIEc8WkHDJrW1tb07nBXHOb%2FPIYz7LWSgsgl8%2B9NclWjz24LrTVmUFR2b0110SE1aYhyqg%2BmkHRozXNkE2LI67KXDy7iMG7uTUnITU5s9WXhSfQi8GvtbUgMz%2BvsNVLSMbfjUHUpzVRX0VXPb7ClCujiEGSyac1xUhY1q4pwqAulkGSkdmnNd5KTiKsJqDVBcTVtLbPL410LW%2BptgRz5dUcixpbW1qzuMFcBW0dDTOnqJIQXgB6SzT11MCPiQAAAABJRU5ErkJggg%3D%3D)](https://unity3d.com) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Table of Contents  
 * [Getting Started](#getting-started)   
   * [Prerequisites](#prerequisites)  
   * [Installing](#installing)
 * [Running Tests](#running-tests)
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
This project is licensed under the MIT license - see [LICENSE.md](LICENSE.md) for more details.
  
