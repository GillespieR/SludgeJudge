using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompositeSamplerManager : MonoBehaviour
{
    [SerializeField]
    AudioSource CompositeSampleAudio;

    [SerializeField]
    ObjectControllerScript objectControllerScript;

    [SerializeField]
    Text debugTextbox;

    [SerializeField]
    Button CompositeSampleStartButton;

    [SerializeField]
    PauseMenu pauseMenuFunctions;

    [SerializeField]
    Text SampleTimeTextbox;

    //Audio clips, in order
    [SerializeField]
    AudioClip IntroClip;
    [SerializeField]
    AudioClip OpenCompositor1Audio;
    [SerializeField]
    AudioClip AddIceAudio;
    [SerializeField]
    AudioClip CloseCompositor1Audio;
    [SerializeField]
    AudioClip OpenControllerAudio;
    [SerializeField]
    AudioClip PlaceHoseAudio;
    [SerializeField]
    AudioClip RemoveHoseAudio;
    [SerializeField]
    AudioClip CoverOnControllerClip;
    [SerializeField]
    AudioClip OpenCompositor2Clip;
    [SerializeField]
    AudioClip CapOnSampleClip;
    [SerializeField]
    AudioClip RemoveAndShakeClip;
    [SerializeField]
    AudioClip CloseCompositor2Clip;
    [SerializeField]
    AudioClip CloseManholeClip;
    [SerializeField]
    AudioClip OuttroClip;

    [SerializeField]
    GameObject[] pauseMenuObjects;
    
    

    RaycastHit grabhit;

    public bool canInstantiate = false;

    public Color endColor;
    public Color beginColor;

    //lerp variables
    private bool isLerping = true;
    private float timeStartedLerping;
    private bool notStartedLerpingYet = true;
    public float timeTakenDuringLerp = 1f;
    private float timeSinceStarted;
    private float percentageComplete;

    //variable for emission lerp
    public float shaderSpeed;

    //outline shader material
    [SerializeField]
    Material CompOutlineMat;
    [SerializeField]
    Material ManholeHighlightMat;
    [SerializeField]
    Material IcePackHighlightMat;
    [SerializeField]
    Material ControlPanelHighlightMat;
    [SerializeField]
    Material HoseOutlineMat;
    [SerializeField]
    Material manholeOriginalMat;
    [SerializeField]
    Material compTopOriginalMat;
    [SerializeField]
    Material icePackOriginalMat;
    [SerializeField]
    Material originalHoseMat;
    [SerializeField]
    Material originalControllerMat;
    [SerializeField]
    Material capOriginalMat;
    [SerializeField]
    Material originalBottleMat;
    [SerializeField]
    Material compTopTransMat;
    [SerializeField]
    Material controlPanelTransMat;

    private IEnumerator CompositeSampleStory()
    {
        CompositeSampleStartButton.gameObject.SetActive(false);
        CompositeSampleAudio.clip = IntroClip;
        CompositeSampleAudio.Play();
        while (true)
        {
            if (!CompositeSampleAudio.isPlaying)
            {
                canInstantiate = true;
                break;
            }
            yield return null;
        }
        //yield return new WaitForSeconds(2f);
        //StartCoroutine(EmissionGlow("Manhole_Cover_low"));
        while (true)
        {
            debugTextbox.text = "checking for object's existence...";
            //GameObject manhole = GameObject.Find("Manhole_Cover_low");
            if(objectControllerScript.objectInstantiated)
            {
                debugTextbox.text = "Found it!";
                yield return new WaitForSeconds(2f);
                break;
            }
            yield return null;
        }
        //Material manholeOriginalMat = GameObject.Find("Manhole_Cover_low").GetComponent<Renderer>().material;
        StartCoroutine(OutlineShaderGlow("Manhole_Cover_low",ManholeHighlightMat));
        while (true)
        {
            
            if (NextEventCheck("Manhole"))
            {
                
                
                    StopCoroutine(OutlineShaderGlow("Manhole_Cover_low", ManholeHighlightMat));
                    GameObject.Find("Manhole_Cover_low").GetComponent<Renderer>().material = manholeOriginalMat;
                    break;
                
                
            }
            yield return null;
        }
        debugTextbox.text = "broke out of loop!";
        GameObject compPrefab = GameObject.Find("Environment");
        Animator compPrefabAnim = compPrefab.GetComponentInParent<Animator>();
        debugTextbox.text = "GameObject.Find found: " + compPrefab.name;
        //Animator compPrefabAnim = compPrefab.GetComponent<Animator>();
        compPrefabAnim.SetBool("OpenManhole", true);
        debugTextbox.text = "Set bool successfully!";
        //Material compTopOriginalMat = GameObject.Find("Top_low").GetComponent<Renderer>().material;
        //Material originalHoseMat = GameObject.Find("Effluent_Hose").GetComponent<MeshRenderer>().material;
        StartCoroutine(OutlineShaderGlow("Top_low",CompOutlineMat));
        CompositeSampleAudio.clip = OpenCompositor1Audio;
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.manholeUncovered)
            {
                if (NextEventCheck("CompositorTop"))
                {
                    StopCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
                    GameObject.Find("Top_low").GetComponent<Renderer>().material = compTopOriginalMat;
                    break;
                }
            }
            
            yield return null;
        }
        compPrefabAnim.SetBool("OpenManhole", false);
        compPrefabAnim.SetBool("OpenCompositor1", true);
        //StartCoroutine(EmissionGlow("Ice_Pack"));
        //Material icePackOriginalMat = GameObject.Find("Ice_Pack").GetComponent<Renderer>().material;
        StartCoroutine(OutlineShaderGlow("Ice_Pack", IcePackHighlightMat));
        StartCoroutine(OutlineShaderGlow("Ice_Pack001", IcePackHighlightMat));
        StartCoroutine(OutlineShaderGlow("Ice_Pack002", IcePackHighlightMat));
        StartCoroutine(OutlineShaderGlow("Ice_Pack003", IcePackHighlightMat));
        CompositeSampleAudio.clip = AddIceAudio;
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.compositorOpened1)
            {
                if (NextEventCheck("IcePack"))
                {
                    StopCoroutine(OutlineShaderGlow("Ice_Pack", IcePackHighlightMat));
                    StopCoroutine(OutlineShaderGlow("Ice_Pack001", IcePackHighlightMat));
                    StopCoroutine(OutlineShaderGlow("Ice_Pack002", IcePackHighlightMat));
                    StopCoroutine(OutlineShaderGlow("Ice_Pack003", IcePackHighlightMat));
                    GameObject.Find("Ice_Pack").GetComponent<Renderer>().material = icePackOriginalMat;
                    GameObject.Find("Ice_Pack001").GetComponent<Renderer>().material = icePackOriginalMat;
                    GameObject.Find("Ice_Pack002").GetComponent<Renderer>().material = icePackOriginalMat;
                    GameObject.Find("Ice_Pack003").GetComponent<Renderer>().material = icePackOriginalMat;
                    break;
                }
            }
            
            yield return null;
        }
        compPrefabAnim.SetBool("OpenCompositor1", false);
        compPrefabAnim.SetBool("AddIce", true);
        //StartCoroutine(EmissionGlow("Top_low"));
        StartCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
        CompositeSampleAudio.clip = CloseCompositor1Audio;
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.iceAdded)
            {
                if (NextEventCheck("CompositorTop"))
                {

                    StopCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
                    GameObject.Find("Top_low").GetComponent<Renderer>().material = compTopOriginalMat;
                    break;
                }
            }           
            
           
            yield return null;
        }
        debugTextbox.text = "Closing compositor";
        compPrefabAnim.SetBool("AddIce", false);
        compPrefabAnim.SetBool("CloseCompositor1", true);
        CompositeSampleAudio.clip = OpenControllerAudio;
        //StartCoroutine("Control_Panel_low");
        //StartCoroutine(OutlineShaderGlow("Control_Panel_low", ControlPanelHighlightMat));
        //yield return new WaitForSeconds(2f);
        StartCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.closingCompositor1)
            {
                if (NextEventCheck("CompositorTop"))
                {
                    StopCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
                    GameObject.Find("Top_low").GetComponent<Renderer>().material = compTopOriginalMat;
                    break;
                }
            }
            
            yield return null;
        }
        debugTextbox.text = "Taking cover off controller";
        compPrefabAnim.SetBool("CloseCompositor1", false);
        compPrefabAnim.SetBool("CoverOffController", true);
        CompositeSampleAudio.clip = PlaceHoseAudio;
        
        CompositeSampleAudio.Play();
        debugTextbox.text = "Beginning effluent hose coroutine";
        //StartCoroutine(OutlineShaderGlow("Effluent_Hose",HoseOutlineMat));
        
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CoverOffAnimationOver)
            {
                debugTextbox.text = "Used event to delay highlight!";
                StartCoroutine(FadePart("Control_Panel_low", controlPanelTransMat));
                StartCoroutine(OutlineShaderGlow("Effluent_Hose", HoseOutlineMat));
                debugTextbox.text = "Effluent Hose coroutine should be running";
                break;
            }
            yield return null;
        }
        
        
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CoverOffAnimationOver)
            {
                if (NextEventCheck("Hose"))
                {
                    //StopCoroutine(OutlineShaderGlow("Effluent_Hose", HoseOutlineMat));
                    //GameObject.Find("Effluent_Hose").GetComponent<MeshRenderer>().material = originalHoseMat;
                    break;
                }
            }
            

            yield return null;
        }
        compPrefabAnim.SetBool("CoverOffController", false);
        compPrefabAnim.SetBool("PlaceHose", true);
        CompositeSampleAudio.clip = RemoveHoseAudio;
        //StartCoroutine(EmissionGlow("Effluent_Hose"));
        //yield return new WaitForSeconds(3f);
        debugTextbox.text = "Hose placed and clip loaded";
        GameObject compTop = GameObject.Find("Control_Panel_low");
        compTop.GetComponent<Renderer>().material = originalControllerMat;
        debugTextbox.text = "beginning second effluent hose coroutine";
        StartCoroutine(OutlineShaderGlow("Effluent_Hose", HoseOutlineMat));
        debugTextbox.text = "second effluent hose coroutine should be running";
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.hoseUnfurled)
            {
                if (NextEventCheck("Hose"))
                {
                    SampleTimeTextbox.text = "10 AM";
                    yield return new WaitForSeconds(2f);
                    SampleTimeTextbox.text = "12 PM";
                    yield return new WaitForSeconds(2f);
                    SampleTimeTextbox.text = "2 PM";
                    debugTextbox.text = "ending effluent hose coroutine";
                    //StopCoroutine(OutlineShaderGlow("Effluent_Hose", HoseOutlineMat));
                    //GameObject.Find("Effluent_Hose").GetComponent<MeshRenderer>().material = originalHoseMat;
                    break;
                }
            }
            
            yield return null;
        }
        compPrefabAnim.SetBool("PlaceHose", false);
        compPrefabAnim.SetBool("RemoveHose", true);
        CompositeSampleAudio.clip = CoverOnControllerClip;
        //StartCoroutine(EmissionGlow("Control_Panel_low"));
        
        StartCoroutine(OutlineShaderGlow("Control_Panel_low", ControlPanelHighlightMat));
        CompositeSampleAudio.Play();
        while (true)
        {
            if (NextEventCheck("Controller"))
            {
                AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
                if (animFunctions.HoseDone)
                {
                    debugTextbox.text = "hose animation is done, stopping controller coroutine";
                    StopCoroutine(OutlineShaderGlow("Control_Panel_low", ControlPanelHighlightMat));
                    GameObject.Find("Control_Panel_low").GetComponent<MeshRenderer>().material = originalControllerMat;
                    break;
                }
                
            }
            yield return null;
        }
        compPrefabAnim.SetBool("RemoveHose", false);
        compPrefabAnim.SetBool("CoverOnController", true);
        CompositeSampleAudio.clip = OpenCompositor2Clip;
        //StartCoroutine(EmissionGlow("Top_low"));
        StartCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.coverOnController2)
            {
                if (NextEventCheck("CompositorTop"))
                {
                    debugTextbox.text = "stopping compositortop outline coroutine";
                    StopCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
                    GameObject.Find("Top_low").GetComponent<Renderer>().material = compTopOriginalMat;
                    break;
                }
            }
            
            yield return null;
        }
        compPrefabAnim.SetBool("CoverOnController", false);
        compPrefabAnim.SetBool("OpenCompositor2", true);
        debugTextbox.text = "Compositor should open to show battle";
        CompositeSampleAudio.clip = CapOnSampleClip;
        //StartCoroutine(EmissionGlow("Bottle_Cap"));
        
        StartCoroutine(OutlineShaderGlow("Bottle_Cap", CompOutlineMat));
        debugTextbox.text = "Bottle cap coroutine running";
        CompositeSampleAudio.Play();
        /*while (true)
        {
            //TODO: find way to select cap, change this from 'compositortop' back to 'cap'
            if (NextEventCheck("CompositorTop"))
            {
                break;
            }
            yield return null;
        }*/
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CompOpenAgain)
            {
                if (!CompositeSampleAudio.isPlaying)
                {
                    compPrefabAnim.SetBool("OpenCompositor2", false);
                    compPrefabAnim.SetBool("CapOnSample", true);
                    break;
                }
            }
            
            yield return null;
        }
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CapOnBottle)
            {
                debugTextbox.text = "Cap is on bottle!";
                StopCoroutine(OutlineShaderGlow("Bottle_Cap", HoseOutlineMat));
                GameObject.Find("Bottle_Cap").GetComponent<MeshRenderer>().material = capOriginalMat;
                break;
            }
            yield return null;
        }
        //compPrefabAnim.SetBool("OpenCompositor2", false);
        //compPrefabAnim.SetBool("CapOnSample", true);
        CompositeSampleAudio.clip = RemoveAndShakeClip;
        //StartCoroutine(EmissionGlow("Sample_Bottle_low"));
        
        StartCoroutine(OutlineShaderGlow("Sample_Bottle_low", CompOutlineMat));
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CapOnBottle)
            {
                if (NextEventCheck("SampleBottle"))
                {
                    StopCoroutine(OutlineShaderGlow("Sample_Bottle_low", CompOutlineMat));
                    GameObject.Find("Sample_Bottle_low").GetComponent<MeshRenderer>().material = originalBottleMat;
                    break;
                }
            }
            
            yield return null;
        }
        compPrefabAnim.SetBool("CapOnSample", false);
        compPrefabAnim.SetBool("RemoveAndShake", true);
        CompositeSampleAudio.clip = CloseCompositor2Clip;
        //StartCoroutine(EmissionGlow("Top_low"));
        StartCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.bottleShaken)
            {
                if (NextEventCheck("CompositorTop"))
                {
                    StopCoroutine(OutlineShaderGlow("Top_low", CompOutlineMat));
                    GameObject.Find("Top_low").GetComponent<MeshRenderer>().material = compTopOriginalMat;
                    break;
                }
            }
           
            yield return null;
        }
        compPrefabAnim.SetBool("RemoveAndShake", false);
        compPrefabAnim.SetBool("CloseCompositor2", true);
        CompositeSampleAudio.clip = CloseManholeClip;
        //StartCoroutine(EmissionGlow("Manhole_Cover_low"));
        StartCoroutine(OutlineShaderGlow("Manhole_Cover_low", ManholeHighlightMat));
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.topOnFinalTime)
            {
                if (NextEventCheck("Manhole"))
                {
                    StopCoroutine(OutlineShaderGlow("Manhole_Cover_low", ManholeHighlightMat));
                    GameObject.Find("Manhole_Cover_low").GetComponent<MeshRenderer>().material = manholeOriginalMat;
                    break;
                }
            }
            
            yield return null;
        }
        compPrefabAnim.SetBool("CloseCompositor2", false);
        compPrefabAnim.SetBool("CloseManhole", true);
        CompositeSampleAudio.clip = OuttroClip;
        CompositeSampleAudio.Play();
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.manholeReplacedFinal)
            {
                debugTextbox.text = "inside last animFunction check";
                if (!CompositeSampleAudio.isPlaying)
                {
                    //pauseMenuFunctions.Pause();
                    foreach(GameObject g in pauseMenuObjects)
                    {
                        if (g.name == "PauseButton")
                        {
                            g.SetActive(false);
                        }
                        else
                        {
                            g.SetActive(true);
                        }
                    }
                    
                    break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator CompositeSampleTesting()
    {
        CompositeSampleStartButton.gameObject.SetActive(false);
        CompositeSampleAudio.clip = IntroClip;
        CompositeSampleAudio.Play();
        while (true)
        {
            if (!CompositeSampleAudio.isPlaying)
            {
                canInstantiate = true;
                break;
            }
            yield return null;
        }
        while (true)
        {
            debugTextbox.text = "checking for object's existence...";
            //GameObject manhole = GameObject.Find("Manhole_Cover_low");
            if (objectControllerScript.objectInstantiated)
            {
                debugTextbox.text = "Found it!";
                yield return new WaitForSeconds(2f);
                break;
            }
            yield return null;
        }
        while (true)
        {

            if (NextEventCheck("Manhole"))
            {


                
                break;


            }
            yield return null;
        }
        debugTextbox.text = "broke out of loop!";
        GameObject compPrefab = GameObject.Find("Environment");
        Animator compPrefabAnim = compPrefab.GetComponentInParent<Animator>();
        debugTextbox.text = "GameObject.Find found: " + compPrefab.name;
        //Animator compPrefabAnim = compPrefab.GetComponent<Animator>();
        compPrefabAnim.SetBool("OpenManhole", true);
        debugTextbox.text = "Set bool successfully!";
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.manholeUncovered)
            {
                if (NextEventCheck("CompositorTop"))
                {
                   
                    break;
                }
            }

            yield return null;
        }
        compPrefabAnim.SetBool("OpenManhole", false);
        compPrefabAnim.SetBool("OpenCompositor1", true);
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.compositorOpened1)
            {
                if (NextEventCheck("IcePack"))
                {
                   
                    break;
                }
            }

            yield return null;
        }
        compPrefabAnim.SetBool("OpenCompositor1", false);
        compPrefabAnim.SetBool("AddIce", true);
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.iceAdded)
            {
                if (NextEventCheck("CompositorTop"))
                {

                    
                    break;
                }
            }


            yield return null;
        }
        debugTextbox.text = "Closing compositor";
        compPrefabAnim.SetBool("AddIce", false);
        compPrefabAnim.SetBool("CloseCompositor1", true);
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.closingCompositor1)
            {
                if (NextEventCheck("CompositorTop"))
                {
                    
                    break;
                }
            }

            yield return null;
        }
        debugTextbox.text = "Taking cover off controller";
        compPrefabAnim.SetBool("CloseCompositor1", false);
        compPrefabAnim.SetBool("CoverOffController", true);
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CoverOffAnimationOver)
            {
                StartCoroutine(FadePart("Control_Panel_low", controlPanelTransMat));
                if (NextEventCheck("Hose"))
                {
                    
                    break;
                }
            }


            yield return null;
        }
        compPrefabAnim.SetBool("CoverOffController", false);
        compPrefabAnim.SetBool("PlaceHose", true);
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.hoseUnfurled)
            {
                GameObject compTop = GameObject.Find("Control_Panel_low");
                compTop.GetComponent<Renderer>().material = originalControllerMat;
                if (NextEventCheck("Hose"))
                {
                    debugTextbox.text = "retracting hose";
                    
                    break;
                }
            }

            yield return null;
        }
        compPrefabAnim.SetBool("PlaceHose", false);
        compPrefabAnim.SetBool("RemoveHose", true);
        while (true)
        {
            if (NextEventCheck("Controller"))
            {
                AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
                if (animFunctions.HoseDone)
                {
                    debugTextbox.text = "hose animation is done, stopping controller coroutine";
                   
                    break;
                }

            }
            yield return null;
        }
        compPrefabAnim.SetBool("RemoveHose", false);
        compPrefabAnim.SetBool("CoverOnController", true);
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.coverOnController2)
            {
                if (NextEventCheck("CompositorTop"))
                {
                    
                    break;
                }
            }

            yield return null;
        }
        compPrefabAnim.SetBool("CoverOnController", false);
        compPrefabAnim.SetBool("OpenCompositor2", true);
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CompOpenAgain)
            {
                if (!CompositeSampleAudio.isPlaying)
                {
                    compPrefabAnim.SetBool("OpenCompositor2", false);
                    compPrefabAnim.SetBool("CapOnSample", true);
                    break;
                }
            }

            yield return null;
        }
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CapOnBottle)
            {
                debugTextbox.text = "Cap is on bottle!";
                
                break;
            }
            yield return null;
        }
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.CapOnBottle)
            {
                if (NextEventCheck("SampleBottle"))
                {
                   
                    break;
                }
            }

            yield return null;
        }
        compPrefabAnim.SetBool("CapOnSample", false);
        compPrefabAnim.SetBool("RemoveAndShake", true);
        while (true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.bottleShaken)
            {
                if (NextEventCheck("CompositorTop"))
                {
                   
                    break;
                }
            }

            yield return null;
        }
        compPrefabAnim.SetBool("RemoveAndShake", false);
        compPrefabAnim.SetBool("CloseCompositor2", true);
        while(true)
        {
            AnimFunctions animFunctions = GameObject.Find("Comp_Sampler").GetComponentInParent<AnimFunctions>();
            if (animFunctions.topOnFinalTime)
            {
                if (NextEventCheck("Manhole"))
                {
                    
                    break;
                }
            }

            yield return null;
        }
        compPrefabAnim.SetBool("CloseCompositor2", false);
        compPrefabAnim.SetBool("CloseManhole", true);
        yield return null;
    }

    public void BeginCompositeSampleModule()
    {
        StartCoroutine("CompositeSampleStory");
    }

    public void BeginCompositeSampleTesting()
    {
        StartCoroutine(CompositeSampleTesting());
    }

    private bool NextEventCheck(string objectToHitTag)
    {

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out grabhit))
            {
                //debugTextbox.text = "collider hit is: " + grabhit.collider.tag;
                if (!CompositeSampleAudio.isPlaying && grabhit.collider.tag == objectToHitTag)
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

    private IEnumerator EmissionGlow(string objectName)
    {
        GameObject theObject = GameObject.Find(objectName);
        //theObject.GetComponent<Renderer>().material.GetFloat("_Ambient");
        //debugTextbox.text = "found " + theObject.name + " at position " + theObject.transform;
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
                //debugTextbox.text = "currentvalue float is: " + currentValue;
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
                //debugTextbox.text = "currentvalue float is: " + currentValue;
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
                //debugTextbox.text = "currentvalue float is: " + currentValue;
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
                //debugTextbox.text = "currentvalue float is: " + currentValue;
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

    private IEnumerator FadePart(string objectName,Material transMat)
    {
        GameObject fadeObject = GameObject.Find(objectName);
        fadeObject.GetComponent<Renderer>().material = transMat;
        float alpha = fadeObject.GetComponent<Renderer>().material.color.a;
        Color fadeObjectColor = fadeObject.GetComponent<Renderer>().material.color;

        float transTime = 2.0f;
        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / transTime)
        {
            Color newColor = new Color(fadeObjectColor.r, fadeObjectColor.g, fadeObjectColor.b, Mathf.Lerp(alpha, 0.2f, t));
            fadeObject.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }

       
    }
    // Start is called before the first frame update
    void Start()
    {
        SampleTimeTextbox.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        /*debugTextbox.text = "script is running";
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out grabhit))
            {
                debugTextbox.text = "collider hit is: " + grabhit.collider.tag;
                

            }
            
        }*/
        
    }
}
