using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class ObjectControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private GameObject ballToResize;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Slider heightSlider;
    [SerializeField]
    public Text debugField;
    [SerializeField]
    private TutorialManager tutScript;
    [SerializeField]
    private GrabSampleManager grabScript;
    [SerializeField]
    private CompositeSamplerManager compScript;
    [SerializeField]
    private RepSampleManager repSampleScript;
    [SerializeField]
    private BottleTypeManager bottleTypeScript;
    public bool objectInstantiated = false;
    public RaycastHit uhit;
    private float currSliderVal;
    private float prevSliderVal = 0;
    private float originalHeight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

        }
        else Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Touch touch = Input.GetTouch(0);
        if (Input.touchCount < 1 || touch.phase != TouchPhase.Began)
        {
            return;
        }
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;
        if (tutScript.canInstantiate||grabScript.canInstantiate||compScript.canInstantiate||repSampleScript.canInstantiate||bottleTypeScript.canInstantiate)
        {
            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                if (!objectInstantiated)
                {
                    Anchor anc = hit.Trackable.CreateAnchor(hit.Pose);
                    ballToResize = Object.Instantiate(prefab, hit.Pose.position, hit.Pose.rotation, anc.transform);
                    originalHeight = ballToResize.transform.position.y;
                    //to make lagoon face right way
                    ballToResize.gameObject.transform.rotation = new Quaternion(0f,180f,0f,0f);
                    //debugField.text = "Y rotation is: "+ballToResize.gameObject.transform.rotation.y;
                    objectInstantiated = true;
                }

            }
        }
        

            /*if (Input.touchCount >= 2)
            {
                Touch touch1 = Input.GetTouch(0);
                ballToResize.gameObject.transform.localScale = new Vector3(touch1.position.y, touch1.position.y, touch1.position.y);
            }*/


        }

    public void ResizeAsset()
    {

        float scale = slider.value;
        ballToResize.gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void RaiseLowerAsset()
    {
        
        
            currSliderVal = heightSlider.value;
            float assetHeight = ballToResize.gameObject.transform.position.y;
        float newAssetHeight = originalHeight + currSliderVal;
        //ballToResize.transform.Translate(new Vector3(0f, currSliderVal, 0f));
        ballToResize.transform.SetPositionAndRotation(new Vector3(ballToResize.transform.position.x, newAssetHeight, ballToResize.transform.position.z), new Quaternion(ballToResize.transform.rotation.x,ballToResize.transform.rotation.y,ballToResize.transform.rotation.z,ballToResize.transform.rotation.w));
        /*if (currSliderVal > prevSliderVal)
        {
            ballToResize.transform.Translate(new Vector3(0f, currSliderVal, 0f));
        }
        else if (currSliderVal < prevSliderVal)
        {
            ballToResize.transform.Translate(new Vector3(0f, -currSliderVal, 0f));
        }*/
        /*if (currSliderVal > prevSliderVal)
        {
            ballToResize.transform.position = new Vector3(ballToResize.transform.position.x,originalHeight+currSliderVal,ballToResize.transform.position.z);
        }
        else if (currSliderVal < prevSliderVal)
        {
            ballToResize.transform.position = new Vector3(ballToResize.transform.position.x, originalHeight - currSliderVal, ballToResize.transform.position.z);
        }*/

        debugField.text = assetHeight.ToString();
            prevSliderVal = currSliderVal;
        
        
    }
}
