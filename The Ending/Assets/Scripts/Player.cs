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

    public LayerMask interactAndObstacleLayer;

    public LayerMask interactionLayer;


    private Animator animator; 
    public Sprite[] players_state;
    private Vector2 forwardDirection;


    public VariableJoystick joestick;
    public Button interactButton;

    private LayerMask combinedLayerMask;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    Collider2D Can_Step(Vector2 direction, LayerMask combinedLayerMask)
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector2 colliderSize = collider.size * transform.localScale;
        Vector2 currentPosition = transform.position;
        currentPosition.y -= 0.8f;
        // ������� ��� �������� ������������
        Vector2 checkPosition = currentPosition + direction * distanceToCheck;

        // ��������� ������� ��������
        Vector2 topLeft = new Vector2(checkPosition.x - colliderSize.x / 2, checkPosition.y + colliderSize.y / 2);
        Vector2 topRight = new Vector2(checkPosition.x + colliderSize.x / 2, checkPosition.y + colliderSize.y / 2);
        Vector2 bottomLeft = new Vector2(checkPosition.x - colliderSize.x / 2, checkPosition.y - colliderSize.y / 2);
        Vector2 bottomRight = new Vector2(checkPosition.x + colliderSize.x / 2, checkPosition.y - colliderSize.y / 2);

        Debug.DrawLine(topLeft, topRight, Color.red);
        Debug.DrawLine(topRight, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red);
        Debug.DrawLine(bottomLeft, topLeft, Color.red);

        // �������� ������� ����������� �� �������
        Collider2D hit = Physics2D.OverlapBox(checkPosition, colliderSize, 0, combinedLayerMask);
        return hit;
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
            combinedLayerMask = obstacleLayer | interactAndObstacleLayer;
            if (Can_Step(forwardDirection, combinedLayerMask) == null)
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
            combinedLayerMask = obstacleLayer | interactAndObstacleLayer;
            if (Can_Step(forwardDirection, combinedLayerMask) == null)
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
            combinedLayerMask = obstacleLayer | interactAndObstacleLayer;
            if (Can_Step(forwardDirection, combinedLayerMask) == null)
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
            combinedLayerMask = obstacleLayer | interactAndObstacleLayer;
            if (Can_Step(forwardDirection, combinedLayerMask) == null)
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

        combinedLayerMask = interactionLayer | interactAndObstacleLayer;
        if (Can_Step(forwardDirection, combinedLayerMask) != null)
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
