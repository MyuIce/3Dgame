using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameItem
{
    string GetItemname();
    Sprite GetItemicon();
    string GetItemexplanation();
}
