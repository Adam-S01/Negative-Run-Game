using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimCam : MonoBehaviour
{
    public PlayerController pc1;
    public bool oneTime;

    public PlayerController pc2;
    public bool oneTime2;

    public PlayerController pc3;
    public bool oneTime3;

    public PlayerController pc4;
    public bool oneTime4;

    public Animator anim;

    

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(oneTime == false){
        if(pc1.col.isTrigger == true){
            anim.SetTrigger("death");
            oneTime = true;
        }
        }

        if(oneTime2== false){
        if(pc2.col.isTrigger == true){
            anim.SetTrigger("death");
            oneTime2 = true;
        }
        }

        if(oneTime3 == false){
        if(pc3.col.isTrigger == true){
            anim.SetTrigger("death");
            oneTime3 = true;
        }
        }

        if(oneTime4 == false){
        if(pc4.col.isTrigger == true){
            anim.SetTrigger("death");
            oneTime4 = true;
        }
        }
    }
}
