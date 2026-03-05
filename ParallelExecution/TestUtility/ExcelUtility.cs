using ParallelExecution.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

namespace ParallelExecution.TestUtility
{
    public class ExcelUtility
    {
        List<Datacollection> dataCol = new List<Datacollection>();

        public class Datacollection
        {
            public int rowNumber { get; set; }
            public string colName { get; set; }
            public string colValue { get; set; }
        }

        public DataTable ConvertExcelToDataTable(string fileName, string sheetNameToOpen)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (var workbook = new XLWorkbook(fileName))
                {
                    var worksheet = workbook.Worksheet(sheetNameToOpen);
                    if (worksheet == null)
                        throw new Exception($"Worksheet {sheetNameToOpen} not found.");

                    bool firstRow = true;
                    foreach (var row in worksheet.RowsUsed())
                    {
                        if (firstRow)
                        {
                            foreach (var cell in row.Cells())
                                dataTable.Columns.Add(cell.Value.ToString());
                            firstRow = false;
                        }
                        else
                        {
                            dataTable.Rows.Add(row.Cells().Select(c => c.Value.ToString()).ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return dataTable;
        }

        public List<Datacollection> PopulateInCollection(string fileName, string sheetName)
        {
            dataCol.Clear();
            DataTable table = ConvertExcelToDataTable(fileName, sheetName);

            for (int row = 1; row <= table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    Datacollection dtTable = new Datacollection()
                    {
                        rowNumber = row,
                        colName = table.Columns[col].ColumnName,
                        colValue = table.Rows[row - 1][col].ToString()
                    };
                    dataCol.Add(dtTable);
                }
            }
            return dataCol;
        }

        public void WriteDataInToExcelFile(string fileName, string sheetName, string fieldColumnName, string valueColumnName, Hashtable fieldAndValueData)
        {
            try
            {
                string fullPath = Path.Combine(BaseClass.CreatedExcelFilePath, fileName);

                using (var workbook = File.Exists(fullPath) ? new XLWorkbook(fullPath) : new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Contains(sheetName) ? workbook.Worksheet(sheetName) : workbook.AddWorksheet(sheetName);

                    // Set column names
                    worksheet.Cell(1, 1).Value = fieldColumnName;
                    worksheet.Cell(1, 2).Value = valueColumnName;

                    int i = 2;
                    foreach (DictionaryEntry item in fieldAndValueData)
                    {
                        // Fix for CS0266: Explicitly convert object to XLCellValue using XLCellValue.FromObject
                        worksheet.Cell(i, 1).Value = XLCellValue.FromObject(item.Key);
                        worksheet.Cell(i, 2).Value = XLCellValue.FromObject(item.Value);
                        i++;
                    }

                    workbook.SaveAs(fullPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void WriteDataTableToExcel(string fileName, string sheetName, DataTable dataTable)
        {
            string fullPath = Path.Combine(BaseClass.CreatedExcelFilePath, fileName);

            using (var workbook = File.Exists(fullPath) ? new XLWorkbook(fullPath) : new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Contains(sheetName) ? workbook.Worksheet(sheetName) : workbook.AddWorksheet(sheetName);

                // Write headers
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dataTable.Columns[i].ColumnName;
                }

                // Write rows
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = ClosedXML.Excel.XLCellValue.FromObject(dataTable.Rows[i][j]);
                    }
                }

                workbook.SaveAs(fullPath);
            }
        }
    }
}
