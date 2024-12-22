using UnityEngine;
using UnityEngine.Audio;

public class AudioZoneControl : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot outdoorSnapshot;
    [SerializeField] private AudioMixerSnapshot indoorSnapshot;
    [SerializeField] private float crossfadeTime = 0.5f;
    [SerializeField] private string triggerTag = "Player";

    private int zoneCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            zoneCount++;
            UpdateAudioZoneSnapshot();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            zoneCount--;
            UpdateAudioZoneSnapshot();
        }
    }

    private void UpdateAudioZoneSnapshot()
    {
        if (zoneCount > 0)
        {
            indoorSnapshot.TransitionTo(crossfadeTime);
        }
        else
        {
            outdoorSnapshot.TransitionTo(crossfadeTime);
        }
    }
}
