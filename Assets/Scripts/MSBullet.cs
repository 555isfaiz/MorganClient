using UnityEngine;

public class MSBullet : MonoBehaviour
{
    public MonoBehaviour shooter;
    float defaultSpeed = MSGlobalParams.Rifle_bullet_fly_speed;
    public int bulletType = 1;

    void FixedUpdate()
    {
        gameObject.transform.Translate(Vector3.forward * defaultSpeed * Time.deltaTime, gameObject.transform);
    }

    void SetShooter(MonoBehaviour shooter)
    {
        this.shooter = shooter;
    }

    void SetSpeed(float speed)
    {
        this.defaultSpeed = speed;
    }

    void SetBulletType(int type)
    {
        this.bulletType = type;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

