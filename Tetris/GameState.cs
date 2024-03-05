namespace Tetris
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBLock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; }

        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBLock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        private bool BlockFits()
        {
            foreach (Position p in CurrentBLock.TilePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBLock;
                CurrentBLock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Block tmp = CurrentBLock;
                CurrentBLock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;
        }

        public void RotateBlockCW()
        {
            CurrentBLock.RotateCW();

            if (!BlockFits())
            {
                CurrentBLock.RotateCCW();
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBLock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBLock.RotateCW();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBLock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBLock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBLock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBLock.Move(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBLock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBLock.Id;
            }
            
            Score += GameGrid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBLock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        public void MoveBlockDown()
        {
            CurrentBLock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBLock.Move(-1, 0);
                PlaceBlock();
            }
        }
    }
}
