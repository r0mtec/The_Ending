using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float speed = 1f;
    public float distance_step = 1f;
    public float distanceToCheck = 5f;

    private SpriteRenderer spriteRenderer;

    private Vector3 targetPosition;
    public LayerMask obstacleLayer;
    public LayerMask interactionLayer;
    private Animator animator; 
    public Sprite[] players_state;
    private Vector2 forwardDirection;


    public VariableJoystick joestick;
    public Button interactButton;


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
        float moveX = joestick.Horizontal;
        float moveY = joestick.Vertical;

        if ((moveX <= 0.5f && moveX >= -0.5f) || (moveY <= 0.5f && moveY >= -0.5f))
        {
            animator.SetInteger("MoveX", 0);
            animator.SetInteger("MoveY", 0);
        }

        if (moveX > 0.5f)
        {
            forwardDirection = Vector2.right;
            if (Can_Step(forwardDirection))
            {
                animator.SetInteger("MoveX", 1);
                animator.SetInteger("MoveY", 0);
                //if (IsAnimationPlaying("WalkRight") || IsAnimationNextPlaying("WalkRight"))
                //{
                    targetPosition = new Vector3(transform.position.x + distance_step, transform.position.y, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
                //}        
            }
        }
        if (moveY > 0.5f)
        {
            forwardDirection = Vector2.up;

            if (Can_Step(forwardDirection))
            {
                animator.SetInteger("MoveX", 0);
                animator.SetInteger("MoveY", 1);
                //if (IsAnimationPlaying("WalkUp") || IsAnimationNextPlaying("WalkUp"))
                //{
                    targetPosition = new Vector3(transform.position.x, transform.position.y + distance_step, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
                //}
            }
        }
        if (moveY < -0.5f)
        {
            forwardDirection = Vector2.down;
            if (Can_Step(forwardDirection))
            {
                animator.SetInteger("MoveX", 0);
                animator.SetInteger("MoveY", -1);
                //if (IsAnimationPlaying("WalkDown") || IsAnimationNextPlaying("WalkDown"))
                //{
                    targetPosition = new Vector3(transform.position.x, transform.position.y - distance_step, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
                //}
            }
        }
        if (moveX < -0.5f)
        {
            forwardDirection = Vector2.left;
            if (Can_Step(forwardDirection))
            {
                animator.SetInteger("MoveX", -1);
                animator.SetInteger("MoveY", 0);
                //if (IsAnimationPlaying("WalkLeft") || IsAnimationNextPlaying("WalkLeft"))
                //{
                    targetPosition = new Vector3(transform.position.x - distance_step, transform.position.y, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
                //}
            }
        }


        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, forwardDirection, distanceToCheck, interactionLayer);
        if (hitInfo.collider != null)
        {
            interactButton.gameObject.SetActive(true);
        }
        else
        {
            interactButton.gameObject.SetActive(false);
        }

    }


    public void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, forwardDirection, distanceToCheck, interactionLayer);
        Debug.DrawRay(transform.position, forwardDirection * distanceToCheck, Color.red);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("fridge"))
            {
                Animator fridgeAnimator = hit.collider.GetComponent<Animator>();
                if (fridgeAnimator != null)
                {
                    fridgeAnimator.SetTrigger("open");
                }
            }
        }
    }

    bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(animationName);
    }
    bool IsAnimationNextPlaying(string animationName)
    {
        AnimatorStateInfo currentState = animator.GetNextAnimatorStateInfo(0);
        return currentState.IsName(animationName);
    }
}
