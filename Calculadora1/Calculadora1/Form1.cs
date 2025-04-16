using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculadora1
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox_Resultado.Enabled = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double num1 = Convert.ToDouble(textBox_Num1.Text);
                double num2 = Convert.ToDouble(textBox_Num2.Text);
                double suma = num1 + num2;
                textBox_Resultado.Text = suma.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("POR FAVOR, INGRESE VALORES NUMERICOS VALIDOS 😁");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double num1 = Convert.ToDouble(textBox_Num1.Text);
                double num2 = Convert.ToDouble(textBox_Num2.Text);
                double suma = num1 - num2;
                textBox_Resultado.Text = suma.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("POR FAVOR, INGRESE VALORES NUMERICOS VALIDOS 😁");
            }
        }
    }
}
