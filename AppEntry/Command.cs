using System.Reflection;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using SharedInterface;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

[assembly: CommandClass(typeof(AppEntry.Command))]

namespace AppEntry
{
    public class Command
    {
        [CommandMethod("Start")]
        public void Start()
        {
            try
            {
                var appAssembly =
                    Assembly.LoadFrom(@"C:\Users\Public\Documents\AutoCAD OEM 2025\build\TinCAD\AppPrototype.dll");

                var implementors = new List<IAppStart?>();
                var types = appAssembly?.GetTypes()
                                .Where(t => t != typeof(IAppStart) &&
                                            typeof(IAppStart).IsAssignableFrom(t)).ToList() ??
                            new List<Type>();

                foreach (var type in types)
                {
                    implementors.Add((IAppStart)Activator.CreateInstance(type)!);
                }

                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager
                    .MdiActiveDocument;


                if (implementors.FirstOrDefault() is IAppStart app)
                {
                    app.Start(acDoc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.Exception sysEx)
            {
                MessageBox.Show(sysEx.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
    }
}