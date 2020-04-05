using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation
{
    public List<Line> lines = new List<Line>();
    public bool displayed = true;

    public Conversation()
    {
        lines.Add(new Line());
    }
}

[System.Serializable]
public class Line
{
    public string line = "Write  NPC dialogue here...";
    public List<Response> responses = new List<Response>();
    public bool endConversation = false;
    public bool responsesDisplayed = true;

    public Line()
    {
        responses.Add(new Response());
    }
}

[System.Serializable]
public class Response
{
    public string response = "Write  Player response here...";
}
