using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicBG : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("BgMusic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
