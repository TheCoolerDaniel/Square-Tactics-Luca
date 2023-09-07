using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool isAlive { get; private set; }
    [SerializeField] private float moveAnimationTime =3;
    [SerializeField] private int health = 3;
    [SerializeField] private float timeBeforeSceneRestart;
    private Enemy collidedEnemy;
    private List<MoveType> nextMoves = new List<MoveType>();

    public void SetNextMoves(List<MoveType> newMoves)
    {
        nextMoves.AddRange(newMoves);
    }

    public bool WantsToMove()
    {
        return nextMoves.Count > 0;
    }
    public void TakeDamage()
    {
        if(isAlive == true)
        {
            health--;
            if(health<= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                isAlive = false;
                foreach (PlayerCorner playerCorner in GetComponentsInChildren<PlayerCorner>())
                {
                    playerCorner.enabled = true;
                }
                Invoke(nameof(ReloadScene), timeBeforeSceneRestart);
            }
        }
    }
    
    public void Move()
    {   
        MoveType nextMove = nextMoves[0];
        switch (nextMove)
        {
            case MoveType.W:
                Walk(Vector3.up);
                break;
            case MoveType.D:
                Walk(Vector3.right);
                break;
            case MoveType.A:
                Walk(Vector3.left);
                break;
            case MoveType.S:
                Walk(Vector3.down);
                break;
            case MoveType.HIT:
                break;
        }
        if (collidedEnemy != null)
        {
            if(nextMove == MoveType.HIT)
            {
                collidedEnemy.Die();
            }
            else
            {
                TakeDamage();
            }
        }
        nextMoves.RemoveAt(0);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Walk(Vector3 direction)
    {
        transform.DOMove(transform.position + direction, moveAnimationTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy currentEnemy))
        {
            collidedEnemy= currentEnemy;
        }
        else if (collision.gameObject.TryGetComponent(out Projectile currentProjectile))
        {
            TakeDamage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy currentEnemy))
        {
            collidedEnemy = null;
        }
    }
    private void Awake()
    {
        isAlive= true;
    }
}
