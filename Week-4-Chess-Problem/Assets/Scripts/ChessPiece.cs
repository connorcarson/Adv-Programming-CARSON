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
    public PieceType type;

    public enum PieceColor
    {
        None,
        Black,
        White
    }
    public PieceColor color;
    
    public ChessPiece(PieceType type, PieceColor color)
    {
        this.type = type;
        this.color = color;
    }
}
