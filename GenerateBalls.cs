using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBalls : MonoBehaviour {

    const int numBalls = 15;
    string num;
    public float ang = 1.0f;
    public float drag = 1.0f;
    public float weight = 1.0f;
    public string attribute;
    public CueStickMovement.BallArray[] ballAr = new CueStickMovement.BallArray[numBalls];
    // Use this for initialization
    void Start () {
        StartCoroutine("CreateBalls");
		for(int i  =0; i < numBalls; i++)
        {
            ballAr[i] = new CueStickMovement.BallArray();
            ballAr[i].setBall(Instantiate(Resources.Load("PoolBall", typeof(GameObject))) as GameObject);
            //ballAr[i].getBall().GetComponent<Renderer>().material.color = Color.cyan;
            if (i  == 0)
            {
                ballAr[i].getBall().transform.localPosition = new Vector3(0, 0, 0);
            }
            if (i == 1 || i == 2)
            {
                if (i == 1) {
                    ballAr[i].getBall().transform.localPosition = new Vector3(-.5f, 0, 1);
                }
                else
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(.5f, 0, 1);
                }
            }
            if (i == 3 || i == 4 || i == 5)
            {
                if(i == 3)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(-1f, 0, 2);
                }
                if (i == 4)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(0f, 0, 2);
                }
                if (i == 5)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(1f, 0, 2);
                }
            }
            if (i == 6 || i == 7 || i == 8 || i == 9)
            {
                if (i == 6)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(-1.5f, 0, 3);
                }
                if (i == 7)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(-.5f, 0, 3);
                }
                if (i == 8)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(.5f, 0, 3);
                }
                if (i == 9)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(1.5f, 0, 3);
                }
            }
            if (i == 10 || i == 11 || i == 12 || i == 13 || i == 14)
            {
                if (i == 10)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(-2f, 0, 4);
                }
                if (i == 11)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(-1f, 0, 4);
                }
                if (i == 12)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(-0f, 0, 4);
                }
                if (i == 13)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(1f, 0, 4);
                }
                if (i == 14)
                {
                    ballAr[i].getBall().transform.localPosition = new Vector3(2f, 0, 4);
                }
            }
            ballAr[i].getBall().GetComponent<Rigidbody>().drag = drag;
            ballAr[i].getBall().GetComponent<Rigidbody>().mass = weight;
            ballAr[i].getBall().GetComponent<Rigidbody>().angularDrag = ang;
        }
        OrderBalls();
    }

    IEnumerator CreateBalls()
    {
        yield return new WaitForSeconds(.5f);
    }

    void OrderBalls()
    {
        List<string> ballArray = new List<string>();
        int count = 13;
        string lastBalltype = "";
        for (int a = 0; a < 14; a++)
        {
            if (a % 2 == 0)
            {
                ballArray.Add("Solid");
            }
            if (a % 2 != 0)
            {
                ballArray.Add("Stripe");
            }
        }
        GameObject last = ballAr[numBalls - 1].getBall();
        ballAr[14].getBall().GetComponent<DestroyBall>().setId(14);
        for (int i = 0; i < numBalls - 1; i++)
        {
            ballAr[i].getBall().GetComponent<DestroyBall>().setId(i);
            if (i == 4)
            {
                ballAr[i].setType("8Ball");
            }
            else
            {
                int rand = Random.Range(0, count);
                ballAr[i].setType(ballArray[rand]);
                ballArray.RemoveAt(rand);
                count--;
                if (i == 10)
                {
                    if (ballAr[i].getType() == "Solid")
                    {
                        lastBalltype = "Stripe";
                        int dex = ballArray.IndexOf(lastBalltype);
                        ballArray.RemoveAt(dex);
                        ballAr[14].setType(lastBalltype);
                    }
                    else
                    {
                        lastBalltype = "Solid";
                        int dex = ballArray.IndexOf(lastBalltype);
                        ballArray.RemoveAt(dex);
                        ballAr[14].setType(lastBalltype);
                        last.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    count--;
                }
            }
            if (ballAr[i].getType() == "Solid")
            {
                GameObject sphere = ballAr[i].getBall();
                sphere.GetComponent<Renderer>().material.color = Color.blue;
            }
            if (ballAr[i].getType() == "8Ball")
            {
                GameObject sphere = ballAr[i].getBall();
                sphere.GetComponent<Renderer>().material.color = Color.black;
                
            }
        }
    }

    public CueStickMovement.BallArray getBallAtIndex(int index)
    {
        return ballAr[index];
    }
}
