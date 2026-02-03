using FrameCreation_WPF.HelperClasses;
using FrameCreation_WPF.Models;
using SAP2000v1;
using System;
using System.Windows;

namespace FrameCreation_WPF.View
{
    public partial class Tool_View : Window
    {
       
        private readonly cSapModel _mySapModel;

       
        public Tool_View(cSapModel sapModel)
        {
            _mySapModel = sapModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNumberOfFrames.Text, out int numberOfFrames) || numberOfFrames < 1)
                {
                    MessageBox.Show("Please enter a valid number of frames (minimum 1)",
                        "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(txtSpacing.Text, out double spacing) || spacing <= 0)
                {
                    MessageBox.Show("Please enter a valid spacing value (greater than 0)",
                        "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                cSapModel mySapModel = _mySapModel;

                if (mySapModel == null)
                {
                    MessageBox.Show("SapModel is null. Plugin did not pass a valid model.",
                        "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                mySapModel.SetPresentUnits(eUnits.kN_m_C);

                SAPAPI_HelperMethods.CreateConcreteMaterial(mySapModel);
                SAPAPI_HelperMethods.CreateRebarMaterial(mySapModel);
                SAPAPI_HelperMethods.CreateColumnCrossSection(mySapModel);
                SAPAPI_HelperMethods.CreateBeamCrossSection(mySapModel);

               
                mySapModel.LoadPatterns.Add("DEAD", eLoadPatternType.Dead, 1, true);

                
                SAPAPI_HelperMethods.CreateLoadPatterns(mySapModel);

                double columnHeight = 5.0;
                double frameDepth = spacing;  

                
                for (int i = 0; i <= numberOfFrames; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        double xPos = i * spacing;
                        double yPos = j * frameDepth;

                        XYZ bottomPoint = new XYZ { X = xPos, Y = yPos, Z = 0 };
                        XYZ topPoint = new XYZ { X = xPos, Y = yPos, Z = columnHeight };

                        SAPAPI_HelperMethods.CreateColumn(mySapModel, bottomPoint, topPoint);

                        SAPAPI_HelperMethods.CreateSupport(mySapModel, bottomPoint.CreatedName,
                            new bool[6] { true, true, true, true, true, true });
                    }
                }

                
                for (int i = 0; i < numberOfFrames; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        double x1 = i * spacing;
                        double x2 = (i + 1) * spacing;
                        double yPos = j * frameDepth;

                        XYZ beamStart = new XYZ { X = x1, Y = yPos, Z = columnHeight };
                        XYZ beamEnd = new XYZ { X = x2, Y = yPos, Z = columnHeight };

                        string beamName = SAPAPI_HelperMethods.CreateBeamBetweenPoints(
                            mySapModel, beamStart, beamEnd);

                        SAPAPI_HelperMethods.AddDistributedLoad(mySapModel, beamName, 5.0);
                    }
                }

               
                for (int i = 0; i <= numberOfFrames; i++)
                {
                    double xPos = i * spacing;

                    XYZ beamStart = new XYZ { X = xPos, Y = 0, Z = columnHeight };
                    XYZ beamEnd = new XYZ { X = xPos, Y = frameDepth, Z = columnHeight };

                    string beamName = SAPAPI_HelperMethods.CreateBeamBetweenPoints(
                        mySapModel, beamStart, beamEnd);

                    SAPAPI_HelperMethods.AddDistributedLoad(mySapModel, beamName, 5.0);
                }

                mySapModel.View.RefreshView();

              
                int analysisResult = mySapModel.Analyze.RunAnalysis();

                if (analysisResult == 0)
                {
                    mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
                    mySapModel.Results.Setup.SetCaseSelectedForOutput("WALL LOAD");

                    MessageBox.Show(
                        $"Successfully created {numberOfFrames} frames with {spacing}m spacing!\n\nAnalysis completed successfully.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Frames created but analysis failed with error code: {analysisResult}",
                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}