using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class T7DoTweenManager : MonoBehaviour
{
    public GameObject[] objectsSpawn; // Objects to enable with effects

    void Start()
    {
        SpawnObjectsWithEffect();
    }

    void SpawnObjectsWithEffect()
    {
        foreach (GameObject obj in objectsSpawn)
        {
            if (obj == null) continue;

            obj.SetActive(true); // Ensure it's active

            // Set initial scale to zero
            obj.transform.localScale = Vector3.zero;

            // Animate scale to normal with bounce effect
            obj.transform.DOScale(Vector3.one, 0.6f)
                .SetEase(Ease.OutBounce);

            // If it has a SpriteRenderer, apply fade-in effect
            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
            if (sprite)
            {
                sprite.DOFade(0, 0); // Start fully transparent
                sprite.DOFade(1, 0.6f); // Fade in smoothly
            }
        }
    }

    public void ChangeLocalYPosition()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        transform.DOLocalMoveY(1065, 0.8f) // Move smoothly to Y = 1065
            .SetEase(Ease.InOutSine); // Smooth start & stop effect

    }
}
