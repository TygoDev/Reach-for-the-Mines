using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private string animationClipName = " ";
    private Animator animator = null;
    private bool isPlaying = false;
    private Systems systems = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        systems = Systems.Instance;
        
        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.hitEvent += Play;
    }

    private void OnDisable()
    {
        systems.inputManager.hitEvent -= Play;
    }

    private void Play()
    {
        isPlaying = !isPlaying;
    }

    void Update()
    {
        // Check if playAnimation has changed
        if (!isPlaying)
        {
            animator.Play(animationClipName, 0, 0f);
        }
    }
}
