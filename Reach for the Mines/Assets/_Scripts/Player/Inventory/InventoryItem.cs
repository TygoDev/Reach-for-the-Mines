using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryItem : MonoBehaviour
{
    public Item item = null;
    public ItemStack itemStack = null;
    public bool empty = true;

    [SerializeField] private Image itemImage = null;
    [SerializeField] private TMP_Text itemAmount = null;
    [SerializeField] private Button itemButton = null;

    private Systems systems = null;

    private void Start()
    {
        systems = Systems.Instance;
    }

    public void FillSlot(ItemStack pItem)
    {
        empty = false;

        item = pItem.One;
        itemImage.sprite = item.icon;
        itemImage.gameObject.SetActive(true);
        itemAmount.text = pItem.Two.ToString();
        itemStack = pItem;
        itemAmount.gameObject.SetActive(true);

        if (item.itemType == ItemType.Station)
        {
            //itemButton.onClick.RemoveAllListeners();
            itemButton.onClick.AddListener(SetStationClick);
        }
    }

    public void ClearSlot()
    {
        empty = true;

        item = null;
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
        itemAmount.text = null;
        itemStack = null;
        itemAmount.gameObject.SetActive(false);
    }

    public void SetStationClick()
    {
        if(transform.parent.name == "InventoryBackground" && SceneManager.GetActiveScene().name == "Personal Plot")
        {
            systems.inputManager.UnPause();
            systems.stationClickedEvent.Invoke(item);
        }
    }

    public Button InventoryItemButton => itemButton;
}
