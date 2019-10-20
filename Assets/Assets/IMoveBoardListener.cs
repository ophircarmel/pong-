using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveBoardListener
{
    // <summary>
    // Move to another board.
    // </summary>
    // <param name="prevoius"> The board to move from. </param>
    // <param name="next"> The board to move to. </param>
    void MoveBoard(int previous, int next);
}
