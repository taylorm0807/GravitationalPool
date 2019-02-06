using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This code will take care of how the Cuestick moves in the scene, and also the actually hitting of the cueball
public class CueStickMovement : MonoBehaviour {

    GameObject Cue;
    public float power;
    float timer;
    bool changePower;
    int ballsLeft;
    string turn;
    GameObject generate;
    int numStriped;
    int numSolid;
    public bool ballSinked;
    bool turnActive;
    BallArray ball;
    bool placed;
    bool ballHit;
    public Player player1;
    public Player player2;
    Canvas can;
    string noMoreStripes;
    string noMoreSolids;
    bool hit8Ball;

    public class BallArray {
        public string typeOfBall;
        public Material material;
        public GameObject ball;
        public BallArray()
        {
            typeOfBall = "";
        }

        public void setBall(GameObject sphere)
        {
            ball = sphere;
        }

        public GameObject getBall()
        {
            return ball;
        }
        public string getType()
        {
            return typeOfBall;
        }

        public Material getMat()
        {
            return material;
        }

        public void setType(string type)
        {
            typeOfBall = type;
        }

        public void setMat(Material mat)
        {
            material = mat;
        }

    }

    public class Player{
        string type;
        bool active;
        int numLeft;

        public Player()
        {
            numLeft = 7;
            active = false;
        }

        public void setType(string goal)
        {
            type = goal;
        }

        public void setActive()
        {
            active = !active;
        }

        public void decreaseNumLeft(int numType)
        {
            numLeft = numType;
        }

        public string getType()
        {
            return type;
        }

        public bool isActive()
        {
            return active;
        }

        public int getNumLeft()
        {
            return numLeft;
        }

    }

    //Initializes all of the variables that are used
	void Start () {
        ballHit = false;
        placed = true;
        Cue = GameObject.Find("Cue Ball");
        generate = GameObject.Find("Pool Balls");
        power = 1.0f;
        timer = 0.0f;
        changePower = false;
        ballsLeft = 15;
        numStriped = 7;
        numSolid = 7;
        ballSinked = false;
        turnActive = false;
        ball = new BallArray();
        can = GameObject.Find("Canvas").GetComponent<Canvas>();
        player1 = new Player();
        player2 = new Player();
        noMoreStripes = "";
        noMoreSolids = "";
        player1.setActive();
    }
	
