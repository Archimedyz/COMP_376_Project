using System.Collections;

public class Stats {

    private float _hp;
    private int _str;
    private int _def;
    private int _spd;

    public float Hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public int Str
    {
        get { return _str; }
        set { _str = value; }
    }
    public int Def
    {
        get { return _def; }
        set { _def = value; }
    }
    public int Spd
    {
        get { return _spd-4; }
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
    /// <param name="strValue"></param>
    /// <returns></returns>
    public float TakeDamage(int strValue)
    {
        Hp -= strValue - Def;
        return Hp;
    }
}
