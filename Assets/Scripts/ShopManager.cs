using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
  public static ShopManager Instance;

  public GameObject shopGrid;
  public ShopItem shopItemPrefab;
  public GameObject filters;
  public Image BigItem;
  public TextMeshProUGUI itemCountText;
  public GameObject addItemsButton;

  public GameObject addItemPanel;
  private int addItemCount = 1;
  public TextMeshProUGUI addItemCountText;
  public TMP_InputField addItemTitle, addItemNumber;
  public Button[] addItemEquipSlots;
  private int addItemSlot = -1;

  private bool[] enabledEquipSlots = new bool[] { true, true, true, true, true };
  private bool showSoldOut = false;

  private void Awake()
  {
    if (Instance == null)
      Instance = this;
    else if (Instance != this)
    {
      Destroy(gameObject);
      return;
    }
  }

  void Start()
  {
    addItemPanel.SetActive(false);

    var shopItems = LoadData();

    if (shopItems.Count == 0)
    {
      shopItems.Add(new ShopItemData { Name = "Boots of Striding", SpriteName = "01", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Feet });
      shopItems.Add(new ShopItemData { Name = "Winged Shoes", SpriteName = "02", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Feet });
      shopItems.Add(new ShopItemData { Name = "Hide Armor", SpriteName = "03", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Chest });
      shopItems.Add(new ShopItemData { Name = "Leather Armor", SpriteName = "04", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Chest });
      shopItems.Add(new ShopItemData { Name = "Cloak of Invisibility", SpriteName = "05", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Consumable });
      shopItems.Add(new ShopItemData { Name = "Eagle-Eye Goggles", SpriteName = "06", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Head });
      shopItems.Add(new ShopItemData { Name = "Iron Helmet", SpriteName = "07", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Head });
      shopItems.Add(new ShopItemData { Name = "Heater Shield", SpriteName = "08", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Hand });
      shopItems.Add(new ShopItemData { Name = "Piercing Bow", SpriteName = "09", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Hand });

      shopItems.Add(new ShopItemData { Name = "War Hammer", SpriteName = "10", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Hand });
      shopItems.Add(new ShopItemData { Name = "Poison Dagger", SpriteName = "11", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Hand });
      shopItems.Add(new ShopItemData { Name = "Minor Healing Potion", SpriteName = "12", CountTotal = 4, CountLeft = 0, EquipSlot = EquipSlot.Consumable });
      shopItems.Add(new ShopItemData { Name = "Minor Stamina Potion", SpriteName = "13", CountTotal = 4, CountLeft = 0, EquipSlot = EquipSlot.Consumable });
      shopItems.Add(new ShopItemData { Name = "Minor Power Potion", SpriteName = "14", CountTotal = 4, CountLeft = 0, EquipSlot = EquipSlot.Consumable });
      shopItems.Add(new ShopItemData { Name = "Boots of Speed", SpriteName = "15", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Feet });
      shopItems.Add(new ShopItemData { Name = "Cloak of Pockets", SpriteName = "16", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Chest });
      shopItems.Add(new ShopItemData { Name = "Empowering Talisman", SpriteName = "17", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Head });
      shopItems.Add(new ShopItemData { Name = "Battle-Axe", SpriteName = "18", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Hand });
      shopItems.Add(new ShopItemData { Name = "Weighted Net", SpriteName = "19", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Hand });

      shopItems.Add(new ShopItemData { Name = "Minor Mana Potion", SpriteName = "20", CountTotal = 4, CountLeft = 0, EquipSlot = EquipSlot.Consumable });
      shopItems.Add(new ShopItemData { Name = "Stun Powder", SpriteName = "21", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Consumable });
      shopItems.Add(new ShopItemData { Name = "Heavy Greaves", SpriteName = "22", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Feet });

      shopItems.Add(new ShopItemData { Name = "Drakescale Helm", SpriteName = "108", CountTotal = 1, CountLeft = 0, EquipSlot = EquipSlot.Head });
      shopItems.Add(new ShopItemData { Name = "Ring of Skulls", SpriteName = "123", CountTotal = 2, CountLeft = 0, EquipSlot = EquipSlot.Consumable });
    }

    if (shopItems.Any(x => x.SpriteName == "52"))
      addItemsButton.SetActive(false);

    ListItems(shopItems);
    UpdateSizes();
    FilterItems();
  }

  private void ListItems(List<ShopItemData> shopItems)
  {
    for (int i = shopGrid.transform.childCount - 1; i >= 0; --i)
    {
      GameObject.Destroy(shopGrid.transform.GetChild(i).gameObject);
    }
    shopGrid.transform.DetachChildren();

    var orderedShopItems = shopItems.OrderBy(x => int.Parse(x.SpriteName)).ToArray();

    for (int i = 0; i < orderedShopItems.Length; i++)
    {
      var data = orderedShopItems[i];

      var shopItem = Instantiate(shopItemPrefab);
      shopItem.UpdateData(data);

      shopItem.transform.SetParent(shopGrid.transform);
    }
  }

  private void UpdateSizes()
  {
    var scale = this.shopGrid.transform.localScale;

    for (int i = 0; i < shopGrid.transform.childCount; i++)
    {
      shopGrid.transform.GetChild(i).GetComponent<ShopItem>().transform.localScale = new Vector3(1f, 1f, 1f);
    }
  }

  private List<ShopItemData> LoadData()
  {
    var fileName = GetPathToSaveFile();

    var shopData = new ShopData();
    var shopItems = new List<ShopItemData>();

    if (File.Exists(fileName))
    {
      var json = File.ReadAllText(fileName);
      JsonUtility.FromJsonOverwrite(json, shopData);
      shopItems = shopData.ShopItems.ToList();
    }

    return shopItems;
  }

  public void SaveData()
  {
    var shopItems = new List<ShopItemData>();

    for (int i = 0; i < shopGrid.transform.childCount; i++)
    {
      var shopItem = shopGrid.transform.GetChild(i).GetComponent<ShopItem>().Data;
      shopItems.Add(shopItem);
    }

    var data = new ShopData { ShopItems = shopItems.ToArray() };
    var json = JsonUtility.ToJson(data);
    var fileName = GetPathToSaveFile();
    Debug.Log(fileName);
    File.WriteAllText(fileName, json);
  }

  private string GetPathToSaveFile()
  {
    return Application.persistentDataPath + "/shop.json";
  }

  public void ToggleEquipSlot(int slot)
  {
    enabledEquipSlots[(int)slot] = !enabledEquipSlots[(int)slot];

    var alpha = enabledEquipSlots[(int)slot] ? 1f : 0.5f;
    filters.transform.GetChild(slot).GetComponent<Image>().color = new Color(1, 1, 1, alpha);

    FilterItems();
  }

  public void ToggleSoldOut()
  {
    showSoldOut = !showSoldOut;

    var alpha = showSoldOut ? 1f : 0.5f;
    filters.transform.GetChild(5).GetComponent<Image>().color = new Color(1, 1, 1, alpha);

    FilterItems();
  }

  private void FilterItems()
  {
    int filterCount = 0;

    for (int i = 0; i < shopGrid.transform.childCount; i++)
    {
      var shopItem = shopGrid.transform.GetChild(i).GetComponent<ShopItem>();

      if (!enabledEquipSlots[(int)shopItem.Data.EquipSlot] || (shopItem.Data.CountLeft == 0 && !showSoldOut))
        shopItem.gameObject.SetActive(false);
      else
      {
        shopItem.gameObject.SetActive(true);
        filterCount++;
      }
    }

    itemCountText.text = $"{filterCount} Items";
  }

  public void ShowItem(ShopItem item)
  {
    BigItem.sprite = Resources.Load<Sprite>($"ShopItems/{item.Data.SpriteName}");
    BigItem.gameObject.SetActive(true);
  }

  public void AddItems()
  {
    var shopItems = LoadData();

    if (shopItems.Any(x => x.SpriteName == "52"))
      return; // Already added

    // Add 50-56
    shopItems.Add(new ShopItemData { Name = "Steel Sabattons", SpriteName = "50", CountTotal = 2, CountLeft = 2, EquipSlot = EquipSlot.Feet });
    shopItems.Add(new ShopItemData { Name = "Shadow Armor", SpriteName = "51", CountTotal = 2, CountLeft = 2, EquipSlot = EquipSlot.Chest });
    shopItems.Add(new ShopItemData { Name = "Protective Charm", SpriteName = "52", CountTotal = 2, CountLeft = 2, EquipSlot = EquipSlot.Head });
    shopItems.Add(new ShopItemData { Name = "Black Knife", SpriteName = "53", CountTotal = 2, CountLeft = 2, EquipSlot = EquipSlot.Hand });
    shopItems.Add(new ShopItemData { Name = "Staff of Eminence", SpriteName = "54", CountTotal = 2, CountLeft = 2, EquipSlot = EquipSlot.Hand });
    shopItems.Add(new ShopItemData { Name = "Super Healing Potion", SpriteName = "55", CountTotal = 4, CountLeft = 4, EquipSlot = EquipSlot.Consumable });
    shopItems.Add(new ShopItemData { Name = "Ring of Brutality", SpriteName = "56", CountTotal = 2, CountLeft = 2, EquipSlot = EquipSlot.Consumable });

    ListItems(shopItems);
    UpdateSizes();
    FilterItems();

    addItemsButton.SetActive(false);

    SaveData();
  }

  public void ShowAddItemPanel()
  {
    addItemPanel.SetActive(true);
  }

  public void IncreaseItemCount()
  {
    if (addItemCount < 9)
      addItemCount++;
    addItemCountText.text = addItemCount.ToString();
  }

  public void DecreaseItemCount()
  {
    if (addItemCount > 1)
      addItemCount--;
    addItemCountText.text = addItemCount.ToString();
  }

  public void SelectEquipSlot(int slot)
  {
    addItemSlot = slot;
    for (int i = 0; i < addItemEquipSlots.Length; i++)
    {
      if (i == slot)
        addItemEquipSlots[i].image.color = new Color(0, 1, 0, 1);
      else
        addItemEquipSlots[i].image.color = new Color(1, 1, 1, 1);
    }
  }

  public void AddItem()
  {
    int itemNumber = 0;

    if (addItemSlot == -1 || addItemTitle.text == "" || !int.TryParse(addItemNumber.text, out itemNumber))
    {
      Debug.Log($"Cannot add item ({addItemTitle.text}, {addItemNumber.text}, {addItemSlot})");
      return;
    }

    var shopItems = LoadData();

    if (shopItems.Any(x => x.SpriteName == addItemNumber.text))
      return; // Already added

    shopItems.Add(new ShopItemData { Name = addItemTitle.text, SpriteName = addItemNumber.text, CountTotal = 2, CountLeft = addItemCount, EquipSlot = (EquipSlot)addItemSlot });

    ListItems(shopItems);
    UpdateSizes();
    FilterItems();

    addItemPanel.SetActive(false);

    SaveData();
  }

  public void CancelAddItem()
  {
    addItemPanel.SetActive(false);
  }
}

[Serializable]
public class ShopData
{
  public ShopItemData[] ShopItems;
}

[Serializable]
public class ShopItemData
{
  public string Name;
  public string SpriteName;
  public int CountTotal;
  public int CountLeft;
  public EquipSlot EquipSlot;
}

public enum EquipSlot : short
{
  Chest,
  Head,
  Feet,
  Hand,
  Consumable
}