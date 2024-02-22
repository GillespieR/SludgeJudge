using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottleTypeManager : MonoBehaviour
{
    [SerializeField]
    ObjectControllerScript objectController;

    [SerializeField]
    AudioSource bottleAudio;

    [SerializeField]
    Button BottleModuleStartButton;

    [SerializeField]
    Text SampleTimeText;

    [SerializeField]
    Text DebugTextbox;

    //Audio clips
    [SerializeField]
    AudioClip IntroClip;

    [SerializeField]
    AudioClip GlassAudioClip;

    [SerializeField]
    AudioClip PlasticAudioClip;

    [SerializeField]
    AudioClip AmberAudioClip;

    [SerializeField]
    AudioClip AllBottlesSelectedClip;

    //lerp variables
    private bool isLerping = true;
    private float timeStartedLerping;
    private bool notStartedLerpingYet = true;
    public float timeTakenDuringLerp = 1f;
    private float timeSinceStarted;
    private float percentageComplete;

    RaycastHit bottlehit;
    RaycastHit bottleCheckHit;


    public bool canInstantiate = false;

    //for checking if user has selected a bottle type
    private bool glassBottleSelected = false;
    private bool plasticBottleSelected = false;
    private bool amberBottleSelected = false;

    //for the OutlineShaderGlow coroutine
    public Color endColor;
    public Color beginColor;

    public void BeginBottleTypeStory()
    {
        StartCoroutine(BottleTypeStory());
    }
    private IEnumerator BottleTypeStory()
    {
        BottleModuleStartButton.gameObject.SetActive(false);
        bottleAudio.clip = IntroClip;
        bottleAudio.Play();

        while (true)
        {
            if (!bottleAudio.isPlaying)
            {
                canInstantiate = true;
                DebugTextbox.text = " can instantiate is true now ";
                break;
            }
            yield return null;
        }
        while (true)
        {
            //DebugTextbox.text = "Inside second event";
            if (objectController.objectInstantiated)
            {
                if (!bottleAudio.isPlaying)
                {
                    if (!glassBottleSelected || !plasticBottleSelected || !amberBottleSelected)
                    {
                        DebugTextbox.text = "Inside bottles boolean check";
                        DebugTextbox.text = "GB: " + glassBottleSelected + " PB: " + plasticBottleSelected + "AB: " + amberBottleSelected;
                        BottleEventCheck();
                    }
                    else
                    {
                        bottleAudio.clip = AllBottlesSelectedClip;
                        bottleAudio.Play();
                        DebugTextbox.text = "All bottles selected!";
                        break;
                    }
                }
            }
           
            
            yield return null;
        }
        yield return null;
    }

    private bool NextEventCheck(string objectToHitTag)
    {

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out bottlehit))
            {
                DebugTextbox.text = "collider hit is: " + bottlehit.collider.tag;
                if (!bottleAudio.isPlaying && bottlehit.collider.tag == objectToHitTag)
                {
                    //debugTextbox.text = "Hit successful!";
                    //debugTextbox.text = "collider hit is: " + grabhit.collider.tag;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private void BottleEventCheck()
    {

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out bottleCheckHit))
            {
                DebugTextbox.text = "collider hit in bottleEventCheck is: " + bottleCheckHit.collider.tag;
                switch (bottleCheckHit.collider.tag)
                {
                    case "glass":
                        DebugTextbox.text = "hit glass bottle!";
                        bottleAudio.clip = GlassAudioClip;
                        bottleAudio.Play();
                        glassBottleSelected = true;
                        break;
                    case "plastic":
                        DebugTextbox.text = "hit plastic bottle!";
                        bottleAudio.clip = PlasticAudioClip;
                        bottleAudio.Play();
                        plasticBottleSelected = true;
                        break;
                    case "amber":
                        DebugTextbox.text = "hit amber bottle!";
                        bottleAudio.clip = AmberAudioClip;
                        bottleAudio.Play();
                        amberBottleSelected = true;
                        break;
                    default:
                        DebugTextbox.text = "Not a bottle!";
                        break;
                }

            }
            
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
