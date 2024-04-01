using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void Execute(Animator anim);
}

public class PerformForward : Command
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isWalkingForward");
    }
}

public class PerformBackward : Command
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isWalkingForward");
    }
}

public class PerformLeft : Command
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isWalkingForward");
    }
}

public class PerformRight : Command
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isWalkingForward");
    }
}

public class PerformJump : Command
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isJumping");
    }
}

public class PerformKick : Command
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isKicking");
    }
}

public class PerformPunch : Command
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger("isPunching");
    }
}

public class DoNothing : Command
{
    public override void Execute(Animator anim)
    {

    }
}