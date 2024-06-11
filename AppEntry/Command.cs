using System.Reflection;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Microsoft.Win32;
using SharedInterface;
using Exception = Autodesk.AutoCAD.Runtime.Exception;
using Registry = Autodesk.AutoCAD.Runtime.Registry;
using RegistryKey = Autodesk.AutoCAD.Runtime.RegistryKey;

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
                Assembly appAssembly =
                     Assembly.LoadFrom(@"C:\Users\Public\Documents\AutoCAD OEM 2025\build\TinCAD\TinDll\AppPrototype.dll");

                List<IAppStart> implementors = new List<IAppStart>();
                List<Type> types = appAssembly.GetTypes()
                    .Where(t => t != typeof(IAppStart) &&
                                typeof(IAppStart).IsAssignableFrom(t)).ToList();

                foreach (Type type in types)
                {
                    implementors.Add((IAppStart)Activator.CreateInstance(type)!);
                }

                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager
                    .MdiActiveDocument;


                if (implementors.FirstOrDefault() is { } app)
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