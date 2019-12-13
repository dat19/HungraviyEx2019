using UnityEngine;

public static class OnGroundChecker
{
    /// <summary>
    /// 衝突の上限数
    /// </summary>
    const int HitMax = 8;

    static ContactFilter2D contactFilter2D = new ContactFilter2D();
    static readonly ContactPoint2D[] contactPoints = new ContactPoint2D[HitMax];

    /// <summary>
    /// 渡したCollider2Dが着地したか(上向きのオブジェクトと接触したか)を確認して、
    /// 着地していたらtrue、そうでなければfalseを返します。
    /// </summary>
    /// <param name="col">チェックしたいCollider2D</param>
    /// <returns>true=着地 / false=空中</returns>
    public static bool Check(Collider2D col)
    {
        contactFilter2D.layerMask = LayerMask.GetMask("Map");

        // 着地チェック。上向きの接触があれば着地
        int hitCount = col.GetContacts(contactFilter2D, contactPoints);
        for (int i = 0; i < hitCount; i++)
        {
            if (contactPoints[i].normal.y >= 0.9f)
            {
                return true;
            }
        }

        return false;
    }
}
