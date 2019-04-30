using UnityEngine;

public class Brick : MonoBehaviour {

    public int addToScore;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name.Contains("Ball")) {
            GameLogic.logic.AddScore(addToScore);
            GameLogic.logic.KillABrick(transform);
            gameObject.SetActive(false);
        }
    }
}
