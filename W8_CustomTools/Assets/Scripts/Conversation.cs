using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation
{
    public List<Line> lines = new List<Line>();
}

[System.Serializable]
public class Line
{
    public string line;
    public List<Response> responses = new List<Response>();
}

[System.Serializable]
public class Response
{
    public string response;
}
