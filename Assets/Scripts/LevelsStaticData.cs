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
    public string Window;
    public string Span;
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

    internal string GetR(int i)
    {
        string s = "";
        switch (i)
        {
            case 0:
                s = R0;
                break;
            case 1:
                s = R1;
                break;
            case 2:
                s = R2;
                break;
            case 3:
                s = R3;
                break;
            case 4:
                s = R4;
                break;
            case 5:
                s = R5;
                break;
            case 6:
                s = R6;
                break;
            case 7:
                s = R7;
                break;
            case 8:
                s = R8;
                break;
            case 9:
                s = R9;
                break;
        }
        return s;
    }
}

[Serializable]
public class Piece {
    public string ID;
    //public string TargetLink;
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

    internal string GetR(int i)
    {
        string s = "";
        switch (i)
        {
            case 0:
                s = R0;
                break;
            case 1:
                s = R1;
                break;
            case 2:
                s = R2;
                break;
            case 3:
                s = R3;
                break;
            case 4:
                s = R4;
                break;
            case 5:
                s = R5;
                break;
            case 6:
                s = R6;
                break;
            case 7:
                s = R7;
                break;
            case 8:
                s = R8;
                break;
            case 9:
                s = R9;
                break;
        }
        return s;
    }
}