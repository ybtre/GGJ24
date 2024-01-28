using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
  public Transform[] caterpillars;

  private  List<Transform> list = new List<Transform>();
  private GameManager manager;

  private void Awake()
  {
    manager = GetComponent<GameManager>();
    list.Add(caterpillars[0]);
    list.Add(caterpillars[1]);
  }


  bool p1_defence_active = false;
  float p1_defence_dur = 1;
  float p1_defence_timer = 0;
  bool p2_defence_active = false;
  float p2_defence_dur = 1;
  float p2_defence_timer = 0;
  // Update is called once per frame
  void Update()
  {
    if(Input.GetKeyDown(KeyCode.Space)) 
    {
      manager.p1_mana += 100;
      manager.p2_mana += 100;
    }
    P1Abilities();
    P2Abilities();

    if(p1_defence_active)
    {
      p1_defence_timer += Time.deltaTime;

      if(p1_defence_timer > p1_defence_dur)
      {

        p1_defence_timer = 0;
        p1_defence_active = false;
      }
    }
    if (p2_defence_active)
    {
      p2_defence_timer += Time.deltaTime;

      if (p2_defence_timer > p2_defence_dur)
      {

        p2_defence_timer = 0;
        p2_defence_active = false;
      }
    }
  }


  private void P1Abilities()
  {
    if (Input.GetKeyUp(KeyCode.V))
    {
      if(!p2_defence_active)
      {
        if (manager.p1_mana >= manager.low)
        {
          Debug.Log("P1: Ability 1");
          manager.p1_mana -= manager.low;

          caterpillars[0].SetPositionAndRotation(list[0].position, caterpillars[0].rotation);
          caterpillars[0].DOScale(.17f, .1f);

          caterpillars[1].SetPositionAndRotation(list[1].position, caterpillars[1].rotation);
          caterpillars[1].DOScale(.17f, .1f);

          caterpillars[0].DOPunchScale(new Vector3(.3f, 0, 0), .2f, 5, 1f);
          caterpillars[1].DOShakeScale(.2f, .1f, 2);

          manager.p2_health -= manager.dmg;
        }
      }
    }

    if (Input.GetKeyUp(KeyCode.B))
    {
      if (manager.p1_mana >= manager.med)
      {
        Debug.Log("P1: Defence");
        manager.p1_mana -= manager.med;

        caterpillars[0].DOShakeScale(2f, .2f, 15);
        p1_defence_active = true;
      }
    }
  }

  private void P2Abilities()
  {
    if (Input.GetKeyUp(KeyCode.I))
    {
      if(!p1_defence_active)
      {
        if (manager.p2_mana >= manager.low)
        {
          Debug.Log("P2: Ability 1");
          manager.p2_mana -= manager.low;

          caterpillars[0].SetPositionAndRotation(list[0].position, caterpillars[0].rotation);
          caterpillars[0].DOScale(.17f, .1f);

          caterpillars[1].SetPositionAndRotation(list[1].position, caterpillars[1].rotation);
          caterpillars[1].DOScale(.17f, .1f);

          caterpillars[1].DOPunchScale(new Vector3(.3f, 0, 0), .2f, 5, 1f);
          caterpillars[0].DOShakeScale(.2f, .1f, 2);

          manager.p1_health -= manager.dmg;
        }
      }
    }

    if (Input.GetKeyUp(KeyCode.O))
    {
      if (manager.p2_mana >= manager.med)
      {
        Debug.Log("P2: Defence");
        manager.p2_mana -= manager.med;

        caterpillars[1].DOShakeScale(2f, .2f, 15);
        p2_defence_active = true;
      }
    }
  }
}
