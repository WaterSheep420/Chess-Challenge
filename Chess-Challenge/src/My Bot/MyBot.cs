using ChessChallenge.API;
using System;
using System.Diagnostics;
using System.IO;

public class MyBot : IChessBot {
    Board board;
    Move[] moves;
    public Move Think(Board b, Timer timer) {
        board = b;
        moves = board.GetLegalMoves();
        Move bestMove = moves[0];
        int bestMoveEval = 0;
        int currentMoveEval;
        foreach (Move move in moves) {
            b.MakeMove(move);
            currentMoveEval = Search(0, int.MinValue, int.MaxValue);
            b.UndoMove(move);
            if (currentMoveEval > bestMoveEval) {
                bestMove = move;
                bestMoveEval = currentMoveEval;
            }
        }
        return bestMove;
    }
    int Search(int depth, int alpha, int beta) {
        if (depth == 0)
            return Evaluate();
        moves = board.GetLegalMoves();
        if (moves.Length == 0) {
            if (board.IsInCheck())
                return int.MinValue;
            return 0;
        }
        foreach (Move move in moves) {
            board.MakeMove(move);
            int evaluation = -Search(depth - 1, -beta, -alpha);
            board.UndoMove(move);
            if (evaluation >= beta) {
                return beta;
            }
            alpha = Math.Max(alpha, evaluation);
        }
        return alpha;
    }
    int Evaluate() {
        int moveValue = CountMaterial(!board.IsWhiteToMove) - CountMaterial(board.IsWhiteToMove);
        return moveValue;
    }
    int CountMaterial(bool isWhite) {

        // Piece values: null, pawn, knight, bishop, rook, queen
        int[] pieceValues = { 0, 100, 300, 310, 500, 900 };

        int material = 0;
        material += board.GetPieceList(PieceType.Pawn, isWhite).Count * pieceValues[1];
        material += board.GetPieceList(PieceType.Knight, isWhite).Count * pieceValues[2];
        material += board.GetPieceList(PieceType.Bishop, isWhite).Count * pieceValues[3];
        material += board.GetPieceList(PieceType.Rook, isWhite).Count * pieceValues[4];
        material += board.GetPieceList(PieceType.Queen, isWhite).Count * pieceValues[5];

        return material;
    }
}