using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kondey
{
    public partial class Form1 : Form
    {

        double C, R, Up, I, dt, dq,T,T3,T5,T8,Tp,T3p,T5p;

        private void button2_Click(object sender, EventArgs e)
        {

        }

        int Iteration = 1000;
        double[] Uc;
        double[] q;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Input();
            Calc();
            DrawGr();
            Output();
        }

        void Input()
        {
            C = double.Parse(tBC.Text);  //значение ёмкости конденсатора
            R = double.Parse(tBR.Text);  //значение резистора
            Up = double.Parse(tBU.Text); //напряжение питания (в данном расчёте постоянное)
        }

        void Calc()
        {
            
            T = R * C; //T - постоянная времени
            T3 = T * 3;
            T5 = T * 5;
            T8 = T * 8;
            dt = T8 / Iteration; //T8 максимальное время для расчёта Iteration - количество точек расчёта, чем больше тем точнее
            Uc = new double[Iteration]; //напруга
            q = new double[Iteration];  //заряды
            Uc[0] = 0;
            q[0] = 0;  
            for (int i = 1; i < Iteration; i++)
            {
                I = (Up - Uc[i - 1]) / R;
                dq = I * dt;
                q[i] = q[i - 1] + dq;
                Uc[i] = q[i] / C;
                //черточки вертикальные
                if (T / dt == (double)i) { Tp = Uc[i]; }
                if (T3 / dt == (double)i) { T3p = Uc[i]; }
                if (T5 / dt == (double)i) { T5p = Uc[i]; }
            }


        }

        void Output()
        {
            tBT.Text = T.ToString();
            tB3T.Text = T3.ToString();
            tB5T.Text = T5.ToString();
            tBTp.Text = string.Format("{0}%", Tp.ToString()) ;
            tB3Tp.Text = string.Format("{0}%", T3p.ToString());
            tB5Tp.Text = string.Format("{0}%", T5p.ToString());
            tBq.Text = q[Iteration-1].ToString();
        }

        void DrawGr()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            for (int i = 1; i < Iteration; i++)
            {
                chart1.Series[0].Points.AddXY( i * dt, Uc[i]);

                if (Uc[i] == Tp) { chart1.Series[1].Points.AddXY(i * dt, Tp); }
                if (Uc[i] == T3p) { chart1.Series[1].Points.AddXY(i * dt, T3p); }
                if (Uc[i] == T5p) { chart1.Series[1].Points.AddXY(i * dt, T5p); }
            }

            

        }


    }
}
