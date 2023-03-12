using UnityEngine;
using MyClasses;
using MyClasses.UI;

public class BootEvent : MonoBehaviour
{
    public void OnPreLoad()
    {
        this.LogInfo("OnPreLoad", "You should do something like configuration", ELogColor.DARK_UI);
    }

    public void OnPostLoad()
    {
        this.LogInfo("OnPostLoad", "You should do something like initialize SDK", ELogColor.DARK_UI);
    }
}
