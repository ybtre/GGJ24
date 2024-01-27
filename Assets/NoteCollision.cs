using UnityEngine;

public class NoteCollision : MonoBehaviour
{
  public GameManager Manager;

  public Collider good;
  public Collider perfect;

  public Location location;
  public Player p;

  private void Awake()
  {
    Manager = FindObjectOfType<GameManager>();
  }

  void Update()
  {
    HandleP1();
    HandleP2();
  }

  private void HandleP1()
  {
    switch (location)
    {
      case Location.TOP:
        {
          if (good != null)
          {
            if (Input.GetKeyUp(KeyCode.A))
            {
              Debug.Log("GOOD");
              Manager.GiveGood(Player.P1);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }

          if (perfect != null)
          {
            if (Input.GetKeyUp(KeyCode.A))
            {
              Debug.Log("PERFECT");
              Manager.GivePerfect(Player.P1);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }
        }
        break;
      case Location.MID:
        {
          if (good != null)
          {
            if (Input.GetKeyUp(KeyCode.W))
            {
              Debug.Log("GOOD");
              Manager.GiveGood(Player.P1);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }

          if (perfect != null)
          {
            if (Input.GetKeyUp(KeyCode.W))
            {
              Debug.Log("PERFECT");
              Manager.GivePerfect(Player.P1);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }
        }
        break;
      case Location.BOT:
        {
          if (good != null)
          {
            if (Input.GetKeyUp(KeyCode.D))
            {
              Debug.Log("GOOD");
              Manager.GiveGood(Player.P1);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }

          if (perfect != null)
          {
            if (Input.GetKeyUp(KeyCode.D))
            {
              Debug.Log("PERFECT");
              Manager.GivePerfect(Player.P1);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }
        }
        break;
    }
  }

  private void HandleP2()
  {
    switch (location)
    {
      case Location.TOP:
        {
          if (good != null)
          {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
              Debug.Log("GOOD");
              Manager.GiveGood(Player.P2);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }

          if (perfect != null)
          {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
              Debug.Log("PERFECT");
              Manager.GivePerfect(Player.P2);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }
        }
        break;
      case Location.MID:
        {
          if (good != null)
          {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
              Debug.Log("GOOD");
              Manager.GiveGood(Player.P2);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }

          if (perfect != null)
          {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
              Debug.Log("PERFECT");
              Manager.GivePerfect(Player.P2);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }
        }
        break;
      case Location.BOT:
        {
          if (good != null)
          {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
              Debug.Log("GOOD");
              Manager.GiveGood(Player.P2);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }

          if (perfect != null)
          {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
              Debug.Log("PERFECT");
              Manager.GivePerfect(Player.P2);
              Manager.TagNoteDestroy(this.gameObject);
            }
          }
        }
        break;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.name == "good")
    {
      good = other;
    }
    if(other.name == "perfect") 
    {
      good = null;
      perfect = other;
    }
  }

  

}
