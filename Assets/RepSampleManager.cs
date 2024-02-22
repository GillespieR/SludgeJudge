using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepSampleManager : MonoBehaviour
{
    [SerializeField]
    Button RepSampleStartButton;

    [SerializeField]
    AudioSource RepSampleAudio;

    [SerializeField]
    ObjectControllerScript ObjectController;

    [SerializeField]
    Text DebugTextbox;

    [SerializeField]
    Text TimeDisplay;

    //Audio clips
    [SerializeField]
    AudioClip RepIntroAudio;

    [SerializeField]
    AudioClip AgitSampleToCamClip;

    [SerializeField]
    AudioClip AgitSampleCappedClip;

    [SerializeField]
    AudioClip StillSampleAcquiredClip;

    [SerializeField]
    AudioClip StillSampleToCamClip;

    [SerializeField]
    AudioClip StillSampleCappedClip;

    [SerializeField]
    AudioClip BothSampleToCamClip;

    //for glow shader

    //glow shader materials
    [SerializeField]
    Material AgitBottleOriginalMat;
    [SerializeField]
    Material AgitBottleGlowMat;
    [SerializeField]
    Material StillBottleOriginalMat;
    [SerializeField]
    Material StillBottleGlowMat;

    //lerp variables
    public float shaderSpeed;
    private bool isLerping = true;
    private float timeStartedLerping;
    private bool notStartedLerpingYet = true;
    public float timeTakenDuringLerp = 1f;
    private float timeSinceStarted;
    private float percentageComplete;

    public Color endColor;
    public Color beginColor;

    RaycastHit repHit;

    Animator repSampleAnim;
    RepAnimFunctions repAnimFunc;

    public bool canInstantiate = false;

    private IEnumerator RepSampleStory()
    {
        RepSampleStartButton.gameObject.SetActive(false);

        RepSampleAudio.clip = RepIntroAudio;
        RepSampleAudio.Play();

        while (true)
        {
            if (!RepSampleAudio.isPlaying)
            {
                canInstantiate = true;
                DebugTextbox.text = " can instantiate is true now ";
                break;
            }
            yield return null;
        }
        while (true)
        {
            if (ObjectController.objectInstantiated)
            {
                repSampleAnim = GameObject.Find("Aggitation_VS_Still_4").GetComponent<Animator>();
                repAnimFunc = GameObject.Find("Aggitation_VS_Still_4").GetComponent<RepAnimFunctions>();
                DebugTextbox.text = "repAnimFunc is: "+repAnimFunc.AgitSampleAcq;
                break;
            }
            yield return null;
        }
        
        StartCoroutine(OutlineShaderGlow("Bottle_004", AgitBottleGlowMat));

        while (true)
        {
            if (NextEventCheck("AgitSample"))
            {
                
                    DebugTextbox.text = "repAnimFunc working";
                    StopCoroutine(OutlineShaderGlow("Bottle_004", AgitBottleGlowMat));
                    GameObject.Find("Bottle_004").GetComponent<MeshRenderer>().material = AgitBottleOriginalMat;
                    repSampleAnim.SetBool("AcquireAgitSample", true);
                    //DebugTextbox.text += "animator boolean set for agitsample";
                    break;
                
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_004", AgitBottleGlowMat));
        RepSampleAudio.clip = AgitSampleToCamClip;
        RepSampleAudio.Play();
        while (true)
        {
            if (repAnimFunc.AgitSampleAcq)
            {
                repSampleAnim.SetBool("AgitSampleToCam", true);
                break;
            }
            yield return null;
        }
        
        while (true)
        {
            if (NextEventCheck("AgitSample"))
            {
                if (repAnimFunc.AgitSampleToCamFinished)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_004", AgitBottleGlowMat));
                    GameObject.Find("Bottle_004").GetComponent<MeshRenderer>().material = AgitBottleOriginalMat;
                    repSampleAnim.SetBool("CapOnAgit", true);
                    repSampleAnim.SetBool("AcquireAgitSample", false);
                    repSampleAnim.SetBool("AgitSampleToCam", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_005", StillBottleGlowMat));
        RepSampleAudio.clip = AgitSampleCappedClip;
        RepSampleAudio.Play();
        while (true)
        {
            if (repAnimFunc.AgitSampleCappedDone)
            {
                if (NextEventCheck("StillSample"))
                {
                    StartCoroutine(OutlineShaderGlow("Bottle_005", StillBottleGlowMat));
                    GameObject.Find("Bottle_005").GetComponent<MeshRenderer>().material = StillBottleOriginalMat;
                    repSampleAnim.SetBool("AcquireStillSample", true);
                    repSampleAnim.SetBool("CapOnAgit", false);
                    break;
                }
            }
            
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_005", StillBottleGlowMat));
        RepSampleAudio.clip = StillSampleAcquiredClip;
        RepSampleAudio.Play();
        repSampleAnim.SetBool("StillSampleToCam",true);
        while (true)
        {
            if (repAnimFunc.StillSampleToCamDone)
            {
                if (NextEventCheck("StillSample"))
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_005", StillBottleGlowMat));
                    GameObject.Find("Bottle_005").GetComponent<MeshRenderer>().material = StillBottleOriginalMat;
                    repSampleAnim.SetBool("AcquireStillSample", false);
                    repSampleAnim.SetBool("StillSampleToCam", false);
                    repSampleAnim.SetBool("CapOnStill", true);
                    break;

                }
            }
            
            yield return null;
        }
        GameObject.Find("WaterMeshAgitBottle").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("WaterMeshSampleBottle").GetComponent<MeshRenderer>().enabled = true;
        RepSampleAudio.clip = StillSampleCappedClip;
        RepSampleAudio.Play();
        repSampleAnim.SetBool("BothSamplesCam", true);
    }

    public void StartRepSampleStory()
    {
        StartCoroutine(RepSampleStory());
    }

    private bool NextEventCheck(string objectToHitTag)
    {

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out repHit))
            {
                DebugTextbox.text = "collider hit is: " + repHit.collider.tag;
                if (!RepSampleAudio.isPlaying && repHit.collider.tag == objectToHitTag)
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


    private IEnumerator OutlineShaderGlow(string objectName, Material highlightMat)
    {
        bool endGlowCycle = false;
        GameObject theObject = GameObject.Find(objectName);
        //theObject.GetComponent<Renderer>().material.GetFloat("_Ambient");
        //debugTextbox.text = "found " + theObject.name + " at position " + theObject.transform;
        Material originalMat = theObject.GetComponent<Renderer>().material;

        theObject.GetComponent<Renderer>().material = highlightMat;

        //Color endColor = new Color(255, 245, 57);
        //Color beginColor = new Color(255, 255, 255);

        float beginTime = 0f;
        while (true)
        {
            while (true)
            {

                if (notStartedLerpingYet)
                {
                    beginTime = Time.time;
                    notStartedLerpingYet = false;
                    isLerping = true;

                }

                if (isLerping)
                {

                    float deltaT = (Time.time - beginTime) * shaderSpeed;

                    Color currentValue = Color.Lerp(beginColor, endColor, deltaT);

                    theObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", currentValue);
                    //debugTextbox.text = "currentvalue float is: " + currentValue;
                    //debugTextbox.text = "currentvalue float is: " + theObject.GetComponent<Renderer>().material.GetColor("_OutlineColor");
                    //DebugTextbox.text = "running shader lerp";
                    if (deltaT >= 1.0f)
                    {
                        //DebugTextbox.text = "Done with shader lerp!";
                        isLerping = false;
                        notStartedLerpingYet = true;

                        break;
                    }
                }

                yield return null;
            }
            while (true)
            {

                if (notStartedLerpingYet)
                {
                    beginTime = Time.time;
                    notStartedLerpingYet = false;
                    isLerping = true;

                }

                if (isLerping)
                {
                    float deltaT = (Time.time - beginTime) * shaderSpeed;

                    Color currentValue = Color.Lerp(endColor, beginColor, deltaT);

                    theObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", currentValue);
                    //debugTextbox.text = "currentvalue float is: " + currentValue;
                    //debugTextbox.text = "currentvalue float is: " + theObject.GetComponent<Renderer>().material.GetColor("_OutlineColor");
                    //DebugTextbox.text = "running shader lerp";
                    if (deltaT >= 1.0f)
                    {
                        //DebugTextbox.text = "Done with shader lerp!";
                        isLerping = false;
                        notStartedLerpingYet = true;

                        break;
                    }
                }

                yield return null;
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
