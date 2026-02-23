using UnityEngine;

public class FoodObject : CellObject
{
    public int AmountGranted = 10;
    public override void PlayerEnterd()
    {
        Destroy(gameObject);// 씬에서 FoodObject 제거

        GameManager.Instance.ChangeFood(AmountGranted);// 게임 매니저에 음식량 증가 알림
    }
}
