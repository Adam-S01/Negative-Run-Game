using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Arrow : MonoBehaviour
{
    public float moveInput;
    public float moveInput2;

    public PlayerController pc;
    private SpriteRenderer skin;

    [SerializeField] private int playerID = 0;

    [SerializeField] private Player playerC;

    void Start()
    {
        playerC = ReInput.players.GetPlayer(playerID);
        skin = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        moveInput = playerC.GetAxis("MoveH");
        moveInput2 = playerC.GetAxis("MoveV");

        //arrow dash

            if(moveInput == 1 && moveInput2 < 0.5f && moveInput2 > -0.5f){//right
                transform.rotation = Quaternion.Euler(0,0,0);
            }else if(moveInput > 0.5f && moveInput2 > 0.5f){//right up
                transform.rotation = Quaternion.Euler(0,0,45);
            }else if(moveInput < 0.5f && moveInput > -0.5f && moveInput2 == 1){// up
                transform.rotation = Quaternion.Euler(0,0,90);
            }else if(moveInput < -0.5f && moveInput2 > 0.5f){//left up
                transform.rotation = Quaternion.Euler(0,0,135);
            }else if(moveInput == -1 && moveInput2 < 0.5f && moveInput2 > -0.5f){//left 
                transform.rotation = Quaternion.Euler(0,0,180);
            }else if(moveInput < -0.5f && moveInput2 < -0.5f){//left down
                transform.rotation = Quaternion.Euler(0,0,-135);
            }else if(moveInput > -0.5f && moveInput < 0.5f && moveInput2 == -1){// down
                transform.rotation = Quaternion.Euler(0,0,-90);
            }else if(moveInput > 0.5f && moveInput2 < 0){// right down
                transform.rotation = Quaternion.Euler(0,0,-45);
            }

            if(pc.canDash){
                skin.color = new Color(1,1,1,1);
            }else{
                skin.color = new Color(1,1,1,0.1f);
            }

    }
}
