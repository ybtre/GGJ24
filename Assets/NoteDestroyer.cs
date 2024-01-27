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
        other.gameObject.transform.DOComplete();
        other.gameObject.transform.DOScale(Vector3.zero, 1f)
          .OnComplete(()=>
          {
            Manager.TagNoteDestroy(other.gameObject);
          });
      }
    }
  }
}
