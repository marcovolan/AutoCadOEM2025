using Document = Autodesk.AutoCAD.ApplicationServices.Document;

namespace SharedInterface
{
    public interface IAppStart
    {  
        void Start(Document doc);
    }
}
