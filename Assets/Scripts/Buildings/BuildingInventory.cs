using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class BuildingInventory : MonoBehaviour
{
    // Leave as -1 if you want to use default stack size in ScriptableObject
    [SerializeField] private int maxStackSize = -1;

    [SerializeField] private int inputStackAmount;
    public int InputStackAmount { get => inputStackAmount; }

    [SerializeField] private int outputStackAmount;
    public int OutputStackAmount { get => outputStackAmount; }

    private List<ItemStack> inputStacks = new List<ItemStack>();
    public List<ItemStack> InputStacks { get => inputStacks; }

    private List<ItemStack> outputStacks = new List<ItemStack>();
    public List<ItemStack> OutputStacks {  get => outputStacks; }

    public UnityEvent InputStackCountChange;
    public UnityEvent OutputStackCountChange;

    public UnityEvent InputStackModified;
    public UnityEvent OutputStackModified;

    // Returns wether it was successful at adding a new stack or not
    public bool AddInputStack(ItemDataSO itemData, int count = 1)
    {
        if (inputStacks.Count < inputStackAmount)
        {
            inputStacks.Add(new ItemStack(itemData, count));
            InputStackModified?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Returns wether it was successful at adding a new stack or not
    public bool AddOutputStack(ItemDataSO itemData, int count = 1)
    {
        if (outputStacks.Count < outputStackAmount)
        {
            outputStacks.Add(new ItemStack(itemData, count));
            OutputStackModified?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeInputStackCount(ItemDataSO item, int change)
    {
        int index = inputStacks.FindIndex(stack => stack.Item == item);

        if (index == -1) return;

        int maxStack = maxStackSize < 0 ? item.maxStackSize : maxStackSize;

        inputStacks[index].Count = Mathf.Clamp(inputStacks[index].Count + change, 0, maxStack);

        if (inputStacks[index].Count <= 0)
        {
            inputStacks.RemoveAt(index);
            InputStackModified?.Invoke();
        }

        InputStackCountChange?.Invoke();
    }

    public void ChangeOutputStackCount(ItemDataSO item, int change)
    {
        int index = outputStacks.FindIndex(stack => stack.Item == item);

        if (index == -1) return;

        outputStacks[index].Count += change;
        
        if (outputStacks[index].Count <= 0)
        {
            outputStacks.RemoveAt(index);
            OutputStackModified?.Invoke();
        }

        OutputStackCountChange?.Invoke();
    }

    public int GetMaxStackSize(ItemDataSO item)
    {
        return maxStackSize < 0 ? item.maxStackSize : maxStackSize;
    }

    public ItemStack GetInputStack(ItemDataSO item)
    {
        return inputStacks.Find(stack => stack.Item == item);
    }

    public ItemStack GetOutputStack(ItemDataSO item)
    {
        return outputStacks.Find(stack => stack.Item == item);
    }
}
