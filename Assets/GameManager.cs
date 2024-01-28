using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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

public enum Winner
{
  NONE,
  P1,
  P2,
}

public class GameManager : MonoBehaviour
{
  [Header("Stats")]
  public int p1_score = 0;
  public int p1_mana = 0;
  public int p1_health = 100;

  public int p2_score = 0;
  public int p2_mana = 0;
  public int p2_health = 100;

  public int dmg = 10;
  public int max_mana = 100;

  public int low = 20;
  public int med = 40;
  public int high = 60;

  [Space(15)]
  [Header("References")]
  public NoteCollision note_pf;
  public GameObject expl_pf;

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

  public Slider[] health_sliders;

  private Winner winner = Winner.NONE;
  private WinnerNum winnerNumber;

  public static GameManager Instance { get; private set; }
  private void Awake()
  {
    // If there is an instance, and it's not me, delete myself.
    DontDestroyOnLoad(this);
    if (Instance != null && Instance != this)
    {
      Destroy(this);
    }
    else
    {
      Instance = this;
    }
  }

  private void Start()
  {
    Application.targetFrameRate = 60;
  }

  public float note_spawn_interval = 1f;
  public float note_spawn_timer = 0f;
  private void Update()
  {
    note_spawn_timer += Time.deltaTime;

    CheckAndTransitionWinner();

    if (note_spawn_timer > note_spawn_interval)
    {
      var loc1 = Random.Range(0, 3);
      CreateNote(loc1, Player.P1);

      var loc2 = Random.Range(0, 3);
      CreateNote(loc2, Player.P2);

      note_spawn_timer = 0;
    }

    p1_score_tmp.text = p1_score.ToString();
    p2_score_tmp.text = p2_score.ToString();

    p1_mana_slider.value = p1_mana;
    p2_mana_slider.value = p2_mana;

    health_sliders[0].value = p1_health;
    health_sliders[1].value = p2_health;
  }

  private void CheckAndTransitionWinner()
  {
    if (p1_health < 0)
    {
      winner = Winner.P1;
      TransitionToGameOver();
    }
    else if (p2_health < 0)
    {
      winner = Winner.P2;
      TransitionToGameOver();
    }

    if (SceneManager.GetActiveScene().name == "GameOver")
    {
      winnerNumber = FindObjectOfType<WinnerNum>();

      if (winner == Winner.P1)
      {
        winnerNumber.tmp.text = "2";
      }
      else if (winner == Winner.P2)
      {
        winnerNumber.tmp.text = "1";
      }
    }
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

  private void TransitionToGameOver()
  {
    SceneManager.LoadScene(1);
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
      if (LOC == 0)
      {
        new_note.transform.DORotate(new Vector3(0, 90, -38), .1f);
      }
      else if (LOC == 1)
      {
        new_note.transform.DORotate(new Vector3(-38, 0, 0), .1f);
      }
      else if (LOC == 2)
      {
        new_note.transform.DORotate(new Vector3(0, -90, 38), .1f);
      }

      notes.Add(new_note);
    }

  }

  public void InstantiateExplosion(Transform AT)
  {
    GameObject new_expl = Instantiate(expl_pf, AT);
    var t = new_expl.GetComponentsInChildren<ParticleSystem>();
    foreach(ParticleSystem p in t)
    {
      p.Play();
    }
  }

  private void InstantiateNoteLeft(int LOC, Player P, out NoteCollision new_note, out Vector3 target)
  {
    new_note = Instantiate(note_pf, left_notes_start[LOC]);
    target = new Vector3(
      left_note_targets[LOC].transform.position.x + 0.2f,
      left_note_targets[LOC].transform.position.y,
      left_note_targets[LOC].transform.position.z);
    new_note.location = (Location)LOC;
    new_note.p = P;
  }

  private void InstantiateNoteRight(int LOC, Player P, out NoteCollision new_note, out Vector3 target)
  {
    new_note = Instantiate(note_pf, right_notes_start[LOC]);
    target = new Vector3(
      right_note_targets[LOC].transform.position.x - 0.2f,
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

}
