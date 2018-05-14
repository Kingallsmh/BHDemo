using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damagable {

    void TakeDamage(int dmg);

    void TakeDamage(int dmg, Vector2 location);

    void PushBack(Vector2 push);
}
