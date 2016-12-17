/*
 * Copyright (c) 2015 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalStateManager : MonoBehaviour {
    public List<GameObject> Players = new List<GameObject>();

    private int deadPlayers = 0;
    private int deadPlayerNumber = -1;

    public void PlayerDied(int playerNumber) {
        deadPlayers++;

        if (deadPlayers == 1) {
            deadPlayerNumber = playerNumber;
            Invoke("CheckPlayersDeath", .3f);
        }
    }

    void CheckPlayersDeath() {
        if (deadPlayers == 1) { //Single dead player, he's the winner

            if (deadPlayerNumber == 1) { //P1 dead, P2 is the winner
                Debug.Log("Player 2 is the winner!");
            }
            else { //P2 dead, P1 is the winner
                Debug.Log("Player 1 is the winner!");
            }
        }
        else {  //Multiple dead players, it's a draw
            Debug.Log("The game ended in a draw!");
        }
    }
}
