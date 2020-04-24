using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxItems = 0;
    public GameObject selectedItem;

    /// <summary> Basic method for adding an Item. Returns false if inventory is full.
    /// Will add item to a stack (itemIsCopy = true) if a stack of that item exists, otherwise adds an unstackable item.  </summary>
    public bool AddItem(GameObject obj)
    {
        if (!InvIsFull())
        {
            Item stack = items.Find(X => X.itemIsCopy == true && X.name == obj.name);
            if (stack != null)
                stack.quantity += 1;
            else
                items.Add(new Item(obj));
            return true;
        }
        return false;
    }

    /// <summary> Adds an item to the inventory and forces it to be a copy (aka stackable). Returns false if inventory is full.
    /// This increases the quantity of an existing stack, or creates a new stack if none exist. </summary>
    public bool AddStackableItem(GameObject obj)
    {
        if (!InvIsFull())
        {
            Item stack = items.Find(X => X.itemIsCopy == true && X.name == obj.name);
            if (stack != null)
                stack.quantity += 1;
            else
                items.Add(new Item(obj.name, obj, 1));
            return true;
        }
        return false;
    }

    /// <summary> Adds an item to the inventory and forces it to be unique. Returns false if inventory is full.
    /// In most </summary>
    public bool AddUniqueItem(GameObject obj)
    {
        if (!InvIsFull())
        {
            items.Add(new Item(obj));
            return true;
        }
        return false;
    }

    /// <summary> Removes the selected item from the list. </summary>
    public GameObject RemoveItem()
    {
        Item foundItem = items.Find(X => X.obj = selectedItem);
        if (foundItem != null)
            items.Remove(foundItem);
        return foundItem.obj;
    }

    /// <summary> Removes item at the given index from the list.  </summary>
    public GameObject RemoveItem(int index)
    {
        if (items.Count > index)
        {
            Item foundItem = items[index];
            items.RemoveAt(index);
            return foundItem.obj;
        }
        return null;
    }

    private bool InvIsFull()
    {
        return items.Count >= maxItems;
    }
}

public class Item
{
    public string name;
    public GameObject obj;
    public bool itemIsCopy = false;
    public int quantity = 1;

    public Item(string Name, GameObject Prefab, int StartingQuantity)
    {
        name = Name;
        obj = Prefab;
        quantity = StartingQuantity;
        itemIsCopy = true;
    }

    public Item(GameObject ObjRef)
    {
        name = ObjRef.name;
        obj = ObjRef;
        quantity = 1;
        itemIsCopy = false;
    }
}
