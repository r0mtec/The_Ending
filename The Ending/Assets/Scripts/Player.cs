using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 1f;
    public float distance_step = 1f;
    public float distanceToCheck = 5f;

    private SpriteRenderer spriteRenderer;

    private Vector3 targetPosition;
    public LayerMask obstacleLayer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    bool Can_Step(Vector2 direction)
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector2 colliderSize = collider.size * transform.localScale;
        Vector2 currentPosition = transform.position;
        // Позиция для проверки столкновения
        Vector2 checkPosition = currentPosition + direction * distanceToCheck;

        // Отрисовка области проверки
        Vector2 topLeft = new Vector2(checkPosition.x - colliderSize.x / 2, checkPosition.y + colliderSize.y / 2);
        Vector2 topRight = new Vector2(checkPosition.x + colliderSize.x / 2, checkPosition.y + colliderSize.y / 2);
        Vector2 bottomLeft = new Vector2(checkPosition.x - colliderSize.x / 2, checkPosition.y - colliderSize.y / 2);
        Vector2 bottomRight = new Vector2(checkPosition.x + colliderSize.x / 2, checkPosition.y - colliderSize.y / 2);

        Debug.DrawLine(topLeft, topRight, Color.red);
        Debug.DrawLine(topRight, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red);
        Debug.DrawLine(bottomLeft, topLeft, Color.red);

        // Проверка наличия коллайдеров на позиции
        Collider2D hit = Physics2D.OverlapBox(checkPosition, colliderSize, 0, obstacleLayer);

        return hit == null;
    }

    void Update()
    {
        Vector2 forwardDirection = Vector2.right;

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            animator.SetInteger("MoveX", 0);
            animator.SetInteger("MoveY", 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            forwardDirection = Vector2.right;
            if (Can_Step(forwardDirection))
            {
                animator.SetInteger("MoveX", 1);
                animator.SetInteger("MoveY", 0);
                targetPosition = new Vector3(transform.position.x + distance_step, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            forwardDirection = Vector2.up;

            if (Can_Step(forwardDirection))
            {
                animator.SetInteger("MoveX", 0);
                animator.SetInteger("MoveY", 1);
                targetPosition = new Vector3(transform.position.x, transform.position.y + distance_step, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            forwardDirection = Vector2.down;
            if (Can_Step(forwardDirection))
            {
                animator.SetInteger("MoveX", 0);
                animator.SetInteger("MoveY", -1);
                targetPosition = new Vector3(transform.position.x, transform.position.y - distance_step, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            forwardDirection = Vector2.left;
            if (Can_Step(forwardDirection))
            {
                animator.SetInteger("MoveX", -1);
                animator.SetInteger("MoveY", 0);
                targetPosition = new Vector3(transform.position.x - distance_step, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        
    }
}
