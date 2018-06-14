using System;
using System.Collections.Generic;

namespace GridWorld
{
    /// <summary>
    /// This player just moves Up. It illustrates the Tanks API.
    /// To compile correctly you must have references to 
    /// GridWorldEngine.dll, GameInterface.dll and Tanks.dll.
    /// </summary>
    public class ambrewer : BasePlayer
    {
        PlayerWorldState MyWorldState; // the piece of the board I can see this turn

        GridSquare[,] MyMap = null; // records the map of all permanent objects in grid squares seen (i.e. ignore living tanks). Unseen GridSquares are null.

        public ambrewer() : base()
        {
            this.Name = "TanksFirstPlayer";
        }

        public override ICommand GetTurnCommands(IPlayerWorldState igrid)
        {
            MyWorldState = (PlayerWorldState)igrid;

            UpdateMyMap(); // update my "map" of squares seen to date

            // Write all of the properties of this player which are inherited from BasePlayer to the Trace Window
            WriteTrace("this.AITimePerGame: " + this.AITimePerGame);
            WriteTrace("this.AITimePerTurn: " + this.AITimePerTurn);
            WriteTrace("this.CPUTimeUsedSeconds: " + this.CPUTimeUsedSeconds);
            WriteTrace("this.ID: " + this.ID);
            WriteTrace("this.MaxNumOverTimeAITurns: " + this.MaxNumOverTimeAITurns);
            WriteTrace("this.MaxNumTurns: " + this.MaxNumTurns);
            WriteTrace("this.Name: " + this.Name);
            WriteTrace("this: " + this);

            // Write all of the properties of this player's PlayerWorldState to the Trace Window
            WriteTrace("MyWorldState.CanSee(PlayerWorldState.Facing.Up,5,0,7,4,mygrid): " + MyWorldState.CanSee(PlayerWorldState.Facing.Up, 5, 0, 7, 4, MyMap));
            WriteTrace("MyWorldState.EmptySquaresSeen: " + MyWorldState.EmptySquaresSeen);
            WriteTrace("MyWorldState.GridHeightInSquares: " + MyWorldState.GridHeightInSquares);
            WriteTrace("MyWorldState.GridWidthInSquares: " + MyWorldState.GridWidthInSquares);
            WriteTrace("MyWorldState.ID: " + MyWorldState.ID);
            WriteTrace("MyWorldState.Kills: " + MyWorldState.Kills);
            WriteTrace("MyWorldState.MaximumVisionDistance: " + MyWorldState.MaximumVisionDistance);
            WriteTrace("MyWorldState.MyFacing: " + MyWorldState.MyFacing);
            WriteTrace("MyWorldState.MyGridSquare: " + MyWorldState.MyGridSquare);
            WriteTrace("MyWorldState.MyVisibleSquares: ");
            foreach (GridSquare gs in MyWorldState.MyVisibleSquares)
                WriteTrace(gs);
            WriteTrace("MyWorldState.PlayerCount: " + MyWorldState.PlayerCount);
            WriteTrace("MyWorldState.RockSquaresSeen: " + MyWorldState.RockSquaresSeen);
            WriteTrace("MyWorldState.Score: " + MyWorldState.Score);
            WriteTrace("MyWorldState.ScorePerEmptySquareSeen: " + MyWorldState.ScorePerEmptySquareSeen);
            WriteTrace("MyWorldState.ScorePerOpposingTankDestroyed: " + MyWorldState.ScorePerOpposingTankDestroyed);
            WriteTrace("MyWorldState.ScorePerRockSquareSeen: " + MyWorldState.ScorePerRockSquareSeen);
            WriteTrace("MyWorldState.ShotsFired: " + MyWorldState.ShotsFired);
            WriteTrace("MyWorldState: " + MyWorldState);
            WriteTrace("MyWorldState.TurnNumber: " + MyWorldState.TurnNumber);

            WriteTrace("My Map:");
            WriteTrace(MyMapToString());

            // the command
            Command c = new Command(Command.Move.Up, true); // move up and shoot
            return c;
        }

        /// <summary>
        /// Update my "map" of all permanent objects (rocks and destroyed tanks) and empty GridSquares I have seen so far.
        /// </summary>
        private void UpdateMyMap()
        {
            int x, y;

            // initialise mygrid if it hasn't yet been created
            if (MyMap == null)
            {
                MyMap = new GridSquare[MyWorldState.GridWidthInSquares, MyWorldState.GridHeightInSquares];
                for (x = 0; x < MyWorldState.GridWidthInSquares; x++)
                    for (y = 0; y < MyWorldState.GridHeightInSquares; y++)
                        MyMap[x, y] = null; // set each unseen grid square to null
            }

            // update the map in mygrid
            foreach (GridSquare gs in MyWorldState.MyVisibleSquares)
            {
                if (MyMap[gs.X, gs.Y] == null) // not yet seen GridSquare gs
                {
                    if (gs.Contents == GridSquare.ContentType.Rock)
                        MyMap[gs.X, gs.Y] = new GridSquare(gs.X, gs.Y, GridSquare.ContentType.Rock);
                    else if (gs.Contents == GridSquare.ContentType.DestroyedTank)
                        MyMap[gs.X, gs.Y] = new GridSquare(gs.X, gs.Y, GridSquare.ContentType.DestroyedTank);
                    else
                        MyMap[gs.X, gs.Y] = new GridSquare(gs.X, gs.Y, GridSquare.ContentType.Empty);

                }
            }
        }

        /// <summary>
        /// Output the current map in a readable text format.
        /// # = blocked, . = empty
        /// </summary>
        private string MyMapToString()
        {
            string outstr = "";

            for (int y = MyWorldState.GridHeightInSquares - 1; y >= 0; y--)
            {
                for (int x = 0; x < MyWorldState.GridWidthInSquares; x++)
                {
                    if (MyMap[x, y] == null)
                        outstr += " "; // not yet seen
                    else if (MyMap[x, y].Contents == GridSquare.ContentType.Rock)
                        outstr += "#"; // rock
                    else if (MyMap[x, y].Contents == GridSquare.ContentType.DestroyedTank)
                        outstr += "*"; // destroyed tank
                    else if (MyWorldState.MyGridSquare.X == x && MyWorldState.MyGridSquare.Y == y)
                        outstr += GetTankSymbol(MyWorldState.MyFacing); // my tank
                    else
                        outstr += "."; // empty
                }
                outstr += "\r\n";
            }

            return outstr;
        }

        /// <summary>
        /// Get the symbol >, v, ^ etc. for ths tank facing
        /// </summary>
        private string GetTankSymbol(PlayerWorldState.Facing facing)
        {
            if (facing == PlayerWorldState.Facing.Up)
                return "^";
            else if (facing == PlayerWorldState.Facing.Down)
                return "v";
            else if (facing == PlayerWorldState.Facing.Left)
                return "<";
            else if (facing == PlayerWorldState.Facing.Right)
                return ">";

            return " "; // shouldn't ever get here
        }
    }
}
