using UnityEngine;

public class Stat {
    public StatName name;
    public virtual int value
    {
        get;
        set;
    }
    public virtual int minValue { get; protected set; }
    public virtual int maxValue { get; protected set; }

    public Stat(StatName name_)
    {
        name = name_;
        minValue = GameParams.MinValue(name_);
        maxValue = GameParams.MaxValue(name_);
        value = minValue;
    }

    public Stat(StatName name_, int initialValue)
    {
        name = name_;
        minValue = GameParams.MinValue(name_);
        maxValue = GameParams.MaxValue(name_);
        value = initialValue;
    }
}
