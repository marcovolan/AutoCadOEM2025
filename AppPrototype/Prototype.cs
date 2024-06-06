using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using SharedInterface;

namespace AppPrototype
{
    public class Prototype: IAppStart
    {
        public void Start(Document doc)
        {
            MainWindow window = new(doc);
            window.Show();
        }
    }
}
