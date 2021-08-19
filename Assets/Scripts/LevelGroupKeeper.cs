using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGroupKeeper : MonoBehaviour
{
    [SerializeField] private LevelGroup _levelGroup;

    public LevelGroup LevelGroup => _levelGroup;
}
