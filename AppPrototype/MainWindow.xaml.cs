using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

namespace AppPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Document _doc;

        public MainWindow(Document doc)
        {
            _doc = doc;
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Database acCurDb = _doc.Database;

                // Starts a new transaction with the Transaction Manager
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    _doc.LockDocument();
                    // Open the Block table record for read
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                        OpenMode.ForRead) as BlockTable;

                    // Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite) as BlockTableRecord;

                    /* Creates a new MText object and assigns it a location,
                    text value and text style */
                    using (MText objText = new MText())
                    {
                        // Specify the insertion point of the MText object
                        objText.Location = new Autodesk.AutoCAD.Geometry.Point3d(2, 2, 0);

                        // Set the text string for the MText object
                        objText.Contents = myText.Text;

                        // Set the text style for the MText object
                        objText.TextStyleId = acCurDb.Textstyle;

                        // Appends the new MText object to model space
                        acBlkTblRec.AppendEntity(objText);

                        // Appends to new MText object to the active transaction
                        acTrans.AddNewlyCreatedDBObject(objText, true);
                    }

                    // Saves the changes to the database and closes the transaction
                    acTrans.Commit();
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
        }
    }
}
