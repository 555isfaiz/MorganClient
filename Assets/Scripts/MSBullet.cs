using UnityEngine;

public class MSBullet : MonoBehaviour
{
    MonoBehaviour shooter;

    void FixedUpdate()
    {
        gameObject.transform.Translate(gameObject.transform.forward * MSGlobalParams.Rifle_bullet_fly_speed * Time.deltaTime);
    }

    void SetShooter(MonoBehaviour shooter)
    {
        this.shooter = shooter;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide!!!!!");
        Destroy(gameObject);
    }
}

