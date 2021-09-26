using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
    public int foodAmount = 100;

    // Update is called once per frame
    void Update()
    {
        if(foodAmount <= 0) {
            Destroy(this);
        }
    }

    public bool TakeFood() {
        if(foodAmount > 0) {
            return true;
        }

        return false;
    }
}
