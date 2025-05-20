using UnityEngine;

public class CowboyAnimatorController : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float speed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;
        anim.SetFloat("Speed", speed);

        if (Input.GetKeyDown(KeyCode.Space))
            anim.SetBool("IsJumping", true);
        else
            anim.SetBool("IsJumping", false);

        if (Input.GetKey(KeyCode.C))
            anim.SetBool("IsCrouching", true);
        else
            anim.SetBool("IsCrouching", false);

        if (Input.GetKeyDown(KeyCode.K))
            anim.SetBool("IsDead", true);
    }
}