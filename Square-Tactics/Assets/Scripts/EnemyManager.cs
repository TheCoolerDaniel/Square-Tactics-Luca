using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float timeBetweenMoves;
    [SerializeField] private float timeBetweenEnemySpawns = 4;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Player player;
    private readonly List<Enemy> enemies = new List<Enemy>();
    private readonly List<Projectile> projectiles = new List<Projectile>();


    public void AddProjectile(Projectile projectile)
    {
        projectiles.Add(projectile);
    }

    private void SpawnEnemies()
    {
        if(!player.WantsToMove() && player.isAlive) {
            Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemies.Add(enemy);
            enemy.Initialize(player, this);
        }
    }

    private void MoveCharacters()
    {
        if (!player.WantsToMove() && player.isAlive)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.MakeMove();
                    GetComponent<AudioSource>().Play();
                }

            }
        }
        else
        {
            player.Move();
        }
    }

    private void MoveAllProjectiles()
    {
        if (!player.WantsToMove() && player.isAlive)
        {
            foreach (Projectile projectile in projectiles)
            {
                if (projectile != null)
                {
                    projectile.Move();
                }

            }
        }
    }


    void Awake()
    {
        InvokeRepeating(nameof(MoveCharacters), timeBetweenMoves, timeBetweenMoves);
        InvokeRepeating(nameof(MoveAllProjectiles), timeBetweenMoves/2, timeBetweenMoves/2);
        InvokeRepeating(nameof(SpawnEnemies), timeBetweenEnemySpawns, timeBetweenEnemySpawns);
    }
}
