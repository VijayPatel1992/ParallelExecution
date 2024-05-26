﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using ParallelExecution.Base;
using ParallelExecution.POM;
using ParallelExecution.TestUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParallelExecution.POM.HomePage;

namespace ParallelExecution.TestClass
{
    [TestClass]
    public class TestCases : BaseClass
    {
        #region Object Creation
      

        HomePage _HomePage;
        ElementPage _ElementPage;
        PracticeForm _PracticeForm;
        UtilityClass _UtilityClass;
        ExcelUtility _ExcelUtility;

        #endregion
        
        [TestMethod]
        public void Test2_Test()
        {
            try
            {
                #region Object and variable initialization.

                _HomePage = new HomePage(Driver);
                _ElementPage = new ElementPage(Driver);
                _UtilityClass = new UtilityClass();
                _ExcelUtility = new ExcelUtility();
                #endregion

                #region Step: 1 Perform Operation on the Home Page.

                _ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");
                _HomePage.ClickOnElements(Driver);
                _UtilityClass.WaitForAjaxLoad(Driver);

                #endregion

                #region Step:2 Click on Left pane elements Checkbox.

                _ElementPage.ClickOnLeftPaneElement(Driver, _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.CheckBox));

                #endregion

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                Console.WriteLine(ex);
            }

        }

       
        [TestMethod]
        public void Test4_SelectValueFromDDL()
        {
            try
            {
                #region Object and variable Initialization

                _HomePage = new HomePage(Driver);
                _ElementPage = new ElementPage(Driver);
                _UtilityClass = new UtilityClass();
                _ExcelUtility = new ExcelUtility();
                _ExcelUtility.PopulateInCollection(rootpath + "\\TestData\\Sample Test data.xlsx", "Patel");

                #endregion

                #region Step:1 Navigates to Elements page

                _HomePage.ClickOnElements(Driver);
                _ElementPage.ClickOnLeftPaneElement(Driver, _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Widgets), _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.SelectMenu));

                #endregion

                #region Step:2  Select values fromt the Drop down.

                _ElementPage.SelectValueFromDroDown();

                #endregion

            }
            catch (Exception ex)
            {               
                Assert.Fail(ex.Message);
                Console.WriteLine(ex);
            }

        }

        
        [TestMethod]
        public void Test5_VerifyUploadFunctionality()
        {
            try
            {
                #region Object and variable Initialization

                _HomePage = new HomePage(Driver);
                _ElementPage = new ElementPage(Driver);
                _UtilityClass = new UtilityClass();
                _ExcelUtility = new ExcelUtility();
                _ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");
                string filepath = Path.Combine(rootpath, "FilesToUpload");
                filepath = filepath + "\\Picture.jpg";

                #endregion

                #region Step:1 Navigates to Elements page

                _HomePage.ClickOnElements(Driver);
                _UtilityClass.WaitForAjaxLoad(Driver);

                #endregion

                #region Step:2 Upload file.

                _ElementPage.ClickOnLeftPaneElement(Driver, _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.UploadAndDownload));
                _ElementPage.UploadFile(filepath);
                Assert.AreEqual(@"C:\fakepath\Picture.jpg", _ElementPage.MethodUploadedFilePath());

                #endregion



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }
        }

        
        [TestMethod]
        public virtual void Test1_VerifyFormFunctionality()
        {
            try
            {

                #region Object and variable Initialization

                _HomePage = new HomePage(Driver);
                _ElementPage = new ElementPage(Driver);
                _PracticeForm = new PracticeForm(Driver); 
                _ExcelUtility = new ExcelUtility(); 
                _UtilityClass = new UtilityClass();
                DataTable FormData = _ExcelUtility.ConvertExcelToDataTable(rootpath + "//TestData//Sample Test data.xlsx", "FormData");
                string UploadFilePath = Path.Combine(rootpath, "FilesToUpload");

                #endregion

                #region Step:1 Navigates to Elements page Verify Form Functionality.

                _HomePage.ClickOnElements(Driver);
                _UtilityClass.WaitForAjaxLoad(Driver);
                _ElementPage.ClickOnLeftPaneElement(Driver, _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Forms), _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.PracticeForm));
                _PracticeForm.FillPracticeForm(0, FormData, UploadFilePath);
                Assert.AreEqual(string.Concat(FormData.Rows[0]["FirstName"].ToString(), ' ', FormData.Rows[0]["LastName"].ToString()), _PracticeForm.GetAnyFieldValueOfSubmittedForm("Student Name"), "Validation failed for Field value.");
                string[] FetchedSubject = _PracticeForm.GetAnyFieldValueOfSubmittedForm("Subjects").Split(',');
                string[] UpdatedSubject = new string[FetchedSubject.Length];

                for (int i = 0; i < FetchedSubject.Length; i++)
                {
                    UpdatedSubject[i] = FetchedSubject[i].TrimStart().ToString().TrimEnd();
                }


                CollectionAssert.AreEqual(FormData.Rows[0]["Subject"].ToString().Split(';'), UpdatedSubject, "Validation failed for Field value.");


                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }
        }

        
        [TestMethod]
        public virtual void Test6_VerifyDeleteRecordFunctionality()
        {
            try
            {
                #region Object and variable Initialization

                _HomePage = new HomePage(Driver);
                _ElementPage = new ElementPage(Driver);
                _PracticeForm = new PracticeForm(Driver);
                _UtilityClass = new UtilityClass();
                string RecordNameTODelete = "Vega";

                #endregion

                #region Step:1 Navigates to Elements page and verify Delete recod functionality.

                _HomePage.ClickOnElements(Driver);
                _UtilityClass.WaitForAjaxLoad(Driver);
                _ElementPage.ClickOnLeftPaneElement(Driver, _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), _UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.WebTables));
                _ElementPage.DeleteRecordFromGrid(RecordNameTODelete);
                Assert.IsFalse(_ElementPage.VerifyDeletedRecordsInGrid(RecordNameTODelete));

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }
        }

        
        [TestMethod]
        public void Test3_SetExcelValue()
        {
            string Filename = "My Data.xlsx";
            _ExcelUtility = new ExcelUtility();
            Hashtable T1 = new Hashtable();
            T1.Add("VIjay", "Patel");
            T1.Add(1, 2);
            T1.Add(0.5, "Vijay");
            _ExcelUtility.WriteDataInToExcelFile(Filename, "Vijay", "Country", "CountryName", T1);
            DataTable FormData = _ExcelUtility.ConvertExcelToDataTable(rootpath + "//TestData//Sample Test data.xlsx", "FormData");
            _ExcelUtility.WriteDataTableToExcel(Filename, "Patel", FormData);
        }


    }
}
