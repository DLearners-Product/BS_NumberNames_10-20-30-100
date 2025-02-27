using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
public class T9Drops : MonoBehaviour, IDropHandler
{
    private T9Manager REF_DragnDrop_V1;
    private Vector3 initialPosition, currentPosition;
    private float elapsedTime, desiredDuration = 0.2f;

    public AudioSource source;
    public AudioClip correctAnswer;

    public AudioClip wrongAnswer;

    public Button checkAnswer;

    public AudioClip coinSound;


    // public GameObject text;

    // public GameObject[] OneObj;


    //  public Text counter;

    // private int oneObjIndex = 0; // Track the current index for OneObj array
    // private int twoObjIndex = 0; // Track the current index for TwoObj array

    public int scoreCheck = 30;


    void Start()
    {
        REF_DragnDrop_V1 = FindObjectOfType<T9Manager>();
        initialPosition = transform.position;
    }


    public void OnDrop(PointerEventData eventData)
    {
        Drag_V1 drag = eventData.pointerDrag.GetComponent<Drag_V1>();

        if (drag.CompareTag(gameObject.tag)) // Comparing tags instead of names
        {
            drag.isDropped = true;
            StartCoroutine(IENUM_LerpTransform(drag.rectTransform, drag.rectTransform.anchoredPosition, GetComponent<RectTransform>().anchoredPosition));

            Debug.Log("correctAnswer");

            // Audio Playback
            source.clip = coinSound;
            source.Play();

            // Append the text from drag child to this GameObject's child text
            Text dragChildText = drag.GetComponentInChildren<Text>(); // Get the Text from drag child
            Text thisChildText = GetComponentInChildren<Text>();      // Get the Text from this GameObject's child

            if (dragChildText != null && thisChildText != null)
            {
                thisChildText.text += dragChildText.text; // Append the drag text to this object's text
            }

            // Start Coroutine to change color to green and then reset
            StartCoroutine(ChangeColorForAWhile());

            // Disable this script
            //   this.enabled = false;
        }
        else
        {

        }
    }

    // Coroutine to change color for 1 second and reset
    IEnumerator ChangeColorForAWhile()
    {
        Image thisImage = GetComponent<Image>(); // Assuming this GameObject has an Image component
        if (thisImage != null)
        {
            Color originalColor = thisImage.color; // Store the original color

            Color greenColor;
            if (ColorUtility.TryParseHtmlString("#1AEA00", out greenColor))
            {
                thisImage.color = greenColor; // Change to green
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second

            thisImage.color = originalColor; // Revert back to original color
        }
    }


    IEnumerator IENUM_LerpTransform(RectTransform obj, Vector3 currentPosition, Vector3 targetPosition)
    {
        while (elapsedTime < desiredDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            obj.anchoredPosition = Vector3.Lerp(currentPosition, targetPosition, percentageComplete);
            yield return null;
        }

        // Setting parent and deactivating object
        obj.gameObject.SetActive(false);

        // Update score
        Text textComponent = this.gameObject.transform.GetChild(0).GetComponent<Text>();
        int currentScore = int.Parse(textComponent.text);
        currentScore += 10;
        textComponent.text = currentScore.ToString();

        // Play particle effect
        this.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Play();

        // Wait for 1 second


        // Wait for 0.5 seconds before reactivating the object
        yield return new WaitForSeconds(0.5f);

        // Reset position and reactivate the object
        obj.gameObject.SetActive(true);
        obj.anchoredPosition = transform.position; // Reset position to initial state

        // Reset elapsed time
        elapsedTime = 0f;
    }

    public void CheckAnswer()
    {
        // Get the text component where the score is stored
        Text textComponent = this.gameObject.transform.GetChild(0).GetComponent<Text>();

        // Parse the text to an integer
        int currentScore;
        if (int.TryParse(textComponent.text, out currentScore))
        {
            // Compare the score with scoreCheck
            if (currentScore == scoreCheck)
            {
                Debug.Log("Correct");
                source.clip = correctAnswer;
                source.Play();
                checkAnswer.gameObject.GetComponent<Button>().interactable = false;
                this.enabled = false; // Disable this script
                                      // Fancy Camera Shake Effect (Correct Answer)
                Camera.main.transform.DOShakeRotation(0.5f, strength: new Vector3(0, 0, 10), vibrato: 8, randomness: 50)
                    .OnComplete(() => Camera.main.transform.DORotate(Vector3.zero, 0.2f)); // Reset rotation

                Camera.main.transform.DOShakeScale(0.5f, strength: new Vector3(0.2f, 0.2f, 0), vibrato: 6, randomness: 20);

                // Optional: Add a smooth zoom-in and out effect for extra impact
                Camera.main.DOOrthoSize(Camera.main.orthographicSize * 0.9f, 0.2f).SetLoops(2, LoopType.Yoyo);
            }
            else
            {
                Debug.Log("Wrong");
                textComponent.text = "0";
                source.clip = wrongAnswer;
                source.Play();
                Camera.main.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90);

            }
        }
        else
        {
            Debug.LogError("Failed to parse score from text component.");
        }
    }
}
