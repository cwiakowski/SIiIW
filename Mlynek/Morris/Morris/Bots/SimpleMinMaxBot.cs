using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml.Controls;
using Morris.Models;
using Morris.Services;

namespace Morris.Bots
{
    public class SimpleMinMaxBot : AbstractBot
    {
        private TreeNode<ScoreHolder> _decisionTree;
        protected int MaxDepth;
        protected int Time;
        protected Stopwatch Stopwatch;
        public SimpleMinMaxBot(FieldState playersState, PlayerType playerType, ref TextBlock movesTextBlock) : base(playersState, playerType, ref movesTextBlock)
        {
            MaxDepth = 3;
        }

        public SimpleMinMaxBot(FieldState playersState, PlayerType playerType, ref TextBlock movesTextBlock, int maxDepth, int time = 0) : base(playersState, playerType, ref movesTextBlock)
        {
            MaxDepth = maxDepth;
            Time = time;
        }

        public override double CalculateBoardState(Board board, bool asEnemy = false)
        {
            return board.GetFields().Count(x => x.State.Equals(PlayersState))*3 - board.GetFields()
                .Count(x => x.State.Equals(_enemyState))*2;
        }

        public double UpdateDecisionTree(TreeNode<ScoreHolder> node, Board board, int placedStones, bool maximizing = true)
        {
            //If node i s a leaf return calculated value (some kind of heury)
            if (node.Level >= MaxDepth)
            {
                var data = node.Data;
                using (board)
                {
                    data.Score = CalculateBoardState(board, !maximizing);
                }
                return data.Score;
            }

            // Adding possible moves
            if (18 <= placedStones)
            {
                //GameState == InGame
                List<Field> playersFields;
                if (maximizing)
                {
                    playersFields = board.GetFields().Where(x => x.State == PlayersState).ToList();
                }
                else
                {
                    playersFields = board.GetFields().Where(x => x.State == _enemyState).ToList();
                }


                foreach (var f in playersFields)
                {
                    var possibleMoves = board.GetAvailableMoves(f.Cords);
                    foreach (var move in possibleMoves)
                    {
                        var moveSh = new ScoreHolder() { Board = board.Copy(), Decision = $"{f.Cords} {move.Cords}", PlacedStones = placedStones };
                        var field1 = moveSh.Board.Get(f.Cords);
                        var field2 = moveSh.Board.Get(move.Cords);
                        var temp = field1.State;
                        field1.State = field2.State;
                        field2.State = temp;
                        moveSh.Board.UpdateLastMove(field1.Cords, field2.Cords, field2.State);
                        if (moveSh.Board.GetMills().Except(board.GetMills()).Any())
                        {
                            //GameState == RemovingStones
                            List<Field> stonesToDelete;
                            if (maximizing)
                            {
                                stonesToDelete = board.GetFields().Where(x => x.State == _enemyState).ToList();
                            }
                            else
                            {
                                stonesToDelete = board.GetFields().Where(x => x.State == PlayersState).ToList();
                            }

                            foreach (var del in stonesToDelete)
                            {
                                var delSh = new ScoreHolder() { Board = moveSh.Board.Copy(), Decision = $"{moveSh.Decision} - {del.Cords}", PlacedStones = moveSh.PlacedStones };
                                delSh.Board.Get(del.Cords).State = FieldState.Empty;
                                node.AddChild(delSh);
                            }
                        }
                        else
                        {
                            node.AddChild(moveSh);
                        }
                    }
                }
            }
            else
            {
                //GameState == PlacingStones
                var possiblePlaces = board.GetFields().Where(x => x.State.Equals(FieldState.Empty)).ToList();
                foreach (var f in possiblePlaces)
                {
                    var sh = new ScoreHolder() {Board = board.Copy(), Decision = f.Cords, PlacedStones = placedStones+1};
                    sh.Board.Get(f.Cords).State = maximizing ? PlayersState : _enemyState;
                    if (sh.Board.GetMills().Except(board.GetMills()).Any())
                    {
                        //GameState == RemovingStones
                        List<Field> stonesToDelete;
                        if (maximizing)
                        {
                            stonesToDelete = board.GetFields().Where(x => x.State == _enemyState).ToList();
                        }
                        else
                        {
                            stonesToDelete = board.GetFields().Where(x => x.State == PlayersState).ToList();
                        }

                        foreach (var del in stonesToDelete)
                        {
                            var delSh = new ScoreHolder() {Board = sh.Board.Copy(), Decision = $"{sh.Decision} - {del.Cords}", PlacedStones = sh.PlacedStones};
                            delSh.Board.Get(del.Cords).State = FieldState.Empty;
                            node.AddChild(delSh);
                        }
                    }
                    else
                    {
                        node.AddChild(sh);
                    }
                }
            }
            //Calculate Score for all possible moves
            foreach (var child in node.Children)
            {
                child.Data.Mills = child.Data.Board.GetMills().ToList();
                child.Data.Score = UpdateDecisionTree(child, child.Data.Board, placedStones, !maximizing);
            }
            //Select value, depending if we are maximizing or minimizing score
            if (maximizing)
            {
                var sum = node.Children.Select(x => x.Data.Score).OrderByDescending(x => x).FirstOrDefault();
                return sum;
            }
            else
            {
                node.Children.Select(x => x.Data.Score).OrderBy(x => x).FirstOrDefault();
                return node.Children.Select(x => x.Data.Score).OrderBy(x => x).FirstOrDefault();
            }
        }

        private void DisposeTree()
        {
            _decisionTree?.Dispose();
        }


        public override ScoreHolder GetBestBoard(Board board, int placedStones)
        {
            DisposeTree();
            Stopwatch.StartNew();
            _decisionTree = new TreeNode<ScoreHolder>(new ScoreHolder());
            UpdateDecisionTree(_decisionTree, board, placedStones);
            var data = _decisionTree.Children.OrderByDescending(x => x.Data.Score).FirstOrDefault()?.Data;
            return data;
        }
    }
}