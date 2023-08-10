using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwainScanning;
using TwainScanning.Bridgex86;
using TwainScanning.Capability.CapabilitySets;
using TwainScanning.NativeStructs;
using WindowsFormsApp6.Properties;


namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        private DataSourceManager dataSourceManager;
        
        public Form1()
        {
            InitializeComponent();
            dataSourceManager = new DataSourceManager();
        }
        private List<TwIdentity> loadSources()
        {
            var list = new List<TwIdentity>();
            var scanners = dataSourceManager.AvailableSources();
            foreach (var item in scanners)
            {
                list.Add(item); 
            }
            return list;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var list = loadSources();
            foreach(var item in list)
            {
                listBox1.Items.Add(item.ProductName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string outPut = @"C:\\Users\\Marwan\\Documents\\ScannedImages"+GenerateNameId()+".jpeg";
            var settings = new ScanSettings();
            settings.Device = listBox1.SelectedItem.ToString(); // default scanner is used if omitted
            settings.TransferMechanism = TwSX.Native;
            settings.ShowUI = false; // show UI from driver software or not
            settings.CloseUIAfterAcquire = true; // close UI from driver software or not
            var pageCap = Bridgex86.GetSupportedPageSizes(listBox1.SelectedItem.ToString());
            
            
            settings.AutoFeed = true;
            settings.ColorMode = TwPixelType.RGB;
            settings.DuplexEnabled = true;
            settings.ImageCount = -1; // scan all available images or limit number of images to 
            
            settings.ImageQuality = 80;
            settings.Resolution = new ScanResolution(300); // resolution can be individual for x 
           
           settings.MultiPageScan = false; // scan into single file with multiple pages (tiff and 
            

           
           Bridgex86.Acquire(outPut,settings);
           pictureBox1.Image = Image.FromFile(outPut);
           pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;



        }
        private string GenerateNameId()
        {
           return Guid.NewGuid().ToString();  
        }
    }
}
