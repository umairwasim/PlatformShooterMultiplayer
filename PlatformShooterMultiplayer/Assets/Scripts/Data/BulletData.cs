using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bullet", menuName ="Data/BulletData")]
public class BulletData : ScriptableObject
{
    public float speed = 10f;
    public int damage = 10;
}
