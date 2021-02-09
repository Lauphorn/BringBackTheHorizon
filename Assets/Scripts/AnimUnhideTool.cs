using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimUnhideTool
{
    [MenuItem("Tools/Animator States Unhide Fix")]
    private static void Fix()
    {
        if (Selection.objects.Length < 1)
            throw new UnityException("Select animator controller(s) before try fix it!");

        int scnt = 0;

        foreach (Object o in Selection.objects)
        {
            AnimatorController ac = o as AnimatorController;
            if (ac == null)
                continue;

            foreach (AnimatorControllerLayer layer in ac.layers)
            {

                foreach (ChildAnimatorState curState in layer.stateMachine.states)
                {
                    scnt = FixState(scnt, curState);
                }

                scnt = FixStateMachines(scnt, layer.stateMachine, layer.stateMachine.stateMachines);

            }
            EditorUtility.SetDirty(ac);
        }
        Debug.Log("Fixing " + scnt + " states done!");
    }

    private static int FixStateMachines(int scnt, AnimatorStateMachine parent, ChildAnimatorStateMachine[] stateMachines)
    {
        foreach (ChildAnimatorStateMachine curStateMachine in stateMachines)
        {
            foreach (var trans in parent.GetStateMachineTransitions(curStateMachine.stateMachine))
            {
                trans.hideFlags = (HideFlags)1;
                scnt++;
            }

            if (curStateMachine.stateMachine.hideFlags != (HideFlags)1)
            {
                curStateMachine.stateMachine.hideFlags = (HideFlags)1;
            }

            if (curStateMachine.stateMachine.stateMachines != null)
            {
                scnt = FixStateMachines(scnt, curStateMachine.stateMachine, curStateMachine.stateMachine.stateMachines);
            }

            if (curStateMachine.stateMachine.entryTransitions != null)
            {
                foreach (AnimatorTransition curTrans in curStateMachine.stateMachine.entryTransitions)
                {
                    curTrans.hideFlags = (HideFlags)1;
                }
            }

            foreach (ChildAnimatorState curState in curStateMachine.stateMachine.states)
            {
                scnt = FixState(scnt, curState);
            }
        }

        return scnt;
    }

    private static int FixState(int scnt, ChildAnimatorState curState)
    {
        if (curState.state.hideFlags != (HideFlags)1)
        {
            curState.state.hideFlags = (HideFlags)1;
            scnt++;
        }

        if (curState.state.motion != null)
        {
            curState.state.motion.hideFlags = (HideFlags)1;
        }

        if (curState.state.transitions != null)
        {
            foreach (AnimatorStateTransition curTrans in curState.state.transitions)
            {
                curTrans.hideFlags = (HideFlags)1;
            }
        }

        if (curState.state.behaviours != null)
        {
            foreach (StateMachineBehaviour behaviour in curState.state.behaviours)
            {
                behaviour.hideFlags = (HideFlags)1;
            }
        }

        return scnt;
    }
}