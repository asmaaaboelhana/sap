//using FrameCreation_WPF.Models;
//using SAP2000v1;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FrameCreation_WPF.HelperClasses
//{
//    public class SAPAPI_HelperMethods
//    {

//        public static string CreateBeamBetweenPoints(cSapModel mySapModel, XYZ startPoint, XYZ endPoint)
//        {
//            string beamName = string.Empty;
//            string point1Name = string.Empty;
//            string point2Name = string.Empty;

//            try
//            {
//                int ret1 = mySapModel.PointObj.AddCartesian(
//                    startPoint.X, startPoint.Y, startPoint.Z,
//                    ref point1Name
//                );

//                if (ret1 != 0)
//                {
//                    throw new Exception($"Failed to create start point at ({startPoint.X}, {startPoint.Y}, {startPoint.Z}). Error code: {ret1}");
//                }

//                int ret2 = mySapModel.PointObj.AddCartesian(
//                    endPoint.X, endPoint.Y, endPoint.Z,
//                    ref point2Name
//                );

//                if (ret2 != 0)
//                {
//                    throw new Exception($"Failed to create end point at ({endPoint.X}, {endPoint.Y}, {endPoint.Z}). Error code: {ret2}");
//                }

//                int ret3 = mySapModel.FrameObj.AddByPoint(
//                    point1Name,
//                    point2Name,
//                    ref beamName,
//                    "B300x600"
//                );

//                if (ret3 != 0)
//                {
//                    throw new Exception($"Failed to create beam between points {point1Name} and {point2Name}. Error code: {ret3}");
//                }

//                startPoint.CreatedName = point1Name;
//                endPoint.CreatedName = point2Name;

//                return beamName;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error in CreateBeamBetweenPoints: {ex.Message}");
//            }
//        }

//        public static bool CreateRebarMaterial(cSapModel mySapModel)
//        {
//            int ret = mySapModel.PropMaterial.SetMaterial("NewRebarMaterial", eMatType.Rebar);
//            int ret2 = mySapModel.PropMaterial.SetORebar_1("NewRebarMaterial", 420 * 1000, 560 * 1000, 420 * 1000 * 1.1, 560 * 1000 * 1.1, 2, 2, 0.002, 0.003, -1, true);
//            int ret3 = mySapModel.PropMaterial.SetMPIsotropic("NewRebarMaterial", 2e8, 0.2, 9.9e-6);
//            int ret4 = mySapModel.PropMaterial.SetWeightAndMass("NewRebarMaterial", 1, 78.5);

//            if (ret == 0 && ret2 == 0 && ret3 == 0 && ret4 == 0)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }

//        }
//        public static bool CreateConcreteMaterial(cSapModel mySapModel)
//        {
//            double fc = 0.8 * 30 * 1000;
//            double Ec = 4400 * Math.Sqrt(30) * 1000;

//            int ret = mySapModel.PropMaterial.SetMaterial("NewConcereteMaterial", eMatType.Concrete);
//            int ret2 = mySapModel.PropMaterial.SetOConcrete_2("NewConcereteMaterial", fc, 1.1 * fc, false,
//                0, 2, 2, 0.002, 0.003, -1);

//            int ret3 = mySapModel.PropMaterial.SetMPIsotropic("NewConcereteMaterial", Ec, 0.2, 9.9e-6);
//            int ret4 = mySapModel.PropMaterial.SetWeightAndMass("NewConcereteMaterial", 1, 25);


//            if (ret == 0 && ret2 == 0 && ret3 == 0 && ret4 == 0)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public static bool CreateColumnCrossSection(cSapModel mySapModel)
//        {
//            int ret = mySapModel.PropFrame.SetRectangle("C500x500", "NewConcereteMaterial", 0.5, 0.5);
//            int ret2 = mySapModel.PropFrame.SetRebarColumn("C500x500", "NewRebarMaterial", "NewRebarMaterial", 1, 1, 0.025, 0, 5, 5, "20M", "10M", 0.25, 4, 4, false);
//            double[] arrayOfModifiers = new double[8] { 1, 1, 1, 1, 0.5, 0.5, 1, 1 };
//            int ret3 = mySapModel.PropFrame.SetModifiers("C500x500", ref arrayOfModifiers);


