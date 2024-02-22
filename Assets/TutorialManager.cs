using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private ARController ARController;
    [SerializeField]
    private AudioSource TutorialAudio;
    [SerializeField]
    private AudioClip introClip;
    [SerializeField]
    private AudioClip resizeControlsClip;
    [SerializeField]
    private AudioClip movementControlsClip;
    [SerializeField]
    private AudioClip controlsInMenuClip;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private ObjectControllerScript objectController;
    [SerializeField]
    private Slider sizeSlider;
    [SerializeField]
    private Slider heightSlider;
    //so that you can only instantiate object once
    public bool canInstantiate = false;
    // Start is called before the first frame update
    void Start()
    {
        sizeSlider.gameObject.SetActive(false);
        heightSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginTutorial()
    {
        StartCoroutine("TutorialStory");
    }

    private IEnumerator TutorialStory()
    {
        //turn off tutorial button
        startButton.gameObject.SetActive(false);
        //play intro audio (asks user to place an asset)
        TutorialAudio.clip = introClip;
        TutorialAudio.Play();
        canInstantiate = true;
        
        while (true)
        {
            if (objectController.objectInstantiated&&!TutorialAudio.isPlaying)
            {
                break;
            }
            yield return null;
        }
        //Play audio explaining resize controls and asking user to use them
        TutorialAudio.clip = resizeControlsClip;
        TutorialAudio.Play();
        //Show resize slider
        sizeSlider.gameObject.SetActive(true);
        float originalSliderVal = sizeSlider.value;
        //wait for user to use slider
        while (true)
        {
            float currSliderVal = sizeSlider.value;
            if (currSliderVal != originalSliderVal && !TutorialAudio.isPlaying)
            {
                break;
            }
            yield return null;
        }
        //Play audio explaining movement controls and asking user to use them
        TutorialAudio.clip = movementControlsClip;
        TutorialAudio.Play();
        //show movement controls
        heightSlider.gameObject.SetActive(true);
        float originalHeightSliderVal = heightSlider.value;
        //wait for user to use controls
        while (true)
        {
            float currHeightSliderVal = heightSlider.value;
            if (originalHeightSliderVal != currHeightSliderVal && !TutorialAudio.isPlaying)
            {
                break;
            }
            yield return null;
        }
        //Play audio telling user controls will be in pause submenu
        TutorialAudio.clip = controlsInMenuClip;
        TutorialAudio.Play();
        yield return new WaitForSeconds(3);
        heightSlider.gameObject.SetActive(false);
        sizeSlider.gameObject.SetActive(false);
    }
}
