using FrameCreation.Models;
using SAP2000v1;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FrameCreation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string aspath = "C:\\Program Files\\Computers and Structures\\SAP2000 25\\SAP2000.exe";

            // Create New Sap2000 Instance 
            //cOAPI mySapObject = (cOAPI)Activator.CreateInstance(Type.GetTypeFromProgID("CSI.SAP2000.API.SapObject"));
            //mySapObject.ApplicationStart();
            //cSapModel mySapModel = mySapObject?.SapModel;
            //mySapModel.InitializeNewModel();
            //mySapModel.File.NewBlank();



            #region Getting SAPModel
            cHelper myHelper = new Helper();
            cOAPI mySapObject = myHelper.GetObject("CSI.SAP2000.API.SapObject");    //to control programe from code
            cSapModel mySapModel = mySapObject?.SapModel;                          //interface of SAP
            mySapModel.SetPresentUnits(eUnits.kN_m_C);                            //Unit setting 
            #endregion


            #region CreateConcrete
         public static bool CreateConcreteMaterial(cSapModel mySapModel)          //Signature
        {
            int ret = mySapModel.PropMaterial.SetMaterial("ConcreteMaterial", eMatType.Concrete);                                      //إنشاء المادة الأساسية
            int ret2 = mySapModel.PropMaterial.SetOConcrete_2("ConcreteMaterial", fc, 1.1 * fc, false, 0.0, 2, 2, 0.002, 0.003, -1);   //تعريف خصائص الخرسانة
            int ret3 = mySapModel.PropMaterial.SetMPIsotropic("ConcereteMaterial", Ec, 0.2, 9.9e-6);                                   //تعريف الخصائص المرنة
            int ret4 = mySapModel.PropMaterial.SetWeightAndMass("ConcereteMaterial", 1, 25);                                           // تعريف الوزن والكتلة

            if (ret == 0 && ret2 == 0 && ret3 == 0 && ret4 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region CreateRebarMaterial
        public static bool CreateRebarMaterial(cSapModel mySapModel)                                    //إنشاء مادة حديد التسليح
        {
            int ret = mySapModel.PropMaterial.SetMaterial("NewRebarMaterial", eMatType.Rebar);         //إنشاء المادة الأساسية
            int ret2 = mySapModel.PropMaterial.SetORebar_1("NewRebarMaterial", 420 * 1000, 560 * 1000, 420 * 1000 * 1.1, 560 * 1000 * 1.1, 2, 2, 0.002, 0.003, -1, true);  //تعريف خصائص حديد التسليح
            int ret3 = mySapModel.PropMaterial.SetMPIsotropic("NewRebarMaterial", 2e8, 0.2, 9.9e-6);   // تعريف الخصائص المرنة
            int ret4 = mySapModel.PropMaterial.SetWeightAndMass("NewRebarMaterial", 1, 78.5);          //تعريف الوزن والكتلة

            if (ret == 0 && ret2 == 0 && ret3 == 0 && ret4 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region CreateColumnCrossSection
        public static bool CreateColumnCrossSection(cSapModel mySapModel)
        {
            int ret = mySapModel.PropFrame.SetRectangle("C500x500", "NewConcereteMaterial", 0.5, 0.5);         //إنشاء المقطع المستطيل
            int ret2 = mySapModel.PropFrame.SetRebarColumn("C500x500", "NewRebarMaterial", "NewRebarMaterial", 1, 1, 0.025, 0, 5, 5, "20M", "10M", 0.25, 4, 4, false);    //تعريف تسليح العمود
                                                                                                                                                                          // تطبيق معاملات التعديل (Modifiers)                                                                                                                                                                                                                                                                                                                         
            double[] arrayOfModifiers = new double[8] { 1, 1, 1, 1, 0.5, 0.5, 1, 1 };
            int ret3 = mySapModel.PropFrame.SetModifiers("C500x500", ref arrayOfModifiers);

            if (ret == 0 && ret2 == 0 && ret3 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region CreateBeamCrossSection
        public static bool CreateBeamCrossSection(cSapModel mySapModel)
        {

            int ret = mySapModel.PropFrame.SetRectangle("B300x600", "NewConcereteMaterial", 0.6, 0.3);
            int ret2 = mySapModel.PropFrame.SetRebarBeam("B300x600", "NewRebarMaterial", "NewRebarMaterial", 0.05, 0.05, 0.2, 0.2, 0.2, 0.2);
            double[] arrayOfModifiers = new double[8] { 1, 1, 1, 1, 0.5, 0.5, 1, 1 };
            int ret3 = mySapModel.PropFrame.SetModifiers("B300x600", ref arrayOfModifiers);

            if (ret == 0 && ret2 == 0 && ret3 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Creation
        public static string CreateBeam(cSapModel mySapModel)
        {
            string newFrameCreated = default;
            int ret = mySapModel.FrameObj.AddByCoord(0, 0, 5, 10, 0, 5, ref newFrameCreated, "B300x600");

            return newFrameCreated;
        }

        public static string CreateColumn(cSapModel mySapModel, XYZ P1, XYZ P2)
        {
            string point1 = default;
            int p1_ret = mySapModel.PointObj.AddCartesian(P1.X, P1.Y, P1.Z, ref point1);

            string point2 = default;
            int p2_ret = mySapModel.PointObj.AddCartesian(P2.X, P2.Y, P2.Z, ref point2);

            string createdColumn = default;
            int ret = mySapModel.FrameObj.AddByPoint(point1, point2, ref createdColumn, "C500x500");

            P1.CreatedName = point1;
            P2.CreatedName = point2;



            return createdColumn;
        }

        public static bool CreateSupport(cSapModel mySapModel, string pointName, bool[] restraintData)
        {
            int ret = mySapModel.PointObj.SetRestraint(pointName, ref restraintData);
            return ret == 0;
        }

        public static bool CreateLoadPatterns(cSapModel mySapModel)
        {
            int ret = mySapModel.LoadPatterns.Add("WALL LOAD", eLoadPatternType.SuperDead, 0, true);
            return ret == 0;
        }

        public static bool AddDistributedLoad(cSapModel mySapModel, string frameName, double value)
        {
            int ret = mySapModel.FrameObj.SetLoadDistributed(frameName, "WALL LOAD", 1, 10, 0, 1, value, value);
            return ret == 0;
        }

        #endregion
    
    }
}
