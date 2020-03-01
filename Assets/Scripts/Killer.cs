using UnityEngine;

public class Killer : MonoBehaviour
{
    private const string PlayerTag = "Player";

    public GameObject StartSign;

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject gameObject = collider.gameObject;
        if (gameObject.tag != PlayerTag)
        {
            return;
        }

        gameObject.transform.parent = null;
        gameObject.transform.position = StartSign.transform.position;

        var rigitbody2d = gameObject.GetComponent<Rigidbody2D>();
        rigitbody2d.velocity = Vector2.zero;
    }
}
