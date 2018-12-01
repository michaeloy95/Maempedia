namespace Maempedia.Interfaces
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);

        byte[] ReadAllBytes(string path);
    }
}
