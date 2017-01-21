using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level {
    public List<Target> Targets { get; set; }
    public List<Piece> Pieces { get; set; }
}



public class Target {
    public string ID { get; set; }
    public string TargetLink { get; set; }
    public string R0 { get; set; }
    public string R1 { get; set; }
    public string R2 { get; set; }
    public string R3 { get; set; }
    public string R4 { get; set; }
    public string R5 { get; set; }
    public string R6 { get; set; }
    public string R7 { get; set; }
    public string R8 { get; set; }
    public string R9 { get; set; }
}

public class Piece {
    public string ID { get; set; }
    public string TargetLink { get; set; }
    public string R0 { get; set; }
    public string R1 { get; set; }
    public string R2 { get; set; }
    public string R3 { get; set; }
    public string R4 { get; set; }
    public string R5 { get; set; }
    public string R6 { get; set; }
    public string R7 { get; set; }
    public string R8 { get; set; }
    public string R9 { get; set; }
}