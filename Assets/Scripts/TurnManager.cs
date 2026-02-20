using UnityEngine;

public class TurnManager 
{
    private int m_TurnCount;

    public TurnManager()
    {
        m_TurnCount = 1;
    }
    public void Tick()
    {
        m_TurnCount += 1;
        Debug.Log("ео:" + m_TurnCount);

        if (OnTick != null)
        {
            OnTick.Invoke();
        }
    }

    public event System.Action OnTick;
}

    