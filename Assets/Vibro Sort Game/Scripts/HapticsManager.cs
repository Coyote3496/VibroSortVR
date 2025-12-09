using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HapticsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI debugText;
    private int hapticFrameMilli = 100;

    private float[] leftHapticsBuffer_amp;
    public float[] LeftHapticsBuffer_amp {
        get { return leftHapticsBuffer_amp;  }
        set {
            leftHapticsBuffer_amp = value;
            leftDuration = value.Length * hapticFrameMilli;
        }
    }
    private float[] leftHapticsBuffer_freq;
    public float[] LeftHapticsBuffer_freq {
        get { return leftHapticsBuffer_freq;  }
        set {
            leftHapticsBuffer_freq = value;
            leftDuration = value.Length * hapticFrameMilli;
        }
    }
    private int leftDuration = 0;
    public bool leftHapticLoop = true;
    public float leftHapticStartTime = 0f;

    private float[] rightHapticsBuffer_amp;
    public float[] RightHapticsBuffer_amp {
        get { return rightHapticsBuffer_amp;  }
        set {
            rightHapticsBuffer_amp = value;
            rightDuration = value.Length * hapticFrameMilli;
        }
    }
    private float[] rightHapticsBuffer_freq;
    public float[] RightHapticsBuffer_freq {
        get { return rightHapticsBuffer_freq;  }
        set {
            rightHapticsBuffer_freq = value;
            rightDuration = value.Length * hapticFrameMilli;
        }
    }
    private int rightDuration = 0;
    public bool rightHapticLoop = true;
    public float rightHapticStartTime = 0f;

    void Start()
    {
        LeftHapticsBuffer_amp = new float[] {0f};
        LeftHapticsBuffer_freq = new float[] {0f};

        RightHapticsBuffer_amp = new float[] {0f};
        RightHapticsBuffer_freq = new float[] {0f};
    }

    // Need a function that takes a relative Haptic Buffer in seconds and returns an Absolute Haptic Buffer in milliseconds.


    private void FixedUpdate() {
        if (leftHapticLoop) {
            int currentHapticFrame_left = ((int)((Time.time - leftHapticStartTime) * 1000) % leftDuration) / hapticFrameMilli;
            OVRInput.SetControllerVibration(leftHapticsBuffer_freq[currentHapticFrame_left], leftHapticsBuffer_amp[currentHapticFrame_left], OVRInput.Controller.LTouch);
        } else {
            if ((Time.time - leftHapticStartTime) * 1000 < leftDuration) {
                int currentHapticFrame_left = ((int)((Time.time - leftHapticStartTime) * 1000) % leftDuration) / hapticFrameMilli;
                OVRInput.SetControllerVibration(leftHapticsBuffer_freq[currentHapticFrame_left], leftHapticsBuffer_amp[currentHapticFrame_left], OVRInput.Controller.LTouch);
            }
        }

        if (rightHapticLoop) {
            int currentHapticFrame_right = ((int)((Time.time - rightHapticStartTime) * 1000) % rightDuration) / hapticFrameMilli;
            OVRInput.SetControllerVibration(rightHapticsBuffer_freq[currentHapticFrame_right], rightHapticsBuffer_amp[currentHapticFrame_right], OVRInput.Controller.RTouch);
        } else {
            if ((Time.time - rightHapticStartTime) * 1000 < rightDuration) {
                int currentHapticFrame_right = ((int)((Time.time - rightHapticStartTime) * 1000) % rightDuration) / hapticFrameMilli;
                OVRInput.SetControllerVibration(rightHapticsBuffer_freq[currentHapticFrame_right], rightHapticsBuffer_amp[currentHapticFrame_right], OVRInput.Controller.RTouch);
            }
        }

        debugText.text = string.Join(" ", rightHapticsBuffer_freq) + "\n" + rightDuration.ToString(); // + " " + currentHapticFrame_right.ToString();//rightHapticsBuffer_freq[currentHapticFrame_right].ToString() + " " + rightHapticsBuffer_amp[currentHapticFrame_right].ToString();
    }

    public void End(OVRInput.Controller c) {
        if (c == OVRInput.Controller.LTouch) {
            LeftHapticsBuffer_amp = new float[] {0f};
            LeftHapticsBuffer_freq = new float[] {0f};
        } else if (c == OVRInput.Controller.RTouch) {
            RightHapticsBuffer_amp = new float[] {0f};
            RightHapticsBuffer_freq = new float[] {0f};
        }
    }
}


// When the task is done, screen turns on.