using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DestroyBall : MonoBehaviour {

    GameObject stick;
    public int id;
    Vector3 curPos;
    bool initialChange;
    public string type;
    // Use this for initialization
    void Start () {
        initialChange = false;
        stick = GameObject.Find("Tip");
        curPos = this.transform.position;
        type = GameObject.Find("Pool Balls").GetComponent<GenerateBalls>().ballAr[id].getType();
    }

    // Update is called once per frame
    void Update () {
        if(this.transform.position != curPos && initialChange == false)
        {
            curPos = this.transform.position;
            initialChange = true;
        }
        if(this.transform.position != curPos && initialChange == true)
        {
            curPos = this.transform.position;
            stick.GetComponent<CueStickMovement>().ballMoved();
        }
	}

    public void setId(int num)
    {
        id = num;
    }

    public int getId()
    {
        return id;
    }

    void OnTriggerEnter(Collider coll)
    {

        if (coll.transform.name == "Pocket")
        {
            stick.GetComponent<CueStickMovement>().ballSinked = true;
            stick.GetComponent<CueStickMovement>().ballDestroyed(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
