using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvispaScript : EnemyScript {

    protected override void Start() {
        base.Start();
        velocity = -2f;
    }

}
