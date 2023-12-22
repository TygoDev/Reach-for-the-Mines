using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public class MinedEvent : MyEvent
{
    public MinedEvent() { }
}

public class SoldEvent : MyEvent
{
    public readonly float totalGold;

    public SoldEvent(float value) 
    {
        totalGold = value;
    }
}

public class BoughtEvent : MyEvent
{
    public readonly float totalGold;
    public readonly Item itemBought;

    public BoughtEvent(float value, Item item)
    {
        totalGold = value;
        itemBought = item;
    }
}

public class StationPlacedEvent : MyEvent
{
    public readonly GameObject station;

    public StationPlacedEvent(GameObject value)
    {
        station = value;
    }
}

public class StationInteractedEvent : MyEvent
{
    public readonly GameObject station;

    public StationInteractedEvent(GameObject value)
    {
        station = value;
    }
}

public class PickaxeUpgradedEvent : MyEvent
{
    public readonly Item pickaxe;

    public PickaxeUpgradedEvent(Item value)
    {
        pickaxe = value;
    }
}

public class SceneSwitchedEvent : MyEvent
{
    public readonly string newScene;

    public SceneSwitchedEvent(string value)
    {
        newScene = value;
    }
}


