
public class Previewer
{
    public CardIndex source { get; private set; }

    public Previewer(Card card) { source = card.id; }
}