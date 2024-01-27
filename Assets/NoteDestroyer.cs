using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDestroyer : MonoBehaviour
{
  public GameManager Manager;
  private void OnTriggerEnter(Collider other)
  {
    if (other != null)
    {
      if (other.name.Contains("Note"))
      {
        Manager.TagNoteDestroy(other.gameObject);
      }
    }
  }
}
