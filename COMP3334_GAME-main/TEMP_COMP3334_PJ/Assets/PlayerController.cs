using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour{
    public float moveSpeed = 1f;

    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    Vector2 movementInput;

    Rigidbody2D rb;

    Animator animator;

    SpriteRenderer spriteRenderer;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate(){

        if (!isLocalPlayer) {
            return;
        }

        //如果移動輸入不為0，嘗試移動
        if (movementInput != Vector2.zero){
            bool success = TryMove(movementInput);

            if (!success){
                success = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!success){
                success = TryMove(new Vector2(0, movementInput.y));
            }

            animator.SetBool("isMoving", success);

            //將人物的方向設置為運動方向
            if (movementInput.x < 0){
                spriteRenderer.flipX = true;

            }
            else if (movementInput.x > 0){
                spriteRenderer.flipX = false;
            }
        }
        else{
            animator.SetBool("isMoving", false);
        }
    }

    private bool TryMove(Vector2 direction){
        if(direction != Vector2.zero){
        //檢查潛在的碰撞
            int count = rb.Cast(
                direction, //x 和 Y 值介於 -1 和 1 之間，表示從身體開始尋找碰撞的方向
                movementFilter, //確定碰撞發生位置的設置，例如要與之碰撞的圖層
                castCollisions, //轉換完成後將找到的碰撞存儲到的碰撞列表
                moveSpeed * Time.fixedDeltaTime + collisionOffset); //施法量等於移動加上和偏移

            if (count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else{
                return false;
            }
        }
        else{
            //沒有前進的方向就不能前進.
            return false;
        }
    }

    public void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}