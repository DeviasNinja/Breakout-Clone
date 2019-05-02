using UnityEngine;

//The brick class that the player hits
public class Brick : MonoBehaviour {

	//The value of the brick to add to score
    public int addToScore;

    //Collision script for when the ball touches the brick
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name.Contains("Ball")) {
            GameLogic.logic.AddScore(addToScore);
            GameLogic.logic.KillABrick(transform);
            gameObject.SetActive(false);
        }
    }
}
