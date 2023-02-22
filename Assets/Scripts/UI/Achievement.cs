using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Achievement", menuName = "")]
public class Achievement : ScriptableObject
{
    public int _id;
    public string _name;
    public string _description;
    public bool _isUnlocked;


    public Achievement(int id, string name, string description, bool isUnlocked = false)
    {
        _id = id;
        _name = name;
        _description = description;
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

    public string Description
    {
        get { return _description; }
    }
    
    
    public bool IsUnlocked
    {
        get { return _isUnlocked; }
        set { _isUnlocked = value; }
    }
}
