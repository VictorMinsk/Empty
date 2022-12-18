using System;
using System.Windows.Forms;

namespace Lab5Graphic
{
    public partial class Form1 : Form
    {
        int n;
        double[] x;
        double[] fx;
        double[] coeffs;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            n = ((int)numericUpDown1.Value);
            dataGridView1.RowCount = n;
            for (int i = 0; i < n; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = "0";
                dataGridView1.Rows[i].Cells[1].Value = "0";
            }
            x = new double[n];
            fx = new double[n];
            coeffs = new double[n];
            btnInterpolate.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < n; i++)
            {
                try
                {
                    x[i] = double.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    fx[i] = double.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                }
                catch (Exception) 
                {
                    MessageBox.Show("Неверно введены исходные данные в "+(i+1)+" строке");
                    return;
                }
            }
            //определение формулы интерполяционного многочлена Лагранжа
            for (int i = 0; i < n; i++)
            {
                double d = 1;
                double[]c = new double[n];
                for (int l = 0; l < n; l++)
                    c[l] = 1.0;
                double[] c2 = new double[n];
                for (int l = 0; l < n; l++)
                    c2[l] = 1.0;
                int m = 0;
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        d *= (x[i] - x[j]);
                        c[0] *= (-x[j]);
                        if (m > 0) c[m] = 1;
                        for (int k = 1; k < m + 1; k++)
                            c[k] = c2[k] * (-x[j]) + c2[k - 1];
                        for (int l = 0; l < n; l++)
                            c2[l] = c[l];
                        m++;
                    }
                }
                for (int j = 0; j < n; j++)
                {
                    c[j] = c[j] * fx[i] / d;
                    coeffs[j] += c[j];
                }
            }
            //построение графика
            chart1.ChartAreas[0].AxisX.Minimum = x[0];
            chart1.ChartAreas[0].AxisX.Maximum = x[n-1];
            chart1.Series[0].Points.DataBindXY(x, fx);
            //вывод формулы на экран
            string result = "L(x)=";
            for (int i = n - 1; i >= 0; i--) { string str= string.Format("{0:+0.00;-0.00}x^{1:d}", coeffs[i], i);
                result += str;
            }
            label3.Text = result;
            btnCalculate.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double xx;
            try
            {
                xx = double.Parse(textBox1.Text);
            }
            catch(Exception)
            {
                MessageBox.Show("Неверно введена точка");
                return;
            }
            label4.Text = string.Format("{0:+0.00;-0.00}", L(xx));
        }

        private double L(double x) //вычисление значения многочлена Лагранжа в точке
        {
            double res = 0.0;
            for (int i = 0; i < n; i++)
                res += Math.Pow(x, i) * coeffs[i];
            return res;
        }
    }
}
