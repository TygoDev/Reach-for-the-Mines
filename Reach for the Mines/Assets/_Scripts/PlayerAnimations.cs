using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator controller;

    private void Start()
    {
        controller = GetComponent<Animator>();
        Systems.Instance.inputManager.moveEvent += CheckForWalking;
        Systems.Instance.inputManager.hitEvent += MiningTrue;
        Systems.Instance.inputManager.stoppedHittingEvent += MiningFalse;
        Systems.Instance.inputManager.sprintEvent += RunningTrue;
        Systems.Instance.inputManager.sprintCanceledEvent += RunningFalse;
    }

    private void OnDisable()
    {
        Systems.Instance.inputManager.moveEvent -= CheckForWalking;
        Systems.Instance.inputManager.hitEvent -= MiningTrue;
        Systems.Instance.inputManager.stoppedHittingEvent -= MiningFalse;
        Systems.Instance.inputManager.sprintEvent -= RunningTrue;
        Systems.Instance.inputManager.sprintCanceledEvent -= RunningFalse;
    }

    private void CheckForWalking(Vector2 value)
    {
        if (value != Vector2.zero)
            controller.SetBool("isWalking", true);
        else
            controller.SetBool("isWalking", false);
    }


    private void MiningTrue()
    {
        controller.SetBool("isMining", true);
    }

    private void MiningFalse()
    {
        controller.SetBool("isMining", false);
    }

    private void RunningTrue()
    {
        controller.SetBool("isRunning", true);
    }

    private void RunningFalse()
    {
        controller.SetBool("isRunning", false);
    }
}