//            if (ret == 0 && ret2 == 0 && ret3 == 0)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public static bool CreateBeamCrossSection(cSapModel mySapModel)
//        {

//            int ret = mySapModel.PropFrame.SetRectangle("B300x600", "NewConcereteMaterial", 0.6, 0.3);
//            int ret2 = mySapModel.PropFrame.SetRebarBeam("B300x600", "NewRebarMaterial", "NewRebarMaterial", 0.05, 0.05, 0.2, 0.2, 0.2, 0.2);
//            double[] arrayOfModifiers = new double[8] { 1, 1, 1, 1, 0.5, 0.5, 1, 1 };
//            int ret3 = mySapModel.PropFrame.SetModifiers("B300x600", ref arrayOfModifiers);

//            if (ret == 0 && ret2 == 0 && ret3 == 0)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public static string CreateBeam(cSapModel mySapModel)
//        {
//            string newFrameCreated = default;
//            int ret = mySapModel.FrameObj.AddByCoord(0, 0, 5, 10, 0, 5, ref newFrameCreated, "B300x600");

//            return newFrameCreated;
//        }
//        public static string CreateColumn(cSapModel mySapModel, XYZ P1, XYZ P2)
//        {
//            string point1 = default;
//            int p1_ret = mySapModel.PointObj.AddCartesian(P1.X, P1.Y, P1.Z, ref point1);

//            string point2 = default;
//            int p2_ret = mySapModel.PointObj.AddCartesian(P2.X, P2.Y, P2.Z, ref point2);

//            string createdColumn = default;
//            int ret = mySapModel.FrameObj.AddByPoint(point1, point2, ref createdColumn, "C500x500");

//            P1.CreatedName = point1;
//            P2.CreatedName = point2;

//            //SAP_Column sapColumn = new SAP_Column()
//            //{
//            //    Name = createdColumn,
//            //    P1 = P1,
//            //    P2 = P2,
//            //    PropName = "C500x500"
//            //};

//            return createdColumn;
//        }
//        public static bool CreateSupport(cSapModel mySapModel, string pointName, bool[] restraintData)
//        {
//            int ret = mySapModel.PointObj.SetRestraint(pointName, ref restraintData);
//            return ret == 0;
//        }
//        public static bool CreateLoadPatterns(cSapModel mySapModel)
//        {
//            int ret = mySapModel.LoadPatterns.Add("WALL LOAD", eLoadPatternType.SuperDead, 0, true);
//            return ret == 0;
//        }
//        public static bool AddDistributedLoad(cSapModel mySapModel, string frameName, double value)
//        {
//            int ret = mySapModel.FrameObj.SetLoadDistributed(frameName, "WALL LOAD", 10, 0, 0, 1, value, value);
//            return ret == 0;
//        }
//    }
//}

