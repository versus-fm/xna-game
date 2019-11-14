using Game5.Env;
using Microsoft.Xna.Framework;

namespace Game5.Service.Services.Interfaces
{
    public interface ICamera
    {
        Entity Focus { get; set; }
        Point ViewSize { get; set; }
        Rectangle VisibleArea { get; }

        Matrix GetInverseMatrix();
        Matrix GetMatrix();
        Vector2 GetPosition();
        float GetScale();
        void LockCamera();
        void Scale(float scale);
        void Translate(Vector2 vector);
        void UnlockCamera();
        void Update();
    }
}