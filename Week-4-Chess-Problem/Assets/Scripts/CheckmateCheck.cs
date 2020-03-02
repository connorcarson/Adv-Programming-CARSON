using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CheckmateCheck : MonoBehaviour
{
    public string noCheckMate;
    public string checkMateInOne;

    public enum FileToCheck
    {
        None,
        NoCheckMate,
        CheckMateInOne,
    }
    public FileToCheck currentFile;
    
    private ChessPiece[,] _chessBoard = new ChessPiece[8, 8];

    private ChessPiece _whiteKing;
    private ChessPiece _blackKing;
    private ChessPiece _attackedPiece;
    
    private void Start()
    {
        switch (currentFile)
        {
            case FileToCheck.NoCheckMate:
                CreateBoard(noCheckMate);
                break;
            case FileToCheck.CheckMateInOne:
                CreateBoard(checkMateInOne);
                break;
            default:
                throw new ArgumentException("File not selected.");
        }

        CheckMate();
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

                _chessBoard[rowIndex, columnIndex] = CreatePiece(cell, new Vector2(rowIndex, columnIndex));
            }
        }
    }

    private ChessPiece CreatePiece(char cell, Vector2 position)
    {
        switch (cell)
        {
            case 'Q':
                return new ChessPiece(ChessPiece.PieceType.Queen, ChessPiece.PieceColor.White, position);
            case 'q':
                return new ChessPiece(ChessPiece.PieceType.Queen, ChessPiece.PieceColor.Black, position);
            case 'K':
                _whiteKing = new ChessPiece(ChessPiece.PieceType.King, ChessPiece.PieceColor.White, position);
                return _whiteKing;
            case 'k':
                _blackKing = new ChessPiece(ChessPiece.PieceType.King, ChessPiece.PieceColor.Black, position);
                return _blackKing;
            case 'R':
                return new ChessPiece(ChessPiece.PieceType.Rook, ChessPiece.PieceColor.White, position);
            case 'r':
                return new ChessPiece(ChessPiece.PieceType.Rook, ChessPiece.PieceColor.Black, position);
            case 'N':
                return new ChessPiece(ChessPiece.PieceType.Knight, ChessPiece.PieceColor.White, position);
            case 'n':
                return new ChessPiece(ChessPiece.PieceType.Knight, ChessPiece.PieceColor.Black, position);
            case 'B':
                return new ChessPiece(ChessPiece.PieceType.Bishop, ChessPiece.PieceColor.White, position);
            case 'b':
                return new ChessPiece(ChessPiece.PieceType.Bishop, ChessPiece.PieceColor.Black, position);
            case 'P':
                return new ChessPiece(ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.White, position);
            case 'p':
                return new ChessPiece(ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.Black, position);
            case '.':
                return new ChessPiece(ChessPiece.PieceType.Empty, ChessPiece.PieceColor.None, position);
            default:
                throw new ArgumentException("Unexpected character. Not a valid piece type.");
        }
    }
    
    private bool CheckMate()
    {
        var totalPossibleMoves = 0;
        for (var rowIndex = 0; rowIndex < _chessBoard.GetLength(0); rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < _chessBoard.GetLength(1); columnIndex++)
            {
                var chessPiece = _chessBoard[rowIndex, columnIndex];
                
                //Check for piece - is cell occupied?
                if (chessPiece.type == ChessPiece.PieceType.Empty) break;
                
                var position = new Vector2(columnIndex, rowIndex);
                
                //Get all legal moves for piece
                var legalMoves = GetLegalMoves(chessPiece, position);
                Debug.Log(chessPiece.color + " " + chessPiece.type + ": " + legalMoves.Count);
                totalPossibleMoves += legalMoves.Count;
                
//                foreach (var move in legalMoves)
//                {
//                    //Make each move one by one
//                    chessBoard[rowIndex, columnIndex] = new ChessPiece(ChessPiece.PieceType.Empty, ChessPiece.PieceColor.None, new Vector2(rowIndex, columnIndex));
//                    chessBoard[(int)move.x, (int)move.y] = chessPiece;
//
//                    //if the move does not result in the king being checked
//                    if (!KingInCheck(chessPiece)) break;
//                    
//                    //Get all legal moves for the king under attack
//                    var kingsMoves = GetLegalMoves(attackedPiece, attackedPiece.position);
//
//                    //if king cannot escape
//                    if (!KingCanEscape(kingsMoves, chessPiece, position))
//                    {
//                        //TODO: Check if other pieces can block the piece(s) that have King in check, break if true
//                        
//                        //If we haven't returned from any of the above checks on a given move, King must be in checkmate!
//                        return true;
//                    }
//                    
//                    //TODO: Undo last move, reset board to original state
//                }
            }
        }
        Debug.Log("Total possible legal moves: " + totalPossibleMoves);
        return false;
    }

    private bool KingCanEscape(List<Vector2> possibleMoves, ChessPiece chessPiece, Vector2 position)
    {
        foreach (var move in possibleMoves)
        {
            //make each move one by one
            _chessBoard[(int)_attackedPiece.position.x, (int)_attackedPiece.position.y] = new ChessPiece(ChessPiece.PieceType.Empty, ChessPiece.PieceColor.None, position);
            _chessBoard[(int) move.x, (int) move.y] = _attackedPiece;
                        
            //if the king is no longer in check
            if (!KingInCheck(chessPiece))
            {
                return true;
            }
        }

        return false;
    }

    private bool KingInCheck(ChessPiece attackingPiece)
    {
        if (attackingPiece.color == ChessPiece.PieceColor.White) _attackedPiece = _blackKing;
        else _attackedPiece = _whiteKing;
        
        for (var rowIndex = 0; rowIndex < _chessBoard.GetLength(0); rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < _chessBoard.GetLength(1); columnIndex++)
            {
                var chessPiece = _chessBoard[rowIndex, columnIndex];
                
                if (chessPiece.color == attackingPiece.color)
                {
                    var position = new Vector2(columnIndex, rowIndex);
                    var legalMoves = GetLegalMoves(chessPiece, position);

                    if (legalMoves.Contains(_attackedPiece.position)) return true;
                }
            }
        }

        return false;
    }

    private ChessPiece GetCell(Vector2 toCheck)
    {
        return _chessBoard[(int)toCheck.x, (int)toCheck.y];
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
            var newPos = new Vector2(piecePos.y, piecePos.x + i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        //get moves left
        for (var i = 1; i <= movesLeft; i++)
        {
            var newPos = new Vector2(piecePos.y, piecePos.x - i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        //get moves up
        for (var i = 1; i <= movesUp; i++)
        {
            var newPos = new Vector2(piecePos.y - i, piecePos.x);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        //get moves down
        for (var i = 1; i <= movesDown; i++)
        {
            var newPos = new Vector2(piecePos.y + i, piecePos.x);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        //get diagonal moves (right and up)
        for (var i = 1; i <= movesDiagonalRightUp; i++)
        {
            var newPos = new Vector2(piecePos.y - i, piecePos.x + i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        //get diagonal moves (left and up)
        for (var i = 1; i <= movesDiagonalLeftUp; i++)
        {
            var newPos = new Vector2(piecePos.y - i, piecePos.x - i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
                
            legalMoves.Add(newPos);
        }

        //get diagonal moves (left and down)
        for (var i = 1; i <= movesDiagonalLeftDown; i++)
        {
            var newPos = new Vector2(piecePos.y + i, piecePos.x - i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        //get diagonal moves (right and down)
        for (var i = 1; i <= movesDiagonalRightDown; i++)
        {
            var newPos = new Vector2(piecePos.y + i, piecePos.x + i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        return legalMoves;
    }

    private List<Vector2> LegalKingMoves(ChessPiece toCheck, Vector2 piecePos)
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

        if (movesRight > 0)
        {
            var newPos = new Vector2(piecePos.y, piecePos.x + 1);
            var cell = GetCell(newPos);
            
            if (cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesLeft > 0)
        {
            var newPos = new Vector2(piecePos.y, piecePos.x - 1);
            var cell = GetCell(newPos);
            
            if (cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesUp > 0)
        {
            var newPos = new Vector2(piecePos.y - 1, piecePos.x);
            var cell = GetCell(newPos);
            
            if (cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesDown > 0)
        {
            var newPos = new Vector2(piecePos.y + 1, piecePos.x);
            var cell = GetCell(newPos);
            
            if (cell.color != toCheck.color) legalMoves.Add(newPos);
        }

        if (movesDiagonalRightUp > 0)
        {
            var newPos = new Vector2(piecePos.y - 1, piecePos.x + 1);
            var cell = GetCell(newPos);
            
            if (cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesDiagonalLeftUp > 0)
        {
            var newPos = new Vector2(piecePos.y - 1, piecePos.x - 1);
            var cell = GetCell(newPos);
            
            if (cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesDiagonalLeftDown > 0)
        {
            var newPos = new Vector2(piecePos.y + 1, piecePos.x - 1);
            var cell = GetCell(newPos);
            
            if (cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesDiagonalRightDown > 0)
        {
            var newPos = new Vector2(piecePos.y + 1, piecePos.x + 1);
            var cell = GetCell(newPos);
            
            if (cell.color != toCheck.color) legalMoves.Add(newPos);
        }

        return legalMoves;
    }

    private List<Vector2> LegalRookMoves(ChessPiece toCheck, Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = 7 - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = 7 - piecePos.x;
        var movesLeft = piecePos.x;
        
        //get moves right
        for (var i = 1; i <= movesRight; i++)
        {
            var newPos = new Vector2(piecePos.y, piecePos.x + i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }
        
        //get moves left
        for (var i = 1; i <= movesLeft; i++)
        {
            var newPos = new Vector2(piecePos.y, piecePos.x - i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }
        
        //get moves up
        for (var i = 1; i <= movesUp; i++)
        {
            var newPos = new Vector2(piecePos.y - i, piecePos.x);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break; 
            
            legalMoves.Add(newPos);
        }
        
        //get moves down
        for (var i = 1; i <= movesDown; i++)
        {
            var newPos = new Vector2(piecePos.y + i, piecePos.x);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break; 
            
            legalMoves.Add(newPos);
        }

        return legalMoves;
    }

    private List<Vector2> LegalKnightMoves(ChessPiece toCheck, Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        
        var movesDown = 7 - piecePos.y;
        var movesUp = piecePos.y;
        var movesRight = 7 - piecePos.x;
        var movesLeft = piecePos.x;

        if (movesRight >= 2 && movesUp >= 1)
        {
            var newPos = new Vector2(piecePos.y - 1, piecePos.x + 2);
            var cell = GetCell(newPos);
            
            if(cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesRight >= 1 && movesUp >= 2)
        {
            var newPos = new Vector2(piecePos.y - 2, piecePos.x + 1);
            var cell = GetCell(newPos);
            
            if(cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesRight >= 2 && movesDown >= 1)
        {
            var newPos = new Vector2(piecePos.y + 1, piecePos.x + 2);
            var cell = GetCell(newPos);
            
            if(cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesRight >= 1 && movesDown >= 2)
        {
            var newPos = new Vector2(piecePos.y + 2, piecePos.x + 1);
            var cell = GetCell(newPos);
            
            if(cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesLeft >= 2 && movesUp >= 1)
        {
            var newPos = new Vector2(piecePos.y - 1, piecePos.x - 2);
            var cell = GetCell(newPos);
            
            if(cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesLeft >= 1 && movesUp >= 2)
        {
            var newPos = new Vector2(piecePos.y - 2, piecePos.x - 1);
            var cell = GetCell(newPos);
            
            if(cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesLeft >= 2 && movesDown >= 1)
        {
            var newPos = new Vector2(piecePos.y + 1, piecePos.x - 2);
            var cell = GetCell(newPos);
            
            if(cell.color != toCheck.color) legalMoves.Add(newPos);
        }
        
        if (movesLeft >= 1 && movesDown >= 2)
        {
            var newPos = new Vector2(piecePos.y + 2, piecePos.x - 1);
            var cell = GetCell(newPos);
            
            if(cell.color != toCheck.color) legalMoves.Add(newPos);
        }

        return legalMoves;
    }

    private List<Vector2> LegalBishopMoves(ChessPiece toCheck, Vector2 piecePos)
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
            var newPos = new Vector2(piecePos.y - i, piecePos.x + i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }
        
        //get diagonal moves (left and up)
        for (var i = 1; i <= movesDiagonalLeftUp; i++)
        {
            var newPos = new Vector2(piecePos.y - i, piecePos.x - i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
                
            legalMoves.Add(newPos);
        }

        //get diagonal moves (left and down)
        for (var i = 1; i <= movesDiagonalLeftDown; i++)
        {
            var newPos = new Vector2(piecePos.y + i, piecePos.x - i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        //get diagonal moves (right and down)
        for (var i = 1; i <= movesDiagonalRightDown; i++)
        {
            var newPos = new Vector2(piecePos.y + i, piecePos.x + i);
            var cell = GetCell(newPos);

            if (cell.color == toCheck.color) break;
            
            legalMoves.Add(newPos);
        }

        return legalMoves;
    }

    private List<Vector2> LegalPawnMoves(ChessPiece toCheck, Vector2 piecePos)
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

        switch (toCheck.color)
        {
            case ChessPiece.PieceColor.Black:
                #region Pawn Moves

                if (movesUp > 0)
                {
                    var newPos = new Vector2(piecePos.y - 1, piecePos.x);
                    var cell = GetCell(newPos);

                    if (cell.color != toCheck.color) legalMoves.Add(newPos);
                }

                if (movesUp > 1)
                {
                    var newPos = new Vector2(piecePos.y - 2, piecePos.x);
                    var cell = GetCell(newPos);

                    //Hacky way to check if pawn is able to move 2 squares
                    if (cell.type == ChessPiece.PieceType.Empty && piecePos.y == 6) legalMoves.Add(newPos);
                }

                if (movesDiagonalRightUp > 0)
                {
                    var newPos = new Vector2(piecePos.y - 1, piecePos.x + 1);
                    var cell = GetCell(newPos);

                    if (cell.color == ChessPiece.PieceColor.Black) legalMoves.Add(newPos);
                }

                if (movesDiagonalLeftUp > 0)
                {
                    var newPos = new Vector2(piecePos.y - 1, piecePos.x - 1);
                    var cell = GetCell(newPos);

                    if (cell.color == ChessPiece.PieceColor.Black) legalMoves.Add(newPos);
                } 
            
                #endregion
                break;
                        
            case ChessPiece.PieceColor.White:
                #region Pawn Moves

                if (movesDown > 0)
                {
                    var newPos = new Vector2(piecePos.y + 1, piecePos.x);
                    var cell = GetCell(newPos);

                    if (cell.color != toCheck.color) legalMoves.Add(newPos);
                }

                if (movesDown > 1)
                {
                    var newPos = new Vector2(piecePos.y + 2, piecePos.x);
                    var cell = GetCell(newPos);
                    
                    //Hacky way to check if pawn is able to move 2 squares
                    if (cell.type == ChessPiece.PieceType.Empty && piecePos.y == 1) legalMoves.Add(newPos);
                }

                if (movesDiagonalRightDown > 0)
                {
                    var newPos = new Vector2(piecePos.y + 1, piecePos.x + 1);
                    var cell = GetCell(newPos);

                    if (cell.color == ChessPiece.PieceColor.White) legalMoves.Add(newPos);
                }

                if (movesDiagonalLeftDown > 0)
                {
                    var newPos = new Vector2(piecePos.y + 1, piecePos.x - 1);
                    var cell = GetCell(newPos);

                    if (cell.color == ChessPiece.PieceColor.White) legalMoves.Add(newPos);
                } 
            
                #endregion
                break;
        }

        return legalMoves;
    }

    private List<Vector2> GetLegalMoves(ChessPiece toCheck, Vector2 piecePos)
    {
        List<Vector2> legalMoves = new List<Vector2>();
        switch (toCheck.type)
        {
           case ChessPiece.PieceType.Queen:
               legalMoves = LegalQueenMoves(toCheck, piecePos);
               break;
           case ChessPiece.PieceType.King:
               legalMoves = LegalKingMoves(toCheck, piecePos);
               break;
           case ChessPiece.PieceType.Rook:
               legalMoves = LegalRookMoves(toCheck, piecePos);
               break;
           case ChessPiece.PieceType.Knight:
               legalMoves = LegalKnightMoves(toCheck, piecePos);
               break;
           case ChessPiece.PieceType.Bishop:
               legalMoves = LegalBishopMoves(toCheck, piecePos);
               break;
           case ChessPiece.PieceType.Pawn:
               legalMoves = LegalPawnMoves(toCheck, piecePos);
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
