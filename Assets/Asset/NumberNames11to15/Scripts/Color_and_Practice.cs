using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Color_and_Practice : MonoBehaviour
{

    #region =======================================user input=======================================
    public Color32[] CLRA_Paint;
    #endregion


    #region =======================================unity reference variables=======================================
    [Header("AUDIO---------------------------------------------------------")]
    [SerializeField] private AudioSource AS_Voice;
    [SerializeField] private AudioSource AS_SFX;
    [SerializeField] private AudioClip[] ACA_NumberNames;
    [SerializeField] private AudioClip AC_ColorPicked;
    [SerializeField] private AudioClip AC_ColorApplied;
    [SerializeField] private AudioClip AC_Intro;
   // [SerializeField] private AudioClip AC_131415;


    [Header("IMAGE---------------------------------------------------------")]
    [SerializeField] private Image IMG_BG;


    [Header("GAME OBJECT---------------------------------------------------------")]
    [SerializeField] private GameObject[] GA_Objects;
    [SerializeField] private GameObject G_LoadingNext;
    [SerializeField] private GameObject G_TransparentScreen;
    [SerializeField] private GameObject G_BlackScreen;
    #endregion


    #region =======================================local variables=======================================	
    private int I_CurrentColorIndex, I_LetterCount = 0, I_CurrentIndex;
    private float elapsedTime, desiredDuration = 1f;
    private Color32 CLR_Current = new Color32(200, 200, 200, 255), CLR_Target;
    private List<int> IL_LettersCount = new List<int>() { 3,3,5,4,4,3,5,5,4,3,6,6,8,8,7,7,9,8,8,6 };
    private List<Image> IMGL_Letters = new List<Image>();
    #endregion


    void Start()
    {
        G_TransparentScreen.SetActive(true);
        StartCoroutine(IENUM_PlayVoice(AC_Intro, 0.75f));
    }


    public void THI_PickColor(int index)
    {
        I_CurrentColorIndex = index;
        CLR_Target = CLRA_Paint[index];
        PlaySFX(AC_ColorPicked);
        StartCoroutine(IENUM_LerpColor(IMG_BG));
    }


    public void THI_ApplyColor(Image img)
    {
        img.color = CLRA_Paint[I_CurrentColorIndex];
        PlaySFX(AC_ColorApplied);

        if (!IMGL_Letters.Contains(img))
        {
            I_LetterCount++;
            IMGL_Letters.Add(img);
            Debug.Log("letter count is " + I_LetterCount);
        }

        if (I_LetterCount == IL_LettersCount[I_CurrentIndex])
        {
            G_TransparentScreen.SetActive(true);
            I_LetterCount = 0;
            IMGL_Letters.Clear();
            PlaySFX(ACA_NumberNames[I_CurrentIndex]);
            G_LoadingNext.SetActive(true);
            Invoke("ShowNextNumber", 3f);
        }
    }


    private void ShowNextNumber()
    {
        G_TransparentScreen.SetActive(false);
        I_CurrentIndex++;

        // if (I_CurrentIndex == 2 || I_CurrentIndex == 3 || I_CurrentIndex == 4)
        // {
        //    // G_TransparentScreen.SetActive(true);
        //   //  StartCoroutine(IENUM_PlayVoice(AC_131415, 0.2f));
        // }

        if (I_CurrentIndex == 20)
        {
            G_LoadingNext.SetActive(false);
            G_BlackScreen.SetActive(true);
            return;
        }

        GA_Objects[I_CurrentIndex - 1].SetActive(false);
        GA_Objects[I_CurrentIndex].SetActive(true);
        G_LoadingNext.SetActive(false);
    }


    private void PlaySFX(AudioClip clip)
    {
        AS_SFX.clip = clip;
        AS_SFX.Play();
    }


    IEnumerator IENUM_PlayVoice(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);

        AS_Voice.clip = clip;
        AS_Voice.Play();

        yield return new WaitForSeconds(clip.length);
        G_TransparentScreen.SetActive(false);
    }


    IEnumerator IENUM_LerpColor(Image img)
    {
        yield return new WaitForSeconds(0.5f);

        while (elapsedTime < desiredDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            img.color = Color.Lerp(CLR_Current, CLR_Target, percentageComplete);
            yield return null;
        }

        //resetting elapsed time back to zero
        elapsedTime = 0f;
        CLR_Current = CLR_Target;
    }



}
