using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

public class CheckmateCheck : MonoBehaviour
{
    public string checkMateInOne;
    public string noCheckMate;

    private Piece[,] board = new Piece[8, 8];
    public enum Piece
    {
        Empty,
        Queen,
        King,
        Rook,
        Knight,
        Bishop,
        WhitePawn,
        BlackPawn
    }

    public Piece toCheck;
    private void Start()
    {
        
    }
    
    private void Update()
    {
        
    }

    private string GetFilePath(string fileName)
    {
        return Application.dataPath + "TextAssets/" + fileName;
    }

    private void LoopThroughBoard(string fileName)
    {
        string[] rows = File.ReadAllLines(GetFilePath(fileName));

        foreach (var row in rows)
        {
            foreach (var cell in row)
            {
                Debug.Log(cell);
                //TODO: Check for piece - is cell occupied?
                //TODO: Look for valid move resulting in checkmate
            }
        }
    }
    
    Vector2[] ValidMoves(Vector2 pos1, Piece piece)
    {
        Vector2[] validMoves;
        
        var movesDown = (board.Length - 1) - pos1.y;
        var movesUp = pos1.y;
        var movesRight = (board.Length - 1) - pos1.x;
        var movesLeft = pos1.x;

        switch (piece)
        {
            case Piece.Queen:
                validMoves = new[]
                {
                    #region Queen Moves
                    new Vector2(pos1.x + 1, pos1.y),
                    new Vector2(pos1.x + 2, pos1.y),
                    new Vector2(pos1.x + 3, pos1.y),
                    new Vector2(pos1.x + 4, pos1.y),
                    new Vector2(pos1.x + 5, pos1.y),
                    new Vector2(pos1.x + 6, pos1.y),
                    new Vector2(pos1.x + 7, pos1.y),

                    new Vector2(pos1.x - 1, pos1.y),
                    new Vector2(pos1.x - 2, pos1.y),
                    new Vector2(pos1.x - 3, pos1.y),
                    new Vector2(pos1.x - 4, pos1.y),
                    new Vector2(pos1.x - 5, pos1.y),
                    new Vector2(pos1.x - 6, pos1.y),
                    new Vector2(pos1.x - 7, pos1.y),

                    new Vector2(pos1.x, pos1.y + 1),
                    new Vector2(pos1.x, pos1.y + 2),
                    new Vector2(pos1.x, pos1.y + 3),
                    new Vector2(pos1.x, pos1.y + 4),
                    new Vector2(pos1.x, pos1.y + 5),
                    new Vector2(pos1.x, pos1.y + 6),
                    new Vector2(pos1.x, pos1.y + 7),

                    new Vector2(pos1.x, pos1.y - 1),
                    new Vector2(pos1.x, pos1.y - 2),
                    new Vector2(pos1.x, pos1.y - 3),
                    new Vector2(pos1.x, pos1.y - 4),
                    new Vector2(pos1.x, pos1.y - 5),
                    new Vector2(pos1.x, pos1.y - 6),
                    new Vector2(pos1.x, pos1.y - 7),

                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x + 2, pos1.y + 2),
                    new Vector2(pos1.x + 3, pos1.y + 3),
                    new Vector2(pos1.x + 4, pos1.y + 4),
                    new Vector2(pos1.x + 5, pos1.y + 5),
                    new Vector2(pos1.x + 6, pos1.y + 6),
                    new Vector2(pos1.x + 7, pos1.y + 7),

                    new Vector2(pos1.x - 1, pos1.y - 1),
                    new Vector2(pos1.x - 2, pos1.y - 2),
                    new Vector2(pos1.x - 3, pos1.y - 3),
                    new Vector2(pos1.x - 4, pos1.y - 4),
                    new Vector2(pos1.x - 5, pos1.y - 5),
                    new Vector2(pos1.x - 6, pos1.y - 6),
                    new Vector2(pos1.x - 7, pos1.y - 7),

                    new Vector2(pos1.x + 1, pos1.y - 1),
                    new Vector2(pos1.x + 2, pos1.y - 2),
                    new Vector2(pos1.x + 3, pos1.y - 3),
                    new Vector2(pos1.x + 4, pos1.y - 4),
                    new Vector2(pos1.x + 5, pos1.y - 5),
                    new Vector2(pos1.x + 6, pos1.y - 6),
                    new Vector2(pos1.x + 7, pos1.y - 7),

                    new Vector2(pos1.x - 1, pos1.y + 1),
                    new Vector2(pos1.x - 2, pos1.y + 2),
                    new Vector2(pos1.x - 3, pos1.y + 3),
                    new Vector2(pos1.x - 4, pos1.y + 4),
                    new Vector2(pos1.x - 5, pos1.y + 5),
                    new Vector2(pos1.x - 6, pos1.y + 6),
                    new Vector2(pos1.x - 7, pos1.y + 7),

                    #endregion
                };
                break;
            case Piece.King:
                validMoves = new []
                {
                    #region King Moves
                    new Vector2(pos1.x + 1, pos1.y),
                    new Vector2(pos1.x - 1, pos1.y),
                    new Vector2(pos1.x, pos1.y + 1),
                    new Vector2(pos1.x, pos1.y - 1), 
                
                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x - 1, pos1.y - 1),
                    new Vector2(pos1.x + 1, pos1.y - 1),
                    new Vector2(pos1.x - 1, pos1.y + 1),
                    #endregion
                };
                break;
            case Piece.Knight:
                validMoves = new[]
                {
                    #region Knight Moves

                    new Vector2(pos1.x + 2, pos1.y + 1),
                    new Vector2(pos1.x + 2, pos1.y - 1),
                    new Vector2(pos1.x - 2, pos1.y + 1),
                    new Vector2(pos1.x - 2, pos1.y - 1),
                    new Vector2(pos1.x + 1, pos1.y + 2),
                    new Vector2(pos1.x - 1, pos1.y + 2),
                    new Vector2(pos1.x + 1, pos1.y - 2),
                    new Vector2(pos1.x - 1, pos1.y - 2),

                    #endregion
                };
                break;
            case Piece.Bishop:
                validMoves = new[]
                {
                    #region Bishop Moves

                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x + 2, pos1.y + 2),
                    new Vector2(pos1.x + 3, pos1.y + 3),
                    new Vector2(pos1.x + 4, pos1.y + 4),
                    new Vector2(pos1.x + 5, pos1.y + 5),
                    new Vector2(pos1.x + 6, pos1.y + 6),
                    new Vector2(pos1.x + 7, pos1.y + 7),