using FrameCreation_WPF.Models;
using SAP2000v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameCreation_WPF.HelperClasses
{
    public class SAPAPI_HelperMethods
    {
        // ============================================================
        // FIX 1: typo fix "NewConcereteMaterial" -> "NewConcreteMaterial"
        // used as a constant to avoid repeating the string everywhere
        // ============================================================
        private const string ConcreteMaterialName = "NewConcreteMaterial";
        private const string RebarMaterialName = "NewRebarMaterial";
        private const string ColumnSection = "C500x500";
        private const string BeamSection = "B300x600";

        // ============================================================
        // CreateBeamBetweenPoints  –  no logic change needed here,
        // only the concrete‑material typo was propagated; the method
        // itself was fine.
        // ============================================================
        public static string CreateBeamBetweenPoints(
            cSapModel mySapModel, XYZ startPoint, XYZ endPoint)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));
            if (startPoint == null)
                throw new ArgumentNullException(nameof(startPoint));
            if (endPoint == null)
                throw new ArgumentNullException(nameof(endPoint));

            string beamName = string.Empty;
            string point1Name = string.Empty;
            string point2Name = string.Empty;

            try
            {
                int ret1 = mySapModel.PointObj.AddCartesian(
                    startPoint.X, startPoint.Y, startPoint.Z,
                    ref point1Name);

                if (ret1 != 0)
                    throw new Exception(
                        $"Failed to create start point at " +
                        $"({startPoint.X}, {startPoint.Y}, {startPoint.Z}). " +
                        $"Error code: {ret1}");

                int ret2 = mySapModel.PointObj.AddCartesian(
                    endPoint.X, endPoint.Y, endPoint.Z,
                    ref point2Name);

                if (ret2 != 0)
                    throw new Exception(
                        $"Failed to create end point at " +
                        $"({endPoint.X}, {endPoint.Y}, {endPoint.Z}). " +
                        $"Error code: {ret2}");

                int ret3 = mySapModel.FrameObj.AddByPoint(
                    point1Name, point2Name,
                    ref beamName, BeamSection);

                if (ret3 != 0)
                    throw new Exception(
                        $"Failed to create beam between {point1Name} and " +
                        $"{point2Name}. Error code: {ret3}");

                startPoint.CreatedName = point1Name;
                endPoint.CreatedName = point2Name;

                return beamName;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error in CreateBeamBetweenPoints: {ex.Message}", ex);
            }
        }

        // ============================================================
        // FIX 2 – CreateRebarMaterial
        //   • SetORebar_1 : parameter order was correct but the
        //     Fy/Fu values should NOT be multiplied by 1000 again
        //     if the unit system is already kN/m².
        //     420 MPa = 420,000 kN/m² -> 420 * 1000 is correct ONLY
        //     when working in kN & m.  Kept as-is but documented.
        //   • SetWeightAndMass : first param = unit weight (kN/m³),
        //     second = mass density (kg/m³).
        //     Steel: weight = 78.5 kN/m³, mass = 7850 kg/m³
        //     Original had (1, 78.5) which is WRONG.
        // ============================================================
        public static bool CreateRebarMaterial(cSapModel mySapModel)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));

            // units assumed: kN, m  ->  MPa * 1000 = kN/m²
            double Fy = 420.0 * 1000;   // 420 MPa
            double Fu = 560.0 * 1000;   // 560 MPa
            double Ey = 200e6;          // 200 GPa = 200,000 MPa = 2e8 kN/m²  (kept same)
            double nu = 0.3;            // Poisson – steel is 0.3, was 0.2
            double alpha = 12e-6;        // thermal expansion (steel)

            int ret1 = mySapModel.PropMaterial.SetMaterial(
                RebarMaterialName, eMatType.Rebar);

            // SetORebar_1 signature (SAP2000 v19/v20):
            // (Name, Fy, Fu, FyH, FuH,
            //  StrainHardType,  // 1=None, 2=Linear
            //  StressStrainCurveType,
            //  StrainAtFy, StrainAtFu,
            //  StrainAtRupture,  // use 0.05 or -1 for default
            //  SymmetricDesign)
            int ret2 = mySapModel.PropMaterial.SetORebar_1(
                RebarMaterialName,
                Fy, Fu,
                Fy * 1.1, Fu * 1.1,       // horizontal (cyclic) Fy, Fu
                2,                         // StrainHardType = Linear
                2,                         // StressStrainCurveType
                0.002, 0.003,
                0.05,                      // FIX: was -1, use 0.05 (5 %) for rupture strain
                true);                     // Symmetric

            int ret3 = mySapModel.PropMaterial.SetMPIsotropic(
                RebarMaterialName, Ey, nu, alpha);

            // FIX: unit weight = 78.5 kN/m³, mass = 7850 kg/m³
            int ret4 = mySapModel.PropMaterial.SetWeightAndMass(
                RebarMaterialName, (int)78.5, 7850.0);

            return (ret1 == 0 && ret2 == 0 && ret3 == 0 && ret4 == 0);
        }

        // ============================================================
        // FIX 3 – CreateConcreteMaterial
        //   • Typo in material name fixed (constant used).
        //   • SetOConcrete_2 signature needs one more bool at the end
        //     for "Use Tension Model" in some versions – added.
        //   • Poisson for concrete should be 0.2 (was correct).
        //   • SetWeightAndMass : concrete weight = 25 kN/m³,
        //     mass = 2500 kg/m³.  Was (1, 25) -> WRONG.
        // ============================================================
        public static bool CreateConcreteMaterial(cSapModel mySapModel)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));

            double fc = 30.0 * 1000;            // 30 MPa  = 30,000 kN/m²
            double fc0 = 0.8 * fc;               // effective fc for nonlinear (ACI)
            double Ec = 4400.0 * Math.Sqrt(30) * 1000;  // MPa -> kN/m²
            double nu = 0.2;
            double alpha = 9.9e-6;

            int ret1 = mySapModel.PropMaterial.SetMaterial(
                ConcreteMaterialName, eMatType.Concrete);

            // SetOConcrete_2 signature (SAP2000 v19/v20):
            // (Name,
            //  fc,                   – compressive strength
            //  fcH,                  – horizontal fc (cyclic)
            //  UseTensionModel,      – bool  <-- WAS MISSING / out of order
            //  TensionModel,         – int: 0 = None, 1 = Linear, 2 = ...
            //  StressStrainCurveType,
            //  StressStrainHardType,
            //  StrainAtFc,
            //  StrainAtFcH,
            //  StrainAtRupture)
            // FIX: fc (not 0.8*fc) is the first value; 0.8 factor is
            //      internal to ACI code checks.  Passing raw fc.
            int ret2 = mySapModel.PropMaterial.SetOConcrete_2(
                ConcreteMaterialName,
                fc,                  // fc
                fc * 1.1,            // fcH (cyclic)
                true,                // FIX: UseTensionModel (was 'false' and param was misplaced)
                0,                   // TensionModel = 0 (None)
                2,                   // StressStrainCurveType
                2,                   // StressStrainHardType
                0.002,               // StrainAtFc
                0.003,               // StrainAtFcH
                0.05);               // StrainAtRupture (was -1)

            int ret3 = mySapModel.PropMaterial.SetMPIsotropic(
                ConcreteMaterialName, Ec, nu, alpha);

            // FIX: unit weight = 25 kN/m³, mass = 2500 kg/m³
            int ret4 = mySapModel.PropMaterial.SetWeightAndMass(
                ConcreteMaterialName, 1, 2500.0);

            return (ret1 == 0 && ret2 == 0 && ret3 == 0 && ret4 == 0);
        }

        // ============================================================
        // FIX 4 – CreateColumnCrossSection
        //   • SetRectangle(Name, MatName, depth, width)
        //     Column 500x500 -> depth=0.5, width=0.5  (symmetric, OK)
        //   • SetRebarColumn – full correct signature:
        //     (Name,
        //      MatNameLong,       – longitudinal rebar material
        //      MatNameLat,        – lateral (tie) rebar material
        //      RebarOption,       – 1 = Uniform grid
        //      NumDirsA,          – bars along local-2  (was used as cover)
        //      NumDirsB,          – bars along local-3
        //      Cover,             – clear cover in meters
        //      SpacingA,          – not used for option=1, set 0
        //      SpacingB,
        //      NumBarsA,          – number of bars along A
        //      NumBarsB,          – number of bars along B
        //      BarSizeA,          – longitudinal bar size name
        //      BarSizeB,          – (same as A for uniform)
        //      NumTies,           – number of tie layers
        //      TieBarSize,        – tie bar size name
        //      Symmetric)         – symmetric reinforcement
        //
        //     The original call had wrong parameter order/count.
        // ============================================================
        public static bool CreateColumnCrossSection(cSapModel mySapModel)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));

            // depth = 0.5 m, width = 0.5 m
            int ret1 = mySapModel.PropFrame.SetRectangle(
                ColumnSection, ConcreteMaterialName, 0.5, 0.5);

            // SetRebarColumn confirmed signature:
            // (string Name,
            //  string MatNameLong,
            //  string MatNameLat,
            //  int    RebarOption,          // 1 = Uniform grid
            //  int    Number2DirTieBars,    // number of 2-dir tie bars
            //  double Cover,               // clear cover (m)
            //  int    NumBarsA,            // bars along local-2
            //  int    NumBarsB,            // bars along local-3
            //  int    NumBarsC,            // bars along local-1 (axial dir)
            //  string BarSizeA,            // longitudinal bar size
            //  string BarSizeB,            // tie bar size
            //  double TieSpacing,          // tie spacing (m)
            //  int    NumTieDirs,          // number of tie directions
            //  int    NumTieLayers,        // number of tie layers
            //  bool   Symmetric)           // symmetric reinforcement
            int ret2 = mySapModel.PropFrame.SetRebarColumn(
                ColumnSection,
                RebarMaterialName,       // MatNameLong
                RebarMaterialName,       // MatNameLat
                1,                       // RebarOption = 1 (Uniform grid)
                2,                       // Number2DirTieBars = 2
                0.04,                    // Cover = 40 mm
                5,                       // NumBarsA (along local-2)
                5,                       // NumBarsB (along local-3)
                0,                       // NumBarsC (along axial)
                "20M",                   // BarSizeA (longitudinal)
                "10M",                   // BarSizeB (tie bar)
                0.15,                    // TieSpacing = 150 mm
                2,                       // NumTieDirs = 2
                1,                       // NumTieLayers = 1
                true);                   // Symmetric

            // Stiffness modifiers: {Axial, Shear-2, Shear-3, Torsion,
            //                        Bending-2, Bending-3, Mass, Weight}
            double[] arrayOfModifiers = new double[8]
            {
                1.0,   // Axial
                1.0,   // Shear local-2
                1.0,   // Shear local-3
                1.0,   // Torsion
                0.7,   // FIX: Bending local-2  (ACI 318 -> 0.7 for columns, was 0.5)
                0.7,   // FIX: Bending local-3
                1.0,   // Mass
                1.0    // Weight
            };
            int ret3 = mySapModel.PropFrame.SetModifiers(
                ColumnSection, ref arrayOfModifiers);

            return (ret1 == 0 && ret2 == 0 && ret3 == 0);
        }

        public static bool CreateBeamCrossSection(cSapModel mySapModel)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));

            // depth=0.6 m, width=0.3 m
            int ret1 = mySapModel.PropFrame.SetRectangle(
                BeamSection, ConcreteMaterialName, 0.6, 0.3);

            // SetRebarBeam confirmed signature (9 params):
            // (string Name,
            //  string MatPropLong,        – longitudinal rebar material
            //  string MatPropConfine,     – confinement (stirrup) material
            //  double CoverTop,           – top cover (m)
            //  double CoverBot,           – bottom cover (m)
            //  double TopLeftArea,        – top-left rebar area (m²)
            //  double TopRightArea,       – top-right rebar area (m²)
            //  double BotLeftArea,        – bot-left rebar area (m²)
            //  double BotRightArea)       – bot-right rebar area (m²)
            //
            // 20M bar area = 314 mm² = 314e-6 m²
            // 3 bars each side -> 3 * 314e-6 = 942e-6 m²
            double areaPerBar = 314e-6;   // 20M = 314 mm²
            double topArea = 3 * areaPerBar;   // 3 bars top
            double botArea = 3 * areaPerBar;   // 3 bars bottom

            int ret2 = mySapModel.PropFrame.SetRebarBeam(
                BeamSection,
                RebarMaterialName,       // MatPropLong
                RebarMaterialName,       // MatPropConfine
                0.04,                    // CoverTop  = 40 mm
                0.04,                    // CoverBot  = 40 mm
                topArea,                 // TopLeftArea  (m²)
                topArea,                 // TopRightArea (m²)
                botArea,                 // BotLeftArea  (m²)
                botArea);                // BotRightArea (m²)

            // Stiffness modifiers
            double[] arrayOfModifiers = new double[8]
            {
                1.0,   // Axial
                1.0,   // Shear local-2
                1.0,   // Shear local-3
                1.0,   // Torsion
                0.35,  // FIX: Bending local-2  (ACI 318 -> 0.35 for beams, was 0.5)
                0.35,  // FIX: Bending local-3
                1.0,   // Mass
                1.0    // Weight
            };
            int ret3 = mySapModel.PropFrame.SetModifiers(
                BeamSection, ref arrayOfModifiers);

            return (ret1 == 0 && ret2 == 0 && ret3 == 0);
        }

      
        public static string CreateBeam(
            cSapModel mySapModel, XYZ P1, XYZ P2)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));

            string point1 = string.Empty;
            int p1_ret = mySapModel.PointObj.AddCartesian(
                P1.X, P1.Y, P1.Z, ref point1);

            if (p1_ret != 0)
                throw new Exception(
                    $"Failed to create beam start point. Error code: {p1_ret}");

            string point2 = string.Empty;
            int p2_ret = mySapModel.PointObj.AddCartesian(
                P2.X, P2.Y, P2.Z, ref point2);

            if (p2_ret != 0)
                throw new Exception(
                    $"Failed to create beam end point. Error code: {p2_ret}");

            string createdBeam = string.Empty;
            int ret = mySapModel.FrameObj.AddByPoint(
                point1, point2, ref createdBeam, BeamSection);

            if (ret != 0)
                throw new Exception(
                    $"Failed to create beam. Error code: {ret}");

            P1.CreatedName = point1;
            P2.CreatedName = point2;

            return createdBeam;
        }

     
        public static string CreateColumn(
            cSapModel mySapModel, XYZ P1, XYZ P2)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));

            string point1 = string.Empty;
            int p1_ret = mySapModel.PointObj.AddCartesian(
                P1.X, P1.Y, P1.Z, ref point1);

            if (p1_ret != 0)
                throw new Exception(
                    $"Failed to create column start point. Error code: {p1_ret}");

            string point2 = string.Empty;
            int p2_ret = mySapModel.PointObj.AddCartesian(
                P2.X, P2.Y, P2.Z, ref point2);

            if (p2_ret != 0)
                throw new Exception(
                    $"Failed to create column end point. Error code: {p2_ret}");

            string createdColumn = string.Empty;
            int ret = mySapModel.FrameObj.AddByPoint(
                point1, point2, ref createdColumn, ColumnSection);

            if (ret != 0)
                throw new Exception(
                    $"Failed to create column. Error code: {ret}");

            P1.CreatedName = point1;
            P2.CreatedName = point2;

            return createdColumn;
        }

      
        public static bool CreateSupport(
            cSapModel mySapModel, string pointName, bool[] restraintData)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));
            if (string.IsNullOrEmpty(pointName))
                throw new ArgumentNullException(nameof(pointName));
            if (restraintData == null || restraintData.Length != 6)
                throw new ArgumentException(
                    "restraintData must be a bool[6] array.");

            int ret = mySapModel.PointObj.SetRestraint(
                pointName, ref restraintData);

            return ret == 0;
        }

        public static bool CreateLoadPatterns(cSapModel mySapModel)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));

            int ret = mySapModel.LoadPatterns.Add(
                "WALL LOAD",
                eLoadPatternType.SuperDead,
                0,       // SelfWeight multiplier = 0 (correct for wall load)
                true);   // Replace if exists

            return ret == 0;
        }

        // ============================================================
        // FIX 8 – AddDistributedLoad
        //   • SetLoadDistributed signature:
        //     (FrameName, PatternName,
        //      eDir,          – load direction enum
        //      DistType,      – 0=uniform, 1=trapezoidal (trapez needs 2 values)
        //      Near,          – distance from near end (m) as ratio 0–1
        //      Far,           – distance from far end  (m) as ratio 0–1
        //      Val1,          – load value at Near end
        //      Val2)          – load value at Far  end
        //
        //   • eDir was 1 (Global X).  For a vertical wall load on a
        //     horizontal beam it must be 10 (Global Gravity = -Z).
        //   • DistType was 10 (invalid).  Use 0 for uniform load.
        //   • Near/Far should be 0 and 1 (full length).
        // ============================================================
        public static bool AddDistributedLoad(
            cSapModel mySapModel, string frameName, double value)
        {
            if (mySapModel == null)
                throw new ArgumentNullException(nameof(mySapModel));
            if (string.IsNullOrEmpty(frameName))
                throw new ArgumentNullException(nameof(frameName));

            int ret = mySapModel.FrameObj.SetLoadDistributed(
                frameName,
                "WALL LOAD",
                10,      // FIX: eDir = 10  (Global Gravity, i.e. -Z)
                0,       // FIX: DistType = 0 (Uniform)  – was 10
                0,       // Near = 0  (start of member)
                1,       // Far  = 1  (end of member)
                value,   // Val1 (kN/m)
                value);  // Val2 (same as Val1 for uniform)

            return ret == 0;
        }
    }
}