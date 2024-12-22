using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private float doorDelayTime;
    [HideInInspector] public bool doorOpen = false;

    private Animator _doorAnimator;
    private AudioSource _doorAudioSource;
    private float _doorOpenTime;
    private bool _pauseInteraction;

    private void Awake()
    {
        _doorAudioSource = GetComponent<AudioSource>();
        _doorAnimator = GetComponent<Animator>();
        _doorOpenTime = _doorAnimator.GetCurrentAnimatorStateInfo(0).length + doorDelayTime;
    }

    public void PlayDoorAnimation()
    {
        if (!doorOpen && !_pauseInteraction)
        {
            _doorAnimator.Play("OpenDoor");
            _doorAudioSource.clip = openSound;
            doorOpen = true;
            _doorAudioSource.Play();
            StartCoroutine(PauseInteraction());
        }
        else if (doorOpen && !_pauseInteraction)
        {
            _doorAudioSource.clip = closeSound;
            _doorAnimator.Play("CloseDoor");
            doorOpen = false;
            _doorAudioSource.Play();
            StartCoroutine(PauseInteraction());
        }
    }

    private IEnumerator PauseInteraction()
    {
        _pauseInteraction = true;
        yield return new WaitForSeconds(_doorOpenTime);
        _pauseInteraction = false;
    }
}
