using UnityEngine;

public class StupidAnimationHack : MonoBehaviour
{
    private Animator animator;
    private int index;

    public string[] animations;
    public string[] layers;
    public int delay = 3;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        Invoke("PlayAnim", 1f);
    }

    private void PlayAnim()
    {
        if (animations.Length < index)
        {
            this.PlayAnimation(layers[index], animations[index]);
            index++;
            Invoke("PlayAnim", delay);
        }
    }

    private void PlayAnimation(string layerName, string animationStateName)
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        animator.speed = 1f;
        animator.Update(0f);
        animator.PlayInFixedTime(animationStateName, layerIndex, 0f);
    }

    private void PlayAnimation(string layerName, string animationStateName, string animationParam, float duration)
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        animator.SetFloat(animationParam, 1f);
        animator.PlayInFixedTime(animationStateName, layerIndex, 0f);
        animator.Update(0f);

        float length = animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        animator.SetFloat(animationParam, length / duration);
    }
}
