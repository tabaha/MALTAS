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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using System.Linq;

namespace MALTAS {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    private void btnSelectFile_Click(object sender, RoutedEventArgs e) {
      using (var dialog = new System.Windows.Forms.OpenFileDialog()) {
        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && dialog.FileName != null) {
          txtXMLPath.Text = dialog.FileName;
        }
      }
    }

    private void btnToCSV_Click(object sender, RoutedEventArgs e) {
      if (txtXMLPath.Text.Equals(string.Empty) || !File.Exists(txtXMLPath.Text))
        return;

      ExportToCSV(txtXMLPath.Text, "|");

    }

    private void ExportToCSV(string xmlPath, string separator) {
      DataSet malDataSet = new DataSet();
      try {
        malDataSet.ReadXml(new StreamReader(xmlPath));
        var baseDir = System.IO.Path.GetDirectoryName(xmlPath);
        foreach(DataTable dt in malDataSet.Tables) {
          using (var writer = new StreamWriter(baseDir + @"\" + dt.TableName + ".csv")) {
            writer.WriteLine(string.Join(separator, dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName)));
            foreach(DataRow row in dt.Rows) {
              writer.WriteLine(string.Join(separator, row.ItemArray.Cast<string>().Select((s => s.Replace(@"\r\n", string.Empty)))));
            }
          }
        }
      }
      catch (Exception e) {
        MessageBox.Show($"RIP IN PIECES {0}", e.Message);
      }
    }
  }
}
