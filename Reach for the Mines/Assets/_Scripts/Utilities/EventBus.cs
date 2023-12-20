using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MyEvent
{

}

public class EventBus<T> where T : MyEvent
{
    public static event Action<T> OnEvent;

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}

public class TestEvent : MyEvent
{

    public readonly int testData;

    public TestEvent(int value)
    {
        testData = value;
    }
}
