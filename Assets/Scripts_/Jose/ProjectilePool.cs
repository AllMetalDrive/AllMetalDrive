using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{

    [Header("Pool Jugador")]
    [SerializeField] private Projectile_ playerProjectilePrefab;
    [SerializeField] private int playerPoolSize = 20;
    private Queue<Projectile_> playerPool = new Queue<Projectile_>();

    private void Awake()
    {
        for (int i = 0; i < playerPoolSize; i++)
        {
            Projectile_ proj = Instantiate(playerProjectilePrefab, transform);
            proj.gameObject.SetActive(false);
            playerPool.Enqueue(proj);
        }
    }

    public Projectile_ GetPlayerProjectile(Vector3 position, Quaternion rotation)
    {
        Projectile_ proj = playerPool.Count > 0 ? playerPool.Dequeue() : Instantiate(playerProjectilePrefab, transform);
        proj.transform.position = position;
        proj.transform.rotation = rotation;
        proj.Initialize(ShooterType.Player);
        proj.gameObject.SetActive(true);
        return proj;
    }


    public void ReturnPlayerProjectile(Projectile_ proj)
    {
        proj.gameObject.SetActive(false);
        playerPool.Enqueue(proj);
    }

}
