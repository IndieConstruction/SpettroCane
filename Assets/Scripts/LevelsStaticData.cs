using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelsStaticData {
    public List<Target> Targets;
    public List<Piece> Pieces;

    public void GetTargetWaveById(string _Id) {

    }
}


[Serializable]
public class Target {
    public string ID;
    public string TargetLink;
    public string R0;
    public string R1;
    public string R2;
    public string R3;
    public string R4;
    public string R5;
    public string R6;
    public string R7;
    public string R8;
    public string R9;
}

[Serializable]
public class Piece {
    public string ID;
    public string TargetLink;
    public string R0;
    public string R1;
    public string R2;
    public string R3;
    public string R4;
    public string R5;
    public string R6;
    public string R7;
    public string R8;
    public string R9;
}