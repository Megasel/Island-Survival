using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftButton : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    private void OnEnable()
    {
        aud.playOnAwake = false;
        aud.loop = false;
    }
    public void Craft()
    {
        if (GetComponent<Button>().interactable)
        {
            aud.Play();
        }
    }
}
