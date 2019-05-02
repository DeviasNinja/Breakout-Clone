/********************
Creator: Jordan Harris
Last edited by: Jordan Harris
Last edited day: 9/11/2017

NFS = Not for Submission. 
This just means its something i want to add and finish eventually. May as well complete it.
********************/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

//Main game logic for the entire game
public class GameLogic : MonoBehaviour {
    public static GameLogic logic; //Singleton design pattern

    //Player Logic
    [SerializeField] protected int score;
    [SerializeField] protected int lives;
    protected float time;

    public bool playing;
    bool endOfGame = true;

    public int force = 250;
    public int level;//NFS
    public int multiplier = 1;
    private int bricksCount = 0;
    private int random;

    public float horizontal = 0;
    public float startTimer = 0;
    public float playerSpeed = 1;

    public GameObject paddle; //Main reference
    public Transform paddleTrans; //Shorter reference
    public Rigidbody ball;
    public List<GameObject> bricks = new List<GameObject>();
    public UnityEngine.UI.Text[] ui;
    public List<PowerUp> powerUps;
    public GameObject start;
    public GameObject game;
    public GameObject submit;
    public UnityEngine.UI.InputField input;
    public AudioSource musicPlayer;
    public AudioClip[] songs;

    string pName = "";

    public Level[] levels;//NFS

    //Singleton awake function
    void Awake() {
        if (logic == null) {
            DontDestroyOnLoad(gameObject);
            logic = this;
        } else if (logic != this) {
            Destroy(gameObject);
        }
    }

    void Start () {
        paddleTrans = paddle.GetComponent<Transform>(); //Link
    }
	
	void Update () {
        //Start timer reference and starts the game when 0
        if (startTimer > 0) {
            startTimer -= Time.deltaTime;
            //Updates the ui screen timer
            if (!ui[4].gameObject.activeSelf)
                ui[4].gameObject.SetActive(true);
            else
                ui[4].text = Mathf.CeilToInt(startTimer).ToString();
        } else if (startTimer <= 0 && !playing && !endOfGame) {
            ball.AddForce(Vector3.down * force); //Starts The ball
            playing = true;
            ui[4].gameObject.SetActive(false);
        }

        //Updates things on the screen, This could be better optimized to only change
        //when the value is actually changed.
        ui[0].text = "TIME: " + time.ToString("F0") + "s";
        ui[1].text = score.ToString() + " :SCORE";
        ui[2].text = "LIVES: " + lives.ToString();
        ui[3].text = "x" + multiplier.ToString() + " :MULTIPLIER";

        //In-game logic
        if (playing && startTimer <= 0) {
            time += Time.deltaTime;
            ball.velocity *= 1.0001f;
            ball.AddForce((Vector3.down * 2) * Time.deltaTime);
            horizontal = Input.GetAxis("Horizontal"); //Gets A and D as input
            //Moves the paddle based on input
            paddleTrans.localPosition += new Vector3(((horizontal * 7) * playerSpeed) * Time.deltaTime, 0, 0);
            //Clamps how far left and right the paddle can go
            paddleTrans.localPosition = new Vector3(
                Mathf.Clamp(paddleTrans.localPosition.x, -7.75f, 7.75f), -3.36f, -1.22f);
        }
    }

    public void Quit() {
        Application.Quit();
    }

    //Adds score to the player
    public void AddScore(int scoreAdd) {
        score += (scoreAdd * multiplier);
    }

    public void KillABrick(Transform pos) {
        bricksCount--;
        if (bricksCount <= 0) {
            GameOver();
        } else {
            //Power up spawner logic using object pooling
            random = Random.Range(0, 100);
            if (random > 0 && random < 70) {
                //Nothing
            } else if (random > 70 && random < 80) {
                PowerUp pUp = powerUps.Find(x => x.type == PowerUp.Type.Multiplier && !x.inUse);
                if (pUp != null) {
                    pUp.Spawn(pos);
                }
            } else if (random > 80 && random < 90) {
                PowerUp pUp = powerUps.Find(x => x.type == PowerUp.Type.BallSpeed && !x.inUse);
                if (pUp != null) {
                    pUp.Spawn(pos);
                }
            } else if (random > 90 && random < 100) {
                PowerUp pUp = powerUps.Find(x => x.type == PowerUp.Type.Speed && !x.inUse);
                if (pUp != null) {
                    pUp.Spawn(pos);
                }
            }
        }
    }

    //Resets the level when the player loses a life
    public void Reset() {
        playerSpeed = 1;
        multiplier = 1;
        playing = false; //Stops player input

        //Resets Ball
        ball.velocity = Vector3.zero;
        ball.transform.localPosition = new Vector3(0, 0, -1.22f);

        //Adds Timer
        startTimer = 3;

        //Resets Paddle
        paddleTrans.localPosition = new Vector3(0, -3.36f, -1.22f);

        //Despawn powerups
        for (int i = 0; i < powerUps.Count; i++) {
            powerUps[i].DeSpawn();
        }
    }

    //Resets the bricks
    public void RestartGame() {
        bricksCount = 0;
        lives = 3;
        playerSpeed = 1;
        multiplier = 1;
        for (int i = 0; i < bricks.Count; i++) {
            bricks[i].SetActive(true);
            bricksCount++;
        }
    }

    //Submits the score to the web.
    public void SubmitScore() {
        if (input.text == "")
            pName = "PlayerUnknown";
        else
            pName = input.text;

        //string url = "http://apps.iversoft.ca/internal-leaderboard/api.php";

        //WWWForm form = new WWWForm();
        //form.AddField("name", pName);
        //form.AddField("score", score.ToString());
        //form.AddField("time", time.ToString());
        //WWW www = new WWW(url, form);

        //StartCoroutine(Upload(www));

        submit.SetActive(false);
        start.SetActive(true);
    }

    System.Collections.IEnumerator Upload(WWW www) {
        yield return www;

        // check for errors
        //if (www.error == null) {
        //    Debug.Log("WWW Ok!: " + www.text);
        //} else {
        //    Debug.Log("WWW Error: " + www.error);
        //}
    }

    //End of game logic
    public void GameOver() {
        musicPlayer.clip = songs[0];
        musicPlayer.volume = .5f;
        musicPlayer.Play();
        bricksCount = 0;
        playerSpeed = 1;
        multiplier = 1;
        playing = false;
        endOfGame = true;
        ball.velocity = Vector3.zero;
        ball.position = new Vector3(0, 0, -1.22f);
        //Turn off all bricks
        for (int i = 0; i < bricks.Count; i++) {
            bricks[i].SetActive(false);
        }
        submit.gameObject.SetActive(true);
        game.SetActive(false);
        start.SetActive(false);
        ui[5].text = "TOTAL TIME\n" + time.ToString("F0") + "s";
        ui[6].text = "TOTAL SCORE\n" + score.ToString();
    }

    //Starts game
    public void StartGame() {
        musicPlayer.clip = songs[1];
        musicPlayer.volume = .5f;
        musicPlayer.Play();
        start.SetActive(false);
        game.SetActive(true);
        endOfGame = false;
        time = 0;
        score = 0;
        RestartGame();
        Reset();
    }

    //Kills player
    public void KillPlayer() {
        lives--;
        if (lives <= 0) {
            GameOver();
        } else {
            Reset();
        }
    }
}

//NFS
[System.Serializable]
public class Level {
    public float startForce;
    public float startAngle;
}
