using Game5;

namespace LevelEditor
{
    internal class LevelEditor : ExtendedGame
    {
        protected override void LoadGameContent()
        {
            IsMouseVisible = true;
            base.LoadGameContent();
        }
    }
}