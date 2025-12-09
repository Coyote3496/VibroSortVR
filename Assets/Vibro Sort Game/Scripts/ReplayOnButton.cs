using UnityEngine;

public class ReplayOnButton : MonoBehaviour
{
    public HapticsManager hM;
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;

    public Transform handTransform;
    public float detectionRadius = 0.15f;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            TryReplayNearestVibration();
        }
    }

    private void TryReplayNearestVibration()
    {
        // Find colliders near hand
        Collider[] hits = Physics.OverlapSphere(handTransform.position, detectionRadius);

        HapticInteractable nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider c in hits)
        {
            HapticInteractable hi = c.GetComponentInParent<HapticInteractable>();
            if (hi != null)
            {
                float d = Vector3.Distance(handTransform.position, hi.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    nearest = hi;
                }
            }
        }

        if (nearest != null)
        {
            Replay(nearest);
        }
    }

    private void Replay(HapticInteractable hi)
    {
        hM.RightHapticsBuffer_amp = hi.hapticBuffer_amp;
        hM.RightHapticsBuffer_freq = hi.hapticBuffer_freq;

        hM.rightHapticLoop = false;
        hM.rightHapticStartTime = Time.time;

        Debug.Log("ReplayOnButton: Replayed vibration from object " + hi.name);
    }
}
