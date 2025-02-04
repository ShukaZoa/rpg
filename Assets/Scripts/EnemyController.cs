using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy에 대한 정보가 있습니다.
//던전에서 죽었을경우 manager한테 자신이 죽었다고 알리고 delegate를 실행합니다.

public class EnemyController : MonoBehaviour
{
    public delegate Node GetNodeByPosition(Vector3 pos, bool isDungeon, string dungeonName);
    public GetNodeByPosition getNodeByPosition;

    public delegate void CalculateEnemyCountInDungeon(int dungeonNumber, int value);
    public CalculateEnemyCountInDungeon calculateEnemyCountInDungeon;

    //가까운 npc, 가까운 npc가 죽으면 다른걸로(이거 뭘로 할지 결정)
    public bool endOfPooling;

    public int myNumber;

    public int health;
    public int dropMoney;
    public int damage;

    public int nowDungeonParentNumber;

    public List<Transform> targetInDungeon;

    public List<Animator> enemyAnimatorList;

    public int xPos;
    public int yPos;

    public bool enemyIsdead;

    private void Awake()
    {
        targetInDungeon = new List<Transform>();
    }

    private void OnEnable()
    {
        if (endOfPooling)
        {
            calculateEnemyCountInDungeon(nowDungeonParentNumber, 1);
        }
    }

    public void Die()
    {
        enemyIsdead = true;
        transform.gameObject.SetActive(false);
        
        addMoney(dropMoney);
        calculateEnemyCountInDungeon(nowDungeonParentNumber, -1);
        removeEnemyFromDungeonEnemyList(transform);

        transform.GetChild(myNumber).gameObject.SetActive(false);

        enemyAnimatorList[myNumber].ResetTrigger("Attack");

        foreach (var npc in targetInDungeon)
        {
            setNewTargetInDungeonRequestToActiveNpc(npc);
        }

        targetInDungeon.RemoveRange(0, targetInDungeon.Count - 1);

        setEnemyNodeArrayOneIntoZero(xPos, yPos);
        StopAllCoroutines();
    }

    public void RemoveNpcFromTargetList(Transform npc)
    {
        targetInDungeon.Remove(npc);
    }

    public delegate void SetEnemyNodeArrayOneIntoZero(int xPos, int yPos);
    public SetEnemyNodeArrayOneIntoZero setEnemyNodeArrayOneIntoZero;

    public delegate void RemoveEnemyFromDungeonEnemyList(Transform enemy);
    public RemoveEnemyFromDungeonEnemyList removeEnemyFromDungeonEnemyList;
    public delegate void SetNewTargetInDungeonRequestToActiveNpc(Transform target);
    public SetNewTargetInDungeonRequestToActiveNpc setNewTargetInDungeonRequestToActiveNpc;

    public delegate void AddMoney(int value);
    public AddMoney addMoney;
}
