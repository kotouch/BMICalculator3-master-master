using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;

namespace BMICalculator
{

    [XmlRoot("BMICalculator", Namespace = "www.bmicalc.ninja")]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string FilePath = "C:/Temp/";
        public string FileName = "yourBMI.xml";
        
        
        public class Customer
        {



            [XmlAttribute("Last Name")]
            public string lastName { get; set; }
            [XmlAttribute("First Name")]
            public string firstName { get; set; }
            [XmlAttribute("Phone Number")]
            public string phoneNumber { get; set; }
            [XmlAttribute("Height")]
            public int heightInches { get; set; }
            [XmlAttribute("Weight")]
            public int weightLbs { get; set; }
            [XmlAttribute("Customer BMI")]
            public int custBMI { get; set; }
            [XmlAttribute("Status")]
            public string statusTitle { get; set; }
        }


      
        public MainWindow()
        {
            InitializeComponent();
            OnLoadStats();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void xClear_Click(object sender, RoutedEventArgs e)
        {
            xLastName.Text = "";
            xFirstName.Text = "";
            xPhone.Text = "";
            xHeight.Text = "";
            xWeight.Text = "";
        }

        private void xSubmit_Click(object sender, RoutedEventArgs e)
        {
            Customer customer1 = new Customer();

            customer1.lastName = xLastName.Text;
            customer1.firstName = xFirstName.Text;
            customer1.phoneNumber = xPhone.Text;

            int currentWeight = 0;
            int currentHeight = 0;
            Int32.TryParse(xWeight.Text, out currentWeight);
            Int32.TryParse(xHeight.Text, out currentHeight);
            customer1.weightLbs = currentWeight;
            customer1.heightInches = currentHeight;

            int bmi;
            bmi = 703 * customer1.weightLbs / (customer1.heightInches * customer1.heightInches);
            customer1.custBMI = bmi;

            string yourBMIstatus = "NA";
            customer1.statusTitle = yourBMIstatus;

            xBMI.Text = Convert.ToString(bmi);

            if (bmi < 18.5)
            {
                //under weight
                xBMIText.Text = "According to CDC.gov you are under weight.";
            }
            else if (bmi > 29.9)
            {
                //obese
                xBMIText.Text = "According to CDC.gov you are obese.";
            }
            else if (bmi > 24.9)
            {
                //over weight
                xBMIText.Text = "According to CDC.gov you are over weight.";
            }
            else
            {
                //normal
                xBMIText.Text = "According to CDC.gov you are the normal weight.";
                


                TextWriter writer = new StreamWriter(FilePath + FileName);
                XmlSerializer ser = new XmlSerializer(typeof(Customer));



                ser.Serialize(writer, customer1);
                writer.Close();
            }
            //MessageBox.Show($"The Customer's name is {customer1.firstName} {customer1.lastName} and they can be reached at {customer1.phoneNumber}. They are {customer1.heightInches} inches tall. Their weight is {customer1.weightLbs}. Their BMI is {bmi}");
        }

        private void OnLoadStats()
        {
            Customer cust = new Customer();

            XmlSerializer des = new XmlSerializer(typeof(Customer));
            using (XmlReader reader = XmlReader.Create(FilePath + FileName))
            {
                cust = (Customer)des.Deserialize(reader);


                xLastName.Text = cust.lastName;
                xFirstName.Text = cust.firstName;
                xPhone.Text = cust.phoneNumber;


            }

            DataSet xmlData = new DataSet();
            xmlData.ReadXml(FilePath + FileName, XmlReadMode.Auto);
            Dataman.ItemsSource = xmlData.Tables[0].DefaultView;

        }
    }
}
