using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepAnimFunctions : MonoBehaviour
{
    public bool AgitSampleAcq = false;
    public bool AgitSampleToCamFinished = false;
    public bool AgitSampleCappedDone = false;
    public bool StillSampleAcquisitionFinished = false;
    public bool StillSampleToCamDone = false;
    public bool StillSampleCappedDone = false;
    public bool BothSamplesToCamDone = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AgitSampleAcquired()
    {
        AgitSampleAcq = true;
    }

    public void AgitSampleToCam()
    {
        AgitSampleToCamFinished = true;
    }

    public void AgitSampleCapped()
    {
        AgitSampleCappedDone = true;
    }

    public void StillSampleAcquired()
    {
        StillSampleAcquisitionFinished = true;
    }

    public void StillSampleToCam()
    {
        StillSampleToCamDone = true;
    }

    public void StillSampleCapped()
    {
        StillSampleCappedDone = true;
    }

    public void BothSamplesToCam()
    {
        BothSamplesToCamDone = true;
    }


}
