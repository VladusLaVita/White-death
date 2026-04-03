using System;
using UnityEngine;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    [Header("Настройка параметров игрока")]
    public int player_hp = 100;
    public int damage_obstacles = 25;
    public int damage_vendigo = 50;

    public List<string> obstacles = new List<string>();
    public Transform vendigo;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (obstacles.Contains(collision.gameObject.name))
        {
            Debug.Log("Есть столкновение!");
        }
    }
}
