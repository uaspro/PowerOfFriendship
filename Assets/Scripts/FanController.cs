using UnityEngine;

public class FanController : MonoBehaviour
{
    private const string PlayerTag = "Player";

    public float LiftForce = 300f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != PlayerTag)
        {
            return;
        }

        var playerRigitbody = collision.gameObject.GetComponent<Rigidbody2D>();
        playerRigitbody.AddForce(new Vector2(0, LiftForce));
    }
}
