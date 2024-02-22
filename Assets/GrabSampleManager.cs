using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class GrabSampleManager : MonoBehaviour
{
    [SerializeField]
    private float shaderSpeed;
    //Script to manage grab sample module
    [SerializeField]
    AudioSource GrabSampleAudio;

    [SerializeField]
    ObjectControllerScript ObjectController;

    [SerializeField]
    Button GrabSampleStartButton;

    [SerializeField]
    AudioClip GrabIntroClip;

    [SerializeField]
    AudioClip AddPreservativeClip;

    [SerializeField]
    AudioClip FillBottle1Clip;

    [SerializeField]
    AudioClip CapBackOnBottle1Clip;

    [SerializeField]
    AudioClip MixPreservative1Clip;

    [SerializeField]
    AudioClip PutSample1InCoolerClip;

    [SerializeField]
    AudioClip LidOffSample2Clip;

    [SerializeField]
    AudioClip FillSample2WithDipperClip;

    [SerializeField]
    AudioClip AddPreservativeSample2Clip;

    [SerializeField]
    AudioClip CapBackOnBottle2Clip;

    [SerializeField]
    AudioClip MixPreservative2Clip;

    [SerializeField]
    AudioClip PutSample2InCoolerClip;

    [SerializeField]
    AudioClip FillBucketClip;

    [SerializeField]
    AudioClip OpenSample3Clip;

    [SerializeField]
    AudioClip FillSample3Clip;

    [SerializeField]
    AudioClip AddPreservativeSample3Clip;

    [SerializeField]
    AudioClip CapBackOnSample3Clip;

    [SerializeField]
    AudioClip MixSample3Clip;

    [SerializeField]
    AudioClip PutSample3InCoolerClip;

    [SerializeField]
    AudioClip OutroClip;

    [SerializeField]
    Text DebugTextbox;

    [SerializeField]
    Text SampleTimeText;
    //lerp variables
    private bool isLerping = true;
    private float timeStartedLerping;
    private bool notStartedLerpingYet = true;
    public float timeTakenDuringLerp = 1f;
    private float timeSinceStarted;
    private float percentageComplete;

    RaycastHit grabhit;
   

    public bool canInstantiate = false;

    //for the OutlineShaderGlow coroutine
    public Color endColor;
    public Color beginColor;

    //outline shader materials
    [SerializeField]
    Material SampleBottle1OriginalMat;
    [SerializeField]
    Material SampleBottle1HighlightMat;
    [SerializeField]
    Material PreservativeBottleOriginalMat;
    [SerializeField]
    Material PreservativeBottleHighlightMat;
    [SerializeField]
    Material SampleBottle2OriginalMat;
    [SerializeField]
    Material SampleBottle2HighlightMat;
    [SerializeField]
    Material SampleBottle3HighlightMat;
    [SerializeField]
    Material SampleBottle3OriginalMat;
    [SerializeField]
    Material DipperHighlightMat;
    [SerializeField]
    Material OriginalDipperMat;
    [SerializeField]
    Material BucketOriginalMat;
    [SerializeField]
    Material BucketHighlightMat;
    //for glow effect
    Color newColor;
    // Start is called before the first frame update
    void Start()
    {
        SampleTimeText.text = " ";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BeginGrabSamplingTesting()
    {
        StartCoroutine(GrabSampleTest());
    }

    public void BeginGrabSampleModule()
    {
        StartCoroutine("GrabSampleStory");
    }

    private IEnumerator GrabSampleStory()
    {
        //Turn off start button
        GrabSampleStartButton.gameObject.SetActive(false);
        //Play intro audio
        GrabSampleAudio.clip = GrabIntroClip;
        GrabSampleAudio.Play();
        while (true)
        {
            if (!GrabSampleAudio.isPlaying)
            {
                canInstantiate = true;
                break;
            }
            yield return null;
        }        
        
        while (true)
        {
            
           
            if (NextEventCheck("SampleBottle"))
            {
                
                break;
            }
            yield return null;
        }
        GameObject sampleBottle1 = GameObject.Find("Bottle_1");
        
        GameObject bottle = GameObject.Find("Combined_Assets_Unity10");
        Animator bottleAnim = bottle.GetComponent<Animator>();
        bottleAnim.SetBool("OpenBottle", true);
        //StartCoroutine(EmissionGlow("Bottle_1"));
        StartCoroutine(OutlineShaderGlow("Bottle_1", SampleBottle1HighlightMat));
        GrabSampleAudio.clip = FillBottle1Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponent<GrabAnimFunctions>();
            //DebugTextbox.text = "GrabAnimFunc is: " + grabAnimFunc.name;
            DebugTextbox.text = "LidOffSampleBottle1 is: " + grabAnimFunc.lidOffSampleBottle1;
            if (NextEventCheck("SampleBottle"))
            {
                if (grabAnimFunc.lidOffSampleBottle1)
                {
                    DebugTextbox.text = "inside second sample bottle check";

                    DebugTextbox.text = "inside first grabAnimFunc check";
                    StopCoroutine(OutlineShaderGlow("Bottle_1", SampleBottle1HighlightMat));
                    GameObject.Find("Bottle_1").GetComponent<Renderer>().material = SampleBottle1OriginalMat;
                    bottleAnim.SetBool("OpenBottle", false);
                    bottleAnim.SetBool("FillBottle", true);
                    SampleTimeText.text = "10 AM";
                    //bottleAnim.SetBool("Shake_Bottle", true);
                    //StartCoroutine("FillBottle");
                    break;
                }
                
                

                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Preservative_Bottle",PreservativeBottleHighlightMat));
        GrabSampleAudio.clip = AddPreservativeClip;
        GrabSampleAudio.Play();
       
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Preservative"))
            {
                if (grabAnimFunc.fillSampleBottle1)
                {
                    StopCoroutine(OutlineShaderGlow("Preservative_Bottle", PreservativeBottleHighlightMat));
                    GameObject.Find("Preservative_Bottle").GetComponent<Renderer>().material = PreservativeBottleOriginalMat;
                    bottleAnim.SetBool("AddPreservativeBottle1", true);
                    bottleAnim.SetBool("FillBottle", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_1", SampleBottle1HighlightMat));
        GrabSampleAudio.clip = CapBackOnBottle1Clip;
        GrabSampleAudio.Play();
        
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle"))
            {
                if (grabAnimFunc.preservativeAddedSample1)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_1", SampleBottle1HighlightMat));
                    GameObject.Find("Bottle_1").GetComponent<Renderer>().material = SampleBottle1OriginalMat;
                    bottleAnim.SetBool("CapBackOnBottle1", true);
                    bottleAnim.SetBool("AddPreservativeBottle1", false);
                    break;
                }
               
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_1", SampleBottle1HighlightMat));
        GrabSampleAudio.clip = MixPreservative1Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle"))
            {
                if (grabAnimFunc.lidBackOnSampleBottle1)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_1", SampleBottle1HighlightMat));
                    GameObject.Find("Bottle_1").GetComponent<Renderer>().material = SampleBottle1OriginalMat;
                    bottleAnim.SetBool("MixPreservative1", true);
                    bottleAnim.SetBool("CapBackOnBottle1", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_1", SampleBottle1HighlightMat));
        GrabSampleAudio.clip = PutSample1InCoolerClip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle"))
            {
                if (grabAnimFunc.sample1Mixed)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_1", SampleBottle1HighlightMat));
                    GameObject.Find("Bottle_1").GetComponent<Renderer>().material = SampleBottle1OriginalMat;
                    bottleAnim.SetBool("PutSample1InCooler", true);
                    bottleAnim.SetBool("MixPreservative1", false);
                    break;
                }
               
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_2",SampleBottle2HighlightMat));
        GrabSampleAudio.clip = LidOffSample2Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle2"))
            {
                if (grabAnimFunc.sampleBottle1InCooler)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_2", SampleBottle2HighlightMat));
                    GameObject.Find("Bottle_2").GetComponent<Renderer>().material = SampleBottle2OriginalMat;
                    bottleAnim.SetBool("LidOffSample2", true);
                    bottleAnim.SetBool("PutSample1InCooler", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Dip_Sampler",DipperHighlightMat));
        GrabSampleAudio.clip = FillSample2WithDipperClip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Dipper"))
            {
                if (grabAnimFunc.lidOffSampleBottle2)
                {
                    StopCoroutine(OutlineShaderGlow("Dip_Sampler", DipperHighlightMat));
                    GameObject.Find("Dip_Sampler").GetComponent<Renderer>().material = OriginalDipperMat;
                    bottleAnim.SetBool("FillSample2", true);
                    bottleAnim.SetBool("LidOffSample2", false);
                    break;
                }
                
            }
            yield return null;
        }
        SampleTimeText.text = "11 AM";
        StartCoroutine(OutlineShaderGlow("Preservative_Bottle", PreservativeBottleHighlightMat));
        GrabSampleAudio.clip = AddPreservativeSample2Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Preservative"))
            {
                if (grabAnimFunc.sampleBottle2Filled)
                {
                    StopCoroutine(OutlineShaderGlow("Preservative_Bottle", PreservativeBottleHighlightMat));
                    GameObject.Find("Preservative_Bottle").GetComponent<Renderer>().material = PreservativeBottleOriginalMat;
                    bottleAnim.SetBool("AddPreservativeSample2", true);
                    bottleAnim.SetBool("FillSample2", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_2", SampleBottle2HighlightMat));
        GrabSampleAudio.clip = CapBackOnBottle2Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle2"))
            {
                if (grabAnimFunc.preservativeAddedSample2)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_2", SampleBottle2HighlightMat));
                    GameObject.Find("Bottle_2").GetComponent<Renderer>().material = SampleBottle2OriginalMat;
                    bottleAnim.SetBool("CapBackOnBottle2", true);
                    bottleAnim.SetBool("AddPreservativeSample2", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_2", SampleBottle2HighlightMat));
        GrabSampleAudio.clip = MixPreservative2Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle2"))
            {
                if (grabAnimFunc.lidOnSampleBottle2)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_2", SampleBottle2HighlightMat));
                    GameObject.Find("Bottle_2").GetComponent<Renderer>().material = SampleBottle2OriginalMat;
                    bottleAnim.SetBool("MixPreservative2", true);
                    bottleAnim.SetBool("CapBackOnBottle2", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_2", SampleBottle2HighlightMat));
        GrabSampleAudio.clip = PutSample2InCoolerClip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle2"))
            {
                if (grabAnimFunc.sampleBottle2Mixed)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_2", SampleBottle2HighlightMat));
                    GameObject.Find("Bottle_2").GetComponent<Renderer>().material = SampleBottle2OriginalMat;
                    bottleAnim.SetBool("PutSample2InCooler", true);
                    bottleAnim.SetBool("MixPreservative2", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bucket",BucketHighlightMat));
        GrabSampleAudio.clip = FillBucketClip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Bucket"))
            {
                if (grabAnimFunc.sampleBottle2InCooler)
                {
                    StopCoroutine(OutlineShaderGlow("Bucket", BucketHighlightMat));
                    GameObject.Find("Bucket").GetComponent<Renderer>().material = BucketOriginalMat;
                    bottleAnim.SetBool("FillBucket", true);
                    bottleAnim.SetBool("PutSample2InCooler", false);
                    break;
                }
               
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_3",SampleBottle3HighlightMat));
        GrabSampleAudio.clip = OpenSample3Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.bucketIsFilled)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
                    GameObject.Find("Bottle_3").GetComponent<Renderer>().material = SampleBottle3OriginalMat;
                    bottleAnim.SetBool("OpenSample3", true);
                    bottleAnim.SetBool("FillBucket", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
        GrabSampleAudio.clip = FillSample3Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.lidOffSampleBottle3)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
                    GameObject.Find("Bottle_3").GetComponent<Renderer>().material = SampleBottle3OriginalMat;
                    bottleAnim.SetBool("FillSample3", true);
                    bottleAnim.SetBool("OpenSample3", false);
                    break;
                }
                
            }
            yield return null;
        }
        SampleTimeText.text = "12 pm";
        StartCoroutine(OutlineShaderGlow("Preservative_Bottle", PreservativeBottleHighlightMat));
        GrabSampleAudio.clip = AddPreservativeSample3Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Preservative"))
            {
                if (grabAnimFunc.sampleBottle3Filled)
                {
                    StopCoroutine(OutlineShaderGlow("Preservative_Bottle", PreservativeBottleHighlightMat));
                    GameObject.Find("Preservative_Bottle").GetComponent<Renderer>().material = PreservativeBottleOriginalMat;
                    bottleAnim.SetBool("AddPreservative3", true);
                    bottleAnim.SetBool("FillSample3", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
        GrabSampleAudio.clip = CapBackOnSample3Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.preservativeAdded3)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
                    GameObject.Find("Bottle_3").GetComponent<Renderer>().material = SampleBottle3OriginalMat;
                    bottleAnim.SetBool("CapBackOnBottle3", true);
                    bottleAnim.SetBool("AddPreservative3", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
        GrabSampleAudio.clip = MixSample3Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.lidOnSample3)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
                    GameObject.Find("Bottle_3").GetComponent<Renderer>().material = SampleBottle3OriginalMat;
                    bottleAnim.SetBool("MixSample3", true);
                    bottleAnim.SetBool("CapBackOnBottle3", false);
                    break;
                }
                
            }
            yield return null;
        }
        StartCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
        GrabSampleAudio.clip = PutSample3InCoolerClip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.sample3Mixed)
                {
                    StopCoroutine(OutlineShaderGlow("Bottle_3", SampleBottle3HighlightMat));
                    GameObject.Find("Bottle_3").GetComponent<Renderer>().material = SampleBottle3OriginalMat;
                    bottleAnim.SetBool("Sample3InCooler", true);
                    bottleAnim.SetBool("MixSample3", false);
                    break;
                }
                
            }
            yield return null;
        }
        GrabSampleAudio.clip = OutroClip;
        GrabSampleAudio.Play();

    }

    private IEnumerator GrabSampleTest()
    {
        //Turn off start button
        GrabSampleStartButton.gameObject.SetActive(false);
        //Play intro audio
        GrabSampleAudio.clip = GrabIntroClip;
        GrabSampleAudio.Play();
        while (true)
        {
            if (!GrabSampleAudio.isPlaying)
            {
                canInstantiate = true;
                break;
            }
            yield return null;
        }

        while (true)
        {


            if (NextEventCheck("SampleBottle"))
            {

                break;
            }
            yield return null;
        }
        GameObject sampleBottle1 = GameObject.Find("Bottle_1");

        GameObject bottle = GameObject.Find("Combined_Assets_Unity10");
        Animator bottleAnim = bottle.GetComponent<Animator>();
        bottleAnim.SetBool("OpenBottle", true);

        GrabSampleAudio.clip = FillBottle1Clip;
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponent<GrabAnimFunctions>();
            //DebugTextbox.text = "GrabAnimFunc is: " + grabAnimFunc.name;
            DebugTextbox.text = "LidOffSampleBottle1 is: " + grabAnimFunc.lidOffSampleBottle1;
            if (NextEventCheck("SampleBottle"))
            {
                if (grabAnimFunc.lidOffSampleBottle1)
                {
                    
                    bottleAnim.SetBool("OpenBottle", false);
                    bottleAnim.SetBool("FillBottle", true);
                    
                    break;
                }

            }
            yield return null;
        }
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Preservative"))
            {
                if (grabAnimFunc.fillSampleBottle1)
                {
                   
                    bottleAnim.SetBool("AddPreservativeBottle1", true);
                    bottleAnim.SetBool("FillBottle", false);
                    break;
                }

            }
            yield return null;
        }
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle"))
            {
                if (grabAnimFunc.preservativeAddedSample1)
                {
                   
                    bottleAnim.SetBool("CapBackOnBottle1", true);
                    bottleAnim.SetBool("AddPreservativeBottle1", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle"))
            {
                if (grabAnimFunc.lidBackOnSampleBottle1)
                {
                    
                    bottleAnim.SetBool("MixPreservative1", true);
                    bottleAnim.SetBool("CapBackOnBottle1", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle"))
            {
                if (grabAnimFunc.sample1Mixed)
                {
                    
                    bottleAnim.SetBool("PutSample1InCooler", true);
                    bottleAnim.SetBool("MixPreservative1", false);
                    break;
                }

            }
            yield return null;
        }
       
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle2"))
            {
                if (grabAnimFunc.sampleBottle1InCooler)
                {
                    
                    bottleAnim.SetBool("LidOffSample2", true);
                    bottleAnim.SetBool("PutSample1InCooler", false);
                    break;
                }

            }
            yield return null;
        }
       
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Dipper"))
            {
                if (grabAnimFunc.lidOffSampleBottle2)
                {
                    
                    bottleAnim.SetBool("FillSample2", true);
                    bottleAnim.SetBool("LidOffSample2", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Preservative"))
            {
                if (grabAnimFunc.sampleBottle2Filled)
                {
                    
                    bottleAnim.SetBool("AddPreservativeSample2", true);
                    bottleAnim.SetBool("FillSample2", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle2"))
            {
                if (grabAnimFunc.preservativeAddedSample2)
                {
                    
                    bottleAnim.SetBool("CapBackOnBottle2", true);
                    bottleAnim.SetBool("AddPreservativeSample2", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle2"))
            {
                if (grabAnimFunc.lidOnSampleBottle2)
                {
                    
                    bottleAnim.SetBool("MixPreservative2", true);
                    bottleAnim.SetBool("CapBackOnBottle2", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle2"))
            {
                if (grabAnimFunc.sampleBottle2Mixed)
                {
                    
                    bottleAnim.SetBool("PutSample2InCooler", true);
                    bottleAnim.SetBool("MixPreservative2", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Bucket"))
            {
                if (grabAnimFunc.sampleBottle2InCooler)
                {
                    
                    bottleAnim.SetBool("FillBucket", true);
                    bottleAnim.SetBool("PutSample2InCooler", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.bucketIsFilled)
                {
                    
                    bottleAnim.SetBool("OpenSample3", true);
                    bottleAnim.SetBool("FillBucket", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.lidOffSampleBottle3)
                {
                    
                    bottleAnim.SetBool("FillSample3", true);
                    bottleAnim.SetBool("OpenSample3", false);
                    break;
                }

            }
            yield return null;
        }
       
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("Preservative"))
            {
                if (grabAnimFunc.sampleBottle3Filled)
                {
                    
                    bottleAnim.SetBool("AddPreservative3", true);
                    bottleAnim.SetBool("FillSample3", false);
                    break;
                }

            }
            yield return null;
        }
       
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.preservativeAdded3)
                {
                    
                    bottleAnim.SetBool("CapBackOnBottle3", true);
                    bottleAnim.SetBool("AddPreservative3", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.lidOnSample3)
                {
                    
                    bottleAnim.SetBool("MixSample3", true);
                    bottleAnim.SetBool("CapBackOnBottle3", false);
                    break;
                }

            }
            yield return null;
        }
        
        GrabSampleAudio.Play();
        while (true)
        {
            GrabAnimFunctions grabAnimFunc = GameObject.Find("Combined_Assets_Unity10").GetComponentInParent<GrabAnimFunctions>();
            if (NextEventCheck("SampleBottle3"))
            {
                if (grabAnimFunc.sample3Mixed)
                {
                    
                    bottleAnim.SetBool("Sample3InCooler", true);
                    bottleAnim.SetBool("MixSample3", false);
                    break;
                }

            }
            yield return null;
        }
        GrabSampleAudio.clip = OutroClip;
        GrabSampleAudio.Play();

    }

    private bool NextEventCheck(string objectToHitTag)
    {
        
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out grabhit))
            {
                //DebugTextbox.text = "collider hit is: " + grabhit.collider.tag;
                if (!GrabSampleAudio.isPlaying && grabhit.collider.tag == objectToHitTag)
                {
                    DebugTextbox.text = "Hit successful!";
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

    private IEnumerator FillBottle()
    {
        
        GameObject fillBottlePos1 = GameObject.Find("FillBottlePos1");
        GameObject fillBottlePos2 = GameObject.Find("FillBottlePos2");
        GameObject sample_bottle = GameObject.Find("Sample_Bottle");
        Vector3 bottleBeginPos = GameObject.Find("Sample_Bottle").transform.position;
        while (true)
        {
            if (notStartedLerpingYet)
            {

                isLerping = true;
                timeStartedLerping = Time.time;
                notStartedLerpingYet = false;
            }

            if (isLerping)
            {
                //DebugTextbox.text = "Should be lerping!  Sample bottle x position is: "+sample_bottle.gameObject.transform.position.x;
                //DebugTextbox.text = "percentageComplete is: "+percentageComplete;
                timeSinceStarted = Time.time - timeStartedLerping;
                percentageComplete = timeSinceStarted / timeTakenDuringLerp;
                sample_bottle.gameObject.transform.position = Vector3.Lerp(bottleBeginPos, fillBottlePos1.gameObject.transform.position, percentageComplete);
                DebugTextbox.text = "after lerp position is: " + sample_bottle.gameObject.transform.position.x;
                //BottomValveLock.gameObject.transform.position = Vector3.Lerp(LockClosedStartPosition, LockClosedEndPosition, percentageComplete);
                //BottomValveLock.gameObject.transform.rotation = Quaternion.Lerp(LockClosedStartRotation, LockClosedEndRotation, percentageComplete);
                if (percentageComplete >= 1.0f)
                {
                    DebugTextbox.text = "lerping wrong!  Check percentageComplete";
                    isLerping = false;
                    notStartedLerpingYet = true;
                    sample_bottle.gameObject.transform.position = fillBottlePos1.gameObject.transform.position;
                    //openValveRoutineFinished = true;
                    break;
                }
            }

            yield return null;
        }
        
    }

    private IEnumerator FlashTextureTo(GameObject obj)
    {
        Material objMat = obj.GetComponent<Renderer>().material;
        Color beginColor = new Color(0f, 0f, 0f);
        Color endColor = new Color(210f, 192f, 27f);
        while (true)
        {
            if (notStartedLerpingYet)
            {

                isLerping = true;
                timeStartedLerping = Time.time;
                notStartedLerpingYet = false;
            }

            if (isLerping)
            {
                //DebugTextbox.text = "Should be lerping!  Sample bottle x position is: "+sample_bottle.gameObject.transform.position.x;
                //DebugTextbox.text = "percentageComplete is: "+percentageComplete;
                timeSinceStarted = Time.time - timeStartedLerping;
                percentageComplete = timeSinceStarted / timeTakenDuringLerp;
               objMat.SetColor("_EmissionColor", Color.Lerp(beginColor,endColor, percentageComplete));
                //DebugTextbox.text = "after lerp position is: " + sample_bottle.gameObject.transform.position.x;
                //BottomValveLock.gameObject.transform.position = Vector3.Lerp(LockClosedStartPosition, LockClosedEndPosition, percentageComplete);
                //BottomValveLock.gameObject.transform.rotation = Quaternion.Lerp(LockClosedStartRotation, LockClosedEndRotation, percentageComplete);
                if (percentageComplete >= 1.0f)
                {
                    DebugTextbox.text = "lerping wrong!  Check percentageComplete";
                    isLerping = false;
                    notStartedLerpingYet = true;
                    StartCoroutine(FlashTextureFrom(obj));
                    //objMat.SetColor("_EmissionColor", new Color(0f, 0f, 0f));
                    //openValveRoutineFinished = true;
                    break;
                }
            }

            yield return null;
        }
    }

    private IEnumerator FlashTextureFrom(GameObject obj)
    {
        Material objMat = obj.GetComponent<Renderer>().material;
        Color endColor = new Color(0f, 0f, 0f);
        Color beginColor = new Color(210f, 192f, 27f);
        while (true)
        {
            if (notStartedLerpingYet)
            {

                isLerping = true;
                timeStartedLerping = Time.time;
                notStartedLerpingYet = false;
            }

            if (isLerping)
            {
                //DebugTextbox.text = "Should be lerping!  Sample bottle x position is: "+sample_bottle.gameObject.transform.position.x;
                //DebugTextbox.text = "percentageComplete is: "+percentageComplete;
                timeSinceStarted = Time.time - timeStartedLerping;
                percentageComplete = timeSinceStarted / timeTakenDuringLerp;
                objMat.SetColor("_EmissionColor", Color.Lerp(beginColor, endColor, percentageComplete));
                //DebugTextbox.text = "after lerp position is: " + sample_bottle.gameObject.transform.position.x;
                //BottomValveLock.gameObject.transform.position = Vector3.Lerp(LockClosedStartPosition, LockClosedEndPosition, percentageComplete);
                //BottomValveLock.gameObject.transform.rotation = Quaternion.Lerp(LockClosedStartRotation, LockClosedEndRotation, percentageComplete);
                if (percentageComplete >= 1.0f)
                {
                    DebugTextbox.text = "lerping wrong!  Check percentageComplete";
                    isLerping = false;
                    notStartedLerpingYet = true;
                    //objMat.SetColor("_EmissionColor", new Color(0f, 0f, 0f));
                    //openValveRoutineFinished = true;
                    break;
                }
            }

            yield return null;
        }
    }

    private IEnumerator GlowShader(string objectName)
    {
        GameObject theObject = GameObject.Find(objectName);
        //theObject.GetComponent<Renderer>().material.GetFloat("_Ambient");
        DebugTextbox.text = "found " + theObject.name + " at position " + theObject.transform;
        theObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(250, 235, 34));
        theObject.GetComponent<Renderer>().material.SetFloat("_Rim", 3.16f);
        float beginValue = 0f;
        float endValue = 0.4f;
        float duration = 1000f;
        while (true)
        {
            
            if (notStartedLerpingYet)
            {

                isLerping = true;
                timeStartedLerping = Time.time;
                notStartedLerpingYet = false;
            }

            if (isLerping)
            {
                //float t = (Time.time - timeStartedLerping) / duration;
                timeSinceStarted = Time.time - timeStartedLerping;
                percentageComplete = timeSinceStarted / duration;
                float currentValue = Mathf.Lerp(beginValue, endValue, percentageComplete);
                theObject.GetComponent<Renderer>().material.SetFloat("_Ambient", currentValue);
                DebugTextbox.text = "running shader lerp";
                if (percentageComplete >= 1.0f)
                {
                    DebugTextbox.text = "Done with shader lerp!";
                    isLerping = false;
                    notStartedLerpingYet = true;
                   
                    break;
                }
            }

            yield return null;
        }

    }

    private IEnumerator ShaderGlowVers2(string objectName)
    {
        GameObject theObject = GameObject.Find(objectName);
        //theObject.GetComponent<Renderer>().material.GetFloat("_Ambient");
        DebugTextbox.text = "found " + theObject.name + " at position " + theObject.transform;
        theObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(250, 235, 34));
        theObject.GetComponent<Renderer>().material.SetFloat("_Rim", 3.48f);
        theObject.GetComponent<Renderer>().material.SetFloat("_Light",0);
        theObject.GetComponent<Renderer>().material.SetFloat("_Ambient", 0);
        float beginValue = 0f;
        float endValue = 0.75f;
        float beginTime = 0f;
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
                //float t = (Time.time - timeStartedLerping) / duration;
                float deltaT = (Time.time - beginTime) * shaderSpeed;
                //timeSinceStarted = Time.time - timeStartedLerping;
                //percentageComplete = timeSinceStarted / duration;
                float currentValue = Mathf.Lerp(beginValue, endValue, deltaT);
                
                theObject.GetComponent<Renderer>().material.SetFloat("_Ambient", currentValue);
                DebugTextbox.text = "ambient float is: "+ theObject.GetComponent<Renderer>().material.GetFloat("_Ambient").ToString();
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
                //float t = (Time.time - timeStartedLerping) / duration;
                float deltaT = (Time.time - beginTime) * shaderSpeed;
                //timeSinceStarted = Time.time - timeStartedLerping;
                //percentageComplete = timeSinceStarted / duration;
                float currentValue = Mathf.Lerp(endValue, beginValue, deltaT);

                theObject.GetComponent<Renderer>().material.SetFloat("_Ambient", currentValue);
                DebugTextbox.text = "ambient float is: " + theObject.GetComponent<Renderer>().material.GetFloat("_Ambient").ToString();
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
                //float t = (Time.time - timeStartedLerping) / duration;
                float deltaT = (Time.time - beginTime) * shaderSpeed;
                //timeSinceStarted = Time.time - timeStartedLerping;
                //percentageComplete = timeSinceStarted / duration;
                float currentValue = Mathf.Lerp(beginValue, endValue, deltaT);

                theObject.GetComponent<Renderer>().material.SetFloat("_Ambient", currentValue);
                DebugTextbox.text = "ambient float is: " + theObject.GetComponent<Renderer>().material.GetFloat("_Ambient").ToString();
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
                //float t = (Time.time - timeStartedLerping) / duration;
                float deltaT = (Time.time - beginTime) * shaderSpeed;
                //timeSinceStarted = Time.time - timeStartedLerping;
                //percentageComplete = timeSinceStarted / duration;
                float currentValue = Mathf.Lerp(endValue, beginValue, deltaT);

                theObject.GetComponent<Renderer>().material.SetFloat("_Ambient", currentValue);
                DebugTextbox.text = "ambient float is: " + theObject.GetComponent<Renderer>().material.GetFloat("_Ambient").ToString();
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

    private IEnumerator EmissionGlow(string objectName)
    {
        GameObject theObject = GameObject.Find(objectName);
        //theObject.GetComponent<Renderer>().material.GetFloat("_Ambient");
        DebugTextbox.text = "found " + theObject.name + " at position " + theObject.transform;
        Color endColor = new Color(147, 102, 0);
        Color beginColor = new Color(0, 0, 0);
        
        float beginTime = 0f;
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

                theObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", currentValue);
                DebugTextbox.text = "currentvalue float is: " +currentValue;
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

                theObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", currentValue);
                DebugTextbox.text = "currentvalue float is: " + currentValue;
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

                Color currentValue = Color.Lerp(beginColor, endColor, deltaT);

                theObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", currentValue);
                DebugTextbox.text = "currentvalue float is: " + currentValue;
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

                theObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", currentValue);
                DebugTextbox.text = "currentvalue float is: " + currentValue;
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
}
