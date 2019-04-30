using UnityEngine;

public class PowerUp : MonoBehaviour {

    public enum Type {
        Multiplier,
        Speed,
        BallSpeed
    }

    public Type type;
    public bool inUse;

    public AudioSource effectPlayer;
    public AudioClip sound;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }

    public void Spawn(Transform position) {
        transform.position = position.position;
        inUse = true;
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * 150);
    }

    public void DeSpawn() {
        inUse = false;
        rb.isKinematic = true;
        transform.position = new Vector3(20, 20, 20);
    }

    void OnTriggerEnter(Collider other) {
        if (other.name.Contains("Paddle") ) {
            switch (type) {
                case Type.Multiplier:
                    GameLogic.logic.multiplier *= 2;
                    break;
                case Type.BallSpeed:
                    GameLogic.logic.ball.velocity *= 1.5f;
                    break;
                case Type.Speed:
                    GameLogic.logic.playerSpeed *= 1.5f;
                    GameLogic.logic.playerSpeed =
                        Mathf.Clamp(GameLogic.logic.playerSpeed, 0, 3);
                    break;
            }
            effectPlayer.PlayOneShot(sound, 0.5f);
            inUse = false;
            rb.isKinematic = true;
            transform.position = new Vector3(20, 20, 20);
        } else if (other.name.Contains("Bounds")) {
            inUse = false;
            rb.isKinematic = true;
            transform.position = new Vector3(20, 20, 20);
        }
    }
}
