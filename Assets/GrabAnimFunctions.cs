using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabAnimFunctions : MonoBehaviour
{
    public bool lidOffSampleBottle1 = false;
    public bool fillSampleBottle1 = false;
    public bool preservativeAddedSample1 = false;
    public bool lidBackOnSampleBottle1 = false;
    public bool sample1Mixed = false;
    public bool sampleBottle1InCooler = false;
    public bool lidOffSampleBottle2 = false;
    public bool sampleBottle2Filled = false;
    public bool preservativeAddedSample2 = false;
    public bool lidOnSampleBottle2 = false;
    public bool sampleBottle2Mixed = false;
    public bool sampleBottle2InCooler = false;
    public bool bucketIsFilled = false;
    public bool lidOffSampleBottle3 = false;
    public bool sampleBottle3Filled = false;
    public bool preservativeAdded3 = false;
    public bool lidOnSample3 = false;
    public bool sample3Mixed = false;
    public bool sampleBottle3InCooler = false;
    public void LidOffSample1()
    {
        lidOffSampleBottle1 = true;
        Text textField = GameObject.Find("DebugField").GetComponent<Text>();
        textField.text += " Lid is off sample bottle 1!";
    }
    public void FillSample1()
    {
        fillSampleBottle1 = true;
    }
    public void AddedPreservative1()
    {
        preservativeAddedSample1 = true;
    }

    public void LidBackOn1()
    {
        lidBackOnSampleBottle1 = true;
    }

    public void Sample1Mix()
    {
        sample1Mixed = true;
    }

    public void Sample1InCooler()
    {
        sampleBottle1InCooler = true;
    }

    public void LidOff2()
    {
        lidOffSampleBottle2 = true;
    }

    public void FillSample2()
    {
        sampleBottle2Filled = true;
    }

    public void AddPreservative2()
    {
        preservativeAddedSample2 = true;
    }

    public void LidOnSample2()
    {
        lidOnSampleBottle2 = true;
    }

    public void MixSample2()
    {
        sampleBottle2Mixed = true;
    }

    public void Sample2InCooler()
    {
        sampleBottle2InCooler = true;
    }

    public void BucketFilled()
    {
        bucketIsFilled = true;
    }

    public void LidOff3()
    {
        lidOffSampleBottle3 = true;
    }

    public void FillSample3()
    {
        sampleBottle3Filled = true;
    }

    public void AddPreservative3()
    {
        preservativeAdded3 = true;
    }

    public void LidOn3()
    {
        lidOnSample3 = true;
    }

    public void MixSample3()
    {
        sample3Mixed = true;
    }

    public void SampleInCooler3()
    {
        sampleBottle3InCooler = true;
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
