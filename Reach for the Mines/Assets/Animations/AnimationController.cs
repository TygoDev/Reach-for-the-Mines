using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private string animationClipName = " ";

    private Animator animator = null;
    private Systems systems = null;
    private bool isPlaying = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        systems = Systems.Instance;

        Subscribe();

        isPlaying = PlayerPrefs.GetInt("IsHitting", 0) == 1;
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

        PlayerPrefs.SetInt("IsHitting", isPlaying ? 1 : 0);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (!isPlaying)
        {
            animator.Play(animationClipName, 0, 0f);
        }
    }
}
