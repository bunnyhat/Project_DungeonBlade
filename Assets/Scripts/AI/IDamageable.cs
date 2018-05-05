using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable  {
    string getObjectName();
    bool takeDamage(int damage);
    GameObject getGameObject();
}