                    new Vector2(pos1.x - 1, pos1.y - 1),
                    new Vector2(pos1.x - 2, pos1.y - 2),
                    new Vector2(pos1.x - 3, pos1.y - 3),
                    new Vector2(pos1.x - 4, pos1.y - 4),
                    new Vector2(pos1.x - 5, pos1.y - 5),
                    new Vector2(pos1.x - 6, pos1.y - 6),
                    new Vector2(pos1.x - 7, pos1.y - 7),

                    new Vector2(pos1.x + 1, pos1.y - 1),
                    new Vector2(pos1.x + 2, pos1.y - 2),
                    new Vector2(pos1.x + 3, pos1.y - 3),
                    new Vector2(pos1.x + 4, pos1.y - 4),
                    new Vector2(pos1.x + 5, pos1.y - 5),
                    new Vector2(pos1.x + 6, pos1.y - 6),
                    new Vector2(pos1.x + 7, pos1.y - 7),

                    new Vector2(pos1.x - 1, pos1.y + 1),
                    new Vector2(pos1.x - 2, pos1.y + 2),
                    new Vector2(pos1.x - 3, pos1.y + 3),
                    new Vector2(pos1.x - 4, pos1.y + 4),
                    new Vector2(pos1.x - 5, pos1.y + 5),
                    new Vector2(pos1.x - 6, pos1.y + 6),
                    new Vector2(pos1.x - 7, pos1.y + 7),

                    #endregion
                };
                break;
            case Piece.Rook:
                validMoves = new[]
                {
                    #region Rook Moves

                    new Vector2(pos1.x + 1, pos1.y),
                    new Vector2(pos1.x + 2, pos1.y),
                    new Vector2(pos1.x + 3, pos1.y),
                    new Vector2(pos1.x + 4, pos1.y),
                    new Vector2(pos1.x + 5, pos1.y),
                    new Vector2(pos1.x + 6, pos1.y),
                    new Vector2(pos1.x + 7, pos1.y),

                    new Vector2(pos1.x - 1, pos1.y),
                    new Vector2(pos1.x - 2, pos1.y),
                    new Vector2(pos1.x - 3, pos1.y),
                    new Vector2(pos1.x - 4, pos1.y),
                    new Vector2(pos1.x - 5, pos1.y),
                    new Vector2(pos1.x - 6, pos1.y),
                    new Vector2(pos1.x - 7, pos1.y),

                    new Vector2(pos1.x, pos1.y + 1),
                    new Vector2(pos1.x, pos1.y + 2),
                    new Vector2(pos1.x, pos1.y + 3),
                    new Vector2(pos1.x, pos1.y + 4),
                    new Vector2(pos1.x, pos1.y + 5),
                    new Vector2(pos1.x, pos1.y + 6),
                    new Vector2(pos1.x, pos1.y + 7),

                    new Vector2(pos1.x, pos1.y - 1),
                    new Vector2(pos1.x, pos1.y - 2),
                    new Vector2(pos1.x, pos1.y - 3),
                    new Vector2(pos1.x, pos1.y - 4),
                    new Vector2(pos1.x, pos1.y - 5),
                    new Vector2(pos1.x, pos1.y - 6),
                    new Vector2(pos1.x, pos1.y - 7),

                    #endregion
                };
                break;
            case Piece.WhitePawn:
                validMoves = new[]
                {
                    #region Pawn Moves

                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x - 1, pos1.y + 1),

                    #endregion
                };
                break;
            case Piece.BlackPawn:
                validMoves = new[]
                {
                    #region Pawn Moves

                    new Vector2(pos1.x + 1, pos1.y + 1),
                    new Vector2(pos1.x - 1, pos1.y + 1),

                    #endregion
                };
                break;
            case Piece.Empty:
                Debug.Log("Cell is empty.");
                validMoves = Array.Empty<Vector2>();
                break;
            default:
                throw new ArgumentNullException();
        }
        
        return validMoves;
    }
    
    // CONNOR: Found these resources/suggestions online...
    //
    // "I'd implement a common way to get all legal moves for a piece, along with their side effects
    // (will this move cause a capture?). Then, on each turn, get the legal moves for the piece that
    // was just moved. If that piece has a legal move that will capture the enemy king, that's a check.
    // Once you have that, you can test if your check is also a checkmate by enumerating all legal moves
    // for the enemy king, and then checking if it can still be captured by any of the enemy pieces.
    // If it's stuck, you have a checkmate."
    //
    // "Detecting Mate: Some programs rely on pseudo-legal move generation, and find Checkmate if all
    // those moves are in fact illegal after making and finding the "refutation" of capturing the king.
    // At the latest, if no legal move was found, programs need the information whether the king is in
    // check to decide about checkmate or stalemate score. Despite, most programs (should be) are aware
    // of check in advance, and use special move generator(s) if in check or even in double check."
    //
    // https://peterellisjones.com/posts/generating-legal-chess-moves-efficiently/
}
