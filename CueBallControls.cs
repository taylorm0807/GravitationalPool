using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBallControls : MonoBehaviour {

    bool collision;
    GameObject tip;
	// Use this for initialization
	void Start () {
        collision = true;
        tip = GameObject.Find("Tip");
    }
	
	// Update is called once per frame
	void Update () {
        
	}


    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.name == "Pocket")
        {
            tip.GetComponent<CueStickMovement>().SwitchTurns();
            print("You hit the cue ball in the pocket, so you scratched");
            tip.GetComponent<CueStickMovement>().ScratchBall();
        }
    }
}
