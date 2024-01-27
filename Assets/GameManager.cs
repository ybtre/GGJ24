using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum Location
{
  TOP = 0,
  MID = 1,
  BOT = 2,
}

public enum Player
{
  P1,
  P2,
}

public class GameManager : MonoBehaviour
{
  [Header("Stats")]
  public int p1_score = 0;
  public int p1_mana = 0;

  public int p2_score = 0;
  public int p2_mana = 0;

  public int max_mana = 100;

  private int low = 20;
  private int med = 40;
  private int high = 60;

  [Space(15)]
  [Header("References")]
  public NoteCollision note_pf;

  public Transform[] left_notes_start;
  public GameObject[] left_note_targets;

  public Transform[] right_notes_start;
  public GameObject[] right_note_targets;

  public List<NoteCollision> notes;
  public List<GameObject> destroyed_notes = new List<GameObject>();

  public TextMeshProUGUI p1_score_tmp;
  public TextMeshProUGUI p2_score_tmp;

  public Slider p1_mana_slider;
  public Slider p2_mana_slider;

  private void Start()
  {
    Application.targetFrameRate = 60;
  }

  public float note_spawn_interval = 1f;
  public float note_spawn_timer = 0f;
  private void Update()
  {
    note_spawn_timer += Time.deltaTime;

    if (note_spawn_timer > note_spawn_interval)
    {
      var loc1 = Random.Range(0, 3);
      CreateNote(loc1, Player.P1);

      var loc2 = Random.Range(0, 3);
      CreateNote(loc2, Player.P2);

      note_spawn_timer = 0;
    }

    P1Abilities();
    P2Abilities();

    p1_score_tmp.text = p1_score.ToString();
    p2_score_tmp.text = p2_score.ToString();

    p1_mana_slider.value = p1_mana;
    p2_mana_slider.value = p2_mana;
  }

  public float note_destroy_interval = 5f;
  public float note_destroy_timer = 0;
  private void LateUpdate()
  {
    note_destroy_timer += Time.deltaTime;

    if (note_destroy_timer > note_destroy_interval)
    {
      PurgeNotes();
      note_destroy_timer = 0;
    }
  }

  private void CreateNote(int LOC, Player P)
  {
    if (P == Player.P1)
    {
      NoteCollision new_note;
      Vector3 target;
      InstantiateNoteLeft(LOC, P, out new_note, out target);

      new_note.transform.DOScale(0.17f, .1f);
      new_note.transform.DOMove(target, 4.0f)
        .SetEase(Ease.Linear);
      if(LOC == 0)
      {
        new_note.transform.DORotate(new Vector3(0, -90, 38), .1f);
      }
      else if(LOC == 1)
      {
        new_note.transform.DORotate(new Vector3(-38, 0, 0), .1f);
      }

      notes.Add(new_note);
    }
    else if (P == Player.P2)
    {
      NoteCollision new_note;
      Vector3 target;
      InstantiateNoteRight(LOC, P, out new_note, out target);

      new_note.transform.DOScale(0.17f, .1f);
      new_note.transform.DOMove(target, 4.0f)
        .SetEase(Ease.Linear);
      new_note.transform.DORotate(new Vector3(0, -90, 38), .1f);

      notes.Add(new_note);
    }

  }

  private void InstantiateNoteLeft(int LOC, Player P, out NoteCollision new_note, out Vector3 target)
  {
    new_note = Instantiate(note_pf, left_notes_start[LOC]);
    target = new Vector3(
      left_note_targets[LOC].transform.position.x + 1f,
      left_note_targets[LOC].transform.position.y,
      left_note_targets[LOC].transform.position.z);
    new_note.location = (Location)LOC;
    new_note.p = P;
  }

  private void InstantiateNoteRight(int LOC, Player P, out NoteCollision new_note, out Vector3 target)
  {
    new_note = Instantiate(note_pf, right_notes_start[LOC]);
    target = new Vector3(
      right_note_targets[LOC].transform.position.x - 1f,
      right_note_targets[LOC].transform.position.y,
      right_note_targets[LOC].transform.position.z);
    new_note.location = (Location)LOC;
    new_note.p = P;
  }

  public void TagNoteDestroy(GameObject OBJ)
  {
    var t = OBJ.GetComponent<NoteCollision>();
    t.DOComplete();
    destroyed_notes.Add(t.gameObject);
    notes.Remove(t);
    t.gameObject.SetActive(false);
  }

  private void PurgeNotes()
  {
    foreach (var note in destroyed_notes)
    {
      Destroy(note);
    }
    destroyed_notes.Clear();
  }

  public void GivePerfect(Player P)
  {
    if (P == Player.P1)
    {
      p1_score += 50;
      p1_mana += 5;
    }
    if(P == Player.P2)
    {
      p2_score += 50;
      p2_mana += 5;
    }
  }

  public void GiveGood(Player P)
  {
    if (P == Player.P1)
    {
      p1_score += 30;
      p1_mana += 3;
    }
    if (P == Player.P2)
    {
      p2_score += 30;
      p2_mana += 3;
    }
  }

  private void P1Abilities()
  {
    if (Input.GetKeyUp(KeyCode.V))
    {
      if(p1_mana >= low)
      {
        Debug.Log("P1: Ability 1");
        p1_mana -= low;
      }
    }

    if (Input.GetKeyUp(KeyCode.B))
    {
      if (p1_mana >= med)
      {
        Debug.Log("P1: Ability 2");
        p1_mana -= med;
      }
    }

    if (Input.GetKeyUp(KeyCode.N))
    {
      if (p1_mana >= high)
      {
        Debug.Log("P1: Ability 3");
        p1_mana -= high;
      }
    }

    if (Input.GetKeyUp(KeyCode.M))
    {
      if (p1_mana >= med)
      {
        Debug.Log("P1: Defence");
        p1_mana -= med;
      }
    }
  }

  private void P2Abilities()
  {
    if (Input.GetKeyUp(KeyCode.Alpha7))
    {
      if (p2_mana >= low)
      {
        Debug.Log("P2: Ability 1");
        p2_mana -= low;
      }
    }

    if (Input.GetKeyUp(KeyCode.Alpha8))
    {
      if (p2_mana >= med)
      {
        Debug.Log("P2: Ability 2");
        p2_mana -= med;
      }
    }

    if (Input.GetKeyUp(KeyCode.Alpha9))
    {
      if (p2_mana >= high)
      {
        Debug.Log("P2: Ability 3");
        p2_mana -= high;
      }
    }

    if (Input.GetKeyUp(KeyCode.Alpha0))
    {
      if (p2_mana >= med)
      {
        Debug.Log("P2: Defence");
        p2_mana -= med;
      }
    }
  }
}
