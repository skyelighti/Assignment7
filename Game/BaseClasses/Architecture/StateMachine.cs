using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
public class StateMachine
{
    //do not modify/if any modifications are made reminder to commit to base asap
    Dictionary<int, System.Action> enterStateFuncLookup = new Dictionary<int, System.Action>();
    Dictionary<int, System.Action<GameTime>> updateStateFuncLookup = new Dictionary<int, System.Action<GameTime>>();
    Dictionary<int, System.Action> exitStateFuncLookup = new Dictionary<int, System.Action>();

    public int CurrentState => currentState;
    int currentState, nextState, numOfStates;

    public StateMachine()
    {
        currentState = -1;
        nextState = -1;
    }
    public StateMachine(int numOfState)
    {
        this.numOfStates = numOfState;
        currentState = -1;
        nextState = -1;
    }

    //sete the three functions for each state
    public void SetStateFunctions(int state, System.Action enterStateFunc, System.Action<GameTime> updateStateFunc, System.Action exitStateFunc)
    {
        if (ValidState(state))
        {
            enterStateFuncLookup[state] = enterStateFunc;
            updateStateFuncLookup[state] = updateStateFunc;
            exitStateFuncLookup[state] = exitStateFunc;
        }
    }

    //total number of states in statemachine
    public void SetNumOfStates(int numOfStates)
    {
        this.numOfStates = numOfStates;
    }

    public void SetEnterStateFunc(int state, System.Action func)
    {
        if (ValidState(state))
        {
            enterStateFuncLookup[state] = func;
            //runs the enter state func 
        }
    }
    public void SetUpdateStateFunc(int state, System.Action<GameTime> func)
    {
        if (ValidState(state))
        {
            updateStateFuncLookup[state] = func;
        }
        //think i need to modify the update function for gameTime
    }
    public void SetExitStateFunc(int state, System.Action func)
    {
        if (ValidState(state))
        {
            exitStateFuncLookup[state] = func;
        }
    }
    public void SetState(int nextState)
    {
        if (ValidState(nextState))
        {
            this.nextState = nextState;
        }

    }

    public void UpdateState(GameTime gameTime)
    {
        ApplyPendingTransition();

        if (currentState >= 0)
        {
            updateStateFuncLookup[currentState](gameTime);
        }//updates current stage;
    }

    /// <summary>
    /// private validation function
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private bool ValidState(int state)
    {
        if (state < numOfStates && state >= 0)
        {
            return true;
            //is valid state
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(state), state,
            "Not a valid state.");
        }
    }

    /// <summary>
    /// applies each transition, does not need to immediate gamplay update
    /// this is useful for loading from a save
    /// </summary>
    public void ApplyPendingTransition()
    {
        if (currentState == nextState || nextState < 0)
        {
            return;
        }

        if (currentState >= 0)
        {
            exitStateFuncLookup[currentState]();
        }

        enterStateFuncLookup[nextState]();
        currentState = nextState;
    }
}
