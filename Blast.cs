using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    void Awake()
    {

        
        
    }

    void OnTriggerStay2D(Collider2D truc){
        if(truc.gameObject.tag == "DeathRight"){
            transform.Rotate(0,180,0);
        }

        if(truc.gameObject.tag == "DeathUp"){
            transform.Rotate(0,-90,0);
        }

        if(truc.gameObject.tag == "DeathLeft"){
            transform.Rotate(0,0,0);
        }

        if(truc.gameObject.tag == "DeathDown"){
            transform.Rotate(0,90,0);
        }
    }

}
