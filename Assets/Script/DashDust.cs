using UnityEngine;

public class DashDust : MonoBehaviour
{
    public float lifeTime = 0.3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
