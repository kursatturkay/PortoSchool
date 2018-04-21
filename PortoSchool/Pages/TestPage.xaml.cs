using PortoSchool.Libs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using OfficeOpenXml.Style;
using OfficeOpenXml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PortoSchool.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        private static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            return bitmapImage;
        }
        private async void button1_Click(object sender, RoutedEventArgs e)
        {

            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(App.WorkingPath);
            var opts = new QueryOptions();
            opts.FileTypeFilter.Add(".jpg");
            opts.FolderDepth = FolderDepth.Deep;
            StorageFileQueryResult query = folder.CreateFileQueryWithOptions(opts);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            foreach (var x in fileList)
            {
             
                listView1.Items.Add(x.DisplayName);
            }

            //image.Source = fileList.FirstOrDefault().Path.Substring();
            BitmapImage img = new BitmapImage();
            img = await LoadImage(fileList.FirstOrDefault());

            image1.Source = img;

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void buttonReadXLS_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dirinfo = new DirectoryInfo(FileUtils.SharedDirectory);
            RunSample3(dirinfo, "s");
        }

        public static void RunSample3(DirectoryInfo outputDir, string connectionString)
        {

            string file = outputDir.FullName + @"\sample3.xlsx";

            if (File.Exists(file)) File.Delete(file);
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\sample3.xlsx");

            // ok, we can run the real code of the sample now
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //// uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                //// get handle to the existing worksheet
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sales");
                //var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");   //This one is language dependent
                //namedStyle.Style.Font.UnderLine = true;
                //namedStyle.Style.Font.Color.SetColor(Color.Blue);


                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "AdventureWorks Inc.";

                    //    const int startRow = 5;
                    //    int row = startRow;
                    //    //Create Headers and format them 

                    //    using (ExcelRange r = worksheet.Cells["A1:G1"])
                    //    {
                    //        r.Merge = true;
                    //        r.Style.Font.SetFromFont(new Font("Britannic Bold", 22, FontStyle.Italic));
                    //        r.Style.Font.Color.SetColor(Color.White);
                    //        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    //        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    //    }
                    //    worksheet.Cells["A2"].Value = "Year-End Sales Report";
                    //    using (ExcelRange r = worksheet.Cells["A2:G2"])
                    //    {
                    //        r.Merge = true;
                    //        r.Style.Font.SetFromFont(new Font("Britannic Bold", 18, FontStyle.Italic));
                    //        r.Style.Font.Color.SetColor(Color.Black);
                    //        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    //        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    //    }

                    //    worksheet.Cells["A4"].Value = "Name";
                    //    worksheet.Cells["B4"].Value = "Job Title";
                    //    worksheet.Cells["C4"].Value = "Region";
                    //    worksheet.Cells["D4"].Value = "Monthly Quota";
                    //    worksheet.Cells["E4"].Value = "Quota YTD";
                    //    worksheet.Cells["F4"].Value = "Sales YTD";
                    //    worksheet.Cells["G4"].Value = "Quota %";
                    //    worksheet.Cells["A4:G4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    worksheet.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    //    worksheet.Cells["A4:G4"].Style.Font.Bold = true;


                    //    // lets connect to the AdventureWorks sample database for some data
                    //    using (SqlConnection sqlConn = new SqlConnection(connectionString))
                    //    {
                    //        sqlConn.Open();
                    //        using (SqlCommand sqlCmd = new SqlCommand("select LastName + ', ' + FirstName AS [Name], EmailAddress, JobTitle, CountryRegionName, ISNULL(SalesQuota,0) AS SalesQuota, ISNULL(SalesQuota,0)*12 AS YearlyQuota, SalesYTD from Sales.vSalesPerson ORDER BY SalesYTD desc", sqlConn))
                    //        {
                    //            using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    //            {
                    //                // get the data and fill rows 5 onwards
                    //                while (sqlReader.Read())
                    //                {
                    //                    int col = 1;
                    //                    // our query has the columns in the right order, so simply
                    //                    // iterate through the columns
                    //                    for (int i = 0; i < sqlReader.FieldCount; i++)
                    //                    {
                    //                        // use the email address as a hyperlink for column 1
                    //                        if (sqlReader.GetName(i) == "EmailAddress")
                    //                        {
                    //                            // insert the email address as a hyperlink for the name
                    //                            string hyperlink = "mailto:" + sqlReader.GetValue(i).ToString();
                    //                            worksheet.Cells[row, 1].Hyperlink = new Uri(hyperlink, UriKind.Absolute);
                    //                        }
                    //                        else
                    //                        {
                    //                            // do not bother filling cell with blank data (also useful if we have a formula in a cell)
                    //                            if (sqlReader.GetValue(i) != null)
                    //                                worksheet.Cells[row, col].Value = sqlReader.GetValue(i);
                    //                            col++;
                    //                        }
                    //                    }
                    //                    row++;
                    //                }
                    //                sqlReader.Close();

                    //                worksheet.Cells[startRow, 1, row - 1, 1].StyleName = "HyperLink";
                    //                worksheet.Cells[startRow, 4, row - 1, 6].Style.Numberformat.Format = "[$$-409]#,##0";
                    //                worksheet.Cells[startRow, 7, row - 1, 7].Style.Numberformat.Format = "0%";

                    //                worksheet.Cells[startRow, 7, row - 1, 7].FormulaR1C1 = "=IF(RC[-2]=0,0,RC[-1]/RC[-2])";

                    //                //Set column width
                    //                worksheet.Column(1).Width = 25;
                    //                worksheet.Column(2).Width = 28;
                    //                worksheet.Column(3).Width = 18;
                    //                worksheet.Column(4).Width = 12;
                    //                worksheet.Column(5).Width = 10;
                    //                worksheet.Column(6).Width = 10;
                    //                worksheet.Column(7).Width = 12;
                    //            }
                    //        }
                    //        sqlConn.Close();
                    //    }

                    //    // lets set the header text 
                    //    worksheet.HeaderFooter.OddHeader.CenteredText = "AdventureWorks Inc. Sales Report";
                    //    // add the page number to the footer plus the total number of pages
                    //    worksheet.HeaderFooter.OddFooter.RightAlignedText =
                    //        string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    //    // add the sheet name to the footer
                    //    worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                    //    // add the file path to the footer
                    //    worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
                    //}
                    //// we had better add some document properties to the spreadsheet 

                    //// set some core property values
                    //xlPackage.Workbook.Properties.Title = "Sample 3";
                    //xlPackage.Workbook.Properties.Author = "John Tunnicliffe";
                    //xlPackage.Workbook.Properties.Subject = "ExcelPackage Samples";
                    //xlPackage.Workbook.Properties.Keywords = "Office Open XML";
                    //xlPackage.Workbook.Properties.Category = "ExcelPackage Samples";
                    //xlPackage.Workbook.Properties.Comments = "This sample demonstrates how to create an Excel 2007 file from scratch using the Packaging API and Office Open XML";

                    //// set some extended property values
                    //xlPackage.Workbook.Properties.Company = "AdventureWorks Inc.";
                    //xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.codeplex.com/MSFTDBProdSamples");

                    //// set some custom property values
                    //xlPackage.Workbook.Properties.SetCustomPropertyValue("Checked by", "John Tunnicliffe");
                    //xlPackage.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1147");
                    //xlPackage.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "ExcelPackage");

                    //// save the new spreadsheet
                    xlPackage.Save();
                }

                // if you want to take a look at the XML created in the package, simply uncomment the following lines
                // These copy the output file and give it a zip extension so you can open it and take a look!
                //FileInfo zipFile = new FileInfo(outputDir.FullName + @"\sample3.zip");
                //if (zipFile.Exists) zipFile.Delete();
                //newFile.CopyTo(zipFile.FullName);

                // return newFile.FullName;
            }
        }

        private void buttonReadXLS_Click_1(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
