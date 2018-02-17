using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Xml.Linq;
using Windows.Storage;
using System.Text.RegularExpressions;

namespace PortoSchool.Libs
{
    public class SheetData
    {
        public ZipArchiveEntry sheetEntryInArchive { get; set; }
        public List<Dictionary<String, String>> data { get; set; }
    }

    public class ExcelReader
    {
        #region Constants

        private const String WORKSHEET = "xl/worksheets/";
        private const String SHARED_STRING = "xl/sharedStrings.xml";

        #endregion

        #region Member Declaration

        //private List<Dictionary<String, String>> dataSet = null;
        private List<String> headers = null;
        private List<String> rowIdentifierList = null; // Holds the name of the rownumber reference - A,AA,BB

        private StorageFile targetFile;
        static List<string> _sharedStrings;

        #endregion
        // Member declaration

        #region Single Instance

        public static ExcelReader excelReader = null;

        public static ExcelReader SharedReader()
        {
         

            if (excelReader == null)
            {
                excelReader = new ExcelReader();
                excelReader.headers = new List<String>();
                excelReader.rowIdentifierList = new List<String>();
                //excelReader.dataSet = new List<Dictionary<String, String>>();
            }

            excelReader.headers.Clear();
            excelReader.rowIdentifierList.Clear();
            return excelReader;
        }

        #endregion

        #region DataProcessing

