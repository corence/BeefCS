using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beefs.util
{
    public interface IVolume<K>
    {
        bool Contains(K volume);
        bool Intersects(K volume);
        K Extend(K volume);
    }

    public class RTree<K, V> where K : IVolume<K> where V : class
    {
        public static readonly int childsPerNode = 3;
        private Node root;

        public RTree()
        {
            this.root = new Node(null);
        }

        public void Add(K key, V value)
        {
            Add(new Leaf(key, value));
        }

        public void Add(Leaf leaf)
        {
            root.Add(leaf);
        }

        public bool Remove(Leaf leaf)
        {
            return root.Remove(leaf);
        }

        public class Node
        {
            public K key;
            public Leaf leaf;
            public List<Node> childs;
            public int numDescendants;

            public Node(Leaf leaf)
            {
                this.leaf = leaf;
                this.childs = new List<Node>();
                this.numDescendants = leaf != null ? 1 : 0;
            }

            public void Add(Leaf leaf)
            {
                if (this.leaf == null)
                {
                    this.leaf = leaf;
                }
                else if (childs.Count < childsPerNode)
                {
                    childs.Add(new Node(leaf));
                }
                else
                {
                    Node target = childs[0];
                    foreach (Node child in childs)
                    {
                        if (child.numDescendants < target.numDescendants)
                        {
                            target = child;
                        }
                    }
                    target.Add(leaf);
                }
                this.key = this.key.Extend(leaf.key);
                ++numDescendants;
            }

            public Leaf Lookup(K query)
            {
                if (leaf.key.Intersects(query))
                {
                    return leaf;
                }
                else
                {
                    foreach (Node child in childs)
                    {
                        Leaf result = child.Lookup(query);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                    return null;
                }
            }

            public bool Remove(Leaf leaf)
            {
                if (this.leaf == leaf)
                {
                    this.leaf = null;
                    --numDescendants;
                    return true;
                }
                else
                {
                    foreach (Node child in childs)
                    {
                        if (child.Remove(leaf))
                        {
                            --numDescendants;
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        public class Leaf
        {
            public readonly K key;
            public readonly V value;

            public Leaf(K key, V value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
