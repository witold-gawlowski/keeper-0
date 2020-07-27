using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public DragScript dragScript;
    float timer;
    public float tutorialAnimationInterval;
    public GameObject animationFromObject;
    public GameObject animationToObject;
    public GameObject handSprite;
    public float animationLength = 0.8f;
    public bool tutorialEnabled;

    public void Init()
    {
        timer = 0;
        PlayTutorialAnimation();
        tutorialEnabled = true;
    }

    public void Disable()
    {
        tutorialEnabled = false;
        handSprite.SetActive(false);
        StopAllCoroutines();
    }

    void PlayTutorialAnimation()
    {
        StartCoroutine(AnimationCoroutine());
    }

    IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        handSprite.transform.position = animationFromObject.transform.position;
        handSprite.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        float startTime = Time.time;
        while (Time.time - startTime < animationLength)
        {
            handSprite.transform.position = Vector3.Lerp(animationFromObject.transform.position,
                animationToObject.transform.position, (Time.time-startTime) / animationLength);
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        handSprite.SetActive(false);
    }

    void Update()
    {
        if (tutorialEnabled)
        {
            timer += Time.deltaTime;
            if (timer > tutorialAnimationInterval)
            {
                if (!dragScript.firstBlockPlaced)
                {
                    PlayTutorialAnimation();
                }
                timer = 0;
            }
        }
    }
}
