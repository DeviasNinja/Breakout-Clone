//Removed Unused Namespaces
using UnityEngine;

//The class for the player balls sound effects
public class Ball : MonoBehaviour {

    public AudioSource effectPlayer;
    public AudioClip[] sounds; //0 = Hit wall, 1 = Hit Paddle, 2 = Killed a Brick

    //Whenever the ball hits something, it determines what it hit 
    //then plays the appropriate sound
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name.Contains("Bounds")) {
            effectPlayer.PlayOneShot(sounds[0], 0.5f);
        } else if (collision.gameObject.name.Contains("Paddle")) {
            effectPlayer.PlayOneShot(sounds[1], 0.5f);
        } else if (collision.gameObject.name.Contains("Layer")) {
            effectPlayer.PlayOneShot(sounds[2], 0.5f);
        }
    }
}
