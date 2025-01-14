using UnityEngine;

public class LinearCardZone : CardZone
{
    public LinearCardZone() : base() {}
    public LinearCardZone(CardZoneName name_, int playerID_) : base(name_, playerID_)
    {
    }
    protected override int Serialize(CardZoneIndex index)
    {
        return index.x;
    }
    protected override CardZoneIndex Deserialize(int position)
    {
        return new CardZoneIndex(position, 0, 0);
    }
    public override CardZone Clone()
    {
        var clone = new LinearCardZone(name, agent);
        clone.CloneCardsFrom(this);
        return clone;
    }
}
