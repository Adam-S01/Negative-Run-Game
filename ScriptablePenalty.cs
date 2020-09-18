using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public enum effect
{
    Jump,
    Speed,
    Invisibility,
    Weight,
    Autojump,
    Gravity,
    Size
}

[CreateAssetMenu(fileName = "New Penalty", menuName = "Penalty")]
public class ScriptablePenalty : ScriptableObject
{
    public effect effectType = effect.Jump;
    [Range(0f, 2f)] public float valueModifier = 1; // the multiplier of the current value
    public float effectDuration = 3f;
    private GameObject player;

            //case effect.Weight:
                //get player rigidbody
                //create temp var that contain player initial rigidbody value
                //verify collision between player
                //get the position of the player colliding
                //add force in the opposite direction (same force as a dash or not ?)
                //set player's rigidbody weight to something else
                //break;
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }

    public IEnumerator Jump()
    {
        GameObject fx = Instantiate(GameManager.instance.malusFXGOPlume);
        GameObject fxPlayer = Instantiate(GameManager.instance.malusFXGOPlayer);
        PlayerController pc = player.GetComponent<PlayerController>();
        fxPlayer.transform.parent = pc.transform;
        fx.transform.parent = FindObjectOfType<Camera>().transform;
        fxPlayer.transform.localPosition = new Vector3(0, 0 ,10);
        fx.transform.localPosition = new Vector3(0, 0 ,10);
        float initJumpForce = pc.jumpForce;
        pc.jumpForce *= valueModifier;
        yield return new WaitForSeconds(effectDuration);
        pc.jumpForce = initJumpForce;
        GameManager.instance.ResetTimer();
        Destroy(fx);
        Destroy(fxPlayer);
        
    }

    public IEnumerator Speed()
    {
        GameObject fx = Instantiate(GameManager.instance.malusFXGOSpeed);
        GameObject fxPlayer = Instantiate(GameManager.instance.malusFXGOPlayer);
        PlayerController pc = player.GetComponent<PlayerController>();
        fxPlayer.transform.parent = pc.transform;
        fx.transform.parent = FindObjectOfType<Camera>().transform;
        fx.transform.localPosition = new Vector3(0, 0 ,10);
        fxPlayer.transform.localPosition = new Vector3(0, 0 ,10);
        float initSpeed = pc.speed;
        float initDashSpeed = pc.dashSpeed;
        pc.speed *= valueModifier;
        pc.dashSpeed *= valueModifier;
        yield return new WaitForSeconds(effectDuration);
        pc.speed = initSpeed;
        pc.dashSpeed = initDashSpeed;
        GameManager.instance.ResetTimer();
        Destroy(fx);
        Destroy(fxPlayer);
    }

    public IEnumerator Invisibility()
    {
        GameObject fx = Instantiate(GameManager.instance.malusFXGOInvisible);
        PlayerController pc = player.GetComponent<PlayerController>();
        fx.transform.parent = FindObjectOfType<Camera>().transform;
        fx.transform.localPosition = new Vector3(0, 0 ,10);
        GameObject go = GameObject.Find("/"+ player.name + "/Player_scale/Player_sprite");
        GameObject go2 = GameObject.Find("/"+ player.name + "/arrow (1)");
        go.SetActive(false);
        go2.SetActive(false);
        yield return new WaitForSeconds(effectDuration);
        go.SetActive(true);
        go2.SetActive(true);
        GameManager.instance.ResetTimer();
        Destroy(fx);
    }

    public IEnumerator Weight()
    {
        yield return 0;
    }

    public IEnumerator AutoJump()
    {
        GameObject fx = Instantiate(GameManager.instance.malusFXGOAutoJump);
        GameObject fxPlayer = Instantiate(GameManager.instance.malusFXGOPlayer);
        PlayerController pc = player.GetComponent<PlayerController>();
        fxPlayer.transform.parent = pc.transform;
        fx.transform.parent = FindObjectOfType<Camera>().transform;
        fx.transform.localPosition = new Vector3(0, 0 ,10);
        fxPlayer.transform.localPosition = new Vector3(0, 0 ,10);
        float timer = 0;
        while (timer < effectDuration)
        {
            if (pc.isGrounded)
            {
                Rigidbody2D rb = pc.GetComponentInParent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, pc.jumpForce);
            }
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GameManager.instance.ResetTimer();
        Destroy(fx);
        Destroy(fxPlayer);
        yield return 0;
    }

    public IEnumerator Gravity()
    {
        GameObject fx = Instantiate(GameManager.instance.malusFXGOGravity);
        GameObject fxPlayer = Instantiate(GameManager.instance.malusFXGOPlayer);
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        fxPlayer.transform.parent = rb.transform;
        fx.transform.parent = FindObjectOfType<Camera>().transform;
        fx.transform.localPosition = new Vector3(0, 0 ,10);
        fxPlayer.transform.localPosition = new Vector3(0, 0 ,10);
        rb.gravityScale = valueModifier;
        yield return new WaitForSeconds(effectDuration);
        rb.gravityScale = 8;
        GameManager.instance.ResetTimer();
        Destroy(fx);
        Destroy(fxPlayer);
    }

    public IEnumerator Size()
    {
        GameObject fx = Instantiate(GameManager.instance.malusFXGOGiant);
        GameObject fxPlayer = Instantiate(GameManager.instance.malusFXGOPlayer);
        PlayerController pc = player.GetComponent<PlayerController>();
        player.transform.localScale = new Vector3(valueModifier, valueModifier, 1);
        fxPlayer.transform.parent = pc.transform;
        fx.transform.parent = FindObjectOfType<Camera>().transform;
        fx.transform.localPosition = new Vector3(0, 0 ,10);
        fxPlayer.transform.localPosition = new Vector3(0, 0 ,10);
        yield return new WaitForSeconds(effectDuration);
        player.transform.localScale = new Vector3(1, 1, 1);
        GameManager.instance.ResetTimer();
        Destroy(fx);
        Destroy(fxPlayer);
    }
}
