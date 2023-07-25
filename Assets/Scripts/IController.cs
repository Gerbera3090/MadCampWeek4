using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    public bool CanMove { get; set; }
    public bool IsFacingRight { get; set; }
    public bool IsMoving { get; set; }
    public bool IsAlive { get; set; }
    public void Dead();
    public void CallKnockBack(Vector2 knockBackForceVector, float knockTime);
    public IEnumerator KnockTimeRoutine(float knockTime);

}
