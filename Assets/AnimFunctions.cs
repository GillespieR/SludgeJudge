using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimFunctions : MonoBehaviour
{
    public bool CoverOffAnimationOver = false;
    public bool CapOnBottle = false;
    public bool TopBackOnComp = false;
    public bool HoseDone = false;
    public bool CompOpenAgain = false;
    public bool coverOnController2 = false;
    public bool manholeUncovered = false;
    public bool compositorOpened1 = false;
    public bool iceAdded = false;
    public bool closingCompositor1 = false;
    public bool hoseUnfurled = false;
    public bool bottleShaken = false;
    public bool topOnFinalTime = false;
    public bool manholeReplacedFinal = false;

    public void ManholeReplacedFinal()
    {
        manholeReplacedFinal = true;
    }
    public void TopOnFinalTime()
    {
        topOnFinalTime = true;
    }
    public void BottleShaken()
    {
        bottleShaken = true;
    }
    public void HoseUnfurled()
    {
        hoseUnfurled = true;
    }
    public void ClosingCompositor1() 
    {
        closingCompositor1 = true;
    }
    public void IceAdded()
    {
        iceAdded = true;
    }
    public void CompositorOpen1()
    {
        compositorOpened1 = true;
    }
    public void ManholeUncovered()
    {
        manholeUncovered = true;
        Text textField = GameObject.Find("DebugField").GetComponent<Text>();
        textField.text = "Manhole animation finished";
    }

    public void CoverOnControllerAgain()
    {
        coverOnController2 = true;
    }
    public void CompositorOpenAgain()
    {
        CompOpenAgain = true;
    }
    public void HoseWithdrawn()
    {
        HoseDone = true;
    }

    public void CoverOffFinished()
    {
        Text textField = GameObject.Find("DebugField").GetComponent<Text>();
        //textField.text = "Event fired off!";
        CoverOffAnimationOver = true;
        
    }

    public void CapShaderOff()
    {
        CapOnBottle = true;
    }

    public void CloseCompositorAfterIceFinished()
    {
        Text textField = GameObject.Find("DebugField").GetComponent<Text>();
        textField.text = "Ice is in, top is on!";
        TopBackOnComp = true;
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
