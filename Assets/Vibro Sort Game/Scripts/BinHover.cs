using UnityEngine;
using System.Collections;

public class BinHover : MonoBehaviour
{
    public HapticsManager hM;
    public OVRInput.Controller controller = OVRInput.Controller.RTouch;

    // Set by ObjectGenerator or BinManager
    public int binIndex = 0; 

    // Runtime-resolved hover pattern
    public float[] hoverAmp;
    public float[] hoverFreq;

    private bool hasPlayed = false;

    // -----------------------------
    // POSITONAL HOVER PATTERNS
    // -----------------------------
    private static readonly float[][] hoverAmps = new float[][]
    {
        // Bin 0 — Strong single thump
        new float[] {1.0f, 0.0f},

        // Bin 1 — Two medium taps
        new float[] {0.6f, 0.6f, 0.0f},

        // Bin 2 — Three quick taps (middle bin)
        new float[] {0.7f, 0.7f, 0.7f, 0.0f},

        // Bin 3 — Soft low rumble
        new float[] {0.3f, 0.3f, 0.3f, 0.0f},

        // Bin 4 — High buzzing flutter
        new float[] {0.9f, 0.4f, 0.9f, 0.0f}
    };

    private static readonly float[][] hoverFreqs = new float[][]
    {
        // Bin 0 — Strong single thump
        new float[] {0.4f, 0.0f},

        // Bin 1 — Two medium taps
        new float[] {0.3f, 0.3f, 0.0f},

        // Bin 2 — Three quick taps (middle bin)
        new float[] {0.5f, 0.5f, 0.5f, 0.0f},

        // Bin 3 — Soft low rumble
        new float[] {0.2f, 0.15f, 0.2f, 0.0f},

        // Bin 4 — High buzzing flutter
        new float[] {1.0f, 0.3f, 1.0f, 0.0f}
    };

    // -----------------------------
    private bool IsHandCollider(string name)
    {
        return name.Contains("Pinch") ||
               name.Contains("Hand") ||
               name.Contains("Pointer");
    }

    // -----------------------------
    private IEnumerator PlayHoverPatternOnce()
    {
        // Use positional patterns
        hoverAmp = hoverAmps[Mathf.Clamp(binIndex, 0, hoverAmps.Length - 1)];
        hoverFreq = hoverFreqs[Mathf.Clamp(binIndex, 0, hoverFreqs.Length - 1)];

        float[] amp = hoverAmp;
        float[] freq = hoverFreq;

        Debug.Log($"BinHover: Playing hover pattern for bin {binIndex}");

        // Push pattern to HapticsManager
        hM.RightHapticsBuffer_amp = amp;
        hM.RightHapticsBuffer_freq = freq;

        hM.rightHapticLoop = false;
        hM.rightHapticStartTime = Time.time;

        // Duration based on number of frames
        int durationMS = amp.Length * 200; // matches your frame size
        yield return new WaitForSeconds(durationMS / 1000f);

        // Stop
        hM.End(controller);
    }

    // -----------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (!IsHandCollider(other.name))
            return;

        if (!hasPlayed)
        {
            hasPlayed = true;
            StartCoroutine(PlayHoverPatternOnce());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsHandCollider(other.name))
            return;

        hasPlayed = false;
        hM.End(controller);
    }
}
