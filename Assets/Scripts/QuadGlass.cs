using System.Collections;
using UnityEngine;

public class QuadGlass : Quad, IAnimatable
{
    [SerializeField] private Animator animator;

    public void PlayDestroyAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Destroy");
            StartCoroutine(DelayedDestroy(0.2f));
            return;
        }

        Destroy(gameObject);
    }

    private IEnumerator DelayedDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}