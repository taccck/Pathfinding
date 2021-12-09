using System;
using System.Collections.Generic;
using System.Collections;

public class SortedNodeSet : IEnumerable<Node>
{
    public Node this[int index] => Nodes[index];
    public int Count => Nodes.Length;

    private Node[] Nodes { get; set; }

    public SortedNodeSet() => Nodes = Array.Empty<Node>();
    
    public void Add(Node node)
    {
        for (int i = 0; i < Count; i++)
        {
            if (Nodes[i] == node) //only sort if the node is already in the array
            {
                Resort(i);
                return;
            }
        }

        Node[] newNode = new Node[Count + 1];
        int j = 0;
        bool added = false;
        for (int i = 0; i < newNode.Length; i++)
        {
            if (!added)
                if (i >= Nodes.Length || node.FCost < Nodes[i].FCost) //find where to put the new node
                {
                    newNode[i] = node;
                    added = true;
                    continue;
                }

            newNode[i] = Nodes[j++];
        }

        Nodes = newNode;
    }

    public void Resort(int index)
    {
        //nodes should only be sorted down
        Node[] newNodes = new Node[Count];
        int j = 0;
        bool sorted = false;
        for (int i = 0; i < Count; i++)
        {
            if (i == index && !sorted) return; //no sorting needed

            if (!sorted)
                if (Nodes[index].FCost < Nodes[i].FCost) //find where to put the node that needs resorting
                {
                    newNodes[i] = Nodes[index];
                    sorted = true;
                    continue;
                }

            if (j == index) j++; //skip the node to resort at its old position 

            newNodes[i] = Nodes[j++];
        }

        Nodes = newNodes;
    }

    public void Remove(int index)
    {
        if (index < 0 || index > Count - 1) throw new IndexOutOfRangeException();

        Node[] newNode = new Node[Count - 1];
        int j = 0;
        for (int i = 0; i < Count; i++)
        {
            if (i == index) continue;

            newNode[j++] = Nodes[i];
        }

        Nodes = newNode;
    }

    public bool Remove(Node node)
    {
        for (int i = 0; i < Count; i++)
        {
            if (Nodes[i] == node)
            {
                Remove(i);
                return true;
            }
        }

        return false;
    }

    public IEnumerator<Node> GetEnumerator()
    {
        foreach (Node n in Nodes)
        {
            yield return n;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}