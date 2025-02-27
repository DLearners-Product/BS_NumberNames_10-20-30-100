using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class T6Drop :MonoBehaviour, IDropHandler
{
    private T6Manager REF_DragnDrop_V1;
    private Vector3 initialPosition, currentPosition;
    private float elapsedTime, desiredDuration = 0.2f;

    public AudioSource source;
    public AudioClip correctAnswer;

    public AudioClip wrongAnswer;

     private Image objectImage;  // Cache the Image component
    private Color originalColor;

    // public GameObject text;

    // public GameObject[] OneObj;


    //  public Text counter;

    // private int oneObjIndex = 0; // Track the current index for OneObj array
    // private int twoObjIndex = 0; // Track the current index for TwoObj array


    void Start()
    {
        REF_DragnDrop_V1 = FindObjectOfType<T6Manager>();
        initialPosition = transform.position;
        objectImage = GetComponent<Image>(); // Cache the image component
        if (objectImage != null)
        {
            originalColor = objectImage.color; // Store the original color
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
    T6Drag drag = eventData.pointerDrag.GetComponent<T6Drag>();

        if (drag == null) return;

        // Correct Answer
        if (drag.name == gameObject.name)
        {
            drag.isDropped = true;
            StartCoroutine(IENUM_LerpTransform(drag.rectTransform, drag.rectTransform.anchoredPosition, GetComponent<RectTransform>().anchoredPosition));

            REF_DragnDrop_V1.CorrectAnswer(drag.name, transform.position);
            Debug.Log("Correct Answer");

            if (objectImage != null)
            {
                // Change color to green with a fancy effect
                objectImage.DOColor(Color.green, 0.5f).OnComplete(() =>
                {
                    objectImage.DOColor(originalColor, 0.5f);
                    transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
                });
            }

            source.clip = correctAnswer;
            source.Play();
        }
        // Wrong Answer
        else
        {
            REF_DragnDrop_V1.WrongAnswer(drag.name);
            Debug.Log("Wrong Answer");

            if (objectImage != null)
            {
                objectImage.DOColor(Color.red, 0.5f).OnComplete(() =>
                {
                    objectImage.DOColor(originalColor, 0.5f);
                });
            }

            // Shake the camera for the wrong answer
            Camera.main.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90);

            source.clip = wrongAnswer;
            source.Play();
        }
    }


    IEnumerator IENUM_LerpTransform(RectTransform obj, Vector3 currentPosition, Vector3 targetPosition)
    {
        while (elapsedTime < desiredDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            //  obj.anchoredPosition = Vector3.Lerp(currentPosition, targetPosition, percentageComplete);
            yield return null;
        }

        //setting parent
        // obj.transform.SetParent(transform);
        // this.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        obj.gameObject.SetActive(false);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
       // this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        // obj.transform.localPosition = Vector2.zero;

        //resetting elapsed time back to zero
        elapsedTime = 0f;
    }
}