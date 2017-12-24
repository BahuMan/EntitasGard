using System;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T> : IEnumerable<T>
{

    private class PrioNode
    {
        public PrioNode(int p, T v)
        {
            prio = p;
            value = v;
        }
        public int prio;
        public T value;
    }
    private List<PrioNode> list;

    public int Count {get { return list.Count; }}

    public PriorityQueue(): this(10)
    {
    }

    public PriorityQueue(int capacity)
    {
        list = new List<PrioNode>(capacity);
    }

    public void Enqueue(int prio, T value)
    {
        list.Add(new PrioNode(prio, value));
        list.Sort(ComparePrio);
    }

    public T Dequeue()
    {
        T result = list[0].value;
        list.RemoveAt(0);
        return result;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var node in list)
        {
            yield return node.value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }

    private int ComparePrio(PrioNode x, PrioNode y)
    {
        return x.prio - y.prio;
    }
}

