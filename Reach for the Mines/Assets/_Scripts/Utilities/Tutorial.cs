using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText = null;
    [SerializeField] private TMP_Text pressEnterText = null;
    [SerializeField] private List<string> tutorialLines = new List<string>();

    private int tutorialIndex = 0;
    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;

        if (systems.tutorialComplete)
        {
            Destroy(gameObject);
            return;
        }

        Initialize();
    }

    private void Initialize()
    {
        UpdateTutorialText();
        Subscribe();
    }

    private void Subscribe()
    {
        systems.inputManager.enterEvent += FirstAdvancement;
    }

    private void Unsubscribe()
    {
        if (systems == null)
            return;

        systems.inputManager.enterEvent -= FirstAdvancement;
        EventBus<SceneSwitchedEvent>.OnEvent -= SwitchedToHUBworld;
        systems.inputManager.mouseRotateEvent -= MouseMoved;
        systems.inputManager.moveEvent -= CharacterMoved;
        systems.inputManager.sprintCanceledEvent -= CharacterSprinted;
        systems.inputManager.jumpEvent -= CharacterJumped;
        EventBus<MinedEvent>.OnEvent -= OreMined;
        EventBus<SoldEvent>.OnEvent -= ItemSold;
        EventBus<SoldEvent>.OnEvent -= EnoughItemsSold;
        EventBus<SceneSwitchedEvent>.OnEvent -= CopperMineEntered;
        EventBus<SceneSwitchedEvent>.OnEvent -= CopperMineExitted;
        EventBus<BoughtEvent>.OnEvent -= CraftingTableBought;
        EventBus<SceneSwitchedEvent>.OnEvent -= PersonalPlotEntered;
        EventBus<StationPlacedEvent>.OnEvent -= CraftingBenchPlaced;
        EventBus<StationInteractedEvent>.OnEvent -= CraftingBenchInteracted;
        systems.inputManager.enterEvent -= AdvanceAfterCraftingBench;
        EventBus<PickaxeUpgradedEvent>.OnEvent -= PickaxeUpgraded;
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    #region Event Listeners
    private void FirstAdvancement()
    {
        NextTutorial();
        TogglePressEnterText(false);

        systems.inputManager.mouseRotateEvent += MouseMoved;
    }

    private void MouseMoved(Vector2 x)
    {
        NextTutorial();

        systems.inputManager.moveEvent += CharacterMoved;
    }

    private void CharacterMoved(Vector2 x)
    {
        NextTutorial();

        systems.inputManager.sprintCanceledEvent += CharacterSprinted;
    }

    private void CharacterSprinted()
    {
        NextTutorial();

        systems.inputManager.jumpEvent += CharacterJumped;
    }

    private void CharacterJumped()
    {
        NextTutorial();

        if (SceneManager.GetActiveScene().name == "Hub World")
        {
            NextTutorial();

            EventBus<MinedEvent>.OnEvent += OreMined;
        }
        else
        {
            EventBus<SceneSwitchedEvent>.OnEvent += SwitchedToHUBworld;
        }
    }

    private void SwitchedToHUBworld(SceneSwitchedEvent sceneSwitchedEvent)
    {
        if (sceneSwitchedEvent.newScene == "Hub World")
        {
            NextTutorial();

            EventBus<MinedEvent>.OnEvent += OreMined;
        }
    }

    private void OreMined(MinedEvent minedEvent)
    {
        NextTutorial();

        EventBus<SoldEvent>.OnEvent += ItemSold;
    }

    private void ItemSold(SoldEvent soldEvent)
    {
        NextTutorial();

        if(soldEvent.totalGold >= 500)
        {
            NextTutorial();

            EventBus<SceneSwitchedEvent>.OnEvent += CopperMineEntered;
        }
        else
        {
            EventBus<SoldEvent>.OnEvent += EnoughItemsSold;
        }
    }

    private void EnoughItemsSold(SoldEvent soldEvent)
    {
        if (soldEvent.totalGold >= 500)
        {
            NextTutorial();

            EventBus<SceneSwitchedEvent>.OnEvent += CopperMineEntered;
        }
    }

    private void CopperMineEntered(SceneSwitchedEvent sceneSwitchedEvent)
    {
        if(sceneSwitchedEvent.newScene == "Copper Cave")
        {
            NextTutorial();

            EventBus<SceneSwitchedEvent>.OnEvent += CopperMineExitted;
        }
    }

    private void CopperMineExitted(SceneSwitchedEvent sceneSwitchedEvent)
    {
        if (sceneSwitchedEvent.newScene == "Hub World")
        {
            NextTutorial();

            EventBus<BoughtEvent>.OnEvent += CraftingTableBought;
        }
    }

    private void CraftingTableBought(BoughtEvent boughtEvent)
    {
        if(boughtEvent.itemBought.name == "Crafting Bench")
        {
            NextTutorial();

            EventBus<SceneSwitchedEvent>.OnEvent += PersonalPlotEntered;
        }
    }

    private void PersonalPlotEntered(SceneSwitchedEvent sceneSwitchedEvent)
    {
        NextTutorial();

        EventBus<StationPlacedEvent>.OnEvent += CraftingBenchPlaced;
    }

    private void CraftingBenchPlaced(StationPlacedEvent stationPlacedEvent)
    {
        if(stationPlacedEvent.station.name == "Crafting Bench(Clone)")
        {
            NextTutorial();

            EventBus<StationInteractedEvent>.OnEvent += CraftingBenchInteracted;
        }
    }

    private void CraftingBenchInteracted(StationInteractedEvent stationInteractedEvent)
    {
        if(stationInteractedEvent.station.name == "Crafting Bench(Clone)")
        {
            NextTutorial();
            TogglePressEnterText(true);

            systems.inputManager.enterEvent += AdvanceAfterCraftingBench;
        }
    }

    private void AdvanceAfterCraftingBench()
    {
        TogglePressEnterText(false);
        NextTutorial();

        EventBus<PickaxeUpgradedEvent>.OnEvent += PickaxeUpgraded;
    }

    private void PickaxeUpgraded(PickaxeUpgradedEvent pickaxeUpgradedEvent)
    {
        NextTutorial();

        EventBus<PickaxeUpgradedEvent>.OnEvent += PickaxeUpgraded;
    }

    #endregion

    private void NextTutorial()
    {
        if (tutorialIndex + 1 < tutorialLines.Count)
        {
            tutorialIndex++;
            UpdateTutorialText();
        }
        else
        {
            Destroy(gameObject);
        }

        Unsubscribe();
    }

    private void UpdateTutorialText()
    {
        tutorialText.text = tutorialLines[tutorialIndex];
    }

    private void TogglePressEnterText(bool active)
    {
        pressEnterText.gameObject.SetActive(active);
    }
}
