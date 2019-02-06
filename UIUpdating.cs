using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIUpdating : MonoBehaviour {

    GameObject Tip;
    Image player1;
    Image player2;
    Text type1;
    Text type2;
    Text p1NumLeft;
    Text p2NumLeft;
    public bool madeItIn;
	// Use this for initialization
	void Start () {
        Tip = GameObject.Find("Tip");
        Canvas can = GameObject.Find("Canvas").GetComponent<Canvas>();
        player1 = GameObject.Find("Player1Image").GetComponent<Image>();
        player1.color = Color.red;
        player2 = GameObject.Find("Player2Image").GetComponent<Image>();
        type1 = GameObject.Find("Type1").GetComponent<Text>();
        type2 = GameObject.Find("Type2").GetComponent<Text>();
        p1NumLeft = GameObject.Find("NumLeft1").GetComponent<Text>();
        p2NumLeft = GameObject.Find("NumLeft2").GetComponent<Text>();

    }
    public void setTypes(string ptype1, string ptype2)
    {
        type1.text = ptype1;
        type2.text = ptype2;

    }
    public void changeTurn(string active)
    {
        if(active == "player1")
        {
            player1.color = Color.red;
            player2.color = Color.white;
        }
        else
        {
            player1.color = Color.white;
            player2.color = Color.red;
        }
    }

    public void setNumLeft(int p1Left, int p2Left)
    {
        p1NumLeft.text = p1Left + "";
        p2NumLeft.text = p2Left + "";

    }

    // Update is called once per frame
    void Update () {
		
	}
}
