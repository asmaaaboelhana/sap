using System;
using SAP2000v1;
using FrameCreation_WPF.View;

namespace FrameCreation_WPF
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class cPlugin
    {
        public int Info(ref string Text)
        {
            Text = "Frame Creation WPF Plugin";
            return 0;
        }

        public void Main(ref cSapModel SapModel, ref cPluginCallback ISapPlugin)
        {
            try
            {
                
                Tool_View window = new Tool_View(SapModel);
                window.ShowDialog();
                ISapPlugin.Finish(0);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error in plugin: {ex.Message}",
                    "Plugin Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                ISapPlugin.Finish(1);
            }
        }
    }
}