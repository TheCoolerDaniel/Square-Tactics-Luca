using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float timeForDeathAnimation;
    private Player player;
    private EnemyManager enemyManager;
    private MoveType nextMove;

    public void Initialize(Player player, EnemyManager enemyManager)
    {
        this.player = player;
        this.enemyManager = enemyManager;
    }

    public void MakeMove()
    {
        float xDistance = XDIstance();
        float yDistance = YDIstance();
        if(yDistance == 0 && xDistance == 0)
        {
            nextMove = MoveType.HIT;
        }  
        else if ((xDistance == 0 && yDistance != 0)|| (yDistance == 0 && xDistance != 0))
        {
            nextMove = MoveType.SHOOT;
        }
        else if(Mathf.Abs(xDistance)>= Mathf.Abs(yDistance))
        {
            if (xDistance> 0)
            {
                nextMove = MoveType.D;
            }
            else
            {
                nextMove= MoveType.A;
            }
        }
        else
        {
            if (yDistance> 0)
            {
                nextMove = MoveType.W;
            }
            else
            {
                nextMove = MoveType.S;
            }
        }
        switch (nextMove)
        {
            case MoveType.W:
                Move(Vector3.up);
                break;
            case MoveType.D:
                Move(Vector3.right);
                break;
            case MoveType.A:
                Move(Vector3.left);
                break;
            case MoveType.S:
                Move(Vector3.down);
                break;
            case MoveType.HIT:
                player.TakeDamage();
                break;
            case MoveType.SHOOT:
                Shoot();
                break;
        }
        
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;
    }

    public void Die()
    {
        foreach (PlayerCorner playerCorner in GetComponentsInChildren<PlayerCorner>())
        {
            playerCorner.enabled = true;
            playerCorner.transform.parent = null;
            Destroy(playerCorner.gameObject, timeForDeathAnimation);
        }
        print("wurde aufgerufen");
        Destroy(gameObject);
    }
    private void Shoot()
    {
        Projectile projectile = Instantiate(this.projectile, shootingPoint.position, shootingPoint.rotation).GetComponent<Projectile>();
        enemyManager.AddProjectile(projectile);
        projectile.Initialize(player.transform.position - transform.position);
    }

    private float XDIstance()
    {
        return player.transform.position.x - transform.position.x;
    }

    private float YDIstance()
    {
        return player.transform.position.y - transform.position.y;
    }

    private void LookAt2D(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        Quaternion desiredRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = desiredRotation;
    }

    void Update()
    {
        if (player != null)
        {
            LookAt2D(player.transform);
        }
    }
}
