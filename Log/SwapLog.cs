using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapLog : MonoBehaviour
{
    public GameObject imageR;
    public GameObject imageL;
    public static SwapLog Instance;
    private Animator animatorR, animatorL;

    private void Awake()
    {
        animatorR = imageR.GetComponent<Animator>();
        animatorL = imageL.GetComponent<Animator>();
        CreateSwapLog();
    }

    public void CreateSwapLog()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void SwapToLogin()
    {
        StartCoroutine(TurnOn(animatorR, imageR));
        StartCoroutine(TurnOff(animatorL, imageL));
    }

    public void SwapToSignUp()
    {
        StartCoroutine(TurnOn(animatorL, imageL));
        StartCoroutine(TurnOff(animatorR, imageR));
    }

    private IEnumerator TurnOn(Animator animator, GameObject gameObject)
    {
        animator.SetBool("Trans", true);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private IEnumerator TurnOff(Animator animator, GameObject gameObject)
    {
        gameObject.SetActive(true);
        animator.SetBool("Trans", false);
        yield return new WaitForSeconds(0.5f);
    }
}
