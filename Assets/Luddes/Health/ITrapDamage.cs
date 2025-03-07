using UnityEngine;

public interface ITrapDamage // Interface mainly for DamageBase
{
    int damageNr {  get; }

    void ApplyDamage(GameObject obj);
}
