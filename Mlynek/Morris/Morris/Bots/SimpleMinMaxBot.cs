using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml.Controls;
using Morris.Models;
using Morris.Services;

namespace Morris.Bots
{
    /*
     * cykl zycia spermiarza
     *      wybor ruchu/dodania piona
     *      sprawdzeniee mlynkow
     *      mozliwe usuwanie wtedy
     *      
     */
    public class SimpleMinMaxBot : AbstractBot
    {
        private TreeNode<ScoreHolder> _decisionTree;
        private int _maxDepth;
        public SimpleMinMaxBot(FieldState playersState, PlayerType playerType, ref TextBlock movesTextBlock) : base(playersState, playerType, ref movesTextBlock)
        {
            _maxDepth = 3;
        }

        public override double CalculateBoardState(Board board, bool asEnemy = false)
        {
            return board.GetFields().Count(x => x.State.Equals(PlayersState))*3 - board.GetFields()
                .Count(x => x.State.Equals(_enemyState))*2;
        }

        public void UpdateDecisionTree(TreeNode<ScoreHolder> node, Board board)
        {
            if (node.Depth > _maxDepth)
            {
                return;
            }

            ScoreHolder scoreHolder;
            if (node.IsRoot)
            {
                scoreHolder = node.Data;
            }
            else
            {
                scoreHolder = node.Parent.Data;
            }
            
            if (18 <= scoreHolder.PlacedStones)
            {
                //moving stones 
                List<Field> playersField = scoreHolder.Board.GetFields().Where(x => x.State.Equals(scoreHolder.PlayersTurn)).ToList();
                foreach (var field in playersField)
                {
                    List<Field> moves = scoreHolder.Board.GetAvailableMoves(field.Cords).ToList();
                    foreach (var move in moves)
                    {
                        var newScoreHolder = new ScoreHolder() { Board = scoreHolder.Board.Copy(), PlayersTurn = scoreHolder.PlayersTurn.Equals(FieldState.P1) ? FieldState.P2 : FieldState.P1, PlacedStones = scoreHolder.PlacedStones, Decision = $"{field.Cords} {move.Cords}" };
                        var field1 = newScoreHolder.Board.Get(field.Cords);
                        var field2 = newScoreHolder.Board.Get(move.Cords);
                        var temp = field1.State;
                        field1.State = field2.State;
                        field2.State = temp;
                        if (newScoreHolder.Board.GetMills().Except(scoreHolder.Board.GetMills()).Any())
                        {
                            var fieldsToDelete = newScoreHolder.Board.GetFields().Where(x =>
                                x.State.Equals(newScoreHolder.PlayersTurn.Equals(FieldState.P1)
                                    ? FieldState.P2
                                    : FieldState.P1)).ToList();
                            foreach (var fd in fieldsToDelete)
                            {
                                var delScoreHolder = new ScoreHolder()
                                {
                                    Board = newScoreHolder.Board.Copy(),
                                    PlayersTurn = newScoreHolder.PlayersTurn,
                                    Decision = $"{field.Cords} {move.Cords}"
                                };
                                delScoreHolder.Board.Get(fd.Cords).State = FieldState.Empty;
                                delScoreHolder.Mills = delScoreHolder.Board.GetMills().ToList();
                                node.AddChild(delScoreHolder);
                            }
                        }
                        else
                        {
                            newScoreHolder.Mills = newScoreHolder.Board.GetMills().ToList();
                            node.AddChild(newScoreHolder);
                        }
                    }

                }
            }
            else
            {
                List<Field> empty = scoreHolder.Board.GetFields()
                    .Where(x => x.State.Equals(FieldState.Empty)).ToList();
                foreach (var f in empty)
                {
                    var newScoreHolder = new ScoreHolder() { Board = scoreHolder.Board.Copy(), PlayersTurn = scoreHolder.PlayersTurn.Equals(FieldState.P1) ? FieldState.P2 : FieldState.P1, PlacedStones = scoreHolder.PlacedStones, Decision = $"{f.Cords}" };
                    if (newScoreHolder.Board.GetMills().Except(scoreHolder.Board.GetMills()).Any())
                    {
                        var fieldsToDelete = newScoreHolder.Board.GetFields().Where(x =>
                            x.State.Equals(newScoreHolder.PlayersTurn.Equals(FieldState.P1)
                                ? FieldState.P2
                                : FieldState.P1)).ToList();
                        foreach (var fd in fieldsToDelete)
                        {
                            var delScoreHolder = new ScoreHolder()
                            {
                                Board = newScoreHolder.Board.Copy(),
                                PlayersTurn = newScoreHolder.PlayersTurn,
                                Decision = $"{f.Cords}"
                            };
                            delScoreHolder.Board.Get(fd.Cords).State = FieldState.Empty;
                            delScoreHolder.Mills = delScoreHolder.Board.GetMills().ToList();
                            node.AddChild(delScoreHolder);
                        }
                    }
                    else
                    {
                        newScoreHolder.Mills = newScoreHolder.Board.GetMills().ToList();
                        newScoreHolder.Board.Get(newScoreHolder.Decision).State = newScoreHolder.PlayersTurn;
                        node.AddChild(newScoreHolder);
                    }
                }
            }

            foreach (var child in node.Children)
            {
                UpdateDecisionTree(child, child.Data.Board);
            }
        }

        public void UpdateDecisionTree(Board board, int placedStones)
        {
            var scoreHolder = new ScoreHolder()
                {Board = board.Copy(), Mills = board.GetMills().ToList(), PlacedStones = placedStones, PlayersTurn = _enemyState};
            _decisionTree = new TreeNode<ScoreHolder>(scoreHolder, 0);
            UpdateDecisionTree(_decisionTree, _decisionTree.Data.Board);   
        }

        public void UpdateTreeScores(TreeNode<ScoreHolder> node)
        {
            if (!node.IsLeaf)
            {
                foreach (var child in node.Children)
                {
                    UpdateTreeScores(child);
                }

                if (node.Depth % 2 == 1)
                {
                    node.Data.Score = node.Children.Max(x => x.Data.Score);
                }
                else
                {
                    node.Data.Score = node.Children.Min(x => x.Data.Score); ;
                }
            }
            else
            {
                if (node.Depth % 2 == 1)
                {
                    node.Data.Score = CalculateBoardState(node.Data.Board);
                }
                else
                {
                    node.Data.Score = CalculateBoardState(node.Data.Board, true);
                }
            }

            if (node.IsRoot)
            {
                node.Data = node.Data;
            }
        }


        public override ScoreHolder GetBestBoard(Board board, int placedStones)
        {
            UpdateDecisionTree(board, placedStones);
            UpdateTreeScores(_decisionTree);
            var data = _decisionTree.Children.OrderByDescending(x => x.Data.Score).FirstOrDefault()?.Data;
            return data;
        }
    }
}