
using System;
using System.Collections.Generic;

public class MTGCard {
    public string Name { get; set; }
    public long ID { get; set; }
    public int CMC { get; set; }
    public string ColorCost { get; set; }
    public string SetCode { get; set; }

    public MTGCard(string name, long iD, int cMC, string colorCost, string setCode) {
        Name = name;
        ID = iD;
        CMC = cMC;
        ColorCost = colorCost;
        SetCode = setCode;
    }
}