	// Update is called once per frame
	void Update () {
        //Check to see if you need to select a pocket to hit the 8Ball into
        if(turn != null && (noMoreStripes == turn || noMoreSolids == turn) && hit8Ball == false)
        {
            print("Select a pocket to hit the 8ball into");
            hit8Ball = true;
            GameObject pocket = new GameObject();
            pocket = choosePocket();
        }

        //Assuming you aren't moving the cue ball bc you scratched, the cuestick will track the mouse and you can hit the ball
        if (placed == true) { 
            Vector3 Temp;
            Temp.x = Input.mousePosition.x;
            Temp.y = Input.mousePosition.y;
            Temp.z = 19.5f;
            //Assuming that you arent toggling the cuesticks power, this will set the cueStick to the mouse's position
            if (changePower == false)
            {
                this.transform.position = Camera.main.ScreenToWorldPoint(Temp);
            }

            //the cueStick is going to be centering around the cue ball to make the shot easier to hit
            this.transform.LookAt(Cue.transform);

            //if you hold down the left mouse button, this sets changepower = true which means that the cuestick will being drawing back for power
            if (Input.GetKey(KeyCode.Mouse0))
            {
                changePower = true;
            }

            //once you release the mouse button, the cuestick drawback timer resets and the cuestick stops osilating back and forth
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                this.transform.position = Camera.main.ScreenToWorldPoint(Temp);
                changePower = false;
                timer = 0.0f;
                this.transform.position += (transform.forward * 1f);
                StartCoroutine(resetPower());
                
            }

            //if you are holding down the mouse button, the cuestick osilates back and forth, increasing and decreasing pwoer as it goes further back
            if (changePower == true)
            {
                timer += Time.deltaTime;

                //if less than a second has gone by, then draw the stick back
                if (timer < 1)
                {
                    this.transform.position -= (transform.forward * .05f);
                    //this.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                    power += .5f;

                }

                //if the timer is between 1 and 2 seconds passed, move the stick back towards the starting position
                if (timer >= 1 && timer < 2)
                {
                    this.transform.position += (transform.forward * .05f);
                    //this.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
                    if (power > 1.00000000f)
                    {
                        power -= .5f;
                    }
                }

                //if the timer gets larger than 2, then reset it so it is easier to keep the constant osilation
                if (timer >= 2)
                {
                    timer = 0.0f;
                }


            }
            //If the cue ball comes to a stop, then we check the results of the turn to see what happened
            if ((Cue.GetComponent<Rigidbody>().velocity.magnitude <= new Vector3(1, 1, 1).magnitude) && turnActive == true)
            {
                checkResults();
                turnActive = false;
            }
        }
        //This code is run if the ball is scratched and the opponent is choosing the location of the cue ball
        //It hides the cue stick temporarily, and makes the ball move with the mouse, and when you click with the mouse,
        //The ball is placed in that spot, and the cue stick goes back to being visible
        else if(placed == false)
        {
            Vector3 Temp;
            Temp.x = Input.mousePosition.x;
            Temp.y = Input.mousePosition.y;
            Temp.z = 19.5f;
            Cue.transform.position = Camera.main.ScreenToWorldPoint(Temp);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
              StartCoroutine(Wait(Temp));
              Cue.GetComponent<SphereCollider>().enabled = !(Cue.GetComponent<SphereCollider>().enabled);
            }
            turnActive = false;
        }
    }

    //Simple courantine to reset the power with a delay, to prevent the stick from storing momentum
    IEnumerator resetPower()
    {
        yield return new WaitForSeconds(.01f);
        power = 1.0f;
    }

    //This courantine places the cue ball wherever the user clicked,
    IEnumerator Wait(Vector3 Temp)
    {
        Cue.transform.position = Camera.main.ScreenToWorldPoint(Temp);
        yield return new WaitForSeconds(.1f);
        placed = true;
        StartCoroutine(resetPower());
        Cue.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }

    //Check if cue stick hit the cue ball, if so, then it sends the ball forward with a velocity
    void OnTriggerEnter(Collider Coll)
    {
        if (Coll.gameObject.name == "Cue Ball" && power > 1.0f)
        {
            turnActive = true;
            Cue.transform.forward = this.transform.forward;
            Cue.GetComponent<Rigidbody>().velocity = transform.forward * power;
            power = 1.0f;
        }
        
    }

    public void ballMoved()
    {
        ballHit = true;
    }

    //This method checks to see what happened and if the turns should switch/if the ball was scratched
    void checkResults()
    {
        //If you sunk the ball of the correct type
        if(ballSinked == true && (ball.getType() == turn))
        {
            print("Good job, you hit the correct ball in");
        }

        //If you sunk a ball but the other players type, switches turn and calls the scrachBall method
        if (ballSinked == true && (ball.getType() != turn))
        {
            print("You sunk a ball, but it was the opponents, so you scratched");
            SwitchTurns();
            ScratchBall();
        }

        //If you didnt sink any balls at all(Not implemented)
        if (ballSinked == false)
        {
            print("You didn't sink any ball this turn, so now its the other players turn");
            SwitchTurns();
            if(ballHit == false)
            {
                print("You scratched because you didn't hit any balls");
                ScratchBall();
            }
        }
        ballSinked = false;
        ballHit = false;
    }

    //This method is to switch turns from player1 to player2 or vice versa, and change the turn variable, as well as the UI
    public void SwitchTurns()
    {
        if(turn == "Solid")
        {
            turn = "Stripe";
        }
        else
        {
            turn = "Solid";
        }
        if (player1.isActive())
        {
            player1.setActive();
            player2.setActive();
            can.GetComponent<UIUpdating>().changeTurn("player2");
        }
        else if (!(player1.isActive()))
        {
            player1.setActive();
            player2.setActive();
            can.GetComponent<UIUpdating>().changeTurn("player1");
        }
       print("It is now " + turn + "'s turn");

    }

    //This method hides the cue stick if the cue ball was scratched, and sets placed to false which causes the loop in 
    //the update function to execute, allowing the opponent to place the ball on the table
    public void ScratchBall()
    {
        
        print("Ball Scratched");
        Cue.GetComponent<SphereCollider>().enabled = !(Cue.GetComponent<SphereCollider>().enabled);
        placed = false;

    }

    public GameObject choosePocket()
    {
        GameObject temp = new GameObject();

        return temp;
    }

    //This method is called from a pool ball when it collides with the pocket
    public void ballDestroyed(GameObject destroyedBall)
    {
        int id = destroyedBall.GetComponent<DestroyBall>().getId();
        ball = generate.GetComponent<GenerateBalls>().getBallAtIndex(id);

        //This if statement checks to see if the ball hit in was the first sink of the game. If so, it initializes the players with 
        //stripes and solids
        if (ballsLeft == 15)
        {
            turn = ball.getType();
            if (player1.isActive())
            {
                player1.setType(turn);
                if(turn == "Solid")
                {
                    player2.setType("Stripe");
                }
                else
                {
                    player2.setType("Solid");
                }
            }
            else{
                player2.setType(turn);
                if (turn == "Solid")
                {
                    player1.setType("Stripe");
                }
                else
                {
                    player1.setType("Solid");
                }
            }
            can.GetComponent<UIUpdating>().setTypes(player1.getType(), player2.getType());
            ballsLeft--;
        }
        //If they sunk the 8ball, they lose the game and it resets the scene(Need to implement when you sink 8ball to win)
        if (ball.getType() == "8Ball")
        {
            ballsLeft = 0;
            print("You sunk the 8Ball, game over!!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //This is executed if they just hit in a ball normally, and it just decrements the number of balls for the respective type that was
        //hit in, and decreases the number of total balls left
        else
        {
            if (ball.getType() == "Solid")
            {
                numSolid--;
            }
            else
            {
                numStriped--;
            }
            ballsLeft--;
        }

        //These logic statements check to see what each players type is, and it sets the local variable for how many they have left
        //equal to the variable for the numSolid and numStriped left
        if(player1.getType() == "Solid")
        {
            player1.decreaseNumLeft(numSolid);
            player2.decreaseNumLeft(numStriped);
        }
        else if (player1.getType() == "Stripe")
        {
            player1.decreaseNumLeft(numStriped);
            player2.decreaseNumLeft(numSolid);
        }

        print("Player 1 is " + player1.getType() + ". Number of " + player1.getType() + " balls left is: " + player1.getNumLeft());
        print("Player 2 is " + player2.getType() + ". Number of " + player2.getType() + " balls left is: " + player2.getNumLeft());

        can.GetComponent<UIUpdating>().setNumLeft(player1.getNumLeft(), player2.getNumLeft());

        //If either of the players sink all of their balls, they win the game and the scene resets(Need to implement 8ball ending)
        if (player1.getNumLeft() == 0 || player2.getNumLeft() == 0)
        {
            if(turn == "Solid")
            {
                noMoreSolids = "Solid";
            }
            else
            {
                noMoreStripes = "Stripe";
            }
            print("Congrats, you sunk all of the ball for your type, you win!!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

