using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickInfo
{
    public float time;
    public int clip;
    public int score;
}
public class RecordInfo
{
    public List<ClickInfo> clickInfoList = new List<ClickInfo>();
}
