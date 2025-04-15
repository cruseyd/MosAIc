using UnityEngine;

public class CardStack : CardZone
{
    public CardStack() : base() {}
    public CardStack(CardStack zone_) : base(zone_) {}
    public CardStack(CardZoneName name_, int playerID_) : base(name_, playerID_)
    {
    }
    protected override int Serialize(CardZoneIndex index)
    {
        return index.z;
    }
    protected override CardZoneIndex Deserialize(int position)
    {
        return new CardZoneIndex(0, 0, position);
    }
}
