using System.Collections;
using UnityEngine;

public class Stats
{
    private int _lvl;
    private int _exp;
    private float _hp;
    private float _maxHp;
    private int _str;
    private int _def;
    private int _spd;
    private int[] _rate;
    private float _minDmg;
    private float _maxDmg;
    private bool _isLevelUp;
    public bool wasCrit;

    public static int[] playerRate = { 20, 2, 2, 2 };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lvl"></param>
    /// <param name="hp"></param>
    /// <param name="str"></param>
    /// <param name="def"></param>
    /// <param name="spd"></param>
    /// <param name="rate">4 int array size for level up rate</param>
    public Stats(int lvl, float hp, int str, int def, int speed, int[] rate)
    {
        init(lvl, (hp + def * 2), hp, str, def, speed, rate);
    }

    public void init(int lvl, float hp, float maxHp, int str, int def, int speed, int[] rate)
    {
        _lvl = lvl;
        Hp = hp;
        MaxHp = maxHp + def * 2;
        Str = str;
        Def = def;
        Spd = speed;
        _rate = rate;
        _exp = 0;
        _isLevelUp = false;
        wasCrit = false;
    }

    private float DmgDiff
    {
        get { return (Str * 0.10f + Spd * 0.02f); }
    }

    private float CritChance
    {
        get { return (Spd * 0.00625f); }
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
        get { return _hp; }
        set
        {
            _hp = value;
            if (_hp < 0)
                _hp = 0;
        }
    }
    public float MaxHp
    {
        get { return _maxHp; }
        set { _maxHp = value; }
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
        get { return _spd; }
        set { _spd = value; }
    }

    public bool isDead()
    {
        return Hp <= 0.0f;
    }
    /// <summary>
    /// With the attack parameter, the health removed is calculated.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public void TakeDamage(int damage)
    {
        if ((damage - Def) > 0)
            Hp -= damage - Def;

    }
    public float DoStaticDamage()
    {
        if (CritChance > Random.value)
        {
            wasCrit = true;
            return Str * 2;
        }
        else
        {
            wasCrit = false;
            return Str;
        }
    }

    public int DoDynamicDamage()
    {
        if (CritChance > Random.value)
        {
            wasCrit = true;
            return (int)(Random.Range(_minDmg, _maxDmg) * 2.0f);
        }
        else
        {
            wasCrit = false;
            return (int)Random.Range(_minDmg, _maxDmg);
        }
    }

    public void LevelUp()
    {
        _lvl++;
        MaxHp = _maxHp + _rate[0] + (_rate[2] * 2);
        Hp = _hp + _rate[0] + (_rate[2] * 2);
        Str = _str + _rate[1];
        Def = _def + _rate[2];
        Spd = _spd + _rate[3];
    }
    public float DamageDealt(int damage)
    {
        if (damage - Def > 0)
            return damage - Def;
        else
            return 0;
    }
}
