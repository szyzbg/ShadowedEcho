using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidModelBugManager
{
    static private Vector3[] historyPosition = new Vector3[10];
    static public void updateHeroPosition(Vector3 p) {
        for (int i = 0; i < 9; i++) {
            historyPosition[i] = historyPosition[i+1];
        }
        historyPosition[9] = p;
    }
    static public Vector3 getHeroPosition() {
        Vector3 toReturn = historyPosition[9];
        for (int i = 9; i > 0; i--) {
            historyPosition[i] = historyPosition[i-1];
        }
        return toReturn;
    }
}
