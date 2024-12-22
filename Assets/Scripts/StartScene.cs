using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class StartScene : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Image blackScreenImage;
    [SerializeField] private Text blackScreenText1;
    [SerializeField] private Text blackScreenText2;
    [SerializeField] private Text hintText;
    [SerializeField] private float blackScreenDuration = 4f;
    [SerializeField] private float hintDuration = 14f;
    [SerializeField] private float fadingDuration = 3f;

    private bool screenTimerIsActive = true;
    private bool hintTimerIsActive = true;

    private void Start()
    {
        blackScreenImage.gameObject.SetActive(true);
        blackScreenText1.gameObject.SetActive(true);
        blackScreenText2.gameObject.SetActive(true);
        hintText.gameObject.SetActive(true);
        audioMixer.SetFloat("soundsVolume", -80f);
        StartCoroutine(FadeScreen());
    }

    private IEnumerator FadeScreen()
    {
        yield return new WaitForSeconds(blackScreenDuration);
        blackScreenImage.CrossFadeAlpha(0, fadingDuration, false);
        blackScreenText1.CrossFadeAlpha(0, fadingDuration, false);
        blackScreenText2.CrossFadeAlpha(0, fadingDuration, false);
        StartCoroutine(FadeAudio("soundsVolume", fadingDuration, 1f));
        yield return new WaitForSeconds(hintDuration);
        hintText.CrossFadeAlpha(0, fadingDuration, false);
    }

    private IEnumerator FadeAudio(string parameter, float duration, float targetVolume)
    {
        float currentTime = 0;
        audioMixer.GetFloat(parameter, out float currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(parameter, Mathf.Log10(newVol) * 20);
            yield return null;
        }
    }
}
