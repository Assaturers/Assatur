using PolygonHead;

namespace Magicka.ModLoader.Globals;

public interface IEntityGlobal : IModded
{
    public bool PreUpdate(DataChannel dataChannel, float deltaTime);
    public bool PostUpdate(DataChannel dataChannel, float deltaTime);
}