using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadoraDigital
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += boton.Text;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += " " + boton.Text + " ";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += " " + boton.Text + " ";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                string expresion = textBox1.Text;
                DataTable dt = new DataTable();
                var resultado = dt.Compute(expresion, "");
                textBox1.Text = resultado.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la expresión", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
             
        private void button14_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += " " + boton.Text + " ";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            textBox1.Text += " " + boton.Text + " ";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}
