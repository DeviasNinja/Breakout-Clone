using UnityEngine;

public class KillArea : MonoBehaviour {
    //"Kills" the player when they enter the trigger
    void OnTriggerEnter(Collider other) {
        if (other.name.Contains("PlayBall"))
            GameLogic.logic.KillPlayer();
    }
}
