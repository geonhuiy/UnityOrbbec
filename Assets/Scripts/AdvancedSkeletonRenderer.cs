using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AdvancedSkeletonRenderer : MonoBehaviour
{
    public LineRenderer leftArmRenderer;
    public LineRenderer rightArmRenderer;
    public LineRenderer leftLegRenderer;
    public LineRenderer rightLegRenderer;
    public LineRenderer torsoRenderer;
    public Transform leftHand;
    public Transform rightHand;
    // calibration value is used to calirate the coefficient of the the range of movement of the user
    // which allows the user to move more naturally in the game.
    public float calibrationValue;
    private long _lastFrameIndex = -1;

    private Astra.Body[] _bodies;
    private Dictionary<int, GameObject[]> _bodySkeletons;

    private readonly Vector3 NormalPoseScale = new Vector3(1, 1, 1);
    private readonly Vector3 GripPoseScale = new Vector3(0.5f, 0.5f, 0.5f);

    public GameObject JointPrefab;
    public Transform JointRoot;

    public Toggle ToggleSeg = null;
    public Toggle ToggleSegBody = null;
    public Toggle ToggleSegBodyHand = null;

    public Toggle ToggleProfileFull = null;
    public Toggle ToggleProfileUpperBody = null;
    public Toggle ToggleProfileBasic = null;

    public Toggle ToggleOptimizationAccuracy = null;
    public Toggle ToggleOptimizationBalanced = null;
    public Toggle ToggleOptimizationMemory = null;
    public Slider SliderOptimization = null;

    private Astra.BodyTrackingFeatures _previousTargetFeatures = Astra.BodyTrackingFeatures.HandPose;
    private Astra.SkeletonProfile _previousSkeletonProfile = Astra.SkeletonProfile.Full;
    private Astra.SkeletonOptimization _previousSkeletonOptimization = Astra.SkeletonOptimization.BestAccuracy;

    void Start()
    {
        _bodySkeletons = new Dictionary<int, GameObject[]>();
        _bodies = new Astra.Body[Astra.BodyFrame.MaxBodies];
    }

    public void OnNewFrame(Astra.BodyStream bodyStream, Astra.BodyFrame frame)
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

        frame.CopyBodyData(ref _bodies);
        UpdateSkeletonsFromBodies(_bodies);
        UpdateBodyFeatures(bodyStream, _bodies);
        UpdateSkeletonProfile(bodyStream);
        UpdateSkeletonOptimization(bodyStream);
    }

