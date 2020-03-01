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
    private Piece toCheck;
    private void Start()
    {
        LoopThroughBoard(checkMateInOne);
        LoopThroughBoard(noCheckMate);
    }
    
    private void Update()
    {
        
    }

    private string GetFilePath(string fileName)
    {
        return Application.dataPath + "/TextAssets/" + fileName;
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

    private List<Vector2> LegalQueenMoves(Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = (board.Length - 1) - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = (board.Length - 1) - piecePos.x;
        var movesLeft = piecePos.x;

        var movesDiagonalRightUp = 0.0f;
        if (movesRight < movesUp) movesDiagonalRightUp = movesRight; 
        else movesDiagonalRightUp = movesUp;

        var movesDiagonalLeftUp = 0.0f;
        if (movesLeft < movesUp) movesDiagonalLeftUp = movesLeft;
        else movesDiagonalLeftUp = movesUp;

        var movesDiagonalLeftDown = 0.0f;
        if (movesLeft < movesDown) movesDiagonalLeftDown = movesLeft;
        else movesDiagonalLeftDown = movesDown;

        var movesDiagonalRightDown = 0.0f;
        if (movesRight < movesDown) movesDiagonalRightDown = movesRight;
        else movesDiagonalRightDown = movesDown;
        
                //get moves right
                for (var i = 1; i < movesRight; i++)
                {
                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y));
                }
                //get moves left
                for (var i = 1; i < movesLeft; i++)
                {
                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y));
                }
                //get moves up
                for (var i = 1; i < movesUp; i++)
                {
                    legalMoves.Add(new Vector2(piecePos.x, piecePos.y + i));
                }
                //get moves down
                for (var i = 1; i < movesDown; i++)
                {
                    legalMoves.Add(new Vector2(piecePos.x, piecePos.y - i));
                }
                //get diagonal moves (right and up)
                for (var i = 1; i < movesDiagonalRightUp; i++)
                {
                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y + i));
                }
                //get diagonal moves (left and up)
                for (var i = 1; i < movesDiagonalLeftUp; i++)
                {
                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y + i));
                }
                //get diagonal moves (left and down)
                for (var i = 1; i < movesDiagonalLeftDown; i++)
                {
                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y - i));
                }
                //get diagonal moves (right and down)
                for (var i = 1; i < movesDiagonalRightDown; i++)
                {
                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y - i));
                }

                return legalMoves;
    }

    private List<Vector2> LegalKingMoves(Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = (board.Length - 1) - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = (board.Length - 1) - piecePos.x;
        var movesLeft = piecePos.x;

        var movesDiagonalRightUp = 0.0f;
        if (movesRight < movesUp) movesDiagonalRightUp = movesRight; 
        else movesDiagonalRightUp = movesUp;

        var movesDiagonalLeftUp = 0.0f;
        if (movesLeft < movesUp) movesDiagonalLeftUp = movesLeft;
        else movesDiagonalLeftUp = movesUp;

        var movesDiagonalLeftDown = 0.0f;
        if (movesLeft < movesDown) movesDiagonalLeftDown = movesLeft;
        else movesDiagonalLeftDown = movesDown;

        var movesDiagonalRightDown = 0.0f;
        if (movesRight < movesDown) movesDiagonalRightDown = movesRight;
        else movesDiagonalRightDown = movesDown;
        
        if(movesRight > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y));
        if(movesLeft > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y));
        if(movesUp > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y + 1));
        if(movesDown > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y - 1));
                
        if(movesDiagonalRightUp > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y + 1));
        if(movesDiagonalLeftUp > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y + 1));
        if(movesDiagonalLeftDown > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y - 1));
        if(movesDiagonalRightDown > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y - 1));

        return legalMoves;
    }

    private List<Vector2> LegalRookMoves(Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = (board.Length - 1) - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = (board.Length - 1) - piecePos.x;
        var movesLeft = piecePos.x;
        
        //get moves right
        for (var i = 1; i < movesRight; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y));
        }
        //get moves left
        for (var i = 1; i < movesLeft; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y));
        }
        //get moves up
        for (var i = 1; i < movesUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x, piecePos.y + i));
        }
        //get moves down
        for (var i = 1; i < movesDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x, piecePos.y - i));
        }

        return legalMoves;
    }

    private List<Vector2> LegalKnightMoves(Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = (board.Length - 1) - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = (board.Length - 1) - piecePos.x;
        var movesLeft = piecePos.x;
        
        if(movesRight >= 2 && movesUp >= 1) legalMoves.Add(new Vector2(piecePos.x + 2, piecePos.y + 1));
        if(movesRight >= 1 && movesUp >= 2) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y + 2));
        if(movesRight >= 2 && movesDown >= 1) legalMoves.Add(new Vector2(piecePos.x + 2, piecePos.y - 1));
        if (movesRight >= 1 && movesDown >= 2) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y - 2));
        if(movesLeft >= 2 && movesUp >= 1) legalMoves.Add( new Vector2(piecePos.x - 2, piecePos.y + 1));
        if (movesLeft >= 1 && movesUp >= 2) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y + 2));
        if(movesLeft >= 2 && movesDown >= 1) legalMoves.Add(new Vector2(piecePos.x - 2, piecePos.y - 1));
        if(movesLeft >= 1 && movesDown >= 2) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y - 2));

        return legalMoves;
    }

    private List<Vector2> LegalBishopMoves(Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = (board.Length - 1) - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = (board.Length - 1) - piecePos.x;
        var movesLeft = piecePos.x;

        var movesDiagonalRightUp = 0.0f;
        if (movesRight < movesUp) movesDiagonalRightUp = movesRight; 
        else movesDiagonalRightUp = movesUp;

        var movesDiagonalLeftUp = 0.0f;
        if (movesLeft < movesUp) movesDiagonalLeftUp = movesLeft;
        else movesDiagonalLeftUp = movesUp;

        var movesDiagonalLeftDown = 0.0f;
        if (movesLeft < movesDown) movesDiagonalLeftDown = movesLeft;
        else movesDiagonalLeftDown = movesDown;

        var movesDiagonalRightDown = 0.0f;
        if (movesRight < movesDown) movesDiagonalRightDown = movesRight;
        else movesDiagonalRightDown = movesDown;
        
        //get diagonal moves (right and up)
        for (var i = 1; i < movesDiagonalRightUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y + i));
        }
        //get diagonal moves (left and up)
        for (var i = 1; i < movesDiagonalLeftUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y + i));
        }
        //get diagonal moves (left and down)
        for (var i = 1; i < movesDiagonalLeftDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y - i));
        }
        //get diagonal moves (right and down)
        for (var i = 1; i < movesDiagonalRightDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y - i));
        }

        return legalMoves;
    }

    private List<Vector2> LegalPawnMoves(Vector2 piecePos, Piece pieceType)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = (board.Length - 1) - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = (board.Length - 1) - piecePos.x;
        var movesLeft = piecePos.x;

        var movesDiagonalRightUp = 0.0f;
        if (movesRight < movesUp) movesDiagonalRightUp = movesRight; 
        else movesDiagonalRightUp = movesUp;

        var movesDiagonalLeftUp = 0.0f;
        if (movesLeft < movesUp) movesDiagonalLeftUp = movesLeft;
        else movesDiagonalLeftUp = movesUp;

        var movesDiagonalLeftDown = 0.0f;
        if (movesLeft < movesDown) movesDiagonalLeftDown = movesLeft;
        else movesDiagonalLeftDown = movesDown;

        var movesDiagonalRightDown = 0.0f;
        if (movesRight < movesDown) movesDiagonalRightDown = movesRight;
        else movesDiagonalRightDown = movesDown;

        switch (pieceType)
        {
            case Piece.WhitePawn:
                #region Pawn Moves
                
                if(movesUp > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y + 1));
                if(movesDiagonalRightUp > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y + 1));
                if(movesDiagonalLeftUp > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y + 1)); 
            
                #endregion
                break;
                        
            case Piece.BlackPawn:
                #region Pawn Moves
                
                if(movesDown > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y - 1));
                if(movesDiagonalRightDown > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y - 1));
                if(movesDiagonalLeftDown > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y - 1)); 
            
                #endregion
                break;
        }

        return legalMoves;
    }

    private List<Vector2> GetLegalMoves(Piece pieceType, Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        switch (pieceType)
        {
           case Piece.Queen:
               legalMoves = LegalQueenMoves(piecePos);
               break;
           case Piece.King:
               legalMoves = LegalKingMoves(piecePos);
               break;
           case Piece.Rook:
               legalMoves = LegalRookMoves(piecePos);
               break;
           case Piece.Knight:
               legalMoves = LegalKnightMoves(piecePos);
               break;
           case Piece.Bishop:
               legalMoves = LegalBishopMoves(piecePos);
               break;
           case Piece.WhitePawn:
               legalMoves = LegalPawnMoves(piecePos, Piece.WhitePawn);
               break;
           case Piece.BlackPawn:
               legalMoves = LegalPawnMoves(piecePos, Piece.BlackPawn);
               break;
           case Piece.Empty:
               Debug.Log("Cell is empty.");
               legalMoves = new List<Vector2>();
               break;
           default:
               throw new ArgumentNullException();
        }

        return legalMoves;
    }
