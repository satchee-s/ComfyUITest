using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderManager : MonoBehaviour
{
    bool isMale = true;

    public void SelectMale()
    {
        isMale = true;
    }

    public void SelectFemale()
    {
        isMale = false;
    }

    public bool GetIsMale()
    {
        return isMale;
    }
}
