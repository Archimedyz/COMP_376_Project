using System.Collections;
using UnityEngine;

public class Stats
{
    private int _lvl;
    private int _exp;
    private float _hp;
    private int _str;
    private int _def;
    private int _spd;
    private int[] _rate;
    private float _minDmg;
    private float _maxDmg;
    private bool _isLevelUp;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lvl"></param>
    /// <param name="hp"></param>
    /// <param name="str"></param>
    /// <param name="def"></param>
    /// <param name="speed"></param>
    /// <param name="rate">4 int array size for level up rate</param>
    public Stats(int lvl, float hp, int str, int def, int speed, int[] rate)
    {
        _lvl = lvl;
        Hp = hp;
        Str = str;
        Def = def;
        Spd = speed;
        _rate = rate;
        _exp = 0;
        _isLevelUp = false;
    }

    private float DmgDiff
    {
        get { return (Str * 0.15f + Spd * 0.02f); }
    }

    private float CritChance
    {
        get { return (Spd * 0.00015f + 0.01f); }
    }
    public int Level
    {
        get { return _lvl; }
    }
    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            while (_exp >= 100)
            {
                Exp -= 100;
                LevelUp();
                IsLevelUp = true;
            }
        }
    }
    public bool IsLevelUp
    {
        get { return _isLevelUp; }
        set { _isLevelUp = value; }
    }
    public float Hp
    {
        get { return _hp + Def*2; }
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
        set { _def = value;}
    }
    public int Spd
    {
        get { return _spd; }
        set { _spd = value; }
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

    public void LevelUp()
    {
        _lvl++;
        Hp = _hp + _rate[0];
        Str = _str + _rate[1];
        Def = _def + _rate[2];
        Spd = _spd + _rate[3];
        _isLevelUp = false;
    }
}
