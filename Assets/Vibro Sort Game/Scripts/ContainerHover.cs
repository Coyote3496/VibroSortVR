using UnityEngine;

public class ContainerHover : MonoBehaviour
{
    public HapticsManager hM;
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;

    private float[] entryAmp  = new float[] { 0.3f, 0.6f, 0.3f, 0.0f };
    private float[] entryFreq = new float[] { 0.2f, 0.4f, 0.2f, 0.0f };

    private float[] exitAmp  = new float[] { 0.6f, 0.3f, 0.6f, 0.0f };
    private float[] exitFreq = new float[] { 0.5f, 0.2f, 0.5f, 0.0f };

    private bool isInside = false;

    private bool IsHandCollider(string name)
    {
        return name.Contains("Pinch") ||
               name.Contains("Hand") ||
               name.Contains("Pointer");
    }

    private void PlayOneShot(float[] amp, float[] freq)
    {
        hM.RightHapticsBuffer_amp = amp;
        hM.RightHapticsBuffer_freq = freq;

        hM.rightHapticLoop = false;   
        hM.rightHapticStartTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsHandCollider(other.name))
            return;

        if (isInside)
            return;

        isInside = true;

        PlayOneShot(entryAmp, entryFreq);

        Debug.Log("ContainerHover: ENTRY pulse played.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsHandCollider(other.name))
            return;

        // Only buzz on a real exit
        if (!isInside)
            return;

        isInside = false;

        PlayOneShot(exitAmp, exitFreq);

        Debug.Log("ContainerHover: EXIT pulse played.");
    }
}