//    List<Vector2> LegalMoves(Vector2 piecePos, Piece piece)
//    {
//        List<Vector2> legalMoves = new List<Vector2>();
//        
//        var movesDown = (board.Length - 1) - piecePos.y;
//        var movesUp = piecePos.y;
//        var movesRight = (board.Length - 1) - piecePos.x;
//        var movesLeft = piecePos.x;
//
//        var movesDiagonalRightUp = 0.0f;
//        if (movesRight < movesUp) movesDiagonalRightUp = movesRight; 
//        else movesDiagonalRightUp = movesUp;
//
//        var movesDiagonalLeftUp = 0.0f;
//        if (movesLeft < movesUp) movesDiagonalLeftUp = movesLeft;
//        else movesDiagonalLeftUp = movesUp;
//
//        var movesDiagonalLeftDown = 0.0f;
//        if (movesLeft < movesDown) movesDiagonalLeftDown = movesLeft;
//        else movesDiagonalLeftDown = movesDown;
//
//        var movesDiagonalRightDown = 0.0f;
//        if (movesRight < movesDown) movesDiagonalRightDown = movesRight;
//        else movesDiagonalRightDown = movesDown;
//
//        switch (piece)
//        {
//            case Piece.Queen:
//                #region Queen Moves
//                
//                //get moves right
//                for (var i = 1; i < movesRight; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y));
//                }
//                //get moves left
//                for (var i = 1; i < movesLeft; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y));
//                }
//                //get moves up
//                for (var i = 1; i < movesUp; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x, piecePos.y + i));
//                }
//                //get moves down
//                for (var i = 1; i < movesDown; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x, piecePos.y - i));
//                }
//                //get diagonal moves (right and up)
//                for (var i = 1; i < movesDiagonalRightUp; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y + i));
//                }
//                //get diagonal moves (left and up)
//                for (var i = 1; i < movesDiagonalLeftUp; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y + i));
//                }
//                //get diagonal moves (left and down)
//                for (var i = 1; i < movesDiagonalLeftDown; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y - i));
//                }
//                //get diagonal moves (right and down)
//                for (var i = 1; i < movesDiagonalRightDown; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y - i));
//                }
//                
//                #endregion
//                break;
//            
//            case Piece.King:
//                #region King Moves
//                
//                if(movesRight > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y));
//                if(movesLeft > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y));
//                if(movesUp > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y + 1));
//                if(movesDown > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y - 1));
//                
//                if(movesDiagonalRightUp > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y + 1));
//                if(movesDiagonalLeftUp > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y + 1));
//                if(movesDiagonalLeftDown > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y - 1));
//                if(movesDiagonalRightDown > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y - 1));
//                
//                #endregion
//                break;
//
//            case Piece.Knight:
//                #region Knight Moves
//            
//                if(movesRight >= 2 && movesUp >= 1) legalMoves.Add(new Vector2(piecePos.x + 2, piecePos.y + 1));
//                if(movesRight >= 1 && movesUp >= 2) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y + 2));
//                if(movesRight >= 2 && movesDown >= 1) legalMoves.Add(new Vector2(piecePos.x + 2, piecePos.y - 1));
//                if (movesRight >= 1 && movesDown >= 2) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y - 2));
//                if(movesLeft >= 2 && movesUp >= 1) legalMoves.Add( new Vector2(piecePos.x - 2, piecePos.y + 1));
//                if (movesLeft >= 1 && movesUp >= 2) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y + 2));
//                if(movesLeft >= 2 && movesDown >= 1) legalMoves.Add(new Vector2(piecePos.x - 2, piecePos.y - 1));
//                if(movesLeft >= 1 && movesDown >= 2) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y - 2));
//                
//                #endregion
//                break;
//            
//            case Piece.Bishop:
//                #region Bishop Moves
//                
//                //get diagonal moves (right and up)
//                for (var i = 1; i < movesDiagonalRightUp; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y + i));
//                }
//                //get diagonal moves (left and up)
//                for (var i = 1; i < movesDiagonalLeftUp; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y + i));
//                }
//                //get diagonal moves (left and down)
//                for (var i = 1; i < movesDiagonalLeftDown; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y - i));
//                }
//                //get diagonal moves (right and down)
//                for (var i = 1; i < movesDiagonalRightDown; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y - i));
//                }   
//            
//                #endregion
//                break;
//            
//            case Piece.Rook:
//                #region Rook Moves
//                
//                //get moves right
//                for (var i = 1; i < movesRight; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y));
//                }
//                //get moves left
//                for (var i = 1; i < movesLeft; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y));
//                }
//                //get moves up
//                for (var i = 1; i < movesUp; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x, piecePos.y + i));
//                }
//                //get moves down
//                for (var i = 1; i < movesDown; i++)
//                {
//                    legalMoves.Add(new Vector2(piecePos.x, piecePos.y - i));
//                }
//                
//                #endregion
//                break;
//            
//            case Piece.WhitePawn:
//                #region Pawn Moves
//                
//                if(movesUp > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y + 1));
//                if(movesDiagonalRightUp > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y + 1));
//                if(movesDiagonalLeftUp > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y + 1)); 
//            
//                #endregion
//                break;
//            
//            case Piece.BlackPawn:
//                #region Pawn Moves
//                
//                if(movesDown > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y - 1));
//                if(movesDiagonalRightDown > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y - 1));
//                if(movesDiagonalLeftDown > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y - 1)); 
//            
//                #endregion
//                break;
//            
//            case Piece.Empty:
//                Debug.Log("Cell is empty.");
//                legalMoves = new List<Vector2>();
//                break;
//            
//            default:
//                throw new ArgumentNullException();
//        }
//        
//        return legalMoves;
//    }
    
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
