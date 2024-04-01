using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Commands
{
    public abstract void Execute(Animator anim);
}

public class PerformForward : Commands
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isWalkingForward");
    }
}

public class PerformBackward : Commands
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isWalkingForward");
    }
}

public class PerformLeft : Commands
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isWalkingForward");
    }
}

public class PerformRight : Commands
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isWalkingForward");
    }
}

public class PerformJump : Commands
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isJumping");
    }
}

public class PerformKick : Commands
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isKicking");
    }
}

public class PerformPunch : Commands
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isPunching");
    }
}

public class DoNothing : Commands
{
    public override void Execute(Animator anim)
    {

    }
}