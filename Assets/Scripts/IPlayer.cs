using Pong;

public abstract class APlayer
{
    protected bool isP1;

    protected APlayer(bool isP1)
    {
        this.isP1 = isP1;
    }

    public abstract Action GetAction(ref GameState gameState);
}