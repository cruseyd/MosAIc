using UnityEngine;

public abstract class Stat {
    public string name;
    public virtual int value { get; set; }
    private int _value;
    public virtual int minValue { get; protected set; }
    public virtual int maxValue { get; protected set; }

    public Stat(string name_, int minValue_, int maxValue_)
    {
        name = name_;
        minValue = minValue_;
        maxValue = maxValue_;
        value = minValue;
    }
}

public class CardStat : Stat {
    public override int value { get; set; }
    public Card card {get; protected set;}
    public CardStat(string name_, int minValue_, int maxValue_, Card card_)
    : base(name_, minValue_, maxValue_)
    {
        card = card_;
    }
}