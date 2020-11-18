public delegate void OnTurnEnd();
public interface ITurnReceiver
{
    void ReceiveTurn(OnTurnEnd onTurnEnd);
}