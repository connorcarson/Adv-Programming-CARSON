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
    
    private ChessPiece[,] chessBoard = new ChessPiece[8, 8];
    
    private void Start()
    {
        //LoopThroughBoard(checkMateInOne);
        CreateBoard(noCheckMate);
    }
    
    private void Update()
    {
        
    }

    private string GetFilePath(string fileName)
    {
        return Application.dataPath + "/TextAssets/" + fileName;
    }

    private void CreateBoard(string fileName)
    {
        string[] rows = File.ReadAllLines(GetFilePath(fileName));

        for (var rowIndex = 0; rowIndex < rows.Length; rowIndex++)
        {
            var row = rows[rowIndex];

            for (var columnIndex = 0; columnIndex < row.Length; columnIndex++)
            {
                var cell = row[columnIndex];
                
                chessBoard[columnIndex, rowIndex] = CreatePiece(cell);
            }
        }
    }

    private ChessPiece CreatePiece(char cell)
    {
        switch (cell)
        {
            case 'Q':
                return new ChessPiece(ChessPiece.PieceType.Queen, ChessPiece.PieceColor.White);
            case 'q':
                return new ChessPiece(ChessPiece.PieceType.Queen, ChessPiece.PieceColor.Black);
            case 'K':
                return new ChessPiece(ChessPiece.PieceType.King, ChessPiece.PieceColor.White);
            case 'k':
                return new ChessPiece(ChessPiece.PieceType.King, ChessPiece.PieceColor.Black);
            case 'R':
                return new ChessPiece(ChessPiece.PieceType.Rook, ChessPiece.PieceColor.White);
            case 'r':
                return new ChessPiece(ChessPiece.PieceType.Rook, ChessPiece.PieceColor.Black);
            case 'N':
                return new ChessPiece(ChessPiece.PieceType.Knight, ChessPiece.PieceColor.White);
            case 'n':
                return new ChessPiece(ChessPiece.PieceType.Knight, ChessPiece.PieceColor.Black);
            case 'B':
                return new ChessPiece(ChessPiece.PieceType.Bishop, ChessPiece.PieceColor.White);
            case 'b':
                return new ChessPiece(ChessPiece.PieceType.Bishop, ChessPiece.PieceColor.Black);
            case 'P':
                return new ChessPiece(ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.White);
            case 'p':
                return new ChessPiece(ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.Black);
            case '.':
                return new ChessPiece(ChessPiece.PieceType.Empty, ChessPiece.PieceColor.None);
            default:
                throw new ArgumentException("Unexpected character. Not a valid piece type.");
        }
    }
    
    private void LoopThroughBoard()
    {
        for (var rowIndex = 0; rowIndex < chessBoard.GetLength(0); rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < chessBoard.GetLength(1); columnIndex++)
            {
                var chessPiece = chessBoard[rowIndex, columnIndex];

                //Check for piece - is cell occupied?
                if (chessPiece.myType == ChessPiece.PieceType.Empty) break;
                
                var position = new Vector2(columnIndex, rowIndex);
                
                //Get all legal moves for piece
                var legalMoves = GetLegalMoves(chessPiece, position);
                Debug.Log(chessPiece.myType + ": " + legalMoves.Count);

                //TODO: Make each valid move
                //TODO: Check if King is in check, return if false, undo move
                //TODO: Check if King can move out of check, return if true, undo move
                //TODO: Check if other pieces can block piece(s) that have King in check, return if true, undo move
                //If we haven't returned from any of the above checks on a given move, King must be in checkmate!
            }
        }
    }

    private string[] GetBoard(string fileName)
    {
        return File.ReadAllLines(GetFilePath(fileName));
    }
    
    private ChessPiece GetCell(Vector2 toCheck)
    {
        return chessBoard[(int)toCheck.x, (int)toCheck.y];
    }

    private List<Vector2> LegalQueenMoves(ChessPiece toCheck, Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();

        var movesDown = 7 - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = 7 - piecePos.x;
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
        for (var i = 1; i <= movesRight; i++)
        {
            var newPos = new Vector2(piecePos.x + i, piecePos.y);
            var cell = GetCell(newPos);
            
            if (cell.myColor == toCheck.myColor) break;
            
            legalMoves.Add(newPos);
        }

        //get moves left
        for (var i = 1; i <= movesLeft; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y));
        }

        //get moves up
        for (var i = 1; i <= movesUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x, piecePos.y + i));
        }

        //get moves down
        for (var i = 1; i <= movesDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x, piecePos.y - i));
        }

        //get diagonal moves (right and up)
        for (var i = 1; i <= movesDiagonalRightUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y + i));
        }

        //get diagonal moves (left and up)
        for (var i = 1; i <= movesDiagonalLeftUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y + i));
        }

        //get diagonal moves (left and down)
        for (var i = 1; i <= movesDiagonalLeftDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y - i));
        }

        //get diagonal moves (right and down)
        for (var i = 1; i <= movesDiagonalRightDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y - i));
        }

        return legalMoves;
    }

    private List<Vector2> LegalKingMoves(Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = 7 - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = 7 - piecePos.x;
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
        
        var movesDown = 7 - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = 7 - piecePos.x;
        var movesLeft = piecePos.x;
        
        //get moves right
        for (var i = 1; i <= movesRight; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y));
        }
        //get moves left
        for (var i = 1; i <= movesLeft; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y));
        }
        //get moves up
        for (var i = 1; i <= movesUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x, piecePos.y + i));
        }
        //get moves down
        for (var i = 1; i <= movesDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x, piecePos.y - i));
        }

        return legalMoves;
    }

    private List<Vector2> LegalKnightMoves(Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = 7 - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = 7 - piecePos.x;
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
        
        var movesDown = 7 - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = 7 - piecePos.x;
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
        for (var i = 1; i <= movesDiagonalRightUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y + i));
        }
        //get diagonal moves (left and up)
        for (var i = 1; i <= movesDiagonalLeftUp; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y + i));
        }
        //get diagonal moves (left and down)
        for (var i = 1; i <= movesDiagonalLeftDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x - i, piecePos.y - i));
        }
        //get diagonal moves (right and down)
        for (var i = 1; i <= movesDiagonalRightDown; i++)
        {
            legalMoves.Add(new Vector2(piecePos.x + i, piecePos.y - i));
        }

        return legalMoves;
    }

    private List<Vector2> LegalPawnMoves(Vector2 piecePos, ChessPiece.PieceColor pieceColor)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = 7 - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = 7 - piecePos.x;
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

        switch (pieceColor)
        {
            case ChessPiece.PieceColor.White:
                #region Pawn Moves
                
                if(movesUp > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y + 1));
                if(movesDiagonalRightUp > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y + 1));
                if(movesDiagonalLeftUp > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y + 1)); 
            
                #endregion
                break;
                        
            case ChessPiece.PieceColor.Black:
                #region Pawn Moves
                
                if(movesDown > 0) legalMoves.Add(new Vector2(piecePos.x, piecePos.y - 1));
                if(movesDiagonalRightDown > 0) legalMoves.Add(new Vector2(piecePos.x + 1, piecePos.y - 1));
                if(movesDiagonalLeftDown > 0) legalMoves.Add(new Vector2(piecePos.x - 1, piecePos.y - 1)); 
            
                #endregion
                break;
        }

        return legalMoves;
    }

    private List<Vector2> GetLegalMoves(ChessPiece toCheck, Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        switch (toCheck.myType)
        {
           case ChessPiece.PieceType.Queen:
               legalMoves = LegalQueenMoves(toCheck, piecePos);
               break;
           case ChessPiece.PieceType.King:
               legalMoves = LegalKingMoves(piecePos);
               break;
           case ChessPiece.PieceType.Rook:
               legalMoves = LegalRookMoves(piecePos);
               break;
           case ChessPiece.PieceType.Knight:
               legalMoves = LegalKnightMoves(piecePos);
               break;
           case ChessPiece.PieceType.Bishop:
               legalMoves = LegalBishopMoves(piecePos);
               break;
           case ChessPiece.PieceType.Pawn:
               legalMoves = LegalPawnMoves(piecePos, toCheck.myColor);
               break;
           case ChessPiece.PieceType.Empty:
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
//        var movesDown = 7 - piecePos.y;
//        var movesUp = piecePos.y;
//        var movesRight = 7 - piecePos.x;
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
