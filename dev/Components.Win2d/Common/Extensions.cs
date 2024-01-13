using Microsoft.UI.Composition;

namespace WinUICommunity;

internal static class Extensions
{
    public static void BindCenterPoint(this Visual target)
    {
        var exp = target.Compositor.CreateExpressionAnimation(
            "Vector3(this.Target.Size.X / 2, this.Target.Size.Y / 2, 0f)");
        target.StartAnimation("CenterPoint", exp);
    }

    public static void BindSize(this Visual target, Visual source)
    {
        var exp = target.Compositor.CreateExpressionAnimation("host.Size");
        exp.SetReferenceParameter("host", source);
        target.StartAnimation("Size", exp);
    }
}
