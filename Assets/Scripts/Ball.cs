using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public AudioSource effectPlayer;
    public AudioClip[] sounds; //0 = Hit wall, 1 = Hit Paddle, 2 = Killed a Brick

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
