using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{

    GameManager _gm;
    public GameObject food;
    float timeBetweenSpawn = 2f;
    float timer = 2f;
    void Start()
    {
        _gm = GameManager.instance;
    }


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            spawnFood();
            timer = timeBetweenSpawn;
        }
    }

    void spawnFood()
    {
       
        GameObject _food = Instantiate(food);
        _food.transform.position = new Vector3(Random.Range(_gm.screenLeftBound,_gm.screenRightBound),
                                               0,
                                               Random.Range(_gm.screenDownBound,_gm.screenUpBound)
                                              );
        _food.gameObject.name = "Food";
    }
}
