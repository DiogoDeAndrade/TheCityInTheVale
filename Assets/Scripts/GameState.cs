using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public Vector3 position;
    public Quaternion rotation;

    [System.Serializable]
    public class InventoryItem
    {
        public GameItem item;
        public int count;
    }
    public List<InventoryItem> inventory;


    public enum DataType { Unknown, Bool, Int };

    [System.Serializable]
    public class DataItem
    {
        public string name;
        public DataType dataType;
        public bool bData;
        public int iData;
    }

    public List<DataItem> dataItems;

    public void AddItemToInventory(GameItem item)
    {
        foreach (var i in inventory)
        {
            if (i.item == item)
            {
                i.count++;
                return;
            }
        }

        InventoryItem ii = new InventoryItem
        {
            count = 1,
            item = item
        };

        inventory.Add(ii);
    }

    public void RemoveFromInventory(GameItem item, int howMany = 1)
    {
        foreach (var i in inventory)
        {
            if (i.item == item)
            {
                i.count -= howMany;
                return;
            }
        }

        inventory.RemoveAll((i) => i.count == 0);
    }

    public bool HasItem(GameItem item, int count = -1)
    {
        foreach (var i in inventory)
        {
            if (i.item == item)
            {
                return (i.count >= count);
            }
        }

        return false;
    }

    public void SetBool(string name, bool value)
    {
        DataItem dataItem = GetDataItem(name, true);

        dataItem.dataType = DataType.Bool;
        dataItem.bData = value;
        if (value) dataItem.iData = 1;
        else dataItem.iData = 0;
    }

    public bool GetBool(string name)
    {
        DataItem dataItem = GetDataItem(name, false);

        if (dataItem == null) return false;
        if (dataItem.dataType != DataType.Bool) return false;

        return dataItem.bData;
    }

    public void SetInt(string name, int value)
    {
        DataItem dataItem = GetDataItem(name, true);

        dataItem.dataType = DataType.Int;
        dataItem.iData = value;
        if (value == 0) dataItem.bData = false;
        else dataItem.bData = true;
    }

    public int GetInt(string name)
    {
        DataItem dataItem = GetDataItem(name, false);

        if (dataItem == null) return 0;
        if (dataItem.dataType != DataType.Int) return 0;

        return (int)dataItem.iData;
    }

    public bool Exists(string name)
    {
        return GetDataItem(name, false) != null;
    }

    DataItem GetDataItem(string name, bool create_if_not_exist)
    {
        foreach (var v in dataItems)
        {
            if (v.name == name) return v;
        }

        if (create_if_not_exist)
        {
            DataItem di = new DataItem
            {
                name = name,
                dataType = DataType.Unknown,
                bData = false,
                iData = 0
            };

            dataItems.Add(di);

            return di;
        }

        return null;
    }

    public void InitExpressionEvaluator()
    {

    }
} 