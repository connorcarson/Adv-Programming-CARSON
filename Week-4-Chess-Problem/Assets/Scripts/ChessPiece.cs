using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChessPiece
{
    public enum PieceType
    {
        Empty,
        Queen,
        King,
        Rook,
        Knight,
        Bishop,
        Pawn
    }
    public PieceType myType;

    public enum PieceColor
    {
        None,
        Black,
        White
    }
    public PieceColor myColor;
    
    public ChessPiece(PieceType type, PieceColor color)
    {
        myType = type;
        myColor = color;
    }
}
