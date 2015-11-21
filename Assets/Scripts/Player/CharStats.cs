using System.Collections;
using UnityEngine;

public class Stats
{

    private float _hp;
    private int _str;
    private int _def;
    private int _spd;
    private float _minDmg;
    private float _maxDmg;

    private float DmgDiff
    {
        get { return (Str * 0.15f + Spd * 0.02f); }
    }

    private float CritChance
    {
        get { return (Spd * 0.00015f + 0.01f); }
    }

    public float Hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public int Str
    {
        get { return _str; }
        set
        {
            _str = value;
            _minDmg = Str - DmgDiff;
            _maxDmg = Str + DmgDiff;
        }
    }
    public int Def
    {
        get { return _def; }
        set { _def = value; }
    }
    public int Spd
    {
        get { return _spd - 4; }
        set { _spd = value; }
    }

    public Stats(float hp, int str, int def, int speed)
    {
        Hp = hp;
        Str = str;
        Def = def;
        Spd = speed;
    }

    public bool isDead()
    {
        return Hp <= 0.0f;
    }
    /// <summary>
    /// With the attack parameter, the health removed is calculated and total remaining is returned.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public float TakeDamage(int damage)
    {
        Hp -= damage - Def;
        return Hp;
    }
    public float DoStaticDamage()
    {
        if (CritChance > Random.value)
            return Str * 2;
        else
            return Str;
    }

    public float DoDynamicDamage()
    {
        if(CritChance>Random.value)
            return Random.Range(_minDmg,_maxDmg)*2;
        else
            return Random.Range(_minDmg, _maxDmg);
    }
}
