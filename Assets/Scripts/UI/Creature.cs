using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "")]
public class Creature : ScriptableObject
{
    public int _id;
    public string _name;
    public int _slainCount;
    public bool _isUnlocked;
    public Sprite _sprite;

    public int _killsToUnlock;
    public int _creatureEncounters;
    public int _creatureKills;

    public Creature(int id, string name, Sprite sprite, int slainCount = 0, bool isUnlocked = false)
    {
        _id = id;
        _name = name;
        _sprite = sprite;
        _slainCount = slainCount;
        _isUnlocked = isUnlocked;
    }
    
    public int Id
    {
        get { return _id; }
    }
    
    public string Name
    {
        get { return _name; }
    }
    
    public int SlainCount
    {
        get { return _slainCount; }
        set { _slainCount = value; }
    }
    
    public bool IsUnlocked
    {
        get { return _isUnlocked; }
        set { _isUnlocked = value; }
    }

    public Sprite Sprite
    {
        get { return _sprite; }
    }

    public int KillsToUnlock
    {
        get { return _killsToUnlock;}
    }

    public int CreatureEncounters
    {
        get { return _creatureEncounters;}
        set { _creatureEncounters = value; }
    }

    public int CreatureKills
    {
        get { return _creatureKills;}
        set { _creatureKills = value; }
    }
}
