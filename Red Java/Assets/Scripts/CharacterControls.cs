using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{
    float horizontal2;
    float vertical2;
    Rigidbody2D rb;
    public int speed;
    public Sprite[] idleSprites;
    public Sprite[] runningSprites;
    SpriteRenderer sprite;
    float spriteTime;
    float bird;

    int spriteCounter; // idle sprite için sayaç.

    int spriteRunningCounter; // Running sprite için sayaç.

    float counterTimer;

    bool IsRunAnimaton = false;
    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        Movement();
        IdleSpriteAnimation();
    }


    void Movement()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            IsRunAnimaton = true;
            transform.position += Vector3.right*Time.deltaTime*speed;
            transform.rotation = Quaternion.Euler(0,0,0);
            RunSpriteAnimation();
            
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            IsRunAnimaton = true;
            transform.position += Vector3.left*Time.deltaTime*speed;
            transform.rotation = Quaternion.Euler(0,180,0);
            RunSpriteAnimation();
            
        }
        
       if (Input.GetKeyDown(KeyCode.UpArrow))
       {
           transform.position += Vector3.up*100*Time.deltaTime;
         
       }
    }

    void IdleSpriteAnimation()
    {
        if (!IsRunAnimaton)
        {
        spriteTime += Time.deltaTime;
        Debug.Log(spriteTime);
        if (spriteTime > 0.09f)
        {
            spriteTime = 0;
          
            spriteCounter++;
            sprite.sprite = idleSprites[spriteCounter];
            if (spriteCounter == 11)
            {
                spriteCounter = 0;
            }
        }
        }
      
        
    }

    void RunSpriteAnimation()
    {
        if (IsRunAnimaton)
        {
        spriteTime += Time.deltaTime;
        Debug.Log(spriteTime);
        if (spriteTime > 0.09f)
        {
            spriteTime = 0;
          
            sprite.sprite = runningSprites[spriteRunningCounter];
            spriteRunningCounter++;
            
            if (spriteRunningCounter == 17)
            {
                spriteRunningCounter = 0;
            }
        }
        }
      
    }
}
