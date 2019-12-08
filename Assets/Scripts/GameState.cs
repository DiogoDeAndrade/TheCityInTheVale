using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public Vector3      position;
    public Quaternion   rotation;

    [System.Serializable]
    public class InventoryItem
    {
        public GameItem item;
        public int      count;
    }
    public List<InventoryItem> inventory;


    public enum DataType { Unknown, Bool, Int, Float };

    [System.Serializable]
    public class DataItem
    {
        public string   name;
        public DataType dataType;
        public bool     bData;
        public int      iData;
        public float    fData;
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
                break;
            }
        }

        inventory.RemoveAll((i) => i.count <= 0);
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

    public bool HasItem(string itemName, int count = -1)
    {
        foreach (var i in inventory)
        {
            if (i.item.IsThisTheItem(itemName))
            {
                return (i.count >= count);
            }
        }

        return false;
    }

    public bool HasPickedUp(GameItem item)
    {
        return GetBool("PickedUp(" + item.name + ")");
    }

    public void SetBool(string name, bool value)
    {
        DataItem dataItem = GetDataItem(name, true);

        dataItem.dataType = DataType.Bool;
        dataItem.bData = value;
        if (value) { dataItem.iData = 1; dataItem.fData = 1.0f; }
        else { dataItem.iData = 0; dataItem.fData = 0.0f; }
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
        dataItem.fData = value;
        if (value == 0) dataItem.bData = false;
        else dataItem.bData = true;
    }

    public int GetInt(string name)
    {
        DataItem dataItem = GetDataItem(name, false);

        if (dataItem == null) return 0;
        if (dataItem.dataType != DataType.Int) return 0;

        return dataItem.iData;
    }

    public void SetFloat(string name, float value)
    {
        DataItem dataItem = GetDataItem(name, true);

        dataItem.dataType = DataType.Float;
        dataItem.fData = value;
        dataItem.iData = Mathf.FloorToInt(value);
        if (value == 0.0f) dataItem.bData = false;
        else dataItem.bData = true;
    }

    public float GetFloat(string name)
    {
        DataItem dataItem = GetDataItem(name, false);

        if (dataItem == null) return 0.0f;
        if (dataItem.dataType != DataType.Float) return 0.0f;

        return dataItem.fData;
    }

    public bool Exists(string name)
    {
        return GetDataItem(name, false) != null;
    }

    DataItem GetDataItem(string name, bool create_if_not_exist)
    {
        string n = name.ToLower();

        foreach (var v in dataItems)
        {
            if (v.name.ToLower() == n) return v;
        }

        if (create_if_not_exist)
        {
            DataItem di = new DataItem
            {
                name = n,
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

    public bool EvaluateCondition(string condition)
    {
        string c = condition;
        bool   negate = false;
        if (c[0] == '!')
        {
            negate = true;
            c = condition.Substring(1);
        }

        bool b = GetBool(c);

        if (negate) return !b;

        return b;
    }
} 