// Lines representing the body
    private Vector3[] leftArmPos = new Vector3[5];
    private Vector3[] rightArmPos = new Vector3[5];
    private Vector3[] leftLegsPos = new Vector3[4];
    private Vector3[] rightLegsPos = new Vector3[4];
    private Vector3[] torsoPos = new Vector3[5];
    
    void UpdateSkeletonsFromBodies(Astra.Body[] bodies)
    {   
        foreach (var body in bodies)
        {
            if (body.Status == Astra.BodyStatus.NotTracking)
            {
                continue;
            }

            GameObject[] joints;
            if (!_bodySkeletons.ContainsKey(body.Id))
            {
                joints = new GameObject[body.Joints.Length];

                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i] = (GameObject)Instantiate(JointPrefab, Vector3.zero, Quaternion.identity);
                    joints[i].transform.SetParent(JointRoot);
                }

                _bodySkeletons.Add(body.Id, joints);
            }
            else
            {
                joints = _bodySkeletons[body.Id];
            }

            for (int i = 0; i < body.Joints.Length; i++)
            {
                var skeletonJoint = joints[i];
                var bodyJoint = body.Joints[i];
                if (bodyJoint.Status != Astra.JointStatus.NotTracked)
                {
                    if (!skeletonJoint.activeSelf)
                    {
                        skeletonJoint.SetActive(true);
                    }

                    skeletonJoint.transform.localPosition =
                        new Vector3(bodyJoint.WorldPosition.X / 1000f,
                                    bodyJoint.WorldPosition.Y / 1000f,
                                    bodyJoint.WorldPosition.Z / 1000f);

                    //skel.Joints[i].Orient.Matrix:
                    // 0, 			1,	 		2,
                    // 3, 			4, 			5,
                    // 6, 			7, 			8
                    // -------
                    // right(X),	up(Y), 		forward(Z)

                    //Vector3 jointRight = new Vector3(
                    //    bodyJoint.Orientation.M00,
                    //    bodyJoint.Orientation.M10,
                    //    bodyJoint.Orientation.M20);

                    Vector3 jointUp = new Vector3(
                        bodyJoint.Orientation.M01,
                        bodyJoint.Orientation.M11,
                        bodyJoint.Orientation.M21);

                    Vector3 jointForward = new Vector3(
                        bodyJoint.Orientation.M02,
                        bodyJoint.Orientation.M12,
                        bodyJoint.Orientation.M22);

                    skeletonJoint.transform.rotation =
                        Quaternion.LookRotation(jointForward, jointUp);

                    if (bodyJoint.Type == Astra.JointType.LeftHand)
                    {
                        UpdateHandPoseVisual(skeletonJoint, body.HandPoseInfo.LeftHand);
                    }
                    else if (bodyJoint.Type == Astra.JointType.RightHand)
                    {
                        UpdateHandPoseVisual(skeletonJoint, body.HandPoseInfo.RightHand);
                    }
                }
                else
                {
                   if (skeletonJoint.activeSelf) skeletonJoint.SetActive(false);
                }
                // LEFT ARM
                if (i == (int)Astra.JointType.LeftHand)
                {
                    leftHand.transform.position = new Vector3(skeletonJoint.transform.position.x*calibrationValue,skeletonJoint.transform.position.y*calibrationValue, 0) ;
                    leftArmPos[0] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.LeftWrist)
                {
                    leftArmPos[1] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.LeftElbow)
                {
                    leftArmPos[2] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.LeftShoulder)
                {
                    leftArmPos[3] = skeletonJoint.transform.position;
                }
                // RIGHT ARM
                else if (i == (int)Astra.JointType.RightHand)
                {
                    rightHand.transform.position = new Vector3(skeletonJoint.transform.position.x*calibrationValue,skeletonJoint.transform.position.y*calibrationValue, 0) ;
                    rightArmPos[0] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.RightWrist)
                {
                    rightArmPos[1] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.RightElbow)
                {
                    rightArmPos[2] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.RightShoulder)
                {
                    rightArmPos[3] = skeletonJoint.transform.position;
                }
                // LEFT LEG
                else if (i == (int)Astra.JointType.LeftFoot)
                {
                    leftLegsPos[0] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.LeftKnee)
                {
                    leftLegsPos[1] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.LeftHip)
                {
                    leftLegsPos[2] = skeletonJoint.transform.position;
                }
                // RIGHT LEG
                else if (i == (int)Astra.JointType.RightFoot)
                {
                    rightLegsPos[0] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.RightKnee)
                {
                    rightLegsPos[1] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.RightHip)
                {
                    rightLegsPos[2] = skeletonJoint.transform.position;
                }
                // TORSO
                else if (i == (int)Astra.JointType.Head)
                {
                    torsoPos[0] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.Neck)
                {
                    torsoPos[1] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.MidSpine)
                {
                    torsoPos[3] = skeletonJoint.transform.position;
                }
                // COMMON JOINTS
                else if (i == (int)Astra.JointType.ShoulderSpine)
                {
                    leftArmPos[4] = skeletonJoint.transform.position;
                    rightArmPos[4] = skeletonJoint.transform.position;
                    torsoPos[2] = skeletonJoint.transform.position;
                }
                else if (i == (int)Astra.JointType.BaseSpine)
                {
                    leftLegsPos[3] = skeletonJoint.transform.position;
                    rightLegsPos[3] = skeletonJoint.transform.position;
                    torsoPos[4] = skeletonJoint.transform.position;
                }
            }
            //print(leftArmPos[1] + "Left arm");    
            //print(rightArmPos);
            //print(rightLegsPos);
            //print(torsoPos);
            //print(leftArmPos);
            if (leftArmRenderer != null)
        {
            leftArmRenderer.SetPositions(leftArmPos);
        }

        //render position of arm renderer in scene
        if (rightArmRenderer != null)
        {
            rightArmRenderer.SetPositions(rightArmPos);
        }
        if (leftLegRenderer != null)
        {
            leftLegRenderer.SetPositions(leftLegsPos);
        }
        if (rightLegRenderer != null)
        {
            rightLegRenderer.SetPositions(rightLegsPos);
        }
        if (torsoRenderer != null)
        {
            torsoRenderer.SetPositions(torsoPos);
        }
        }
    }

    private void UpdateHandPoseVisual(GameObject skeletonJoint, Astra.HandPose pose)
    {
        Vector3 targetScale = NormalPoseScale;
        if (pose == Astra.HandPose.Grip)
        {
            targetScale = GripPoseScale;
        }
        skeletonJoint.transform.localScale = targetScale;
    }

    private void UpdateBodyFeatures(Astra.BodyStream bodyStream, Astra.Body[] bodies)
    {
        if (ToggleSeg != null &&
            ToggleSegBody != null &&
            ToggleSegBodyHand != null)
        {
            Astra.BodyTrackingFeatures targetFeatures = Astra.BodyTrackingFeatures.Segmentation;
            if (ToggleSegBodyHand.isOn)
            {
                targetFeatures = Astra.BodyTrackingFeatures.HandPose;
            }
            else if (ToggleSegBody.isOn)
            {
                targetFeatures = Astra.BodyTrackingFeatures.Skeleton;
            }

            if (targetFeatures != _previousTargetFeatures)
            {
                _previousTargetFeatures = targetFeatures;
                foreach (var body in bodies)
                {
                    if (body.Status != Astra.BodyStatus.NotTracking)
                    {
                        bodyStream.SetBodyFeatures(body.Id, targetFeatures);
                    }
                }
                bodyStream.SetDefaultBodyFeatures(targetFeatures);
            }
        }
    }

    private void UpdateSkeletonProfile(Astra.BodyStream bodyStream)
    {
        if (ToggleProfileFull != null &&
            ToggleProfileUpperBody != null &&
            ToggleProfileBasic != null)
        {
            Astra.SkeletonProfile targetSkeletonProfile = Astra.SkeletonProfile.Full;
            if (ToggleProfileFull.isOn)
            {
                targetSkeletonProfile = Astra.SkeletonProfile.Full;
            }
            else if (ToggleProfileUpperBody.isOn)
            {
                targetSkeletonProfile = Astra.SkeletonProfile.UpperBody;
            }
            else if (ToggleProfileBasic.isOn)
            {
                targetSkeletonProfile = Astra.SkeletonProfile.Basic;
            }

            if (targetSkeletonProfile != _previousSkeletonProfile)
            {
                _previousSkeletonProfile = targetSkeletonProfile;
                bodyStream.SetSkeletonProfile(targetSkeletonProfile);
            }
        }
    }

    private void UpdateSkeletonOptimization(Astra.BodyStream bodyStream)
    {
        if (ToggleOptimizationAccuracy != null &&
            ToggleOptimizationBalanced != null &&
            ToggleOptimizationMemory != null &&
            SliderOptimization != null)
        {
            int targetOptimizationValue = (int)_previousSkeletonOptimization;
            if (ToggleOptimizationAccuracy.isOn)
            {
                targetOptimizationValue = (int)Astra.SkeletonOptimization.BestAccuracy;
            }
            else if (ToggleOptimizationBalanced.isOn)
            {
                targetOptimizationValue = (int)Astra.SkeletonOptimization.Balanced;
            }
            else if (ToggleOptimizationMemory.isOn)
            {
                targetOptimizationValue = (int)Astra.SkeletonOptimization.MinimizeMemory;
            }

            if (targetOptimizationValue != (int)_previousSkeletonOptimization)
            {
                Debug.Log("Set optimization slider: " + targetOptimizationValue);
                SliderOptimization.value = targetOptimizationValue;
            }

            Astra.SkeletonOptimization targetSkeletonOptimization = Astra.SkeletonOptimization.Balanced;
            int sliderValue = (int)SliderOptimization.value;

            switch (sliderValue)
            {
                case 1:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization1;
                    break;
                case 2:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization2;
                    break;
                case 3:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization3;
                    break;
                case 4:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization4;
                    break;
                case 5:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization5;
                    break;
                case 6:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization6;
                    break;
                case 7:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization7;
                    break;
                case 8:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization8;
                    break;
                case 9:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization9;
                    break;
                default:
                    targetSkeletonOptimization = Astra.SkeletonOptimization.Optimization9;
                    SliderOptimization.value = 9;
                    break;
            }

            if (targetSkeletonOptimization != _previousSkeletonOptimization)
            {
                UpdateOptimizationToggles(targetSkeletonOptimization);

                Debug.Log("SetSkeletonOptimization: " + targetSkeletonOptimization);
                _previousSkeletonOptimization = targetSkeletonOptimization;
                bodyStream.SetSkeletonOptimization(targetSkeletonOptimization);
            }
        }
    }

    private void UpdateOptimizationToggles(Astra.SkeletonOptimization optimization)
    {
        ToggleOptimizationMemory.isOn = optimization == Astra.SkeletonOptimization.MinimizeMemory;
        ToggleOptimizationBalanced.isOn = optimization == Astra.SkeletonOptimization.Balanced;
        ToggleOptimizationAccuracy.isOn = optimization == Astra.SkeletonOptimization.BestAccuracy;
    }
}
