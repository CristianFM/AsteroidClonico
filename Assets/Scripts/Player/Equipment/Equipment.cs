using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    // estas funciones nos serviran para interactuar con los diferentes equipamientos cuando los tratemos desde un 
    //punto de vista aleatorio y no sepamos que script estamos modificando.
    public abstract void setAttributesValues(string variable, float value);
    public abstract float getAttributesValues(string variable);
    public abstract string getLvText(string text);
    public abstract float[] getAllAttributesValues();
    public abstract void lvUp();

}
