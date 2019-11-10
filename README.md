# UnityOrbbec
> Project repository for a Unity-based 3-D motion controlled game/platform designed for the elderly.  
## Table of Contents  
 * [Getting Started](#getting-started)   
   * [Prerequisites](#prerequisites)  
   * [Installing](#installing)
 * [Running Tests](#running-tests)
   * [Setting up GitLab for Continuous Integration](#setting-up-gitlab-for-continuous-integration)
 * [Deployment](#deployment)  
 
## Getting Started  
### Prerequisites   
  - Unity  
  
  Download Unity Hub from  
  https://unity3d.com/get-unity/download  
  >Unity 2019.211f1 was used to create this project.  
  After the installation is complete, open Unity Hub and add Unity 2019.211f1 under Installs - Add.  
  Add "Android Build Support" module.
  
### Installing  
 - Clone the repo to your local machine  
 ```git clone https://github.com/geonhuiy/UnityOrbbec.git```

## Running Tests
### Setting up GitLab for Continuous Integration
1. Setting up GitLab Runner
  - Install GitLab Runner on GNU/Linux, macOS, FreeBSD, Windows. Detailed instructions can be found under:  
    > https://docs.gitlab.com/runner/install/  
  - Register installed Runner  
    > https://docs.gitlab.com/runner/register/  
    > Runner tags should include 'Unity' and/or 'Orbbec'  
    > Choose 'shell' for executor when prompt during Runner registration  
    
## Deployment  
Placeholder  
