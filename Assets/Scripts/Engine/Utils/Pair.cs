using System;
using UnityEngine;

[Serializable]
public struct Pair<U,V> {
    public U first;
    public V second;

    public Pair(U u, V v)
    {
        first = u;
        second = v;
    }
}