        private async Task<List<Dictionary<String, String>>> ReadSheetData(ZipArchiveEntry worksheet)
        {
            List<Dictionary<String, String>> sheetDataSet = null;

            await Task.Run(() =>
            {
                try
                {
                    if (worksheet != null)
                    {
                        this.headers.Clear();
                        this.rowIdentifierList.Clear();

                        sheetDataSet = new List<Dictionary<string, string>>();
                        using (var sr = worksheet.Open())
                        {
                            XDocument xdoc = XDocument.Load(sr);
                            //get element to first sheet data
                            XNamespace xmlns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
                            XElement sheetData = xdoc.Root.Element(xmlns + "sheetData");

                            //build header and row defintion list 
                            var firstRow = sheetData.Elements().FirstOrDefault();

                            foreach (var c in firstRow.Elements())
                            {
                                //the c element, if have attribute t, will need to consult sharedStrings
                                string val = c.Elements().FirstOrDefault().Value;
                                if (c.Attribute("t") != null)
                                {
                                    headers.Add(_sharedStrings[Convert.ToInt32(val)]);
                                }
                                else
                                {
                                    headers.Add(val);
                                }

                                // Row Identifiers will helpfull in tracking the empty cells
                                var rowAttribute = c.Attribute("r");

                                if (rowAttribute != null)
                                {
                                    String rowAttributeValue = rowAttribute.Value;
                                    int pos = Regex.Match(rowAttributeValue, "[0-9]").Index;

                                    rowAttributeValue = rowAttributeValue.Substring(0, pos);

                                    rowIdentifierList.Add(rowAttributeValue);
                                }
                            }

                            foreach (var row in sheetData.Elements())
                            {
                                //skip row 1
                                if (row.Attribute("r").Value == "1")
                                    continue;
                                Dictionary<string, string> rowData = new Dictionary<string, string>();
                                int i = 0;
                                var elementsToLookup = row.Elements();
                                for (int rowResolverIndex = 0; rowResolverIndex < elementsToLookup.Count(); rowResolverIndex++)
                                {
                                    var c = elementsToLookup.ElementAt(rowResolverIndex);

                                    // Get the row number of the cell to check whether we are missing out any entry 
                                    // due to empty data
                                    var rowAttribute = c.Attribute("r");

                                    if (rowAttribute != null)
                                    {
                                        String actualRowIdentifier = rowAttribute.Value;
                                        int pos = Regex.Match(actualRowIdentifier, "[0-9]").Index;

                                        actualRowIdentifier = actualRowIdentifier.Substring(0, pos);

                                        // Get the element from rowIdentifierList for the index 

                                        for (int lookupIndex = i; lookupIndex < rowIdentifierList.Count(); lookupIndex++)
                                        {
                                            String expectedRowIdentifier = rowIdentifierList.ElementAt(lookupIndex);

                                            if (expectedRowIdentifier.Equals(actualRowIdentifier))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                String keyToIndex = headers[lookupIndex];
                                                if (!rowData.ContainsKey(keyToIndex))
                                                {
                                                    rowData.Add(keyToIndex, ""); // Placing an empty data for the particular header
                                                    i++;
                                                }
                                            }
                                        }
                                    }

                                    //down to each c element

                                    // Check for element value v and get the value
                                    string val = String.Empty;

                                    //-- Fails when formula / formatting options are available

                                    String temp = String.Empty;

                                    var elements = c.Elements();

                                    if (elements.Count() > 0)
                                    {
                                        temp = elements.FirstOrDefault().Value;
                                    }
                                    else
                                    {
                                        // Invalid/No data
                                    }
                                    var _matchedElementsList = elements.Where(element => element.Name.LocalName.Equals("v"));

                                    var _element = String.Empty;

                                    if (_matchedElementsList.Count() > 0)
                                    {
                                        val = _matchedElementsList.FirstOrDefault().Value;
                                    }
                                    else
                                    {
                                        // Invalid/No data
                                    }

                                    // Get the value after checking

                                    if (c.Attribute("t") != null)
                                    {
                                        var valueOfStringType = c.Attribute("t").Value;

                                        // Check whether it is "s" / "e"

                                        // Do this if it is s

                                        if (valueOfStringType.Equals("s"))
                                        {
                                            rowData.Add(headers[i], _sharedStrings[Convert.ToInt32(val)]);
                                        }
                                        else if (valueOfStringType.Equals("e"))
                                        {
                                            rowData.Add(headers[i], temp);
                                        }
                                    }
                                    else if (c.Attribute("s") != null)
                                    {
                                        // Date is stored in excel as days passed since January 1 1990
                                        // Logic for converting the long values (days since january 1 1990)

                                        //Excel manages the date in older system 
                                        // Refer this link : http://stackoverflow.com/questions/11968570/converting-double-to-datetime

                                        // One Fix is to subtract 2 from the value

                                        // Or using the 
                                        //DateTime myDate = DateTime.FromOADate(41172); -- TODO : Check this 

                                        String dateValueAsString = String.Empty;

                                        if (!String.IsNullOrEmpty(val))
                                        {
                                            long daysSince1990 = Convert.ToInt64(Convert.ToDouble(val) - 2); // Val - extracted from excel sheet

                                            DateTime startDate = new DateTime(1900, 01, 01);
                                            var date = startDate.AddDays(daysSince1990);

                                            dateValueAsString = date.ToString("MM/dd/yyyy"); // Specifying the format for displaying the date as string
                                        }

                                        rowData.Add(headers[i], dateValueAsString);
                                    }
                                    else
                                    {
                                        rowData.Add(headers[i], val);
                                    }
                                    i++;
                                }

                                for (int escapedEntriesIndex = i; escapedEntriesIndex < rowIdentifierList.Count; escapedEntriesIndex++)
                                {
                                    String escapedKey = headers.ElementAt(escapedEntriesIndex);

                                    if (!String.IsNullOrEmpty(escapedKey))
                                    {
                                        if (!rowData.ContainsKey(escapedKey))
                                        {
                                            rowData.Add(escapedKey, "");
                                        }
                                    }
                                }

                                sheetDataSet.Add(rowData);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    

                    throw exception;
                }


            });

            return sheetDataSet;
        }

        private List<string> CollectSharedStrings(ZipArchiveEntry sharedStringsEntry)
        {

            List<String> sharedStringsList = new List<string>();

            using (var sr = sharedStringsEntry.Open())
            {
                XDocument xdoc = XDocument.Load(sr);
                sharedStringsList =
                    (
                    from e in xdoc.Root.Elements()
                    select e.Elements().FirstOrDefault().Value
                    ).ToList();
            }

            return sharedStringsList;
        }

        public async Task<List<SheetData>> ParseSpreadSheetFile(StorageFile _targetFile)
        {
            List<SheetData> allSheetsData = new List<SheetData>();
            List<ZipArchiveEntry> listOfSheets = new List<ZipArchiveEntry>();
            // Clearing processing values
            excelReader.headers.Clear();
            excelReader.rowIdentifierList.Clear();
            this.targetFile = _targetFile;

            try
            {

                if (_targetFile == null)
                {
                    throw new ArgumentNullException("Target file is empty");
                }
                else
                {
                    Stream fileAsStream = targetFile.OpenStreamForReadAsync().Result;
                    ZipArchive z = new ZipArchive(fileAsStream);

                    var sharedString = z.GetEntry(SHARED_STRING);
                    var listOfSharedStrings = this.CollectSharedStrings(sharedString);

                    if (_sharedStrings == null)
                    {
                        _sharedStrings = new List<string>();
                    }
                    _sharedStrings.Clear();
                    _sharedStrings = listOfSharedStrings.ToList();

                    var entries = z.Entries;

                    foreach (var anEntry in entries)
                    {
                        if (anEntry.Name.Contains("sheet") && !anEntry.Name.Contains("rels"))
                        {
                            listOfSheets.Add(anEntry);
                        }
                    }

                    foreach (var aSheet in listOfSheets)
                    {
                        var currentSheetData = await this.ReadSheetData(aSheet);
                        SheetData sheetData = new SheetData();
                        sheetData.sheetEntryInArchive = aSheet;
                        sheetData.data = currentSheetData;
                        allSheetsData.Add(sheetData);
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return allSheetsData;
        }

        #endregion

    }
}